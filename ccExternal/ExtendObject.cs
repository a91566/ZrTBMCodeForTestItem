/*
 * 2017年6月14日 11:25:21 郑少宝 对象扩展类
 * 
 * 偏偏，偏偏
 * 你已在岸上走
 * 我仍旧立在水中候
 */
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZrTBMCodeForTestItem.ccExtend
{
	public static class ExtendObject
	{
		/// <summary>
		/// 深复制
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static object Clone(this object obj)
		{
			using (Stream objectStream = new MemoryStream())
			{
				//利用 Serialization 序列化与反序列化完成引用对象的复制, 因此 obj 需要  Serializable 特性
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(objectStream, obj);
				objectStream.Seek(0, SeekOrigin.Begin);
				return formatter.Deserialize(objectStream);
			}
		}
	}
}
