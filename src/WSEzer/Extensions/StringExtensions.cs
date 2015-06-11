using System;
using System.Security.Cryptography;
using System.Text;

namespace WSEzer
{
    public static class StringExtensions
    {
        public static string ToBase64String(this string @string)
        {
            var stringBytes = Encoding.UTF8.GetBytes(@string);
            return stringBytes.ToBase64String();
        }

        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] ToSha1(this string input)
        {
            return new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}