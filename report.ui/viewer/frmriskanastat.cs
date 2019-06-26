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
    public partial class frmriskanastat : frmBaseMdi
    {
        public frmriskanastat()
        {
            InitializeComponent();
        }
        List<EntityDeptList> deptList = null;

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

        private void frmriskanastat_Load(object sender, EventArgs e)
        {
            DateTime dtmNow = DateTime.Now;
            this.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteEnd.DateTime = dtmNow;
            this.cboDept.Properties.Items.Add("");
            using (ProxyAnaReport proxy = new ProxyAnaReport())
            {
                deptList = proxy.Service.getDeptList();

                if(deptList != null)
                {
                    foreach(EntityDeptList vo in deptList)
                    {
                        this.cboDept.Properties.Items.Add(vo.deptName);
                    }
                }
            }

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
            if (deptList != null && !string.IsNullOrEmpty(this.cboDept.Text))
            {
                EntityDeptList vo = deptList.Find(t => t.deptName.Equals(this.cboDept.Text ));
                if(vo != null)
                    dicParm.Add(Function.GetParm("deptCode", vo.deptCode));
            }

            try
            {
                uiHelper.BeginLoading(this);
                if (dicParm.Count > 0)
                {
                    using (ProxyAnaReport proxy = new ProxyAnaReport())
                    {
                        List<EntityRiskAna> dataSource = proxy.Service.getRiskAnaStat(dicParm); 
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