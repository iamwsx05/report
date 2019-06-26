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
    /// 编辑
    /// </summary>
    public partial class frmAnaEdit2 : frmBasePopup
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmAnaEdit2(decimal _anaId)
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
                AnaId = _anaId;
            } 
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// AnaId
        /// </summary>
        decimal AnaId { get; set; }

        /// <summary>
        /// 是否保存
        /// </summary>
        public bool IsSave { get; set; }

        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        void Init()
        {
            string xmlData = string.Empty;
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    xmlData = proxy.Service.GetRegister2Xml(this.AnaId);
                }
                if (!string.IsNullOrEmpty(xmlData))
                {
                    SetXmlData(xmlData);
                }
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
            xmlData += string.Format("<F001>{0}</F001>", this.chk01.Checked ? "1" : "0");
            xmlData += string.Format("<F002>{0}</F002>", this.chk02.Checked ? "1" : "0");
            xmlData += string.Format("<F003>{0}</F003>", this.chk03.Checked ? "1" : "0");
            xmlData += string.Format("<F004>{0}</F004>", this.chk04.Checked ? "1" : "0");
            xmlData += string.Format("<F005>{0}</F005>", this.chk05.Checked ? "1" : "0");
            xmlData += string.Format("<F006>{0}</F006>", this.chk06.Checked ? "1" : "0");
            xmlData += string.Format("<F007>{0}</F007>", this.chk07.Checked ? "1" : "0");
            xmlData += string.Format("<F008>{0}</F008>", this.chk08.Checked ? "1" : "0");
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
                this.chk01.Checked = false;
                this.chk02.Checked = false;
                this.chk03.Checked = false;
                this.chk04.Checked = false;
                this.chk05.Checked = false;
                this.chk06.Checked = false;
                this.chk07.Checked = false;
                this.chk08.Checked = false;
            }
            else
            {
                Dictionary<string, string> dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                this.chk01.Checked = (Function.Int(dicData["F001"]) == 1 ? true : false);
                this.chk02.Checked = (Function.Int(dicData["F002"]) == 1 ? true : false);
                this.chk03.Checked = (Function.Int(dicData["F003"]) == 1 ? true : false);
                this.chk04.Checked = (Function.Int(dicData["F004"]) == 1 ? true : false);
                this.chk05.Checked = (Function.Int(dicData["F005"]) == 1 ? true : false);
                this.chk06.Checked = (Function.Int(dicData["F006"]) == 1 ? true : false);
                this.chk07.Checked = (Function.Int(dicData["F007"]) == 1 ? true : false);
                this.chk08.Checked = (Function.Int(dicData["F008"]) == 1 ? true : false);
            }
        }
        #endregion

        #endregion

        #region 事件

        private void frmAnaEdit2_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void blbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    if (proxy.Service.Register2Edit(this.AnaId, GetXmlData()) > 0)
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
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    if (proxy.Service.Register2Edit(this.AnaId, null) > 0)
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

        #endregion

    }
}
