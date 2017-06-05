using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZrCHKFormBase;
using CHK_Accelerator;

namespace CHK
{
    public class CheckClassControl : ICheckClassForm
    {

        /// <summary>
        /// 中润唯一编码
        /// </summary>
        public string ZrCode = "A0054";

        ZrCHKFormBase.CHKFormBase ICheckClassForm.GetSampleInfoControl()
        {
            return new UcSampleInfo();
        }

        ZrCHKFormBase.CHKProessFormBase ICheckClassForm.GetProcessInfoControl()
        {
            return new UcProcessInfo();
        }
        ZrCHKFormBase.CHKFormBase ICheckClassForm.GetProcessContentControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检查是否是对应的检测项目库
        /// </summary>
        /// <param name="ZrCode"></param>
        /// <returns></returns>
        public bool CheckCheckClass(string ZrCode)
        {
            return this.ZrCode == ZrCode;
        }


    }
}
