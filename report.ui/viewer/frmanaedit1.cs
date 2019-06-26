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
    public partial class frmAnaEdit1 : frmBasePopup
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmAnaEdit1(decimal _anaId)
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
                this.clbcDoct.Items.Clear();
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    xmlData = proxy.Service.GetRegister1Xml(this.AnaId);
                    bool isChecked = (!string.IsNullOrEmpty(xmlData) ? false : true);
                    List<EntityCodeOperator> lstDoct = proxy.Service.GetAnaOperator();
                    if (lstDoct != null && lstDoct.Count > 0)
                    {
                        foreach (EntityCodeOperator item in lstDoct)
                        {
                            this.clbcDoct.Items.Add(item.operName + " " + item.rankName, isChecked);
                        }
                    }
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
            xmlData += string.Format("<F001>{0}</F001>", this.rdo001.SelectedIndex);
            xmlData += string.Format("<F002>{0}</F002>", this.rdo002.SelectedIndex);
            xmlData += string.Format("<F003>{0}</F003>", this.rdo003.SelectedIndex);
            xmlData += string.Format("<F004>{0}</F004>", this.rdo004.SelectedIndex);
            xmlData += string.Format("<F005>{0}</F005>", this.rdo005.SelectedIndex);
            xmlData += string.Format("<F006>{0}</F006>", this.rdo006.SelectedIndex);
            xmlData += string.Format("<F007>{0}</F007>", this.rdo007.SelectedIndex);

            xmlData += string.Format("<F011>{0}</F011>", this.chk001.Checked ? "1" : "0");
            xmlData += string.Format("<F012>{0}</F012>", this.chk002.Checked ? "1" : "0");
            xmlData += string.Format("<F013>{0}</F013>", this.chk003.Checked ? "1" : "0");
            xmlData += string.Format("<F014>{0}</F014>", this.chk004.Checked ? "1" : "0");
            xmlData += string.Format("<F021>{0}</F021>", this.chk005.Checked ? "1" : "0");
            xmlData += string.Format("<F022>{0}</F022>", this.chk006.Checked ? "1" : "0");
            xmlData += string.Format("<F023>{0}</F023>", this.chk007.Checked ? "1" : "0");

            xmlData += string.Format("<F031>{0}</F031>", this.txt001.Text.Trim());

            string node = string.Empty;
            for (int i = 0; i < clbc01.Items.Count; i++)
            {
                node = "F10" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc01.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }
            for (int i = 0; i < clbc02.Items.Count; i++)
            {
                node = "F20" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc02.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }
            for (int i = 0; i < clbc03.Items.Count; i++)
            {
                node = "F30" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc03.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }
            for (int i = 0; i < clbc04.Items.Count; i++)
            {
                node = "F40" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc04.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }
            for (int i = 0; i < clbc05.Items.Count; i++)
            {
                node = "F50" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc05.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }
            for (int i = 0; i < clbc06.Items.Count; i++)
            {
                node = "F60" + (i + 1).ToString();
                xmlData += string.Format("<{0}>{1}</{2}>", node, (clbc06.Items[i].CheckState == CheckState.Checked ? "1" : "0"), node);
            }

            xmlData += string.Format("<F701>{0}</F701>", this.chk701.Checked ? "1" : "0");
            xmlData += string.Format("<F702>{0}</F702>", this.chk702.Checked ? "1" : "0");
            xmlData += string.Format("<F703>{0}</F703>", this.chk703.Checked ? "1" : "0");
            xmlData += string.Format("<F704>{0}</F704>", this.chk704.Checked ? "1" : "0");
            xmlData += string.Format("<F705>{0}</F705>", this.chk705.Checked ? "1" : "0");
            xmlData += string.Format("<F706>{0}</F706>", this.chk706.Checked ? "1" : "0");
            xmlData += string.Format("<F707>{0}</F707>", this.chk707.Checked ? "1" : "0");
            xmlData += string.Format("<F708>{0}</F708>", this.chk708.Checked ? "1" : "0");
            xmlData += string.Format("<F709>{0}</F709>", this.chk709.Checked ? "1" : "0");
            xmlData += string.Format("<F710>{0}</F710>", this.chk710.Checked ? "1" : "0");
            xmlData += string.Format("<F711>{0}</F711>", this.txt002.Text.Trim());

            string zrDoct = string.Empty;
            string fzrDoct = string.Empty;
            string zzDoct = string.Empty;
            string ysDoct = string.Empty;
            string doctDesc = string.Empty;
            for (int i = 0; i < clbcDoct.Items.Count; i++)
            {
                if (clbcDoct.Items[i].CheckState == CheckState.Checked)
                {
                    doctDesc = clbcDoct.Items[i].Value.ToString();
                    switch (doctDesc.Split(' ')[1].Trim())
                    {
                        case "主任医师":
                            zrDoct += doctDesc.Split(' ')[0] + ",";
                            break;
                        case "副主任医师":
                            fzrDoct += doctDesc.Split(' ')[0] + ",";
                            break;
                        case "主治医师":
                            zzDoct += doctDesc.Split(' ')[0] + ",";
                            break;
                        case "医师":
                            ysDoct += doctDesc.Split(' ')[0] + ",";
                            break;
                        default:
                            break;
                    }
                }
            }
            if (zrDoct != string.Empty) zrDoct = zrDoct.TrimEnd(',');
            if (fzrDoct != string.Empty) fzrDoct = fzrDoct.TrimEnd(',');
            if (zzDoct != string.Empty) zzDoct = zzDoct.TrimEnd(',');
            if (ysDoct != string.Empty) ysDoct = ysDoct.TrimEnd(',');
            xmlData += string.Format("<F801>{0}</F801>", zrDoct);
            xmlData += string.Format("<F802>{0}</F802>", fzrDoct);
            xmlData += string.Format("<F803>{0}</F803>", zzDoct);
            xmlData += string.Format("<F804>{0}</F804>", ysDoct);

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
                this.rdo001.SelectedIndex = 0;
                this.rdo002.SelectedIndex = 0;
                this.rdo003.SelectedIndex = 0;
                this.rdo004.SelectedIndex = 0;
                this.rdo005.SelectedIndex = 0;
                this.rdo006.SelectedIndex = 0;
                this.rdo007.SelectedIndex = 0;

                this.chk001.Checked = false;
                this.chk002.Checked = false;
                this.chk003.Checked = false;
                this.chk004.Checked = false;
                this.chk005.Checked = false;
                this.chk006.Checked = false;
                this.chk007.Checked = false;
                this.txt001.Text = string.Empty;

                string node = string.Empty;
                for (int i = 0; i < clbc01.Items.Count; i++)
                {
                    node = "F10" + (i + 1).ToString();
                    clbc01.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < clbc02.Items.Count; i++)
                {
                    node = "F20" + (i + 1).ToString();
                    clbc02.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < clbc03.Items.Count; i++)
                {
                    node = "F30" + (i + 1).ToString();
                    clbc03.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < clbc04.Items.Count; i++)
                {
                    node = "F40" + (i + 1).ToString();
                    clbc04.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < clbc05.Items.Count; i++)
                {
                    node = "F50" + (i + 1).ToString();
                    clbc05.Items[i].CheckState = CheckState.Unchecked;
                }
                for (int i = 0; i < clbc06.Items.Count; i++)
                {
                    node = "F60" + (i + 1).ToString();
                    clbc06.Items[i].CheckState = CheckState.Unchecked;
                }

                this.chk701.Checked = false;
                this.chk702.Checked = false;
                this.chk703.Checked = false;
                this.chk704.Checked = false;
                this.chk705.Checked = false;
                this.chk706.Checked = false;
                this.chk707.Checked = false;
                this.chk708.Checked = false;
                this.chk709.Checked = false;
                this.chk710.Checked = false;
                this.txt002.Text = string.Empty;

                for (int i = 0; i < clbcDoct.Items.Count; i++)
                {
                    clbcDoct.Items[i].CheckState = CheckState.Checked;
                }
            }
            else
            {
                Dictionary<string, string> dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                this.rdo001.SelectedIndex = Function.Int(dicData["F001"]);
                this.rdo002.SelectedIndex = Function.Int(dicData["F002"]);
                this.rdo003.SelectedIndex = Function.Int(dicData["F003"]);
                this.rdo004.SelectedIndex = Function.Int(dicData["F004"]);
                this.rdo005.SelectedIndex = Function.Int(dicData["F005"]);
                this.rdo006.SelectedIndex = Function.Int(dicData["F006"]);
                this.rdo007.SelectedIndex = Function.Int(dicData["F007"]);

                this.chk001.Checked = (Function.Int(dicData["F011"]) == 1 ? true : false);
                this.chk002.Checked = (Function.Int(dicData["F012"]) == 1 ? true : false);
                this.chk003.Checked = (Function.Int(dicData["F013"]) == 1 ? true : false);
                this.chk004.Checked = (Function.Int(dicData["F014"]) == 1 ? true : false);
                this.chk005.Checked = (Function.Int(dicData["F021"]) == 1 ? true : false);
                this.chk006.Checked = (Function.Int(dicData["F022"]) == 1 ? true : false);
                this.chk007.Checked = (Function.Int(dicData["F023"]) == 1 ? true : false);
                this.txt001.Text = dicData["F031"];

                string node = string.Empty;
                for (int i = 0; i < clbc01.Items.Count; i++)
                {
                    node = "F10" + (i + 1).ToString();
                    clbc01.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }
                for (int i = 0; i < clbc02.Items.Count; i++)
                {
                    node = "F20" + (i + 1).ToString();
                    clbc02.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }
                for (int i = 0; i < clbc03.Items.Count; i++)
                {
                    node = "F30" + (i + 1).ToString();
                    clbc03.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }
                for (int i = 0; i < clbc04.Items.Count; i++)
                {
                    node = "F40" + (i + 1).ToString();
                    clbc04.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }
                for (int i = 0; i < clbc05.Items.Count; i++)
                {
                    node = "F50" + (i + 1).ToString();
                    clbc05.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }
                for (int i = 0; i < clbc06.Items.Count; i++)
                {
                    node = "F60" + (i + 1).ToString();
                    clbc06.Items[i].CheckState = (dicData[node] == "1" ? CheckState.Checked : CheckState.Unchecked);
                }

                this.chk701.Checked = (Function.Int(dicData["F701"]) == 1 ? true : false);
                this.chk702.Checked = (Function.Int(dicData["F702"]) == 1 ? true : false);
                this.chk703.Checked = (Function.Int(dicData["F703"]) == 1 ? true : false);
                this.chk704.Checked = (Function.Int(dicData["F704"]) == 1 ? true : false);
                this.chk705.Checked = (Function.Int(dicData["F705"]) == 1 ? true : false);
                this.chk706.Checked = (Function.Int(dicData["F706"]) == 1 ? true : false);
                this.chk707.Checked = (Function.Int(dicData["F707"]) == 1 ? true : false);
                this.chk708.Checked = (Function.Int(dicData["F708"]) == 1 ? true : false);
                this.chk709.Checked = (Function.Int(dicData["F709"]) == 1 ? true : false);
                this.chk710.Checked = (Function.Int(dicData["F710"]) == 1 ? true : false);
                this.txt002.Text = dicData["F711"];

                List<string> lstDoct = new List<string>();
                if (!string.IsNullOrEmpty(dicData["F801"]))
                {
                    lstDoct.AddRange(dicData["F801"].Split(','));
                }
                if (!string.IsNullOrEmpty(dicData["F802"]))
                {
                    lstDoct.AddRange(dicData["F802"].Split(','));
                }
                if (!string.IsNullOrEmpty(dicData["F803"]))
                {
                    lstDoct.AddRange(dicData["F803"].Split(','));
                }
                if (!string.IsNullOrEmpty(dicData["F804"]))
                {
                    lstDoct.AddRange(dicData["F804"].Split(','));
                }
                if (lstDoct.Count > 0)
                {
                    for (int i = 0; i < clbcDoct.Items.Count; i++)
                    {
                        if (lstDoct.IndexOf(clbcDoct.Items[i].Value.ToString().Split(' ')[0]) >= 0)
                        {
                            clbcDoct.Items[i].CheckState = CheckState.Checked;
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 事件

        private void frmAnaEdit1_Load(object sender, EventArgs e)
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
                    if (proxy.Service.Register1Edit(this.AnaId, GetXmlData()) > 0)
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
                    if (proxy.Service.Register1Edit(this.AnaId, null) > 0)
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
