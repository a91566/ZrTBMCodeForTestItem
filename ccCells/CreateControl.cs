/*
 * 2017年6月8日 08:55:23 郑少宝
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
			if (item.TypeLength.StartsWith("varchar") && item.ZrIsEnum)
			{
				c = new ZrControl.ZrDynamicComboBox();				
			}
			else if (item.TypeLength.StartsWith("varchar") && !item.ZrIsEnum)
			{
				c = new ZrControl.ZrDynamicTextBox();
			}
			else if (item.TypeLength.StartsWith("datetime"))
			{
				c = new ZrControl.ZrDateTimePicker();
			}
			else if (item.TypeLength.StartsWith("bit"))
			{
				c = new ZrControl.ZrDynamicCheckBox();
			}
			else
			{
				return null;
			}
			(c as ZrControl.IZrControl).SetZrControlExternalInfo(item);
			return c;
		}

		
	}
}
