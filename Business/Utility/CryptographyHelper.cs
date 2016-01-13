using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Security; 

using System.Security.Cryptography;

namespace SkillBank.Site.Services.Utility
{
    public static class CryptographyHelper
    {


        #region Encrypt
        //public static string EncryptByMD5(string input)
        //{
        //    MD5 md5Hasher = MD5.Create();
        //    byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

        //    StringBuilder sBuilder = new StringBuilder();
        //    //将每个字节转为16进制
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }

        //    return sBuilder.ToString();  
        //}

        public static string EncryptByMD5(string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5"); ; 
        }

        public static string EncryptBySHA1(string source)
        {
            //SHA1 sha = new SHA1CryptoServiceProvider();
            //byte[] bytes = Encoding.Unicode.GetBytes(input);
            //byte[] result = sha.ComputeHash(bytes);
            //return BitConverter.ToString(result);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
        }

        #endregion


        #region CheckEncrypt

        public static bool CheckEncryptByMD5(string source, string output)
        {
            return output.Equals(EncryptByMD5(source));
        }

        public static bool CheckEncryptBySHA1(string source, string output)
        {
            return output.Equals(EncryptBySHA1(source));
        }

        #endregion

    }
}
