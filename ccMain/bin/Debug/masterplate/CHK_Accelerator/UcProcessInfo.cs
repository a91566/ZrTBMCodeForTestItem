using ZrCHKFormBase;

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

		/// <summary>
        /// 初始化后执行的函数
        /// </summary>
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
