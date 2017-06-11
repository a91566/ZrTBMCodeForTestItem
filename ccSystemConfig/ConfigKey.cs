using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZrTBMCodeForTestItem.ccSystemConfig
{
	public enum ConfigKey
	{
		/// <summary>
		/// 主机名
		/// </summary>
		DB_Host,
		/// <summary>
		/// 数据库名称
		/// </summary>
		DB_Name,
		/// <summary>
		/// 数据库用户
		/// </summary>
		DB_User,
		/// <summary>
		/// 数据库密码
		/// </summary>
		DB_PWD,
		/// <summary>
		/// 数据库端口
		/// </summary>
		DB_port,
		/// <summary>
		/// 目标 .Net 环境版本
		/// </summary>
		TargetFrameworkVersion,
		/// <summary>
		/// 生成的位置
		/// </summary>
		Folder,
		/// <summary>
		/// 开始行号
		/// </summary>
		TableHeaderRowIndex,
		/// <summary>
		/// 行高
		/// </summary>
		RowHeight,
		/// <summary>
		/// 限宽
		/// </summary>
		MaxWidth,
		/// <summary>
		/// 退出的颜色
		/// </summary>
		ExitColor,
		/// <summary>
		/// Excel 列宽 与 像素的比例转换
		/// </summary>
		ExcelWithToPxScale
	}
}
