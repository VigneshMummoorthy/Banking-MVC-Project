using System.Security.Cryptography;
using System.Text;

namespace iStart.Helper
{
    public class AesEncryptionHelper
    {
        private static readonly string Key = "your-32-char-secret-key-123456789012";

        public static string Encrypt(string plainText)
        {
            var keyBytes = Encoding.UTF8.GetBytes(Key);
            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV();
            var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(aes.IV.Concat(cipherBytes).ToArray());
        }

        public static string Decrypt(string encryptedText)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);
            var iv = fullCipher.Take(16).ToArray();
            var cipher = fullCipher.Skip(16).ToArray();
            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
