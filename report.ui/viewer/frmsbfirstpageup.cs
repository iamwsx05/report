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
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;
using Report.Ui.controller;

namespace Report.Ui
{
    public partial class frmSbFirstPageUp : frmBaseMdi
    {
        public frmSbFirstPageUp()
        {
            InitializeComponent();
        }

        #region override

        /// <summary>
        /// 上传
        /// </summary>
        public override void Confirm()
        {
            ((ctlFirstPageUpload)Controller).Apply();
        }

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlFirstPageUpload)Controller).Query();
        }


        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlFirstPageUpload)Controller).Export();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlFirstPageUpload)Controller).Print(); //uiHelper.Print(this.gcData);
        }

        #endregion

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlFirstPageUpload();
            Controller.SetUI(this);
        }
        #endregion

        #region 事件
        #region
        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //{
            //    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //    e.Appearance.ForeColor = Color.Gray;
            //    e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            //}
        }

        private void gvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            ((ctlFirstPageUpload)Controller).RowCellStyle(gvData, e);
        }
        #endregion

        #region
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            ((ctlFirstPageUpload)Controller).Edit();
        }

        #endregion

        private void frmSbFirstPageUp_Load(object sender, EventArgs e)
        {
            ((ctlFirstPageUpload)Controller).MthInit();
            //((ctlFirstPageUpload)Controller).OnStart();
            ctlYbUpLoadAuto autoCtl = new ctlYbUpLoadAuto();
            autoCtl.Init(1);
        }

        private void rdoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((ctlFirstPageUpload)Controller).Query();
        }
        #endregion
    }
}