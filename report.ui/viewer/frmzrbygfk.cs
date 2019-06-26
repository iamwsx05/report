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
using Common.Entity;
using weCare.Core.Utils;

namespace Report.Ui
{
    public partial class frmZrbygfk : frmBasePopup
    {
        public frmZrbygfk(decimal _rptId)
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
                RptId = _rptId;
            }            
        }

        #region 变量.属性

        /// <summary>
        /// RptId
        /// </summary>
        decimal RptId { get; set; }

        /// <summary>
        /// 是否保存
        /// </summary>
        public bool IsSave { get; set; }

        #endregion

        #region Init
        /// <summary>
        /// 
        /// </summary>
        void Init()
        {
            string xmlData = string.Empty;
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    xmlData = proxy.Service.GetRegisterZrbygfk(this.RptId);
                    bool isChecked = (!string.IsNullOrEmpty(xmlData) ? false : true);
                }
                if (!string.IsNullOrEmpty(xmlData))
                {
                    SetXmlData(xmlData);
                }
                else
                    SetXmlData(null);
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region GetXmlData
        /// <summary>
        /// GetXmlData
        /// </summary>
        /// <returns></returns>
        string GetXmlData()
        {
            string xmlData = string.Empty;
            xmlData += string.Format("<F001>{0}</F001>", this.rdo001.SelectedIndex);
            xmlData += string.Format("<F002>{0}</F002>", this.dtefirst.Text);
            xmlData += string.Format("<F003>{0}</F003>", this.chkFirst.Checked ? "1" : "0");
            xmlData += string.Format("<F004>{0}</F004>", this.txtAlt.Text);
            xmlData += string.Format("<F005>{0}</F005>", this.rdoIgm.SelectedIndex);
            xmlData += string.Format("<F006>{0}</F006>", this.rdoGcjc.SelectedIndex);
            xmlData += string.Format("<F007>{0}</F007>", this.rdoHBs.SelectedIndex);
            xmlData += string.Format("<F008>{0}</F008>", this.rdoSymptom.SelectedIndex);
            return "<XmlData>" + xmlData + "</XmlData>";
        }
        #endregion

        #region SetXmlData
        /// <summary>
        /// SetXmlData
        /// </summary>
        /// <param name="xmlData"></param>
        void SetXmlData(string xmlData)
        {
            if (string.IsNullOrEmpty(xmlData))
            {
                this.rdo001.SelectedIndex = -1;
                this.dtefirst.Text = "";
                this.chkFirst.Checked = false;
                this.txtAlt.Text = "";
                this.rdoIgm.SelectedIndex = -1;
                this.rdoGcjc.SelectedIndex = -1;
                this.rdoHBs.SelectedIndex = -1;
                this.rdoSymptom.SelectedIndex = -1;
            }
            else
            {
                Dictionary<string, string> dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                this.rdo001.SelectedIndex = Function.Int(dicData["F001"]);
                this.dtefirst.Text = dicData["F002"];
                this.chkFirst.Checked = (Function.Int(dicData["F003"]) == 1 ? true : false);
                this.txtAlt.Text = dicData["F004"];
                this.rdoIgm.SelectedIndex = Function.Int(dicData["F005"]);
                this.rdoGcjc.SelectedIndex = Function.Int(dicData["F006"]);
                this.rdoHBs.SelectedIndex = Function.Int(dicData["F007"]);
                this.rdoSymptom.SelectedIndex = Function.Int(dicData["F008"]);
            }
        }
        #endregion

        private void frmZrbYgfk_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void blbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    if (proxy.Service.RegisterZrbygfk(this.RptId, GetXmlData()) > 0)
                    {
                        this.IsSave = true;
                        DialogBox.Msg("保存成功！");
                    }
                    else
                    {
                        DialogBox.Msg("保存失败。");
                    }
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }

        private void blbiDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogBox.Msg("确定是否删除当前记录？？", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    if (proxy.Service.RegisterZrbygfk(this.RptId, null) > 0)
                    {
                        this.IsSave = true;
                        this.SetXmlData(string.Empty);
                        DialogBox.Msg("删除记录成功！");
                    }
                    else
                    {
                        DialogBox.Msg("删除记录失败。");
                    }
                }
            }
        }

        private void blbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void chkFirst_CheckStateChanged(object sender, EventArgs e)
        {
            if(chkFirst.Checked == true)
            {
                this.dtefirst.Text = "";
            }
        }

        private void dtefirst_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dtefirst.Text))
                this.chkFirst.Checked = false;
        }
    }
}