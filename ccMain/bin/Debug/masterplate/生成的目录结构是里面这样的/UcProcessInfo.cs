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
            txb_密度_标准值_vch.ZrField = "密度_标准值"; txb_密度_标准值_vch.ZrFieldLength = 200;
            cmb_密度_检测结论_vch.ZrField = "密度_检测结论"; cmb_密度_检测结论_vch.ZrFieldLength = 200;
            txb_密度结果值_nmr.ZrField = "密度结果值"; txb_密度结果值_nmr.ZrFieldLength = 200;
            txb_密度_判定结论_vch.ZrField = "密度_判定结论"; txb_密度_判定结论_vch.ZrFieldLength = 200;
            txb_含固量_标准值_vch.ZrField = "含固量_标准值"; txb_含固量_标准值_vch.ZrFieldLength = 200;
            cmb_含固量_检测结论_vch.ZrField = "含固量_检测结论"; cmb_含固量_检测结论_vch.ZrFieldLength = 200;
            txb_含固量_nmr.ZrField = "含固量"; txb_含固量_nmr.ZrFieldLength = 200;
            txb_含固量_判定结论_vch.ZrField = "含固量_判定结论"; txb_含固量_判定结论_vch.ZrFieldLength = 200;
            txb_氯离子含量_标准值_vch.ZrField = "氯离子含量_标准值"; txb_氯离子含量_标准值_vch.ZrFieldLength = 200;
            cmb_氯离子含量_检测结论_vch.ZrField = "氯离子含量_检测结论"; cmb_氯离子含量_检测结论_vch.ZrFieldLength = 200;
            txb_氯离子含量_nmr.ZrField = "氯离子含量"; txb_氯离子含量_nmr.ZrFieldLength = 200;
            txb_氯离子含量_判定结论_vch.ZrField = "氯离子含量_判定结论"; txb_氯离子含量_判定结论_vch.ZrFieldLength = 200;
            txb_总碱量_标准值_vch.ZrField = "总碱量_标准值"; txb_总碱量_标准值_vch.ZrFieldLength = 200;
            cmb_总碱量_检测结论_vch.ZrField = "总碱量_检测结论"; cmb_总碱量_检测结论_vch.ZrFieldLength = 200;
            txb_总碱量_nmr.ZrField = "总碱量"; txb_总碱量_nmr.ZrFieldLength = 200;
            txb_总碱量_判定结论_vch.ZrField = "总碱量_判定结论"; txb_总碱量_判定结论_vch.ZrFieldLength = 200;
            txb_PH值_标准值_vch.ZrField = "PH值_标准值"; txb_PH值_标准值_vch.ZrFieldLength = 200;
            cmb_PH值_检测结论_vch.ZrField = "PH值_检测结论"; cmb_PH值_检测结论_vch.ZrFieldLength = 200;
            txb_PH值_nmr.ZrField = "PH值"; txb_PH值_nmr.ZrFieldLength = 200;
            txb_PH值_判定结论_vch.ZrField = "PH值_判定结论"; txb_PH值_判定结论_vch.ZrFieldLength = 200;
            txb_细度_标准值_vch.ZrField = "细度_标准值"; txb_细度_标准值_vch.ZrFieldLength = 200;
            cmb_细度_检测结论_vch.ZrField = "细度_检测结论"; cmb_细度_检测结论_vch.ZrFieldLength = 200;
            txb_细度_nmr.ZrField = "细度"; txb_细度_nmr.ZrFieldLength = 200;
            txb_细度_判定结论_vch.ZrField = "细度_判定结论"; txb_细度_判定结论_vch.ZrFieldLength = 200;
            txb_含水率_标准值_vch.ZrField = "含水率_标准值"; txb_含水率_标准值_vch.ZrFieldLength = 200;
            cmb_含水率_检测结论_vch.ZrField = "含水率_检测结论"; cmb_含水率_检测结论_vch.ZrFieldLength = 200;
            txb_含水率_nmr.ZrField = "含水率"; txb_含水率_nmr.ZrFieldLength = 200;
            txb_含水率_判定结论_vch.ZrField = "含水率_判定结论"; txb_含水率_判定结论_vch.ZrFieldLength = 200;
            txb_终凝时间_标准值_vch.ZrField = "终凝时间_标准值"; txb_终凝时间_标准值_vch.ZrFieldLength = 200;
            txb_初凝时间_标准值_vch.ZrField = "初凝时间_标准值"; txb_初凝时间_标准值_vch.ZrFieldLength = 200;
            cmb_凝结时间_检测结论_vch.ZrField = "凝结时间_检测结论"; cmb_凝结时间_检测结论_vch.ZrFieldLength = 200;
            txb_初凝时间_nmr.ZrField = "初凝时间"; txb_初凝时间_nmr.ZrFieldLength = 200;
            txb_终凝时间_nmr.ZrField = "终凝时间"; txb_终凝时间_nmr.ZrFieldLength = 200;
            txb_凝结时间_判定结论_vch.ZrField = "凝结时间_判定结论"; txb_凝结时间_判定结论_vch.ZrFieldLength = 200;
            txb_抗压强度比_标准值1d_vch.ZrField = "抗压强度比_标准值1d"; txb_抗压强度比_标准值1d_vch.ZrFieldLength = 200;
            cmb_抗压强度比_检测结论1d_vch.ZrField = "抗压强度比_检测结论1d"; cmb_抗压强度比_检测结论1d_vch.ZrFieldLength = 200;
            txb_抗压强度比1d_nmr.ZrField = "抗压强度比1d"; txb_抗压强度比1d_nmr.ZrFieldLength = 200;
            txb_抗压强度比_判定结论1d_vch.ZrField = "抗压强度比_判定结论1d"; txb_抗压强度比_判定结论1d_vch.ZrFieldLength = 200;
            txb_抗压强度比_标准值_vch.ZrField = "抗压强度比_标准值"; txb_抗压强度比_标准值_vch.ZrFieldLength = 200;
            cmb_抗压强度比_检测结论_vch.ZrField = "抗压强度比_检测结论"; cmb_抗压强度比_检测结论_vch.ZrFieldLength = 200;
            txb_抗压强度比_nmr.ZrField = "抗压强度比"; txb_抗压强度比_nmr.ZrFieldLength = 200;
            txb_抗压强度比_判定结论_vch.ZrField = "抗压强度比_判定结论"; txb_抗压强度比_判定结论_vch.ZrFieldLength = 200;


          

        }

        public override void DataInitialAddtionAfter()
        {
            base.DataInitialAddtionAfter();

            base.CheckItemResultChange(cmb_密度_检测结论_vch, txb_密度_判定结论_vch, tp密度, false);
            base.CheckItemResultChange(cmb_含固量_检测结论_vch, txb_含固量_判定结论_vch, tp含固量, false);
            base.CheckItemResultChange(cmb_氯离子含量_检测结论_vch, txb_氯离子含量_判定结论_vch, tp氯离子含量, false);
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
