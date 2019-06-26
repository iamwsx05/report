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
using DevExpress.XtraReports.UI;
using System.IO;
using Report.Entity;
using Common.Utils;

namespace Report.Ui
{
    /// <summary>
    /// 麻醉手术登记
    /// </summary>
    public partial class frmAnaRegister1 : frmBaseMdi
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmAnaRegister1()
        {
            InitializeComponent();
        }
        #endregion

        #region override

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            this.Query();
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            this.EditRegister();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            //EntityAnaRegister1 vo = GetRowObject();
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
            //EntityAnaRegister1 vo = GetRowObject();
            //if (vo != null && Function.Dec(this.rptId) > 0)
            //{
            //    frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(this.rptId)));
            //    frm.ShowDialog();
            //}
            uiHelper.Print(this.gcData);
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
            this.rptId = 2;
        }
        #endregion

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        void Query()
        {
            string beginDate = this.dteStart.Text.Trim();
            string endDate = this.dteEnd.Text.Trim();
            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
            }
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    dateScope = beginDate + " ~ " + endDate;
                    this.gcData.DataSource = proxy.Service.GetAnaRegister1(beginDate, endDate);
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region EditRegister
        /// <summary>
        /// Edit
        /// </summary>
        void EditRegister()
        {
            EntityAnaRegister1 vo = GetRowObject();
            if (vo != null && vo.AnaId > 0)
            {
                frmAnaEdit1 frm = new frmAnaEdit1(vo.AnaId);
                frm.ShowDialog();
                if (frm.IsSave)
                {
                    this.Query();
                }
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
            xr.DataSource = this.gcData.DataSource as List<EntityAnaRegister1>;
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
        EntityAnaRegister1 GetRowObject()
        {
            if (this.gvData.FocusedRowHandle < 0) return null;
            return this.gvData.GetRow(this.gvData.FocusedRowHandle) as EntityAnaRegister1;
        }
        #endregion

        #region RowCellStyle
        /// <summary>
        /// RowCellStyle
        /// </summary>
        /// <param name="e"></param>
        internal void RowCellStyle(DevExpress.XtraGrid.Views.Grid.GridView gv, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column == gv.FocusedColumn && e.RowHandle == gv.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.FromArgb(251, 165, 8);
                e.Appearance.BackColor2 = Color.White;
            }
            else
            {
                //if (GetFieldValueStr(gv, e.RowHandle, EntityEventDisplay.Columns.reportType) == "跟踪报告")
                //{
                //    e.Appearance.ForeColor = Color.Crimson;
                //}
                //else
                //{
                //    e.Appearance.ForeColor = Color.FromArgb(0, 92, 156);
                //}
            }
            gv.Invalidate();
        }
        #endregion

        #endregion

        #region 事件

        private void frmAnaRegister1_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void gvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            this.RowCellStyle(this.gvData, e);
        }

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            this.EditRegister();
        }

        #endregion

    }
}
