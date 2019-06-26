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

namespace Report.Ui
{
    public partial class frmOutpatientInterview : frmBaseMdi
    {
        public frmOutpatientInterview()
        {
            InitializeComponent();
            if (DesignMode == false)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(-2000, 0);
            }
        }

        #region override

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlOutpatientInterview();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlOutpatientInterview)Controller).Query();
        }

        /// <summary>
        /// 新事件
        /// </summary>
        public override void New()
        {
            ((ctlOutpatientInterview)Controller).NewEvent();
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            ((ctlOutpatientInterview)Controller).EditInterview();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public override void Delete()
        {
            ((ctlOutpatientInterview)Controller).DelInterview();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlOutpatientInterview)Controller).Export();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlOutpatientInterview)Controller).Print();
        }

        #endregion

        private void frmOutpatientInterview_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            ((ctlOutpatientInterview)Controller).Init();
        }

        private void gvReport_DoubleClick(object sender, EventArgs e)
        {
            ((ctlOutpatientInterview)Controller).EditInterview();
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
    }
}