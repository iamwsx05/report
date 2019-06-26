using Common.Controls;
using Common.Entity;
using Common.Utils;
using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Report.Ui
{
    /// <summary>
    /// 医疗器械不良事件汇总
    /// </summary>
    public partial class frmStatInstrument : frmBaseMdi
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmStatInstrument()
        {
            InitializeComponent();
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 报表XR
        /// </summary>
        XtraReport xr = null;

        #endregion

        #region override

        public override void Statistics()
        {
            this.Stat();
        }

        #region Export
        /// <summary>
        /// Export
        /// </summary>
        public override void Export()
        {
            if (xr != null && xr.DataSource != null)
            {
                xr.Name = this.Text;
                uiHelper.Export(xr);
            }
        }
        #endregion

        public override void Preview()
        {
            if (xr != null && xr.DataSource != null)
            {
                xr.PrintDialog();
            }
        }
        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        void Init()
        {
            #region 参数
            List<EntityRptEventParm> EventParmData = null;
            using (ProxyEntityFactory proxy = new ProxyEntityFactory())
            {
                EventParmData = EntityTools.ConvertToEntityList<EntityRptEventParm>(proxy.Service.SelectFullTable(new EntityRptEventParm()));
            }
            #endregion

            #region xr
            decimal printId = 0;
            EntitySysReport rptVo = null;
            if (EventParmData != null)
            {
                if (EventParmData.Any(t => t.eventId == "12" && t.keyId == "printId"))
                {
                    printId = Function.Dec(EventParmData.FirstOrDefault(t => t.eventId == "12" && t.keyId == "printId").keyValue);
                }
            }
            if (printId > 0)
            {
                using (ProxyCommon proxy = new ProxyCommon())
                {
                    rptVo = proxy.Service.GetReport(printId);
                }
            }
            else
            {
                return;
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
            #endregion

            this.Stat();
        }
        #endregion

        #region Stat
        /// <summary>
        /// Stat
        /// </summary>
        void Stat()
        {
            #region 条件
            if (this.dteStart.DateTime.Date > this.dteEnd.DateTime.Date)
            {
                DialogBox.Msg("开始时间不能大于结束时间。");
                this.dteStart.Focus();
                return;
            }
            string startDate = this.dteStart.Text;
            string endDate = this.dteEnd.Text;
            string deptCode = string.Empty;
            if (this.ucDept.DeptVo != null && !string.IsNullOrEmpty(this.ucDept.DeptName) && !string.IsNullOrEmpty(this.ucDept.DeptVo.deptCode))
            {
                deptCode = this.ucDept.DeptVo.deptCode;
            }
            #endregion

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    xr.DataSource = proxy.Service.GetStatInstrument(startDate, endDate, deptCode);
                    XRControl xc; //报表上的组件
                    xc = xr.FindControl("lblDate", true);
                    if (xc != null) (xc as XRLabel).Text = " " + startDate + " ~ " + endDate;
                    xr.CreateDocument();
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #endregion

        #region 事件

        private void frmStatInstrument_Load(object sender, System.EventArgs e)
        {
            Init();
        }

        private void frmStatInstrument_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                this.Stat();
            }
        }

        #endregion

    }
}
