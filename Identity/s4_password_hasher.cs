using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace S4Sales.Identity
{
    public class S4PasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : S4IdentityBase
    {

        internal static string GenerateSalt()
        {
            string base64String;
            using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                byte[] numArray = new byte[16];
                rNGCryptoServiceProvider.GetBytes(numArray);
                base64String = Convert.ToBase64String(numArray);
            }
            return base64String;
        }

        public string HashPassword(TUser user, string password)
        {
            return HashPassword(password, user.password_salt, new HMACSHA1());
        }

        private string HashPassword(string pass, string salt, HashAlgorithm algorithm)
        {
            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bRet = null;

            HashAlgorithm hm = algorithm;
            if (hm is KeyedHashAlgorithm)
            {
                KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                if (kha.Key.Length == bSalt.Length)
                {
                    kha.Key = bSalt;
                }
                else if (kha.Key.Length < bSalt.Length)
                {
                    byte[] bKey = new byte[kha.Key.Length];
                    Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                    kha.Key = bKey;
                }
                else
                {
                    byte[] bKey = new byte[kha.Key.Length];
                    for (int iter = 0; iter < bKey.Length;)
                    {
                        int len = Math.Min(bSalt.Length, bKey.Length - iter);
                        Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                        iter += len;
                    }
                    kha.Key = bKey;
                }
                bRet = kha.ComputeHash(bIn);
            }
            else
            {
                byte[] bAll = new byte[bSalt.Length + bIn.Length];
                Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                bRet = hm.ComputeHash(bAll);
            }
            var convert = Convert.ToBase64String(bRet);

            return convert;
        }
        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashed, string provided)
        {
            return hashed == HashPassword(user, provided) ? 
                PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
