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
using System.Collections;
using weCare.Core.Utils;
using Report.Entity;
using System.IO;
using weCare.Core.Entity;
using DevExpress.XtraReports.UI;
using Common.Utils;

namespace Report.Ui
{
    public partial class frmRptProportion : frmBaseMdi
    {
        public frmRptProportion()
        {
            InitializeComponent();
        }
        private string KangJunStr = string.Empty;
        private string JyStr = string.Empty;
        private int statDept = 0;
        private void frmRptProportion_Load(object sender, EventArgs e)
        {
            init();
        }


        /// <summary>
        /// 报表XR
        /// </summary>
        XtraReport xr = null;

        #region override

        /// <summary>
        /// Statistics
        /// </summary>
        public override void Statistics()
        {
            if (rdoFlag.SelectedIndex == 0)
                this.stat(0);
            else if (rdoFlag.SelectedIndex == 1)
                this.stat(1);
        }

        /// <summary>
        /// Export
        /// </summary>
        public override void Export()
        {
            //uiHelper.ExportToXls(this.gvData);
            if (xr != null && xr.DataSource != null)
            {
                xr.Name = this.Text;
                uiHelper.Export(xr);
            }
        }

        /// <summary>
        /// Preview
        /// </summary>
        public override void Preview()
        {
            //uiHelper.Print(this.gcData);
            if (xr != null && xr.DataSource != null)
            {
                xr.PrintDialog();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// init
        /// </summary>
        void init()
        {
            string[] arrStr = null;
            string strTmp = string.Empty;

            DateTime dtmNow = DateTime.Now;
            this.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteDateEnd.DateTime = dtmNow;

            using (ProxyAnaReport proxy = new ProxyAnaReport())
            {
                strTmp = proxy.Service.GetSysParamStr("3069");
                if (!string.IsNullOrEmpty(strTmp))
                {
                    arrStr = strTmp.Split('*');
                    foreach (string str in arrStr)
                    {
                        JyStr += "'" + str + "'" + ",";
                    }
                    JyStr = "(" + JyStr.TrimEnd(',') + ")";
                }
            }

            decimal printId = 23;
            EntitySysReport rptVo = null;

            using (ProxyCommon proxy = new ProxyCommon())
            {
                rptVo = proxy.Service.GetReport(printId);
            }

            xr = new XtraReport();
            if (rptVo != null)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(rptVo.rptFile, 0, rptVo.rptFile.Length);
                xr.LoadLayout(ms);
            }
            this.ucPrintControl.PrintingSystem = xr.PrintingSystem;
            xr.CreateDocument();
        }

        void stat(int rdoFlg)
        {
            string beginDate = this.dteDateStart.Text.Trim();
            string endDate = this.dteDateEnd.Text.Trim();
            Dictionary<string, string> dicParam = new Dictionary<string, string>();

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParam.Add("reportDate", beginDate + "|" + endDate);
            }

            if(!string.IsNullOrEmpty(JyStr))
            {
                dicParam.Add("JyStr", JyStr);
            }

            try
            {
                uiHelper.BeginLoading(this);
                if (dicParam.Count > 0)
                {
                    using (ProxyAnaReport proxy = new ProxyAnaReport())
                    {
                        //List<EntityRptYzb> dataSource = null;
                        if (rdoFlg == 0)
                        {
                            xr.DataSource = proxy.Service.GetMzRptYzb(dicParam, statDept);
                        }
                        else if (rdoFlg == 1)
                        {
                            xr.DataSource = proxy.Service.GetZyRptYzb(dicParam, statDept);
                        }
                        
                        //this.gcData.DataSource = dataSource;
                        XRControl xc; //报表上的组件
                        xc = xr.FindControl("lblDate", true);
                        if (xc != null) (xc as XRLabel).Text = " " + beginDate + " ~ " + endDate;
                        xr.CreateDocument();
                    }
                }
                else
                {
                    DialogBox.Msg("请输入查询条件。");
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }

        #endregion

        private void rdoFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoFlag.SelectedIndex == 0)
            {
                XRControl xc; //报表上的组件
                xc = xr.FindControl("lblTip", true);
                if (xc != null) (xc as XRLabel).Text = "门诊科室药材比报表";
                xr.CreateDocument();
                //this.stat(0);
            }
            else if (rdoFlag.SelectedIndex == 1)
            {
                XRControl xc; //报表上的组件
                xc = xr.FindControl("lblTip", true);
                if (xc != null) (xc as XRLabel).Text = "住院科室药材比报表";
                xr.CreateDocument();
                //this.stat(0);
            }
        }
    }
}