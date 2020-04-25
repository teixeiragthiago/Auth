using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Auth.SharedKernel
{
    public class Encryption
    {
        private const string AllowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789abcdefghijkmnopqrstuvwxyz!@$?-";
        private const string Key = "CNS8U3RS99VC4ZLYUXAKHLO4YJQZ1ZGZWYOPQRSZGQHJKAZXQRYHASGHW";

        public static string CreateChar(string input)
        {
            var finalPassWord = string.Empty;
            for (var i = 0; i <= input.Length - 1; i++)
            {
                var index = AllowedChars.IndexOf(input[i]);
                index += 10;
                if (index > AllowedChars.Length - 1)
                    index = index - (AllowedChars.Length - 1);

                finalPassWord += AllowedChars[index];
            }

            return finalPassWord;
        }

        public static string TrueCharValue(string input)
        {
            var originalPassWord = string.Empty;
            for (var i = 0; i <= input.Length - 1; i++)
            {
                var index = AllowedChars.IndexOf(input[i]);
                index -= 10;
                if (index - 1 < 0)
                    index = (AllowedChars.Length) + (index - 1);

                originalPassWord += AllowedChars[index];
            }

            return originalPassWord;
        }

        public static string CreateDefaultPassWord(int stringLength)
        {
            var rd = new Random();
            var chars = new char[stringLength];
            for (var i = 0; i < stringLength; i++)
                chars[i] = AllowedChars[rd.Next(0, AllowedChars.Length)];

            return new string(chars);
        }

        public static string Encrypt(string input)
        {
            var bytesBuff = Encoding.Unicode.GetBytes(input);
            using (var aes = Aes.Create())
            {
                using (var crypto = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (var mStream = new MemoryStream())
                    {
                        using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Dispose();
                        }
                        input = Convert.ToBase64String(mStream.ToArray());
                    }
                }
            }
            return input;
        }

        public static string Decrypt(string cryptoInput)
        {
            var bytesBuff = Convert.FromBase64String(cryptoInput);
            using (var aes = Aes.Create())
            {
                using (var crypto = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    aes.Key = crypto.GetBytes(32);
                    aes.IV = crypto.GetBytes(16);
                    using (var mStream = new MemoryStream())
                    {
                        using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            cStream.Dispose();
                        }
                        cryptoInput = Encoding.Unicode.GetString(mStream.ToArray());
                    }
                }

            }
            return cryptoInput;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("PassWord informado é inválido!");

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (storedHash.Length != 64)
                return false;
            if (storedSalt.Length != 128)
                return false;

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.Where((t, i) => t != storedHash[i]).Any())
                    return false;
            }

            return true;
        }
    }
}