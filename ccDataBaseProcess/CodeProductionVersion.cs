/*
 * 2017年6月16日 14:38:33 郑少宝 版本信息
 * 
 * 本来想摘颗星星给你
 * 但是想想还是算了
 * 我够得着星星
 * 够不着你
 */

namespace ccZrCHKCodeProduction.ccDataBaseProcess
{
	public class CodeProductionVersion
	{
		/// <summary>
		/// 连接字符串
		/// </summary>
		public readonly string connstring;

		/// <summary>
		/// 连接字符串
		/// </summary>
		/// <param name="connstring"></param>
		public CodeProductionVersion(string connstring)
		{
			this.connstring = connstring;
		}

		/// <summary>
		/// 更新最新版本
		/// </summary>
		/// <param name="version">写入的版本</param>
		/// <param name="isAdd">是否新增</param>
		/// <returns></returns>
		public bool Update(string version, bool isAdd)
		{
			zsbApps.DBHelper.DBHelper db = zsbApps.DBHelper.DBHelper.Instance(zsbApps.DBHelper.DBClassify.MsSql, this.connstring);
			string exsql;
			if (isAdd)
			{
				exsql = $"INSERT INTO T_SYS_SystemConfig(ConfigKey,ConfigValue)VALUES('CodeProductionVersion','{version}')";
			}
			else
			{
				exsql = $"UPDATE T_SYS_SystemConfig SET ConfigValue='{version}' WHERE ConfigKey='CodeProductionVersion'";
			}
			return db.ExecuteSql(exsql) == 1;
		}

		/// <summary>
		/// 获取最新版本
		/// </summary>
		/// <returns>返回一个对象，如果为 null, 说明数据库没有这个配置，需要新增一条</returns>
		public object GetVersion()
		{
			zsbApps.DBHelper.DBHelper db = zsbApps.DBHelper.DBHelper.Instance(zsbApps.DBHelper.DBClassify.MsSql, this.connstring);
			string strsql = $"SELECT ConfigValue FROM T_SYS_SystemConfig WHERE ConfigKey='CodeProductionVersion'";
			return db.GetSingle(strsql);
		}
	}
}
