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
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    public partial class frmOccupatioExpStat : frmBaseMdi
    {
        public frmOccupatioExpStat()
        {
            InitializeComponent();
        }

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
            uiHelper.ExportToXls(this.gvData);
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            uiHelper.Print(this.gcData);
        }

        #endregion

        private void frmOccupatioExpStat_Load(object sender, EventArgs e)
        {
            DateTime dtmNow = DateTime.Now;
            this.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteEnd.DateTime = dtmNow;
        }

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        internal void Query()
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
            if (this.ucDept.DeptVo != null && !string.IsNullOrEmpty(this.ucDept.DeptVo.deptCode))
            {
                dicParm.Add(Function.GetParm("deptCode", this.ucDept.DeptVo.deptCode));
            }

            try
            {
                uiHelper.BeginLoading(this);
                if (dicParm.Count > 0)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        List<EntityOccupationexp> dataSource = proxy.Service.GetStatOccupationexp(dicParm);;
                        this.gcData.DataSource = dataSource;
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
    }
}