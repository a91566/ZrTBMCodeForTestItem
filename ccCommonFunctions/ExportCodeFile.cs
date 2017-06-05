/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 如果有一天我们变陌生了，那么我就重新认识你。
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCommonFunctions
{
	/// <summary>
	/// 导出文件
	/// </summary>
	public class ExportCodeFile
	{
		/// <summary>
		/// 程序路径
		/// </summary>
		private readonly string path;
		/// <summary>
		/// 中润编码
		/// </summary>
		private readonly string zrCode;
		/// <summary>
		/// 类库名称
		/// </summary>
		private readonly string assemblyName;
		/// <summary>
		/// 命名空间
		/// </summary>
		private readonly string rootNamespace;
		/// <summary>
		/// Guid
		/// </summary>
		private readonly string assemblyGuid;
		/// <summary>
		/// 模版的应用程序集
		/// </summary>
		private const string sourceAssembly = "CHK_Accelerator";

		/// <summary>
		/// 导出文件类
		/// </summary>
		/// <param name="zrCode">中润编码</param>
		/// <param name="assemblyName">程序名称</param>
		/// <param name="rootNamespace">命名空间</param>
		/// <param name="path">生成的路径</param>
		public ExportCodeFile(string zrCode, string assemblyName, string rootNamespace, string path)
		{
			this.zrCode = zrCode;
			this.assemblyName = assemblyName;
			this.rootNamespace = rootNamespace;
			this.path = path;
			this.assemblyGuid = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// 创建所需的目录
		/// </summary>
		/// <returns></returns>
		public bool InitFolder()
		{
			try
			{
				string caFilePath = $@"{this.path}\{this.assemblyName}";
				if (Directory.Exists(caFilePath)) Directory.Delete(caFilePath, true);
				List<string> list = new List<string>()
				{
					$@"{caFilePath}",
					$@"{caFilePath}\bin\Debug",
					$@"{caFilePath}\obj\Debug\TempPE",
					$@"{caFilePath}\Properties",
				};
				foreach (var item in list)
				{
					if (!Directory.Exists(item))
					{
						Directory.CreateDirectory(item);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// 导出工程文件
		/// </summary>
		public void Export_csproj()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}.csproj";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains("ProjectGuid"))
				{
					this.replaceGuid(ref words[i]);
				}
				if (words[i].Contains(sourceAssembly) && words[i].Contains("RootNamespace"))
				{
					words[i] = words[i].Replace(sourceAssembly, this.rootNamespace);
				}
				if (words[i].Contains(sourceAssembly) && words[i].Contains("AssemblyName"))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
					//最后一个需要替换的，
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\{this.assemblyName}.csproj";
			Function.WriteFile(words, targetPath);
		}

		/// <summary>
		/// 导出类文件
		/// </summary>
		public void Export_CheckClassControl()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\CheckClassControl.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
				}
				if (words[i].Contains("public string ZrCode"))
				{
					this.replaceZrCode(ref words[i]);//
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\CheckClassControl.cs";
			Function.WriteFile(words, targetPath);
		}

		/// <summary>
		/// 导出 AssemblyInfo 文件
		/// </summary>
		public void Export_AssemblyInfo()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\AssemblyInfo.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
				}
				if (words[i].Contains("Microsoft"))
				{
					words[i] = words[i].Replace("Microsoft", "中润");
				}
				if (words[i].Contains("Microsoft 2016"))
				{
					words[i] = words[i].Replace("Microsoft 2016", "中润 2017");
				}
				if (words[i].Contains("Guid"))
				{
					this.replaceGuid(ref words[i]);
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\Properties\AssemblyInfo.cs";
			Function.WriteFile(words, targetPath, true);
		}

		/// <summary>
		/// 导出 AssemblyInfo 文件，用模版替换的方法，不是推荐，因为源项目修改的话，还要把文件改成模版的样子
		/// </summary>
		public void Export_AssemblyInfo_Masterplate()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\AssemblyInfo.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains("#AssemblyName#"))
				{
					words[i] = words[i].Replace("#AssemblyName#", this.assemblyName);
				}
				if (words[i].Contains("#AssemblyCompany#"))
				{
					words[i] = words[i].Replace("#AssemblyCompany#", "中润");
				}
				if (words[i].Contains("#AssemblyCopyright#"))
				{
					words[i] = words[i].Replace("#AssemblyCopyright#", "中润 2017");
				}
				if (words[i].Contains("#Guid#"))
				{
					words[i] = words[i].Replace("#Guid#", this.assemblyGuid);
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\Properties\AssemblyInfo.cs";
			Function.WriteFile(words, targetPath,true);
		}

		/// <summary>
		/// 替换新的 Guid
		/// </summary>
		/// <param name="text">待替换的文本</param>
		private void replaceGuid(ref string text)
		{
			string patten = @"[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}";
			Regex regex = new Regex(patten, RegexOptions.IgnoreCase);
			Match match = regex.Match(text);
			if (match.Success)
			{
				text = text.Replace(match.Value, this.assemblyGuid);
			}
		}

		/// <summary>
		/// 替换新的 ZrCode
		/// </summary>
		/// <param name="text">待替换的文本</param>
		private void replaceZrCode(ref string text)
		{
			string patten = @"A[0-9]{4}";
			Regex regex = new Regex(patten, RegexOptions.IgnoreCase);
			Match match = regex.Match(text);
			if (match.Success)
			{
				text = text.Replace(match.Value, this.zrCode);
			}
		}

		#region 收样信息
		/// <summary>
		/// 导出收样的设计文件
		/// </summary>
		public void Export_Trust()
		{
			this.Export_Trust_CS();
			this.Export_Trust_Designer();
			this.Export_Trust_Resx();
		}
		/// <summary>
		/// 导出收样的设计文件 后台代码
		/// </summary>
		private void Export_Trust_CS()
		{

		}
		/// <summary>
		/// 导出收样的设计文件 窗体设计
		/// </summary>
		private void Export_Trust_Designer()
		{

		}
		/// <summary>
		/// 导出收样的设计文件 资源文件
		/// </summary>
		private void Export_Trust_Resx()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\UcSampleInfo.resx";
			string targetPath = $@"{this.path}\{this.assemblyName}\UcSampleInfo.resx";
			File.Copy(sourcePath, targetPath, true);
		}
		#endregion

		#region 试验信息
		/// <summary>
		/// 导出试验的设计文件
		/// </summary>
		public void Export_Trial()
		{
			this.Export_Trial_CS();
			this.Export_Trial_Designer();
			this.Export_Trial_Resx();
		}
		/// <summary>
		/// 导出试验的设计文件 后台代码
		/// </summary>
		private void Export_Trial_CS()
		{

		}
		/// <summary>
		/// 导出试验的设计文件 窗体设计
		/// </summary>
		private void Export_Trial_Designer()
		{

		}
		/// <summary>
		/// 导出试验的设计文件 资源文件
		/// </summary>
		private void Export_Trial_Resx()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\UcProcessInfo.resx";
			string targetPath = $@"{this.path}\{this.assemblyName}\UcProcessInfo.resx";
			File.Copy(sourcePath, targetPath, true);
		}
		#endregion
	}
}
