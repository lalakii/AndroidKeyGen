using Org.BouncyCastle.Asn1;
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
using static System.Math;

namespace AndroidKeyGen
{
    public static class CertUtils
    {
        public enum AlgorithmType
        {
            RSA,
            DSA,
            EC,
        }

        public static int Create(System.IO.FileStream save, AlgorithmType type, int year, string alias, string pwd, System.DateTime date)
        {
            SecureRandom rdm = new();
            DerObjectIdentifier algorithm = null;
            IAsymmetricCipherKeyPairGenerator generator = null;
            KeyGenerationParameters param = null;
            switch (type)
            {
                case AlgorithmType.RSA:
                    algorithm = PkcsObjectIdentifiers.Sha256WithRsaEncryption;
                    generator = new RsaKeyPairGenerator();
                    param = new KeyGenerationParameters(rdm, 2048);
                    break;

                case AlgorithmType.EC:
                    algorithm = X9ObjectIdentifiers.ECDsaWithSha256;
                    generator = new ECKeyPairGenerator();
                    param = new KeyGenerationParameters(rdm, 256);
                    break;

                case AlgorithmType.DSA:
                    algorithm = NistObjectIdentifiers.DsaWithSha256;
                    var dsa = new DsaParametersGenerator();
                    dsa.Init(1024, 80, rdm);
                    generator = new DsaKeyPairGenerator();
                    param = new DsaKeyGenerationParameters(rdm, dsa.GenerateParameters());
                    break;
            }

            generator.Init(param);
            var pair = generator.GenerateKeyPair();
            var x509 = new X509V3CertificateGenerator();
            var name = new X509Name($"c={alias}");
            x509.SetIssuerDN(name);
            x509.SetSubjectDN(name);
            x509.SetNotBefore(date);
            x509.SetNotAfter(date.AddYears(year));
            x509.SetPublicKey(pair.Public);
            x509.SetSerialNumber(BigInteger.ValueOf(Abs(rdm.NextLong())));
            var store = new Pkcs12StoreBuilder().Build();
            store.SetKeyEntry(alias, new AsymmetricKeyEntry(pair.Private), [new X509CertificateEntry(x509.Generate(new Asn1SignatureFactory(new AlgorithmIdentifier(algorithm), pair.Private)))]);
            try
            {
                store.Save(save, pwd.ToCharArray(), rdm);
            }
            catch
            {
                return 2;
            }

            return 0;
        }
    }
}