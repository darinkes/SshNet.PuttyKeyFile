using System;
using System.IO;
using System.Linq;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using Renci.SshNet.Security;

namespace SshNet.PuttyKeyFile.Tests
{
    public class PuttyKeyFileTest
    {
        [SetUp]
        public void Setup()
        {
        }

        private void TestKey<TKey>(string keyName, string comment, int keyLength = 0, string? pass = null) where TKey : Key
        {
            var keyStream = GetKey($"{keyName}.ppk");
            if (keyStream is null)
                throw new NullReferenceException(nameof(keyStream));

            var keyFile = new PuttyKeyFile(keyStream, pass);

            ClassicAssert.IsInstanceOf<TKey>(((KeyHostAlgorithm)keyFile.HostKeyAlgorithms.First()).Key);
            ClassicAssert.AreEqual(keyLength, ((KeyHostAlgorithm)keyFile.HostKeyAlgorithms.First()).Key.KeyLength);
            ClassicAssert.AreEqual(comment, ((KeyHostAlgorithm)keyFile.HostKeyAlgorithms.First()).Key.Comment);
        }

        private void TestKeyFail<TKey>(string keyName, string? pass = null) where TKey : Key
        {
            var keyStream = GetKey($"{keyName}.ppk");
            if (keyStream is null)
                throw new NullReferenceException(nameof(keyStream));

            //Renci.SshNet.Common.SshException: 'Invalid PuTTY private key'
            Assert.Throws(Is.TypeOf<Renci.SshNet.Common.SshException>()
                .And.Message.EqualTo("Invalid PuTTY private key"),
            () => _ = new PuttyKeyFile(keyStream, pass));
        }

        private void DoTests<TKey>(string keyName, string encKeyName, string comment, int keyLength = 0, string? pass = null) where TKey : Key
        {
            TestKey<TKey>(keyName, comment, keyLength);
            TestKey<TKey>(encKeyName, comment, keyLength, pass);
            TestKeyFail<TKey>(encKeyName, Guid.NewGuid().ToString());
        }

        [Test]
        public void Test_RSA2048()
        {
            DoTests<RsaKey>("rsa2048", "rsa2048pass", "rsa-key-20210312", 2048, "12345");
        }

        [Test]
        public void Test_RSA3072()
        {
            DoTests<RsaKey>("rsa3072","rsa3072pass", "rsa-key-20210312", 3072, "12345");
        }

        [Test]
        public void Test_RSA4096()
        {
            DoTests<RsaKey>("rsa4096","rsa4096pass", "rsa-key-20210312", 4096, "12345");
        }

        [Test]
        public void Test_RSA8192()
        {
            DoTests<RsaKey>("rsa8192","rsa8192pass", "rsa-key-20210312", 8192, "12345");
        }

        [Test]
        public void Test_ECDSA256()
        {
            DoTests<EcdsaKey>("ecdsa256","ecdsa256pass", "ecdsa-key-20210312", 256, "12345");
        }

        [Test]
        public void Test_ECDSA384()
        {
            DoTests<EcdsaKey>("ecdsa384","ecdsa384pass", "ecdsa-key-20210312", 384, "12345");
        }

        [Test]
        public void Test_ECDSA521()
        {
            DoTests<EcdsaKey>("ecdsa521","ecdsa521pass", "ecdsa-key-20210312", 521, "12345");
        }

        [Test]
        public void Test_ED25519()
        {
            DoTests<ED25519Key>("ed25519","ed25519pass", "ed25519-key-20210312", 256, "12345");
        }

        private static Stream? GetKey(string keyName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"SshNet.PuttyKeyFile.Tests.TestKeys.{keyName}");
        }
    }
}