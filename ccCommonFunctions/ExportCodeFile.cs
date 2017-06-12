/*
 * 2017年6月2日 09:59:02 郑少宝 导出文件
 * 
 * 如果有一天我们变陌生了，那么我就重新认识你。
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccLanguage;

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
		/// 显示信息委托
		/// </summary>
		/// <param name="operateInfo">输出信息</param>
		public delegate void ShowOperateInfoHandle(string operateInfo);
		/// <summary>
		/// 显示信息事件
		/// </summary>
		public event ShowOperateInfoHandle ShowOperateInfoEvent;

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
		/// 输出状态嘻嘻你
		/// </summary>
		/// <param name="text"></param>
		private void outPut(string text)
		{
			if (this.ShowOperateInfoEvent == null) return;
			Application.DoEvents();
			ShowOperateInfoEvent(text);
		}

		/// <summary>
		/// 创建所需的目录
		/// </summary>
		/// <returns></returns>
		public bool InitFolder()
		{
			try
			{
				outPut(Language.OutputInfo_CreateFolder);
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
			outPut(Language.OutputInfo_CreateCsproj);
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\{sourceAssembly}.csproj";
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
			outPut(Language.OutputInfo_CreateControlClass);
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\CheckClassControl.cs";
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
			outPut(Language.OutputInfo_CreateAssembly);
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\Properties\AssemblyInfo.cs";
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
			outPut(Language.OutputInfo_CreateAssembly);
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
		/// <param name="panModel">控件列表大小等属性</param>
		public void Export_Trust(Control panModel)
		{
			Size ucSize = this.getUCSize(panModel);

			outPut(Language.OutputInfo_CreateTrust);
			this.export_Trust_CS();

			outPut(Language.OutputInfo_CreateTrustDesigner);
			this.export_Trust_Designer(panModel, ucSize);

			outPut(Language.OutputInfo_CreateTrustResx);
			this.export_Trust_Resx();
		}

		/// <summary>
		/// 导出收样的设计文件 后台代码
		/// </summary>
		private void export_Trust_CS()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcSampleInfo.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\UcSampleInfo.cs";
			Function.WriteFile(words, targetPath, true);
		}

		/// <summary>
		/// 导出收样的设计文件 窗体设计
		/// </summary>
		/// <param name="panModel">控件列表大小等属性</param>
		private void export_Trust_Designer(Control panModel, Size ucSize)
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcSampleInfo.Designer.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
				}
				if (words[i].Contains("InitializeComponent"))
				{
					operateDesigner_Trust(words, i, panModel, ucSize);
					break;
				}
			}
		}

		/// <summary>
		/// 导出收样的设计文件 资源文件
		/// </summary>
		private void export_Trust_Resx()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcSampleInfo.resx";
			string targetPath = $@"{this.path}\{this.assemblyName}\UcSampleInfo.resx";
			File.Copy(sourcePath, targetPath, true);
		}

		/// <summary>
		/// 处理设计文件
		/// </summary>
		/// <param name="words">原内容</param>
		/// <param name="index">所处位置</param>
		/// <param name="panModel">控件列表大小等属性</param>
		private void operateDesigner_Trust(string[] words, int index, Control panModel, Size ucSize)
		{
			string[] temp = new string[index + 3];
			Array.Copy(words, temp, temp.Length);
			List<string> list = temp.ToList();
			//生成控件的语句
			var listNewControl = new List<string>();
			///控件属性的语句
			var listProperty = new List<string>();
			///控件属性的语句
			var listControlsAdd = new List<string>();
			///控件定义的语句
			var listDefine = new List<string>();

			this.addControlPropertyDefineInfo_Trust(panModel, ref listNewControl, ref listProperty, ref listControlsAdd, ref listDefine);
			this.operateControlPropertyDefineInfo_Trust(ref listNewControl, ref listProperty, ref listControlsAdd, ref listDefine, ucSize);
			
			list.AddRange(listNewControl);
			list.AddRange(listProperty);
			list.AddRange(listControlsAdd);
			list.AddRange(listDefine);
			list.Add("	}");
			list.Add("}");
			string targetPath = $@"{this.path}\{this.assemblyName}\UcSampleInfo.Designer.cs";
			Function.WriteFile(list, targetPath, true);
		}

		/// <summary>
		/// 添加控件的定义及属性信息
		/// 如：private System.Windows.Forms.TabControl tcTest;
		/// </summary>
		/// <param name="pan">控件容器</param>
		/// <param name="listNewControl">new 语句</param>
		/// <param name="listProperty">属性语句</param>
		/// <param name="listDefine">定义语句</param>
		private void addControlPropertyDefineInfo_Trust(Control pan, ref List<string> listNewControl, ref List<string> listProperty, ref List<string> listControlsAdd, ref List<string> listDefine)
		{
			foreach (Control item in pan.Controls)
			{
				listNewControl.Add(this.getControlNew(item));
				listProperty.AddRange(this.getControlProperty(item));
				listControlsAdd.Add(this.getControlAddToParent_Trust(item));
				listDefine.Add(this.getControlDefine(item));
				if (item.Controls.Count > 0)
				{
					addControlPropertyDefineInfo_Trust(item, ref listNewControl, ref listProperty, ref listControlsAdd, ref listDefine);
				}
			}
		}	

		/// <summary>
		/// 获取创建控件的语句
		/// </summary>
		/// <param name="c">控件</param>
		/// <param name="topParent">顶级控件</param>
		/// <returns></returns>
		private string getControlAddToParent_Trust(Control c)
		{
			return $"			this.Controls.Add(this.{c.Name});";
		}

		/// <summary>
		/// 信息再处理
		/// </summary>
		/// <param name="listNewControl">new 语句</param>
		/// <param name="listProperty">属性语句</param>
		/// <param name="listDefine">定义语句</param>
		private void operateControlPropertyDefineInfo_Trust(ref List<string> listNewControl, ref List<string> listProperty,
			ref List<string> listControlsAdd, ref List<string> listDefine, Size ucSize)
		{
			//使用 Tab 制表符
			listNewControl.Add("			this.SuspendLayout()");


			listControlsAdd.Insert(0, $"			this.AutoScroll = true;");
			listControlsAdd.Insert(0, $"			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
			listControlsAdd.Insert(0, $"			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);");
			listControlsAdd.Insert(0, "");
			listControlsAdd.Add(@"			this.Name = ""UcSampleInfo"";");
			listControlsAdd.Add($"			this.Size = new System.Drawing.Size({ucSize.Width}, {ucSize.Height});");
			listControlsAdd.Add("			this.ResumeLayout(false);");
			listControlsAdd.Add("			this.PerformLayout();");
			listControlsAdd.Add("		}");
			listControlsAdd.Add("");
			listControlsAdd.Add("		#endregion");
			listControlsAdd.Add("");
		}

		#endregion

		#region 试验信息
		/// <summary>
		/// 导出试验的设计文件
		/// </summary>
		/// <param name="panModel">控件列表大小等属性</param>
		public void Export_Trial(Control panModel)
		{
			Size ucSize = this.getUCSize(panModel);

			outPut(Language.OutputInfo_CreateTrial);
			this.export_Trial_CS();

			outPut(Language.OutputInfo_CreateTrial);
			this.export_Trial_Designer(panModel, ucSize);

			outPut(Language.OutputInfo_CreateTrial);
			this.export_Trial_Resx();

			outPut(Language.OutputInfo_Done);
		}
		/// <summary>
		/// 导出试验的设计文件 后台代码
		/// </summary>
		private void export_Trial_CS()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcProcessInfo.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
					break;
				}
			}
			string targetPath = $@"{this.path}\{this.assemblyName}\UcProcessInfo.cs";
			Function.WriteFile(words, targetPath, true);
		}

		/// <summary>
		/// 导出试验的设计文件 窗体设计
		/// </summary>
		/// <param name="panModel">控件列表大小等属性</param>
		private void export_Trial_Designer(Control panModel, Size ucSize)
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcProcessInfo.Designer.cs";
			string[] words = Function.ReadFile(sourcePath);
			for (int i = 0; i < words.Length; i++)
			{
				if (words[i].Contains(sourceAssembly))
				{
					words[i] = words[i].Replace(sourceAssembly, this.assemblyName);
				}
				if (words[i].Contains("InitializeComponent"))
				{
					operateDesigner_Trial(words, i, panModel, ucSize);
					break;
				}
			}
		}
		/// <summary>
		/// 导出试验的设计文件 资源文件
		/// </summary>
		private void export_Trial_Resx()
		{
			string sourcePath = $@"{Application.StartupPath}\masterplate\{sourceAssembly}\UcProcessInfo.resx";
			string targetPath = $@"{this.path}\{this.assemblyName}\UcProcessInfo.resx";
			File.Copy(sourcePath, targetPath, true);
		}

		/// <summary>
		/// 处理设计文件
		/// </summary>
		/// <param name="words">原内容</param>
		/// <param name="index">所处位置</param>
		/// <param name="panModel">控件列表大小等属性</param>
		private void operateDesigner_Trial(string[] words, int index, Control panModel, Size ucSize)
		{
			string[] temp = new string[index + 3];
			Array.Copy(words, temp, temp.Length);
			List<string> list = temp.ToList();
			//生成控件的语句
			var listNewControl = new List<string>();
			///控件属性的语句
			var listProperty = new List<string>();
			///控件属性的语句
			var listControlsAdd = new List<string>();
			///控件定义的语句
			var listDefine = new List<string>();
			///舒心控件语句， tabcontrol,tabpage 需要
			var listSuspendLayout = new List<string>();

			this.addControlPropertyDefineInfo_Trial(panModel.Name, panModel, ref listNewControl, ref listProperty, ref listControlsAdd, 
				ref listDefine, ref listSuspendLayout);
			this.operateControlPropertyDefineInfo_Trial(ref listNewControl, ref listProperty, ref listControlsAdd, ref listDefine, 
				ref listSuspendLayout, ucSize);

			list.AddRange(listNewControl);
			list.AddRange(listSuspendLayout);
			list.AddRange(listProperty);
			list.AddRange(listControlsAdd);
			list.AddRange(listDefine);
			list.Add("	}");
			list.Add("}");
			string targetPath = $@"{this.path}\{this.assemblyName}\UcProcessInfo.Designer.cs";
			Function.WriteFile(list, targetPath, true);
		}

		/// <summary>
		/// 添加控件的定义及属性信息
		/// 如：private System.Windows.Forms.TabControl tcTest;
		/// </summary>
		/// <param name="pan">控件容器</param>
		/// <param name="listNewControl">new 语句</param>
		/// <param name="listProperty">属性语句</param>
		/// <param name="listDefine">定义语句</param>
		private void addControlPropertyDefineInfo_Trial(string topParentName, Control pan, ref List<string> listNewControl, 
			ref List<string> listProperty, ref List<string> listControlsAdd, ref List<string> listDefine,
			ref List<string> listSuspendLayout)
		{
			foreach (Control item in pan.Controls)
			{
				listNewControl.Add(this.getControlNew(item));
				listProperty.AddRange(this.getControlProperty(item));
				listControlsAdd.Add(this.getControlAddToParent_Trial(item, topParentName));
				listDefine.Add(this.getControlDefine(item));
				if (item is TabControl || item is TabPage)
				{
					listSuspendLayout.Add($"            this.{item.Name}.SuspendLayout();");
				}
				if (item.Controls.Count > 0)
				{
					addControlPropertyDefineInfo_Trial(topParentName, item, ref listNewControl, ref listProperty, ref listControlsAdd, 
						ref listDefine, ref listSuspendLayout);
				}
			}
		}

		/// <summary>
		/// 获取创建控件的语句
		/// </summary>
		/// <param name="c">控件</param>
		/// <param name="topParent">顶级控件</param>
		/// <returns></returns>
		private string getControlAddToParent_Trial(Control c, string topParentName)
		{
			if (c.Parent.Name == topParentName)
			{
				return $"			this.Controls.Add(this.{c.Name});";
			}
			else
			{
				return $"			this.{c.Parent.Name}.Controls.Add(this.{c.Name});";
			}
		}

		/// <summary>
		/// 信息再处理
		/// </summary>
		/// <param name="listNewControl">new 语句</param>
		/// <param name="listProperty">属性语句</param>
		/// <param name="listDefine">定义语句</param>
		private void operateControlPropertyDefineInfo_Trial(ref List<string> listNewControl, ref List<string> listProperty,
			ref List<string> listControlsAdd, ref List<string> listDefine, ref List<string> listSuspendLayout, Size ucSize)
		{
			//使用 Tab 制表符
			listSuspendLayout.Add("			this.SuspendLayout();");


			listControlsAdd.Insert(0, $"			this.AutoScroll = true;");
			listControlsAdd.Insert(0, $"			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");
			listControlsAdd.Insert(0, $"			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);");
			listControlsAdd.Insert(0, "");
			listControlsAdd.Add(@"			this.Name = ""UcSampleInfo"";");
			listControlsAdd.Add($"			this.Size = new System.Drawing.Size({ucSize.Width}, {ucSize.Height});");
			listControlsAdd.Add("			this.ResumeLayout(false);");
			listControlsAdd.Add("			this.PerformLayout();");
			listControlsAdd.Add("		}");
			listControlsAdd.Add("");
			listControlsAdd.Add("		#endregion");
			listControlsAdd.Add("");
		}

		#endregion

		/// <summary>
		/// 获取控件属性的语句
		/// </summary>
		/// <param name="c">控件</param>
		/// <returns></returns>
		private List<string> getControlProperty(Control c)
		{
			var result = new List<string>();
			result.Add($"			//");
			result.Add($"			// {c.Name}");
			result.Add($"			//");
			result.Add($@"			this.{c.Name}.Name = ""{c.Name}""");
			result.Add($@"			this.{c.Name}.Location = new System.Drawing.Point({c.Location.X}, {c.Location.Y});");
			result.Add($@"			this.{c.Name}.Size = new System.Drawing.Size({c.Size.Width}, {c.Size.Height});");
			result.AddRange(this.getControlPropertySpecial(c));
			//switch (c)
			//{
			//	case Label lbl:
			//		result.Add($@"			this.{c.Name}.AutoSize = ""{lbl.AutoSize.ToString()}""");
			//		break;
			//	case ZrControl.ZrDynamicTextBox txb:
			//		result.Add($@"			this.{c.Name}.ZrDescription = ""{txb.ZrDescription}""");
			//		break;
			//}
			return result;
		}

		/// <summary>
		/// 获取创建控件的语句
		/// </summary>
		/// <param name="c">控件</param>
		/// <returns></returns>
		private string getControlNew(Control c)
		{
			if (c is Label || c is TabControl || c is TabPage)
			{
				return $"			this.{c.Name} = new {c.GetType().FullName}();";
			}
			else
			{
				return $"			this.{c.Name} = new {c.GetType().FullName}(this.components);";
			}
		}

		/// <summary>
		/// 获取创建控件的语句
		/// </summary>
		/// <param name="c">控件</param>
		/// <returns></returns>
		private string getControlDefine(Control c)
		{
			return $"		private {c.GetType().FullName} {c.Name};";
		}

		/// <summary>
		/// 获取用户控件的宽度
		/// </summary>
		/// <param name="pan">容器</param>
		private Size getUCSize(Control pan)
		{
			List<int> listRight = new List<int>();
			List<int> listTop = new List<int>();
			getUCSize(pan, ref listRight, ref listTop);
			listRight.Sort();
			Size ucSize = new Size(listRight[listRight.Count - 1] + 10, listTop[listTop.Count - 1] + 10);
			return ucSize;
		}

		/// <summary>
		/// 获取用户控件的宽度
		/// </summary>
		/// <param name="pan">容器</param>
		/// <param name="listRight">集合</param>
		/// <param name="listTop">集合</param>
		private void getUCSize(Control pan, ref List<int> listRight, ref List<int> listTop)
		{
			foreach (Control item in pan.Controls)
			{
				listRight.Add(item.Left + item.Width);
				listTop.Add(item.Top + item.Height);
				if (item.Controls.Count > 0)
				{
					getUCSize(item, ref listRight, ref listTop);
				}
			}
		}

		/// <summary>
		/// 通过反射获取必要属性的值
		/// </summary>
		/// <param name="c">控件</param>
		/// <returns></returns>
		private List<string> getControlPropertySpecial(Control c)
		{
			List<string> list = new List<string>();
			List<string> listStringPropertyInfo = new List<string>() { "ZrDescription", "ZrField", "ZrFormat", "ZrTable", "ZrVerify" };
			List<string> listIntPropertyInfo = new List<string>() { "TabIndex", "ZrFieldLength" };

			Type type = c.GetType();
			PropertyInfo property = null;
			foreach (var item in listStringPropertyInfo)
			{
				property = type.GetProperty(item);
				if (property != null)
				{
					object obj = property.GetValue(c, null);
					list.Add(string.Format("			this.{0}.{1} = {2};", c.Name, item,
						obj == null ? "null" : string.Format(@"""{0}""", obj.ToString())));
				}
			}

			foreach (var item in listIntPropertyInfo)
			{
				property = type.GetProperty(item);
				if (property != null)
				{
					object obj = property.GetValue(c, null);
					list.Add(string.Format("			this.{0}.{1} = {2};", c.Name, item,
						obj == null ? "0" : obj.ToString()));
				}
			}

			property = type.GetProperty("ZrIsNotNull");
			if (property != null)
			{
				object obj = property.GetValue(c, null);
				list.Add(string.Format("			this.{0}.ZrIsNotNull = {1};", c.Name,
					obj == null ? "false" : obj.ToString().ToLower()));
			}

			property = type.GetProperty("ZrIsReadOnly");
			if (property != null)
			{
				object obj = property.GetValue(c, null);
				list.Add(string.Format("			this.{0}.ZrIsReadOnly = {1};", c.Name,
					obj == null ? "false" : obj.ToString().ToLower()));
			}

			property = type.GetProperty("BorderColor");
			if (property != null)
			{
				object obj = property.GetValue(c, null);
				list.Add(string.Format("			this.{0}.BorderColor = System.Drawing.Color.{1};", c.Name, ((Color)obj).Name));
			}
			return list;
		}
	}
}
