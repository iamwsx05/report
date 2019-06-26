using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common.Controls;
using weCare.Core.Utils;
using weCare.Core.Entity;

namespace Report.Ui
{
    public partial class frmAdverseEventStat : frmBaseMdi
    {
        public frmAdverseEventStat()
        {
            InitializeComponent();
        }



        #region override

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlAdverseEventAll();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlAdverseEventAll)Controller).Query();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            uiHelper.ExportToXls(this.gvReport);
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            uiHelper.Print(this.gcReport);
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            ((ctlAdverseEventAll)Controller).EditEvent();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public override void Delete()
        {
            ((ctlAdverseEventAll)Controller).DelEvent();
        }

        #endregion

        private void frmadverseeventstat_Load(object sender, EventArgs e)
        {
            ((ctlAdverseEventAll)Controller).Init();
        }

        #region 方法

        #endregion

        private void gvReport_DoubleClick(object sender, EventArgs e)
        {
            ((ctlAdverseEventAll)Controller).EditEvent();
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