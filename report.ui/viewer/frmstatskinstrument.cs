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

namespace Report.Ui
{
    public partial class frmStatSkinStrument : frmBaseMdi
    {
        public frmStatSkinStrument()
        {
            InitializeComponent();
        }

        Dictionary<string, string> dicDept = null;
        Dictionary<string, string> dicType = null;

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
            //EntityNursEventStrument vo = GetRowObject();
            //if (vo != null && Function.Dec(this.rptId) > 0)
            //{
            //    frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(this.rptId)));
            //    frm.ShowDialog();
            //}
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

            this.cboPart.Properties.Items.Add("");
            this.cboPart.Properties.Items.Add("骶尾椎骨处");
            this.cboPart.Properties.Items.Add("坐骨处");
            this.cboPart.Properties.Items.Add("股骨粗隆处");
            this.cboPart.Properties.Items.Add("跟骨处");
            this.cboPart.Properties.Items.Add("足踝处");
            this.cboPart.Properties.Items.Add("肩胛处");
            this.cboPart.Properties.Items.Add("枕骨处");
            this.cboPart.Properties.Items.Add("肘部");
            this.cboPart.Properties.Items.Add("多部位");
            this.cboPart.Properties.Items.Add("其它部位");

            this.cboFQ.Properties.Items.Add("");
            this.cboFQ.Properties.Items.Add("1期");
            this.cboFQ.Properties.Items.Add("2期");
            this.cboFQ.Properties.Items.Add("3期");
            this.cboFQ.Properties.Items.Add("4期");
            this.cboFQ.Properties.Items.Add("无法分期");
            this.cboFQ.Properties.Items.Add("深部组织损伤");

            DateTime dtmNow = DateTime.Now;
            this.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteEnd.DateTime = dtmNow;

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

                dicType = proxy.Service.GetAdversSkinType();
                if (dicType != null)
                {
                    foreach (var dic in dicType)
                    {
                        this.cboType.Properties.Items.Add(dic.Value);
                    }
                }
            }

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

            if (!string.IsNullOrEmpty(this.cboPart.Text))
            {
                string partCode = this.cboPart.Text;
                dicParm.Add(Function.GetParm("PartCode", partCode));
            }

            if (!string.IsNullOrEmpty(this.cboFQ.Text))
            {
                string fqCode = this.cboFQ.Text;
                dicParm.Add(Function.GetParm("FqCode", fqCode));
            }

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    dateScope = beginDate + " ~ " + endDate;
                    this.gcData.DataSource = proxy.Service.GetStatSkinEventInstrument(dicParm);
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

        private void frmStatSkinStrument_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void gvData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            //if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            //{
            //    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            //    e.Appearance.ForeColor = Color.Gray;
            //    e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            //}
        }

        #endregion
    }
}