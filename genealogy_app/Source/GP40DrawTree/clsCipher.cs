using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GP40Common
{
    public class clsCipher
    {
        private const int SaltSize = 8;


        public static void EncryptString(string path, string content)
        {
            File.WriteAllText(path, Encrypt(content, "12345678"));

            //FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            //DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            //cryptic.Key = UTF8Encoding.UTF8.GetBytes("12345678");
            //cryptic.IV = UTF8Encoding.UTF8.GetBytes("12345678");

            //CryptoStream crStream = new CryptoStream(stream, cryptic.CreateEncryptor(), CryptoStreamMode.Write);


            //byte[] data = UTF8Encoding.UTF8.GetBytes(content);

            //crStream.Write(data, 0, data.Length);

            //crStream.Close();
            //stream.Close();
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string DecryptString(string path)
        {
            if (File.Exists(path))
            {
                return Decrypt(File.ReadAllText(path), "12345678");
            }

            return string.Empty;

            //FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            //DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            //cryptic.Key = UTF8Encoding.UTF8.GetBytes("12345678");
            //cryptic.IV = UTF8Encoding.UTF8.GetBytes("12345678");

            //CryptoStream crStream = new CryptoStream(stream,
            //    cryptic.CreateDecryptor(), CryptoStreamMode.Read);

            //StreamReader reader = new StreamReader(crStream);

            //string data = reader.ReadToEnd();

            //reader.Close();
            //stream.Close();

            //return data;
        }

        private static string Encrypt(string data, string key)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            string encData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return encData;
        }

        private static string Decrypt(string data, string key)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }

            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decData;
        }

        private static byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        //public static void Encrypt(FileInfo targetFile, string password, Action<CryptoStream> action)
        //{
        //    var keyGenerator = new Rfc2898DeriveBytes(password, SaltSize);
        //    var rijndael = Rijndael.Create();

        //    // BlockSize, KeySize in bit --> divide by 8
        //    rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
        //    rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

        //    using (var fileStream = targetFile.Create())
        //    {
        //        // write random salt
        //        fileStream.Write(keyGenerator.Salt, 0, SaltSize);

        //        using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
        //        {
        //            // write data
        //            if (action != null)
        //            {
        //                //using (StreamWriter swEncrypt = new StreamWriter(cryptoStream))
        //                //{

        //                //    //Write all data to the stream.
        //                //    action(swEncrypt);
        //                //}

        //                action(cryptoStream);
        //            }

        //            //cryptoStream.Flush();
        //        }
        //    }
        //}

        //public static void Decrypt(FileInfo sourceFile, string password, Action<CryptoStream> action)
        //{
        //    // read salt
        //    var fileStream = sourceFile.OpenRead();
        //    var salt = new byte[SaltSize];
        //    fileStream.Read(salt, 0, SaltSize);

        //    // initialize algorithm with salt
        //    var keyGenerator = new Rfc2898DeriveBytes(password, salt);
        //    var rijndael = Rijndael.Create();
        //    rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
        //    rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

        //    // decrypt
        //    using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateDecryptor(), CryptoStreamMode.Read))
        //    {
        //        // read data
        //        if (action != null)
        //        {
        //            action(cryptoStream);
        //        }
        //    }
        //}
    }
}