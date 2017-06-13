using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ZrCHKFormBase;
using ZrCHKCommonFrom;

namespace CHK_Accelerator
{
    public partial class UcProcessInfo : CHKProessFormBase
    {
        public UcProcessInfo()
        {
            InitializeComponent();
        }

        public override void ControlAll()
        {
             
        }

        public override void DataInitialAddtionAfter()
        {
            base.DataInitialAddtionAfter();
        }


        /// <summary>
        /// 设置自动试验数据
        /// </summary>
        public override bool SetAutoMachineData()
        {
            var dicData = base.GetAutoMachineData("");

            return true;       
		}        
    }
}
