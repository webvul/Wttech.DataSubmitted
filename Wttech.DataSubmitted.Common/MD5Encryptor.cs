using System.Security.Cryptography;
/**
* Copyright (c) 2014,安徽皖通科技股份有限公司 All rights reserved.
* 文件功能描述：这是一个MD5加密算法类文件类文件
* 创建标识：ta0395侯兴鼎20141030
*/
using System.Text;

namespace Wttech.DataSubmitted.Common
{
    /// <summary>
    /// MD5加密算法
    /// </summary>        
    public class MD5Encryptor
    {
        #region 9 Public Methods

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="text">需要加密的字符串</param>
        /// <returns>加密过后的字符串</returns>
        public string Encrypt(string text)
        {
            //1、创建MD5对象
            MD5 md5 = new MD5CryptoServiceProvider();
            //2、将字符编码为一个字节序列
            byte[] data = System.Text.Encoding.Default.GetBytes(text);
            //3、计算data字节数组的哈希值 
            byte[] md5data = md5.ComputeHash(data);
            //4、释放MD5对象中的所有资源
            md5.Clear();
            //5、创建字符存储器并初始化
            StringBuilder str = new StringBuilder();
            str.Append(string.Empty);
            //6、存储加密字符
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                str.Append(md5data[i].ToString("x").PadLeft(2, '0'));
            }
            //7、返回经过MD5加密过后的字符串
            return str.ToString();
        }

        #endregion
    }
}