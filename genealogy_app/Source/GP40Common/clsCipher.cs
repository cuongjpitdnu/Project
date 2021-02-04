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
            FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            cryptic.Key = UTF8Encoding.UTF8.GetBytes("12345678");
            cryptic.IV = UTF8Encoding.UTF8.GetBytes("12345678");

            CryptoStream crStream = new CryptoStream(stream, cryptic.CreateEncryptor(), CryptoStreamMode.Write);


            byte[] data = UTF8Encoding.UTF8.GetBytes(content);

            crStream.Write(data, 0, data.Length);

            crStream.Close();
            stream.Close();
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
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            DESCryptoServiceProvider cryptic = new DESCryptoServiceProvider();

            cryptic.Key = UTF8Encoding.UTF8.GetBytes("12345678");
            cryptic.IV = UTF8Encoding.UTF8.GetBytes("12345678");

            CryptoStream crStream = new CryptoStream(stream,
                cryptic.CreateDecryptor(), CryptoStreamMode.Read);

            StreamReader reader = new StreamReader(crStream);

            string data = reader.ReadToEnd();

            reader.Close();
            stream.Close();

            return data;
        }

        public static void Encrypt(FileInfo targetFile, string password, Action<CryptoStream> action)
        {
            var keyGenerator = new Rfc2898DeriveBytes(password, SaltSize);
            var rijndael = Rijndael.Create();

            // BlockSize, KeySize in bit --> divide by 8
            rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

            using (var fileStream = targetFile.Create())
            {
                // write random salt
                fileStream.Write(keyGenerator.Salt, 0, SaltSize);

                using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    // write data
                    if (action != null)
                    {
                        //using (StreamWriter swEncrypt = new StreamWriter(cryptoStream))
                        //{

                        //    //Write all data to the stream.
                        //    action(swEncrypt);
                        //}

                        action(cryptoStream);
                    }

                    //cryptoStream.Flush();
                }
            }
        }

        public static void Decrypt(FileInfo sourceFile, string password, Action<CryptoStream> action)
        {
            // read salt
            var fileStream = sourceFile.OpenRead();
            var salt = new byte[SaltSize];
            fileStream.Read(salt, 0, SaltSize);

            // initialize algorithm with salt
            var keyGenerator = new Rfc2898DeriveBytes(password, salt);
            var rijndael = Rijndael.Create();
            rijndael.IV = keyGenerator.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = keyGenerator.GetBytes(rijndael.KeySize / 8);

            // decrypt
            using (var cryptoStream = new CryptoStream(fileStream, rijndael.CreateDecryptor(), CryptoStreamMode.Read))
            {
                // read data
                if (action != null)
                {
                    action(cryptoStream);
                }
            }
        }
    }
}