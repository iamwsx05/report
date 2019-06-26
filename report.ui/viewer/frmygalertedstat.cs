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
using weCare.Core.Utils;

namespace Report.Ui
{
    public partial class frmYgAlertedStat : frmBaseMdi
    {
        public frmYgAlertedStat()
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
            uiHelper.Print(gcData);
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
                    this.gcData.DataSource = proxy.Service.getAlertStat(beginDate, endDate);
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
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
        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Appearance.ForeColor = Color.Gray;
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        #endregion
    }
}