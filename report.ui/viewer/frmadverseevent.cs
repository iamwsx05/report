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

namespace Report.Ui
{
    /// <summary>
    /// 不良事件上报
    /// </summary>
    public partial class frmAdverseEvent : frmBaseMdi
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmAdverseEvent()
        {
            InitializeComponent();
            if (DesignMode == false)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(-2000, 0);
            }
        }
        #endregion

        #region 外部接口

        /// <summary>
        /// 事件ID: 11 医疗安全； 12 医疗器械； 13 护理； 14 药品； 15 输血记录； 16 输血回报； 17 职业暴露登记; 18 护理质量异常
        /// </summary>
        internal string EventId = "11";

        /// <summary>
        /// 外部接口
        /// </summary>
        /// <param name="_regType"></param>
        public void Show2(string _EventId)
        {
            EventId = _EventId;
            this.Show();
        }
        #endregion

        #region override

        #region CreateController
        /// <summary>
        /// CreateController
        /// </summary>
        protected override void CreateController()
        {
            base.CreateController();
            Controller = new ctlAdverseEvent();
            Controller.SetUI(this);
        }
        #endregion

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            ((ctlAdverseEvent)Controller).Query();
        }

        /// <summary>
        /// 新事件
        /// </summary>
        public override void New()
        {
            ((ctlAdverseEvent)Controller).NewEvent();
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        public override void Edit()
        {
            ((ctlAdverseEvent)Controller).EditEvent();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        public override void Delete()
        {
            ((ctlAdverseEvent)Controller).DelEvent();
        }

        /// <summary>
        /// 导出
        /// </summary>
        public override void Export()
        {
            ((ctlAdverseEvent)Controller).Export();
        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            ((ctlAdverseEvent)Controller).Print();
        }

        #endregion

        #region 事件

        private void frmAdverseEvent_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;
            ((ctlAdverseEvent)Controller).Init();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.ucDept.Visible = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void gvReport_DoubleClick(object sender, EventArgs e)
        {
            ((ctlAdverseEvent)Controller).EditEvent();
        }

        private void gvReport_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            ((ctlAdverseEvent)Controller).RowCellStyle(gvReport, e);
        }

        private void gvReport_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //((ctlAdverseEvent)Controller).RowStyle(gvReport, e);
        }

        private void gvReport_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
