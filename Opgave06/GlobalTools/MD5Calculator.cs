using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace GlobalTools
{
    public class MD5Calculator 
    {

        //
        //  source: https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k%28System.Security.Cryptography.MD5%29%3Bk%28TargetFrameworkMoniker-.NETFramework,Version%3Dv4.6%29%3Bk%28DevLang-csharp%29&rd=true
        //
        public static string GetHash(string text)
        {
            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
