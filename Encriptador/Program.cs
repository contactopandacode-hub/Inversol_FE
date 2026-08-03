using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encriptador
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter input: ");
            byte[] encryptedRaw = EncryptStringToBytes_Aes(Console.ReadLine());
            TextCopy.Clipboard.SetText(Convert.ToBase64String(encryptedRaw));
        }

        public static byte[] EncryptStringToBytes_Aes(string plainText)
        {
            String key = Environment.GetEnvironmentVariable("NETCORE_KEY");
            if (key == null)
                key = "hyb91p4nhvcnlmlkye17uyfz63q5jtcy";
            else if (key.Trim().Length != 32)
                key = "hyb91p4nhvcnlmlkye17uyfz63q5jtcy";

            var Key = Encoding.UTF8.GetBytes(key);
            var IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }
    }
}