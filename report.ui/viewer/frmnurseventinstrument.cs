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
using Common.Utils;
using DevExpress.XtraReports.UI;
using System.IO;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    public partial class frmNursEventInstrument : frmBaseMdi
    {
        public frmNursEventInstrument()
        {
            InitializeComponent();
        }
        Dictionary<string, string> dicDept { get; set; }
        Dictionary<string, string> dicType { get; set; }
        Dictionary<string, string> spanTime { get; set; }
        Dictionary<string, string> dicRank { get; set; }
        Dictionary<string, string> dicShcd { get; set; }
        Dictionary<string, string> dicSbyy { get; set; }

        #region override

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            this.Query();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            //EntityNursEventStrument vo = GetRowObject();
            //if (vo != null && Function.Dec(this.rptId) > 0)
            //{
            //    XtraReport xr = GetXR(Function.Dec(this.rptId));
            //    if (xr != null && xr.DataSource != null)
            //    {
            //        xr.Name = this.Text;
            //        uiHelper.Export(xr);
            //    }
            //}

            uiHelper.ExportToXls(this.gvData);
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            EntityNursEventStrument vo = GetRowObject();
            if (vo != null && Function.Dec(this.rptId) > 0)
            {
                frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(this.rptId)));
                frm.ShowDialog();
            }
        }

        #endregion

        #region 变量.属性

        int rptId { get; set; }

        string dateScope { get; set; }

        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        void Init()
        {
            this.rptId = 15;
            this.cboDept.Properties.Items.Add("");
            this.cboType.Properties.Items.Add("");
            this.cboSbyy.Properties.Items.Add("");
            this.cboShcd.Properties.Items.Add("");
            this.cboRank.Properties.Items.Add("");
            this.cboHepenSpan.Properties.Items.Add("");
            this.cboLevel.Properties.Items.Add("");
            this.cboLevel.Properties.Items.Add("I");
            this.cboLevel.Properties.Items.Add("II");
            this.cboLevel.Properties.Items.Add("III");
            this.cboLevel.Properties.Items.Add("IV");
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                dicDept = proxy.Service.GetAdversDept();
                if (dicDept != null)
                {
                    foreach (var dic in dicDept)
                    {
                        this.cboDept.Properties.Items.Add(dic.Value);
                    }
                }

                dicType = proxy.Service.GetAdversType();
                if (dicType != null)
                {
                    foreach (var dic in dicType)
                    {
                        this.cboType.Properties.Items.Add(dic.Value);
                    }
                }

                spanTime = proxy.Service.GetSpanTime();
                if (spanTime != null)
                {
                    foreach (var dic in spanTime)
                    {
                        this.cboHepenSpan.Properties.Items.Add(dic.Value + "|" + dic.Key);
                    }
                }

                dicRank = proxy.Service.GetEventDicparm(30);
                if (dicRank != null)
                {
                    foreach (var dic in dicRank)
                    {
                        this.cboRank.Properties.Items.Add(dic.Value);
                    }
                }

                dicShcd = proxy.Service.GetEventDicparm(31);
                if (dicShcd != null)
                {
                    foreach (var dic in dicShcd)
                    {
                        this.cboShcd.Properties.Items.Add(dic.Value);
                    }
                }

                dicSbyy = proxy.Service.GetEventDicparm(32);
                if (dicSbyy != null)
                {
                    foreach (var dic in dicSbyy)
                    {
                        this.cboSbyy.Properties.Items.Add(dic.Value);
                    }
                }
            }

            DateTime dtmNow = DateTime.Now;
            this.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteEnd.DateTime = dtmNow;
        }
        #endregion

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        void Query()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = this.dteStart.Text.Trim();
            string endDate = this.dteEnd.Text.Trim();
            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }

                dicParm.Add(Function.GetParm("reportDate", beginDate + "|" + endDate));
            }

            if (!string.IsNullOrEmpty(this.cboDept.Text))
            {
                dicParm.Add(Function.GetParm("deptCode", dicDept.FirstOrDefault(q => q.Value == this.cboDept.Text).Key.Trim()));
            }

            if (!string.IsNullOrEmpty(this.cboType.Text))
            {
                string typeCode = dicType.FirstOrDefault(q => q.Value == this.cboType.Text).Key.Trim();
                //string typeName = this.cboType.Text;
                dicParm.Add(Function.GetParm("TypeCode", dicType.FirstOrDefault(q => q.Value == this.cboType.Text).Key.Trim()));
            }

            if (!string.IsNullOrEmpty(this.cboLevel.Text))
            {
                dicParm.Add(Function.GetParm("level", this.cboLevel.Text));
            }

            if (!string.IsNullOrEmpty(this.cboHepenSpan.Text))
            {
                dicParm.Add(Function.GetParm("tspan", this.cboHepenSpan.Text.Split('|')[1]));
            }

            if (!string.IsNullOrEmpty(this.cboRank.Text))
            {
                dicParm.Add(Function.GetParm("Rank", dicRank.FirstOrDefault(q => q.Value == this.cboRank.Text).Value.Trim()));
            }

            if (!string.IsNullOrEmpty(this.cboShcd.Text))
            {
                dicParm.Add(Function.GetParm("Shcd", dicShcd.FirstOrDefault(q => q.Value == this.cboShcd.Text).Key.Trim())); 
            }

            if (!string.IsNullOrEmpty(this.cboSbyy.Text))
            {
                dicParm.Add(Function.GetParm("Sbyy", dicSbyy.FirstOrDefault(q => q.Value == this.cboSbyy.Text).Key.Trim())); 
            }

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    dateScope = beginDate + " ~ " + endDate;
                    this.gcData.DataSource = proxy.Service.GetStatNursEventInstrument(dicParm);
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region XR
        /// <summary>
        /// GetXR
        /// </summary>
        /// <returns></returns>
        XtraReport GetXR(decimal rptId)
        {
            EntitySysReport rptVo = null;
            using (ProxyCommon proxy = new ProxyCommon())
            {
                rptVo = proxy.Service.GetReport(this.rptId);
            }
            XtraReport xr = new XtraReport();
            if (rptVo != null)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(rptVo.rptFile, 0, rptVo.rptFile.Length);
                xr.LoadLayout(ms);
            }
            xr.DataSource = this.gcData.DataSource as List<EntityNursEventStrument>;
            XRControl xc; //报表上的组件
            xc = xr.FindControl("lblDate", true);
            if (xc != null) (xc as XRLabel).Text = " " + dateScope;
            xr.CreateDocument();
            return xr;
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityNursEventStrument GetRowObject()
        {
            if (this.gvData.FocusedRowHandle < 0) return null;
            return this.gvData.GetRow(this.gvData.FocusedRowHandle) as EntityNursEventStrument;
        }
        #endregion

       
        #endregion

        #region 事件

        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void frmNursEventInstrument_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        #endregion
    }
}