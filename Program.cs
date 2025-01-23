using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;

namespace AndroidKeyGen
{
    public static class Program
    {
        public static string wd = string.Empty;

        public enum AlgorithmType
        {
            RSA,
            DSA,
            EC,
        }

        [STAThread]
        private static void Main()
        {
            var main = Assembly.GetExecutingAssembly();
            wd = Path.GetDirectoryName(main.Location);
            foreach (var resName in main.GetManifestResourceNames())
            {
                using var res = main.GetManifestResourceStream(resName);
                if (resName.EndsWith(".gz"))
                {
                    using var gzDll = new GZipStream(res, CompressionMode.Decompress);
                    using var ms = new MemoryStream();
                    gzDll.CopyTo(ms);
                    AppDomain.CurrentDomain.AssemblyResolve += (_, __) => Assembly.Load(ms.ToArray());
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AndroidKeyGenForm().GetForm());
        }

        public static class JksUtils
        {
            public static int CreateKeyStore(AlgorithmType type, int year, string alias, string pwd, DateTime date)
            {
                string algorithm = null;
                IAsymmetricCipherKeyPairGenerator generator = null;
                KeyGenerationParameters param = null;
                SecureRandom rdm = new();
                switch (type)
                {
                    case AlgorithmType.RSA:
                        algorithm = PkcsObjectIdentifiers.Sha256WithRsaEncryption.ToString();
                        generator = new RsaKeyPairGenerator();
                        param = new KeyGenerationParameters(rdm, 2048);
                        break;

                    case AlgorithmType.EC:
                        algorithm = X9ObjectIdentifiers.ECDsaWithSha256.ToString();
                        generator = new ECKeyPairGenerator();
                        param = new KeyGenerationParameters(rdm, 256);
                        break;

                    case AlgorithmType.DSA:
                        algorithm = NistObjectIdentifiers.DsaWithSha256.ToString();
                        var dsa = new DsaParametersGenerator();
                        dsa.Init(1024, 80, rdm);
                        generator = new DsaKeyPairGenerator();
                        param = new DsaKeyGenerationParameters(rdm, dsa.GenerateParameters());
                        break;
                }

                if (generator == null)
                {
                    return -1;
                }

                generator.Init(param);
                var pair = generator.GenerateKeyPair();
                var x509 = new X509V3CertificateGenerator();
                var name = new X509Name("c=" + alias);
                x509.SetIssuerDN(name);
                x509.SetSubjectDN(name);
                x509.SetNotBefore(date);
                x509.SetNotAfter(date.AddYears(year));
                x509.SetPublicKey(pair.Public);
                x509.SetSerialNumber(BigInteger.ValueOf(Math.Abs(rdm.NextLong())));
                var store = new Pkcs12StoreBuilder().Build();
                store.SetKeyEntry(alias, new AsymmetricKeyEntry(pair.Private), [new X509CertificateEntry(x509.Generate(new Asn1SignatureFactory(algorithm, pair.Private)))]);
                var ext = Path.GetExtension(alias);
                if (Directory.Exists(alias) || !ext.Contains("jks") || !ext.Contains("keystore"))
                {
                    alias += ".jks";
                }

                var savePath = Path.Combine(wd, Path.GetFileName(alias));
                if (Directory.Exists(savePath))
                {
                    MessageBox.Show("名称冲突，请修改名称！", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 1;
                }

                try
                {
                    using var fs = File.Create(savePath);
                    store.Save(fs, pwd.ToCharArray(), rdm);
                }
                catch
                {
                    MessageBox.Show("写入失败，请尝试更改名称或在其他路径运行此工具!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 2;
                }
                return 0;
            }
        }
    }
}