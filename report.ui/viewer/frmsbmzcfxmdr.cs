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
using Report.Ui.controller;

namespace Report.Ui
{
    public partial class frmSbMzcfxmdr : frmBaseMdi
    {
        public frmSbMzcfxmdr()
        {
            InitializeComponent();
        }

        #region override

        /// <summary>
        /// 上传
        /// </summary>
        public override void Confirm()
        {
            ((ctlSbMzchxmUpload)Controller).Apply();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlSbMzchxmUpload)Controller).Export();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlSbMzchxmUpload)Controller).Print(); //uiHelper.Print(this.gcData);
        }


        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            //1 手动检索
            ((ctlSbMzchxmUpload)Controller).Query();
        }
        //删除上传处方
        public override void Delete()
        {
            ((ctlSbMzchxmUpload)Controller).Delete();
        }

        #endregion

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlSbMzchxmUpload();
            Controller.SetUI(this);
        }
        #endregion


        #region 事件


        private void gvCfData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                //e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //e.Appearance.ForeColor = Color.Gray;
                //e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gvCfMsgData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                //e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                //e.Appearance.ForeColor = Color.Gray;
                //e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gvCfData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            ((ctlSbMzchxmUpload)Controller).RowCellStyle(gvCfData, e);
        }

        private void gvCfData_DoubleClick(object sender, EventArgs e)
        {
            ((ctlSbMzchxmUpload)Controller).Edit();
        }

        private void frmSbMzcfxmdr_Load(object sender, EventArgs e)
        {
            ((ctlSbMzchxmUpload)Controller).Init();
            ctlYbUpLoadAuto autoCtl = new ctlYbUpLoadAuto();
            autoCtl.Init(2);
        }

        #endregion 
    }
}