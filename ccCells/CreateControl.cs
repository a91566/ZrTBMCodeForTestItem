/*
 * 2017年6月8日 08:55:23 郑少宝
 * 
 * 最温柔的月光
 * 也敌不过
 * 你转瞬的回眸
 */
using System.Windows.Forms;

namespace ZrTBMCodeForTestItem.ccCells
{
	public class CreateControl
	{
		
		/// <summary>
		/// 创建控件类
		/// </summary>
		public CreateControl()
		{
			
		}

		

		
		

		/// <summary>
		/// 创建 ZrControl （只负责创建出控件，不处理大小位置 容器）
		/// </summary>
		/// <param name="item">控件的信息</param>
		/// <returns></returns>
		public Control CreateZrControl(ZrControlExternalInfoFromFile  item)
		{
			Control c;
			if (item == null)
			{
				c = new Label();
				c.Name = $"lbl{System.Guid.NewGuid().ToString("N")}";
				c.Text = "无字段信息";
				c.ForeColor = System.Drawing.Color.Red;
				return c;
			}
			if (item.ZrIsEnum)
			{
				c = new ZrControl.ZrDynamicComboBox();
				c.Name = $"cmb{item.ZrField}";
			}
			else
			{			
				string columnType = item.TypeLength.ToLower();
				if (columnType.StartsWith("varchar") 
					|| columnType.StartsWith("nvarchar")
					|| columnType.StartsWith("int")
					|| columnType.StartsWith("decimal")
					|| columnType.StartsWith("double")
					|| columnType.StartsWith("float")
					|| columnType.StartsWith("numeric")
					)
				{
					c = new ZrControl.ZrDynamicTextBox();
					c.Name = $"txb{item.ZrField}";
					if (item.ZrIsNotNull)
					{
						(c as ZrControl.ZrDynamicTextBox).BorderColor = System.Drawing.Color.Red;
					}
				}
				else if (columnType.StartsWith("datetime"))
				{
					c = new ZrControl.ZrDateTimePicker();
					c.Name = $"dtp{item.ZrField}";
				}
				else if (columnType.StartsWith("bit"))
				{
					c = new ZrControl.ZrDynamicCheckBox();
					c.Name = $"ckb{item.ZrField}";
				}
				else
				{
					c = new Label();
					c.Name = $"lbl{System.Guid.NewGuid().ToString("N")}";
					c.Text = "未知控件类型";
					c.ForeColor = System.Drawing.Color.Red;
					return c;
				}
			}
						
			if (c is ZrControl.IZrControl)
			{
				(c as ZrControl.IZrControl).SetZrControlExternalInfo(item);
			}
			return c;
		}		
	}
}
