using System.Security.Cryptography;
using System.Text;

namespace Rental_Application.EntityLayer.Utility
{
    public class AESHelper
    {
        private readonly byte[] _key;
        private const int KeySize = 256;
        private const int IvSize = 16; // AES block size
        private const int SaltSize = 32; // Recommended salt size
        private static readonly string _EncryptionKey;

        public AESHelper(string passphrase)
        {
            // Generate a secure key using PBKDF2 with SHA256
            using (var deriveBytes = new Rfc2898DeriveBytes(passphrase, SaltSize, 100000, HashAlgorithmName.SHA256))
            {
                _key = deriveBytes.GetBytes(KeySize / 8);
            }
        }

        public string Encrypt(string plainText)
        {
            byte[] iv = GenerateRandomBytes(IvSize);
            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                    encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                }
            }

            // Combine IV + encrypted data
            byte[] result = new byte[iv.Length + encrypted.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] iv = new byte[IvSize];
            byte[] cipherText = new byte[encryptedBytes.Length - IvSize];

            // Extract IV and ciphertext
            Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, iv.Length, cipherText, 0, cipherText.Length);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private byte[] GenerateRandomBytes(int size)
        {
            byte[] bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }
    }
}
