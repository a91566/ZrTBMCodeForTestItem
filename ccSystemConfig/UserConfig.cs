/*
 * 2017年6月4日 16:50:00 郑少宝 用户配置
 * 
 * 跟我走吧。
 * 不愿意吗？
 * 不愿意的话，那就让我跟你走吧。
 */
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using zsbApps.DBHelper;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace ZrTBMCodeForTestItem.ccSystemConfig
{
	public class UserConfig
	{
		/// <summary>
		/// 数据处理类
		/// </summary>
		private DBHelper dbhelper;
		/// <summary>
		/// 配置的信息
		/// </summary>
		private Dictionary<string, string> dictConfig;
		/// <summary>
		/// 用户配置
		/// </summary>
		/// <param name="isSave">保存操作</param>
		public UserConfig(bool isSave)
		{
			string path = $@"{Application.StartupPath}\config.sqlite3";
			if (System.IO.File.Exists(path))
			{
				string linkpath = $@"Data Source={path}";
				this.dbhelper = DBHelper.Instance(DBClassify.SQLite, linkpath);
				if (isSave) return;
				this.init();
			}
			else
			{
				ccCommonFunctions.Function.MsgError("配置文件不存在。");
			}
		}
		
		/// <summary>
		/// 初始化查询出所有的配置
		/// </summary>
		private void init()
		{
			DataTable dt = this.dbhelper.QueryTable("SELECT ccKey,ccValue FROM T_SYS_Config WHERE IsDel=0;");
			this.dictConfig = dt.Rows.Cast<DataRow>().ToDictionary(x => x[0].ToString(), x => x[1].ToString()); 
		}

		/// <summary>
		/// 获取配置
		/// </summary>
		/// <param name="key">主键</param>
		/// <returns></returns>
		public string GetConfig(string key)
		{
			if (this.dictConfig == null) return "";
			return this.dictConfig.Where(i => i.Key == key).Single().Value;
		}


		/// <summary>
		/// 获取配置
		/// </summary>
		/// <param name="key">主键</param>
		/// <returns></returns>
		public string GetConfig(ConfigKey key)
		{			
			return this.GetConfig(key.ToString());
		}

		/// <summary>
		/// 更新默认值
		/// </summary>
		/// <param name="dict">默认值字典数据</param>
		/// <returns></returns>
		public int UpdateConfig(Dictionary<string, string> dict)
		{
			List<string> list = new List<string>();
			foreach (var item in dict)
			{
				list.Add($"UPDATE T_SYS_Config SET ccValue='{item.Value}' WHERE ccKey='{item.Key}' AND IsDel='0'");
			}
			return this.dbhelper.ExecuteSql(list);
		}

		/// <summary>
		/// 获取数据库连接
		/// </summary>
		/// <returns></returns>
		public string GetDBLinkString()
		{
			ccCommonFunctions.DES des = new ccCommonFunctions.DES();
			return $"server={des.Decrypt(GetConfig(ConfigKey.DB_Host))};database={des.Decrypt(GetConfig(ConfigKey.DB_Name))};" +
				$"uid={des.Decrypt(GetConfig(ConfigKey.DB_User))};pwd={des.Decrypt(GetConfig(ConfigKey.DB_PWD))}";
		}
	}
}
