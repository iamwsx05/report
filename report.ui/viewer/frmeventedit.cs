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
    /// 编辑事件
    /// </summary>
    public partial class frmEventEdit : frmBasePopup
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmEventEdit(EntityEventDisplay _EventDisplayVo)
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
                ((ctlEventEdit)Controller).EventDisplayVo = _EventDisplayVo;
            }
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 是否保存操作
        /// </summary>
        internal bool IsSave { get; set; }

        #endregion

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlEventEdit();
            Controller.SetUI(this);
        }
        #endregion

        #region 事件

        private void frmEventEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            this.timer.Enabled = true;
        }

        private void frmEventEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalPatient.currPatient = null;
        }

        private void blbiClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlEventEdit)Controller).Clear();
        }

        private void blbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlEventEdit)Controller).Save();
        }

        private void blbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlEventEdit)Controller).Print();
        }

        private void blbiExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlEventEdit)Controller).Export();
        }

        private void blbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            ((ctlEventEdit)Controller).Init();
        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ctlEventEdit)Controller).GetPatient();
                this.showPanelForm.RefreshPatInfo();
            }
        }

        private void rdoFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtCardNo.Enabled = true;
            if (rdoFlag.SelectedIndex == 1)
                this.lblFlagName.Text = "住院号: ";
            else if (rdoFlag.SelectedIndex == 0)
                this.lblFlagName.Text = "诊疗卡号:";
            else if (rdoFlag.SelectedIndex == 2)
                this.lblFlagName.Text = "职工工号:";
            else if (rdoFlag.SelectedIndex == 3) //无患者
            {
                this.lblFlagName.Text = "";
                this.txtCardNo.Enabled = false;
                this.txtCardNo.Text = "\\";
                ((ctlEventEdit)Controller).GetPatient();
                this.showPanelForm.RefreshPatInfo();
            }
        }

        #endregion
       
    }
}
