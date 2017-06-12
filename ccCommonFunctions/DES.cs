/*
 * 2017年6月4日 12:48:48 郑少宝 DES 加解密
 * 
 * 你有没有那么一刻
 * 抱着某人
 * 感觉就像拥有了整个世界
 */
using System;
using System.Security.Cryptography;
using System.Text;

namespace ZrTBMCodeForTestItem.ccCommonFunctions
{
	public class DES
	{
		/// <summary>
		/// DES 加解密
		/// </summary>
		public DES()
		{

		}
		/// <summary>
		/// DES 加解密
		/// </summary>
		/// <param name="key">密钥</param>
		public DES(string key)
		{
			this.Key = key;
		}

		/// <summary>
		/// 密钥 必须为8位
		/// </summary>
		public string Key { get; set; }
		private string _key
		{
			get
			{
				if (string.IsNullOrEmpty(this.Key) || this.Key.Length < 9)
				{
					return "13579ABc";
				}
				return this.Key;
			}
		}

		/// <summary>
		/// 进行 DES 加密
		/// </summary>
		/// <param name="text">要加密的字符串。</param>
		/// <returns>以Base64格式返回的加密字符串。</returns>
		public string Encrypt(string text)
		{
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
				des.Key = ASCIIEncoding.ASCII.GetBytes(this._key);
				des.IV = ASCIIEncoding.ASCII.GetBytes(this._key);
				using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(inputByteArray, 0, inputByteArray.Length);
						cs.FlushFinalBlock();
						return Convert.ToBase64String(ms.ToArray());
					}
				}				
			}
		}


		/// <summary>
		/// 进行 DES 解密。
		/// </summary>
		/// <param name="text">要解密的以Base64</param>
		/// <returns>已解密的字符串。</returns>
		public string Decrypt(string text)
		{
			byte[] inputByteArray = Convert.FromBase64String(text);
			using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
			{
				des.Key = ASCIIEncoding.ASCII.GetBytes(this._key);
				des.IV = ASCIIEncoding.ASCII.GetBytes(this._key);
				using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(inputByteArray, 0, inputByteArray.Length);
						cs.FlushFinalBlock();
						return Encoding.UTF8.GetString(ms.ToArray());
					}
				}		
			}
		}
	}
}
