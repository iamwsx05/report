using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    /// <summary>
    /// 传染病上报
    /// </summary>
    public partial class frmContagion : frmBaseMdi
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmContagion()
        {
            InitializeComponent();
        }
        #endregion

        #region override

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlContagion();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlContagion)Controller).Query();
        }

        /// <summary>
        /// 新事件
        /// </summary>
        public override void New()
        {
            ((ctlContagion)Controller).NewReport();
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            ((ctlContagion)Controller).EditReport();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public override void Delete()
        {
            ((ctlContagion)Controller).DelReport();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlContagion)Controller).Export();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlContagion)Controller).Print();
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 报表ID: 
        /// 21      3-I.艾滋病病毒感染孕产妇/婚检妇女基本情况登记卡
        /// 22      3-II.艾滋病病毒感染孕产妇妊娠及所生婴儿登记卡
        /// 23      3-III.艾滋病病毒感染产妇及所生儿童随访登记卡
        /// 24      4-I.梅毒感染孕产妇登记卡
        /// 25      4-II.艾滋病病毒感染产妇及所生儿童随访登记卡
        /// 26      4-III.梅毒感染产妇所生儿童随访登记卡
        /// </summary>
        internal string ReportId = "21";

        /// <summary>
        /// 外部接口
        /// </summary>
        /// <param name="_regType"></param>
        public void Show2(string _ReportId)
        {
            ReportId = _ReportId;
            this.Show();
        }
        #endregion

        #region 事件

        private void frmContagion_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            ((ctlContagion)Controller).Init();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.ucDept.Visible = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void gvReport_DoubleClick(object sender, EventArgs e)
        {
            ((ctlContagion)Controller).EditReport();
        }

        private void gvReport_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            ((ctlContagion)Controller).RowCellStyle(gvReport, e);
        }

        private void gvReport_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #endregion


    }
}
