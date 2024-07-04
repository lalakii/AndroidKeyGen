using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Nist;
using System.Reflection;
using System.Windows.Forms;

namespace AndroidKeyGen
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var main = Assembly.GetExecutingAssembly();
            foreach (string resName in main.GetManifestResourceNames())
            {
                Stream res = main.GetManifestResourceStream(resName);
                if (resName.EndsWith(".dll"))
                {
                    var ms = new MemoryStream();
                    res.CopyTo(ms);
                    AppDomain.CurrentDomain.AssemblyResolve += (_, __) => Assembly.Load(ms.ToArray());
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AndroidKeyGen());
        }

        public enum AlgorithmType
        {
            RSA, DSA, EC
        }

        public sealed class JksUtils
        {
            public int CreateKeyStore(AlgorithmType type, int year, string alias, string pwd)
            {
                string algorithm = null;
                IAsymmetricCipherKeyPairGenerator generator = null;
                KeyGenerationParameters param = null;
                var rdm = new SecureRandom();
                switch (type)
                {
                    case AlgorithmType.RSA:
                        algorithm = PkcsObjectIdentifiers.Sha256WithRsaEncryption.ToString();
                        generator = new RsaKeyPairGenerator();
                        param = new KeyGenerationParameters(rdm, 2048);
                        break;

                    case AlgorithmType.DSA:
                        algorithm = NistObjectIdentifiers.DsaWithSha256.ToString();
                        var dsa = new DsaParametersGenerator();
                        dsa.Init(1024, 100, rdm);
                        generator = new DsaKeyPairGenerator();
                        param = new DsaKeyGenerationParameters(rdm, dsa.GenerateParameters());
                        break;

                    case AlgorithmType.EC:
                        algorithm = X9ObjectIdentifiers.ECDsaWithSha256.ToString();
                        generator = new ECKeyPairGenerator();
                        param = new KeyGenerationParameters(rdm, 256);
                        break;
                }
                if (generator == null) return -1;
                generator.Init(param);
                var pair = generator.GenerateKeyPair();
                var x509 = new X509V3CertificateGenerator();
                var name = new X509Name("c=" + alias);
                var date = DateTime.Now;
                x509.SetIssuerDN(name);
                x509.SetSubjectDN(name);
                x509.SetNotBefore(date);
                x509.SetNotAfter(date.AddYears(year));
                x509.SetPublicKey(pair.Public);
                x509.SetSerialNumber(BigInteger.ValueOf(Math.Abs(rdm.NextLong())));
                var store = new Pkcs12StoreBuilder().Build();
                store.SetKeyEntry(alias, new AsymmetricKeyEntry(pair.Private), new[] { new X509CertificateEntry(x509.Generate(new Asn1SignatureFactory(algorithm, pair.Private))) });
                var ext = Path.GetExtension(alias);
                if (Directory.Exists(alias) || !ext.Contains("jks") || !ext.Contains("keystore"))
                {
                    alias += ".jks";
                }
                using (var fs = new FileStream(alias, FileMode.Create, FileAccess.Write))
                {
                    store.Save(fs, pwd.ToCharArray(), rdm);
                }
                return 0;
            }
        }
    }
}