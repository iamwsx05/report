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
using Report.Entity;
using Common.Entity;
using Report.Ui.controller;

namespace Report.Ui
{
    public partial class frmInterviewEdit : frmBasePopup
    {
        public frmInterviewEdit(EntityOutpatientInterview _entityInterview)
        {
            InitializeComponent();

            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
                ((ctlinterviewedit)Controller).InterviewVo = _entityInterview;
            }
        }


        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlinterviewedit();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 是否保存操作
        /// </summary>
        internal bool IsSave { get; set; }

        private void frmInterviewEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            this.timer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            ((ctlinterviewedit)Controller).Init();
        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ctlinterviewedit)Controller).GetPatient();
                this.showPanelForm.RefreshPatInfo();
            }
        }

        private void blbSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlinterviewedit)Controller).Save();
        }

        private void blbPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlinterviewedit)Controller).Print();
        }

        private void blbExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlinterviewedit)Controller).Export();
        }

        private void blbClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void blbClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlinterviewedit)Controller).Clear();
        }

        private void frmInterviewEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalPatient.currPatient = null;
        }

        private void rdoFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoFlag.SelectedIndex == 1)
                this.lblFlagName.Text = "住院号: ";
            else
                this.lblFlagName.Text = "诊疗卡号:";
        }
    }
}