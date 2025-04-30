using System.Security.Cryptography;
using System.Text;

namespace Barnamenevisan.Core.Extensions;

public static class StringExtensions
{
    public static string EncodeWithMD5(this string str)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            byte[] hash = md5.ComputeHash(inputBytes);
            string result = BitConverter.ToString(hash);
            return result;
        }
    }
}