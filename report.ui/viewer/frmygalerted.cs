using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Common.Controls;
using weCare.Core.Utils;
using Report.Biz;
using weCare.Core.Entity;

namespace Report.Ui
{
    public partial class frmYgAlerted : frmBaseMdi
    {
        public frmYgAlerted()
        {
            InitializeComponent();
        }

        string dateScope { get; set; }
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
            uiHelper.ExportToXls(this.gvData);
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            uiHelper.Print(this.gcData);
        }

        #region 方法

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        void Query()
        {
            string beginDate = this.dteStart.Text.Trim();
            string endDate = this.dteEnd.Text.Trim();
            List<EntityParm> dicParm = new List<EntityParm>();

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParm.Add(Function.GetParm("queryDate", beginDate + "|" + endDate));
            }

            if (this.ucDept.DeptVo != null && !string.IsNullOrEmpty(this.ucDept.DeptVo.deptCode))
            {
                dicParm.Add(Function.GetParm("deptCode", this.ucDept.DeptVo.deptCode));
            }

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    dateScope = beginDate + " ~ " + endDate;
                    this.gcData.DataSource = proxy.Service.getAlertInfo(dicParm);
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region downYgAlertInfo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int downYgAlertInfo(string beginDate, string endDate)
        {
            int ret = -1;
            int ret1 = -1;
            int ret2 = -1;
            int ret3 = -1;
            int ret4 = -1;

            using (ProxyAnaReport proxy = new ProxyAnaReport())
            {
                try
                {
                    ret = proxy.Service.GetCheckLisInfo(beginDate, endDate);
                    ret1 = proxy.Service.GetOrderAlertInfo(beginDate, endDate);
                    ret2 = proxy.Service.GetVsInfo(beginDate, endDate);
                    ret3 = proxy.Service.GetVsInfo2(beginDate, endDate);
                    ret4 = proxy.Service.GetVsInfo3(beginDate, endDate);
                }
                catch (Exception e)
                {
                    ExceptionLog.OutPutException(e);
                }
                if (ret < 0 || ret1 < 0 || ret2 < 0 || ret3 < 0 || ret4 < 0)
                    return -1;
                else
                    return 0;
            }
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
            
            gv.Invalidate();
        }
        #endregion
        #endregion

        #region 事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            this.RowCellStyle(this.gvData, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTp_Click(object sender, EventArgs e)
        {
            DateTime endTime = Function.Datetime(this.dteDown.Text.Trim() + ":59");
            string endDate = endTime.ToString("yyyy-MM-dd HH:mm:ss");
            string beginDate = (endTime.AddDays(-1)).ToString("yyyy-MM-dd") + " 00:00:00";
            int ret = downYgAlertInfo(beginDate, endDate);
            if (ret >= 0)
                DialogBox.Msg("同步成功！");
            else
                DialogBox.Msg("同步失败！");
        }

        #endregion


        private void frmYgAlerted_Load(object sender, EventArgs e)
        {
            DateTime dtmNow = DateTime.Now;
            dteDown.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            this.dteStart.DateTime = dtmNow;
            this.dteEnd.DateTime = dtmNow;
        }
    }
}