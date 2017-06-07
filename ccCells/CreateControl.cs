using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZrTBMCodeForTestItem.ccEcternal;

namespace ZrTBMCodeForTestItem.ccCells
{
	public class CreateControl
	{
		/// <summary>
		/// 父容器
		/// </summary>
		private Control parent;
		/// <summary>
		/// 行高
		/// </summary>
		private int maxWidth;
		/// <summary>
		/// 行高
		/// </summary>
		private int rowHeight;

		/// <summary>
		/// 创建控件类
		/// </summary>
		/// <param name="parent">父容器</param>
		/// <param name="rowHeight">行高</param>
		public CreateControl(Control parent, int maxWidth, int rowHeight)
		{
			this.parent = parent;
			this.maxWidth = maxWidth;
			this.rowHeight = rowHeight;
		}

		/// <summary>
		/// 创建
		/// </summary>
		/// <param name="list">控件列表</param>
		/// <param name="startLocation">起始位置</param>
		public void Create(List<ControlDBInfo> list, Point startLocation)
		{
			foreach (ControlDBInfo item in list)
			{
				Application.DoEvents();
				this.addLabelTextBox(ref startLocation, item);
			}
		}


		//(int right, int top) return (txb.Location.X + txb.Width, txb.Top);
		/// <summary>
		/// 生成控件
		/// </summary>
		/// <param name="startLocation">起始位置</param>
		/// <param name="item">控件信息</param>
		private void addLabelTextBox(ref Point startLocation, ControlDBInfo item)
		{
			//Label lbl = new Label();
			//lbl.TextAlign = ContentAlignment.MiddleRight;
			//lbl.AutoSize = true;
			//lbl.Text = item.Label;
			//lbl.Parent = this.parent;
			//ZrControl.ZrDynamicTextBox txb = new ZrControl.ZrDynamicTextBox();
			//txb.Name = $"txb{item.Label}";
			//txb.Parent = this.parent;
			////计算坐标，是否需要换行显示，有可能需要换 N 行
			//int right = startLocation.X + 20 + lbl.Width + 6 + txb.Width;
			////如果右边位置超出，则换行，起始位置为 10
			//if (right > this.maxWidth)
			//{
			//	startLocation.X = 10;
			//	startLocation.Y = startLocation.Y + this.rowHeight;
			//}
			//lbl.Location = new Point(startLocation.X + 20, startLocation.Y);
			//txb.Location = new Point(lbl.Location.X + lbl.Width + 6, startLocation.Y - 4);
			//startLocation = new Point(txb.Location.X + txb.Width, startLocation.Y);
		}

		private Control createOne(ControlDBInfo item)
		{
			Control c;
			if (item.TypeLength.StartsWith("varchar") && item.IsEnum)
			{
				c = new ZrControl.ZrDynamicComboBox();
			}
			else if (item.TypeLength.StartsWith("varchar") && !item.IsEnum)
			{
				c = new ZrControl.ZrDynamicTextBox();
			}
			else
			{
				return null;
			}
			this.setZrControlExternalInfo(c, item);
			return c;
		}

		private void setZrControlExternalInfo(Control control, ControlDBInfo item)
		{
			switch (control)
			{
				case ZrControl.ZrDynamicTextBox txb:
					txb.ZrDescription = item.Description;
					txb.ZrField = item.ColumnName;
					txb.ZrFieldLength = item.TypeLength.GetInt();
					txb.ZrFormat = item.Format;
					txb.ZrIsEnum = item.IsEnum;
					txb.ZrIsNotNull = item.IsNotNull;
					txb.ZrIsReadOnly = item.IsReadOnly;
					txb.ZrTable = item.TableName;
					txb.ZrVerify = item.Verify;
					break;
				case ZrControl.ZrDynamicComboBox cmb:
					cmb.ZrDescription = item.Description;
					cmb.ZrField = item.ColumnName;
					cmb.ZrFieldLength = item.TypeLength.GetInt();
					cmb.ZrFormat = item.Format;
					cmb.ZrIsEnum = item.IsEnum;
					cmb.ZrIsNotNull = item.IsNotNull;
					cmb.ZrIsReadOnly = item.IsReadOnly;
					cmb.ZrTable = item.TableName;
					cmb.ZrVerify = item.Verify;
					break;
				default:
					break;
			}
		}
	}
}
