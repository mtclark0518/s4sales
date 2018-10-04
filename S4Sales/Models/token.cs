using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace S4Sales.Models
{
    public class DownloadToken
    {
        #region Props
        const string secret = "ooohIeyejustDieedInyourArms2kNigh7";
        public DateTime issued {get; set;}
        public DateTime expiration {get; set;}
        public string cart { get; set; }
        public string charge { get; set; }
        public string hsmv_report_number { get; set; }
        public bool exchanged { get; internal set; }
        public DateTime exchanged_ts { get; set; }

        #endregion

        public DownloadToken()
        {
            issued = DateTime.Now;
            expiration = issued.AddDays(3);
            exchanged = false;
        }
        public DownloadToken(string crt, string hsmv, string chg) : this()
        {
            cart = crt;
            hsmv_report_number = hsmv;
            charge = chg;
        }
        #region Public Methods
        public string Mint()
        {
            string str = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}","|",
                cart, hsmv_report_number, charge, exchanged, issued, expiration);

            byte[] encrypted = Encrypt(str);
            string minted = Convert.ToBase64String(encrypted);
            return minted;
        }
        public string[] Consume(string token)
        {
            byte[] s = Convert.FromBase64String(token);
            string decrypted = Decrypt(s);
            string[] loot = decrypted.Split(new[] {"|"}, StringSplitOptions.None );

            return loot; 
        }
        #endregion

        #region Private Methods
        public byte[] Encrypt(string str)
        {
            byte[] encrypted;
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            // Create an Aes object
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] salt = new UTF8Encoding().GetBytes(secret);
                var rfc = new Rfc2898DeriveBytes(secret, salt, 10000, HashAlgorithmName.SHA512);
                aes.Key = rfc.GetBytes(aes.KeySize/8);
                aes.IV = rfc.GetBytes(aes.BlockSize/8);
                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                // Create the streams used for encryption.
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                    }
                    encrypted = ms.ToArray();
                }
            }
            return encrypted;
        }
        public string Decrypt(byte[] cipher)
        {
            string decrypted;
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] salt = new UTF8Encoding().GetBytes(secret);
                var rfc = new Rfc2898DeriveBytes(secret, salt, 10000, HashAlgorithmName.SHA512);
                aes.Key = rfc.GetBytes(aes.KeySize/8);
                aes.IV = rfc.GetBytes(aes.BlockSize/8);
                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                // Create the streams used for decryption.
                using (var ms = new MemoryStream(cipher))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }
            return decrypted;
        }
        #endregion
    }
}