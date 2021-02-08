using System;
using System.Security.Cryptography;
using System.Text;

namespace KeyCipher
{
    public class Cipher
    {
        private static bool isPadding = false;
        private static string publicKey = "NDA5NiE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+eGVJVnJMNlJVTEVzYVZRSWFBekswRzZlVm1OQVhnTHNwVDlKRWNPTWFvYmFEelJQM0Z6a1Bjdlk1dkdDSXFHQlYzVjh5MGUrMloxZ3NFdkx1Y25GNmJaMjlNaGFJZlJPb1M2Z3lQQThVd05vMElqaEVQOEVBUDBPU1c2anJUM2h4RXd1VGNKTVlWS2NqOStiWHM0UWpsQ09JM1YrZzB3UHBtVWc5ZEtsZmE2ZHZmOU5CSXhKNmFrT3A5V3ZVdHVpcy9MakxzN2RTa200SnJ4YTdaRTVXNW1xYTZCQ3NDQXBMNHBja0ZEZFpaWlY5aUtxc0MvM1gzS1B3SFRNbFprL2JmU1gxRUN1VXJXWnl3bStvdFZ4bk12ajlnZDltYkRkVUdrRi9sZmZWZG5qZFdsY2QyN3poWEZ5ZkNIUEZ1Q3dEalQ1WU1WSUlMYll0MjBVRVkwOVFjRENURDAydks4OEdLSUpZcWpOTFhibjlUZk1TdXE5VTBLWFFXT0ljMXpzREx5T05EOGhXSm9WQkVKTW10ZGNFb3h6OVpFS2huZUYrcERUdWI3b3lmQVhjRldqV0EyUllJY1JjSUVxajRLWVduRm41UTJiTDMydklSQmE4SG1WTEFSVGJOclVNZTFhdUxlZTZSUjhUNW85a3cyOFRqZGVJd0h4YVArUjROWkwzTnNvY0FJYmpUeTZPbUlYVzh3aG5DRVplcUtpdTE5Z0c2ZzY5YlVVK2N4N29ORWppSUJQK2hpYWF0Wmcrc1NPbHBobXZJcVUxQXd5Y2ZaZWVPaGdVZm1sS1lzQkIwM1lEZUdUaGQ0RVFwUlhUR0lxY0FkcmdxbjhPTEtlYm9rdlB1UmNTdVpxYmJaclR5Q1QvTTdmdkNhV1FNMXVjbjl4YVpBUnFTTm94MUU9PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48L1JTQUtleVZhbHVlPg==";
        private static string privateKey = "NDA5NiE8UlNBS2V5VmFsdWU+PE1vZHVsdXM+eGVJVnJMNlJVTEVzYVZRSWFBekswRzZlVm1OQVhnTHNwVDlKRWNPTWFvYmFEelJQM0Z6a1Bjdlk1dkdDSXFHQlYzVjh5MGUrMloxZ3NFdkx1Y25GNmJaMjlNaGFJZlJPb1M2Z3lQQThVd05vMElqaEVQOEVBUDBPU1c2anJUM2h4RXd1VGNKTVlWS2NqOStiWHM0UWpsQ09JM1YrZzB3UHBtVWc5ZEtsZmE2ZHZmOU5CSXhKNmFrT3A5V3ZVdHVpcy9MakxzN2RTa200SnJ4YTdaRTVXNW1xYTZCQ3NDQXBMNHBja0ZEZFpaWlY5aUtxc0MvM1gzS1B3SFRNbFprL2JmU1gxRUN1VXJXWnl3bStvdFZ4bk12ajlnZDltYkRkVUdrRi9sZmZWZG5qZFdsY2QyN3poWEZ5ZkNIUEZ1Q3dEalQ1WU1WSUlMYll0MjBVRVkwOVFjRENURDAydks4OEdLSUpZcWpOTFhibjlUZk1TdXE5VTBLWFFXT0ljMXpzREx5T05EOGhXSm9WQkVKTW10ZGNFb3h6OVpFS2huZUYrcERUdWI3b3lmQVhjRldqV0EyUllJY1JjSUVxajRLWVduRm41UTJiTDMydklSQmE4SG1WTEFSVGJOclVNZTFhdUxlZTZSUjhUNW85a3cyOFRqZGVJd0h4YVArUjROWkwzTnNvY0FJYmpUeTZPbUlYVzh3aG5DRVplcUtpdTE5Z0c2ZzY5YlVVK2N4N29ORWppSUJQK2hpYWF0Wmcrc1NPbHBobXZJcVUxQXd5Y2ZaZWVPaGdVZm1sS1lzQkIwM1lEZUdUaGQ0RVFwUlhUR0lxY0FkcmdxbjhPTEtlYm9rdlB1UmNTdVpxYmJaclR5Q1QvTTdmdkNhV1FNMXVjbjl4YVpBUnFTTm94MUU9PC9Nb2R1bHVzPjxFeHBvbmVudD5BUUFCPC9FeHBvbmVudD48UD56RmNxNGxpNmppUVludExkMWo0dTBtcHRmR0JkMVJxd3NOVm5hQnVZMlFvVUV4VVBMZGY0a2IzN1NrSUZ1RmxSa054eFBOcERWNUhSUnV6eXZWd3lJcXFxTllZcmRQbmkvR3pqdUxsaXF4OFRnOEFkeVVnYzF2QkVQSGd0N2lLaVlxSXAyT2pYVWZUTlkzVWVzODMrTENIaGxkZWNGcHpOdGh3MjZkeEdqS3VqUThqamFkZWI5T21wMHlELzRDMSsrQUF0K0trdlgvSEcxOFh0aHo4S3R3ZGprRklNSldvYkJZL3FIV3BHV0k4QWhUdjJQOFA1M2xoODZLRTEwdVMxRDFwVG1maldLQXV2empnMkpxMVE5SkdoSytGT0ZSK0tON2tjRHByNGp4SEFpWHVrSDl4TGFURXFTMTRRY3VzZU4rWHVxSGp0QmhFRnBRenFGMlpFb3c9PTwvUD48UT45K2ovOCtDNHY4Vm4xT2M5c2FNeHNsdmVuczJQYjJhb2kvNjhqOXZYSzhTOThYcFUvVWg5cXBpakpxbm81S1UzUWxoSDZucDlDQ3BPY1RqRXR6c3ZBQmRPVUkzbkxWQVU3Sjd2UVZsRGlPYmt5eXFPd21JSGNHMWJSUDJKd3E3ekVESmNUd2M0OW45VEl1ZEk2c2ZJRExoYVBZOVdzQ3V4eGhXM1Faa3NBSHYwRlZFZGtDL2V2VVQrelYxZk9SbTlRMFpGUkVKclNZUElKT3pJNTUzMk5CQUFBemR4WmRJb25QblNqM1kzUytTd0VPRDh4Vmh2Vm9KalVGVklRSGdZMUdXOWtWUlZLNFdoRWQ0dUVkU2tNNVJydnNPMkdCN21LWnZJdUhDQnMwcU1DN3ZtclhJYWNIUVpabkt4emdsNkRzYW11eTEyVW5oMzltSmgwNExQZXc9PTwvUT48RFA+dUphZGxLN1doNFN3QVBrM2tFdkgvQ2hNRXlqeVdvbXJ4M0pmNFRuY3oyTTlVeWVZK0s2NlN0MjEzUlRiR2ZjRnFiaVpTb0ZZaUFpZlpsU3h5c0U0UVB5dW1YOUVUbjFWd1BXVW1OQlBBNG9sRk1VOW94QlRqUTZxZFgyUzhDVzFUeWh5YVJHZFFObkZsK3k2K1c1Q0J4ZnNNbkg4L0toVlpBY0V6Q2xDU2R6YXRXY0ptQmsvTnZURmhvV3UzNUg1dW9wZS9OdklnZ0Zrd0pKNXljejNIdk83eXlseTgzRndTNHYvenVhcitWL2s5blorTW9nVWRmaTFqV2ZqdVVrRkRvTm9OaFVGVUZPT1V0bVFnVXZiYjdNR1J4OVdhOXA3LzI1bW5BTCtPaSt0SUFUaTlDMEtxbEhSaFJmS0FDWEl2SEluTDJWZHFEZEVSQjd3Y0liZnhRPT08L0RQPjxEUT55S2I1b3phYjBmdkJFbjBuWGdPc1VhUXlCMjV4VGNWVlhob01ISlBmRURucFV1MmtwR3hyNmd3bzRUNWNsZE56YW10eHFjTUNOLzJtQjlYZXdqOVF4MDRWd1BWWnl6OHA4R3FqVTgwZjhFcVJuQzhSNVJyYTBwQndjdUtUQWRRQVAzZkRadU45bzJWaXNMbHFOejBFR2VlTWdvN2xweWwyZGRGVnhlNW53dXlRbmtCTklVcmRpV3V5b0IyVzJPeVFWMGNsQVI2Ukg2dGQ3eVVIWnhCZkdieHBoakQxdzJmYUgxUEVyUUVOdUl1Y1JjMFVTZFpPSjBMWmgwZS9kdzY5YWd2KzV3WE8rYTdxc3orVklqRnR4YUczbzJ0Y2t5RlBRUllwMjc2VCs3MjJwRGl0ZUcwR0lVYVcyR2kwcVFMYW0rTUZTREovdGpoelFDUlk4WXpLeVE9PTwvRFE+PEludmVyc2VRPnBXME5Hemp6Z0kvUm10Z1ZGUWdoQkROVWtLQ3E0K2QwZHYrcWtNY2ErcFVvQTZmODFVWnNSQU1uSWJyWXJmK3B0NDRudndBZTdiRENjWFQvUGtBT0R2QVlPYkJSVWRYUlJ0Ty9oTmdOekhnSzdkcVd2WVo3T2NXTHJ3T0JPUUlBUXVFem1WeEdqV0w1RVVxWXcvRXBENjRDTE9nR0xnekZuRmpNZlN6NzRxZkVZOEhmOXdmQit6RlVacFRoVHB6VmpiaVBrWVFIRWFFVDR6eUJnQlU5b0VsZXBOQW5iOUJza1BncmFJcEJZdUdIU0dIV3h4UW5BVVNjQkR3UmdRTnpRT2hzQlI0eHJFRXZHcXNtbzNuZnVuQ3o3M2l4TmdnamxzYUNwSktpTjlGS2hIRHlZZ1JPT3VPWDdiYyt0VEt2R0ZGeU4zZGl1akZrWEZzeW56Wko3QT09PC9JbnZlcnNlUT48RD5UUjVvUGhOaXhLVFgzeEN0MmdjcVA0THMyRlFEOE4rSTVIbStlMXJQYlhDeXFQeEZKMmRZV0RFS2ppNk8vZm9kVXoxcGJqdSt5RWFLT1FyQkVkM3Q4TUI0cWJzdlRVNU9Hb2oyaG5rQnVZUjVvOVBFaXdBbnlrN1U4NDlYNEp0ZVVrY3lRSUc1akxCS2JkUENWSGRRZ25Zb2Z1ODZaK2NMcmxoNU9QUm9KSXdaMDhNY2UrcEVuL2lQeXA3L25mWVUrYlp1RFg3M1JRMW41RWpCMHhZbTJXa1F2ck1OYW1DT1RJWEt2eitVbVJPMjJwNzJFSVlSRFRTemkxaTVwbys3NVpsT1ZwMENreDlOR1pEcHA4SXhiZDhVTDMwSjJRelJvNVhhalI0bjN1aDB1ZnhlMmZqMmxRbjZpU0FQK2lXRTEzU1R2OEFDOVNUdFhzYjI3RVEwRzZ6SDlrYVI3Ymd0SFR2bkQ1eG42VTV3Q2QwcVp1cnpNdVJ0L3hnOXdxVGpkMC9mMGE0S0RpNWtsbis4QWtNWTB5VEwzVHFCYWZzNVlyM1JTdmsxc3ZkYkdMOWVZYnFHY09JczVCQ1RKQXpJQmREVkptR1YxRDNPRkFUaGxuaS9xUDE1REdXcGZPY3NLeUgvZmo2NW9zNitqVzhoZHVwd2x2TDRCZUdHVndEUVR2cDVyNUhoYUx4djJsVXR6MFhRejFEZEptUGdzN0hTQUJLVm5iczY4eGE4SHhYVDQvTlJSR25NNUdNdVBSVjNCaTdjSE5xVWJHY1NMSzA1U2NzY2ZLeWZ4UmovR25yMmVKQ00rTUR0QUVsckJBWUxqRlhSL084dHJ3bi9FbzVMaGIyTVlBVXBKSE5KQjRtYU1rYSszaE5VbU1ZMy9wZTJDUUlsQ05tTkF3VT08L0Q+PC9SU0FLZXlWYWx1ZT4=";

        public static string EncryptText(string text)
        {
            int keySize = 0;
            string publicKeyXml = "";
            getKeyFromEncryptionString(publicKey, out keySize, out publicKeyXml);
            var encrypted = encrypt(Encoding.UTF8.GetBytes(text), keySize, publicKeyXml);
            return Convert.ToBase64String(encrypted);
        }
        public static string DecryptText(string text)
        {
            int keySize = 0;
            string publicAndPrivateKeyXml = "";
            getKeyFromEncryptionString(privateKey, out keySize, out publicAndPrivateKeyXml);
            var decrypted = Decrypt(Convert.FromBase64String(text), keySize, publicAndPrivateKeyXml);
            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] encrypt(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            int maxLength = getMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException(String.Format("Maximum data length is {0}", maxLength), "data");
            if (!isKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, isPadding);
            }
        }

        private static byte[] Decrypt(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (!isKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, isPadding);
            }
        }

        private static int getMaxDataLength(int keySize)
        {
            if (isPadding)
            {
                return ((keySize - 384) / 8) + 7;
            }
            return ((keySize - 384) / 8) + 37;
        }

        private static bool isKeySizeValid(int keySize)
        {
            return keySize >= 384 &&
                    keySize <= 16384 &&
                    keySize % 8 == 0;
        }

        private static string includeKeyInEncryptionString(string publicKey, int keySize)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(keySize.ToString() + "!" + publicKey));
        }

        private static void getKeyFromEncryptionString(string rawkey, out int keySize, out string xmlKey)
        {
            keySize = 0;
            xmlKey = "";

            if (rawkey != null && rawkey.Length > 0)
            {
                byte[] keyBytes = Convert.FromBase64String(rawkey);
                var stringKey = Encoding.UTF8.GetString(keyBytes);

                if (stringKey.Contains("!"))
                {
                    var splittedValues = stringKey.Split(new char[] { '!' }, 2);

                    try
                    {
                        keySize = int.Parse(splittedValues[0]);
                        xmlKey = splittedValues[1];
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public enum RSAKeySize
        {
            Key512 = 512,
            Key1024 = 1024,
            Key2048 = 2048,
            Key4096 = 4096
        }

        public class RSAKeysTypes
        {
            public string PublicKey { get; set; }
            public string PrivateKey { get; set; }
        }

        public class RSACryptographyKeyGenerator
        {
            public RSAKeysTypes GenerateKeys(RSAKeySize rsaKeySize)
            {
                int keySize = (int)rsaKeySize;
                if (keySize % 2 != 0 || keySize < 512)
                    throw new Exception("Key should be multiple of two and greater than 512.");

                var rsaKeysTypes = new RSAKeysTypes();

                using (var provider = new RSACryptoServiceProvider(keySize))
                {
                    var publicKey = provider.ToXmlString(false);
                    var privateKey = provider.ToXmlString(true);

                    var publicKeyWithSize = IncludeKeyInEncryptionString(publicKey, keySize);
                    var privateKeyWithSize = IncludeKeyInEncryptionString(privateKey, keySize);

                    rsaKeysTypes.PublicKey = publicKeyWithSize;
                    rsaKeysTypes.PrivateKey = privateKeyWithSize;
                }

                return rsaKeysTypes;
            }

            private string IncludeKeyInEncryptionString(string publicKey, int keySize)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(keySize.ToString() + "!" + publicKey));
            }
        }

        static void Main(string[] args)
        {
            //RSACryptographyKeyGenerator test = new RSACryptographyKeyGenerator();
            //RSAKeysTypes type = test.GenerateKeys(RSAKeySize.Key4096);

            string text = "BFEBFBFF000906EA8CEC4BB1287D/4Z5P8T2/CNWS2008BQ00QC/!00:80:A3:B2:D9:07!00:80:A3:B2:D9:2B!00:80:A3:B2:D8:A4!!,00:80:A3:B2:D9:07,00:80:A3:B2:D9:2B,00:80:A3:B2:D8:A4,00:80:A3:B2:D8:A5,00:80:A3:B2:D8:A8";
            string encrypted = EncryptText(text);
            Console.WriteLine(encrypted);
            string decrypted = DecryptText(encrypted);
            Console.WriteLine(decrypted);
            Console.Read();
        }
    }
}
