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
using Report.Ui.controller;

namespace Report.Ui
{
    /// <summary>
    /// 传染病上报
    /// </summary>
    public partial class frmZrbbgAdvers : frmBaseMdi
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmZrbbgAdvers()
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
            Controller = new ctlZrbbg();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlZrbbg)Controller).Query();
        }

        /// <summary>
        /// 新事件
        /// </summary>
        public override void New()
        {
            ((ctlZrbbg)Controller).NewReport();
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            ((ctlZrbbg)Controller).EditReport();
        }

        #region EditRegisterYgfk
        /// <summary>
        /// Edit
        /// </summary>
        public override void Copy()
        {
            ((ctlZrbbg)Controller).EditRegisterYgfk();
        }
        #endregion

        /// <summary>
        /// 删除事件
        /// </summary>
        public override void Delete()
        {
            ((ctlZrbbg)Controller).DelReport();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlZrbbg)Controller).Export();
        }

        public override void Statistics()
        {
            ((ctlZrbbg)Controller).Exportlist();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlZrbbg)Controller).Print();
        }

        #endregion

        #region 外部接口

        /// <summary> 
        /// 31      中华人民共和国传染病报告卡
        /// 32      传染病报告卡艾滋病性病附卡
        /// 33      传染病报告卡(梅毒)附卡
        /// </summary>
        internal string ReportId = "31";

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

        private void frmZrbbgAdvers_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            ((ctlZrbbg)Controller).Init();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.ucDept.Visible = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            ((ctlZrbbg)Controller).EditReport();
        }

        private void gvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            ((ctlZrbbg)Controller).RowCellStyle(gvData, e);
        }

        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gvData_CustomDrawRowIndicator_1(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
