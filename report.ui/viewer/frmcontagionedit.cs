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
    public partial class frmContagionEdit : frmBasePopup
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmContagionEdit(EntityContagionDisplay _ContagionDisplayVo)
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
                ((ctlContagionEdit)Controller).ContagionDisplayVo = _ContagionDisplayVo;
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
            Controller = new ctlContagionEdit();
            Controller.SetUI(this);
        }
        #endregion

        #region 事件

        private void frmContagionEdit_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            this.timer.Enabled = true;
        }

        private void frmContagionEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalPatient.currPatient = null;
        }

        private void blbiClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlContagionEdit)Controller).Clear();
        }

        private void blbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlContagionEdit)Controller).Save();
        }

        private void blbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlContagionEdit)Controller).Print();
        }

        private void blbiExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((ctlContagionEdit)Controller).Export();
        }

        private void blbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            ((ctlContagionEdit)Controller).Init();
        }

        private void txtCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ((ctlContagionEdit)Controller).GetPatient();
                this.showPanelForm.RefreshPatInfo();
            }
        }

        private void rdoFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoFlag.SelectedIndex == 1)
                this.lblFlagName.Text = "住院号: ";
            else
                this.lblFlagName.Text = "诊疗卡号:";
        }

        #endregion
        
    }
}
