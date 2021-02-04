using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GP40Main.Utility
{

    /// <summary>
    /// Meno        : Support Encrypt/Decrypt data
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    public static class EncryptHelper
    {

        public static string Encrypt(this string data, string key)
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

        public static string Decrypt(this string data, string key)
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









        //// This constant determines the number of iterations for the password bytes generation function.
        //private const int DerivationIterations = 1000;
        //// This constant is used to determine the keysize of the encryption algorithm in bits.
        //// We divide this by 8 within the code below to get the equivalent number of bytes.
        //private const int Keysize = 256;

        ///// <summary>
        ///// Decrypts the specified cipher text.
        ///// </summary>
        ///// <param name="cipherText">The cipher text.</param>
        ///// <param name="passPhrase">The pass phrase.</param>
        ///// <returns></returns>
        //public static string Decrypt(this string cipherText, string passPhrase)
        //{
        //    if (string.IsNullOrEmpty(cipherText))
        //    {
        //        return string.Empty;
        //    }

        //    // Get the complete stream of bytes that represent:
        //    // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
        //    var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
        //    // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
        //    var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
        //    // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
        //    var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
        //    // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
        //    var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

        //    using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        //    {
        //        var keyBytes = password.GetBytes(Keysize / 8);
        //        using (var symmetricKey = new RijndaelManaged())
        //        {
        //            symmetricKey.BlockSize = 256;
        //            symmetricKey.Mode = CipherMode.CBC;
        //            symmetricKey.Padding = PaddingMode.PKCS7;
        //            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
        //            {
        //                using (var memoryStream = new MemoryStream(cipherTextBytes))
        //                {
        //                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        //                    {
        //                        var plainTextBytes = new byte[cipherTextBytes.Length];
        //                        var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        //                        memoryStream.Close();
        //                        cryptoStream.Close();
        //                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Encrypts the specified plain text.
        ///// </summary>
        ///// <param name="plainText">The plain text.</param>
        ///// <param name="passPhrase">The pass phrase.</param>
        ///// <returns></returns>
        //public static string Encrypt(this string plainText, string passPhrase)
        //{
        //    if (string.IsNullOrEmpty(plainText))
        //    {
        //        return string.Empty;
        //    }

        //    // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
        //    // so that the same Salt and IV values can be used when decrypting.
        //    var saltStringBytes = Generate256BitsOfRandomEntropy();
        //    var ivStringBytes = Generate256BitsOfRandomEntropy();
        //    var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        //    {
        //        var keyBytes = password.GetBytes(Keysize / 8);
        //        using (var symmetricKey = new RijndaelManaged())
        //        {
        //            symmetricKey.BlockSize = 256;
        //            symmetricKey.Mode = CipherMode.CBC;
        //            symmetricKey.Padding = PaddingMode.PKCS7;
        //            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
        //            {
        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        //                    {
        //                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        //                        cryptoStream.FlushFinalBlock();
        //                        // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
        //                        var cipherTextBytes = saltStringBytes;
        //                        cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
        //                        cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
        //                        memoryStream.Close();
        //                        cryptoStream.Close();
        //                        return Convert.ToBase64String(cipherTextBytes);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static string Sha512(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);

            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }

        ///// <summary>
        ///// Generate256s the bits of random entropy.
        ///// </summary>
        ///// <returns></returns>
        //private static byte[] Generate256BitsOfRandomEntropy()
        //{
        //    var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
        //    using (var rngCsp = new RNGCryptoServiceProvider())
        //    {
        //        // Fill the array with cryptographically secure random bytes.
        //        rngCsp.GetBytes(randomBytes);
        //    }
        //    return randomBytes;
        //}
    }
}
