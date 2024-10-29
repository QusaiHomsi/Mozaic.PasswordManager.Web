using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
// this is used to encrypt the password that the systemuser(User) saved in useraccounts the key is the systemuser.password
public static class SymmetricEncryption
{
    public static string Encrypt(string plainText, string password)
    {
        using (var aes = Aes.Create())
        {
            var key = new byte[32]; 
            var iv = new byte[16];  

            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                Array.Copy(hashedKey, key, key.Length);
            }

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }

            aes.Key = key;
            aes.IV = iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string encryptedText, string password)
    {
        using (var aes = Aes.Create())
        {
            var key = new byte[32];
            var iv = new byte[16];

            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                Array.Copy(hashedKey, key, key.Length);
            }

            var fullCipher = Convert.FromBase64String(encryptedText);
            Array.Copy(fullCipher, iv, iv.Length);

            aes.Key = key;
            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                try
                {
                    using (var ms = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
                catch (CryptographicException)
                {
                    throw new Exception("Decryption failed. The password may be incorrect.");
                }
            }
        }
    }

}
