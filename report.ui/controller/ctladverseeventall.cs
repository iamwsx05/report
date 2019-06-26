using Common.Controls;
using Common.Entity;
using Report.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Report.Ui
{
    public class ctlAdverseEventAll : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmAdverseEventStat Viewer = null;

        /// <summary>
        /// 变量
        /// </summary>
        Dictionary<string, string> dicEventType = null;
        string dateScope { get; set; }
        List<EntityEventDisplay> datasource = new List<EntityEventDisplay>();
        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmAdverseEventStat)child;
        }
        #endregion

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            

            Viewer.cboEventType.Properties.Items.Add("");
            Viewer.cboLevel.Properties.Items.AddRange(new object[] { "", "I", "II", "III", "IV" });

            this.Viewer.lueReporter.Properties.SetSize();
            this.Viewer.lueReporter.Properties.PopupWidth = 155;
            this.Viewer.lueReporter.Properties.PopupHeight = 250;
            this.Viewer.lueReporter.Properties.ValueColumn = EntityCodeOperator.Columns.operCode;
            this.Viewer.lueReporter.Properties.DisplayColumn = EntityCodeOperator.Columns.operName;
            this.Viewer.lueReporter.Properties.Essential = false;
            this.Viewer.lueReporter.Properties.IsShowColumnHeaders = true;
            this.Viewer.lueReporter.Properties.ColumnWidth.Add(EntityCodeOperator.Columns.operCode, 70);
            this.Viewer.lueReporter.Properties.ColumnWidth.Add(EntityCodeOperator.Columns.operName, 85);
            this.Viewer.lueReporter.Properties.ColumnHeaders.Add(EntityCodeOperator.Columns.operCode, "编码");
            this.Viewer.lueReporter.Properties.ColumnHeaders.Add(EntityCodeOperator.Columns.operName, "名称");
            this.Viewer.lueReporter.Properties.ShowColumn = EntityCodeOperator.Columns.operCode + "|" + EntityCodeOperator.Columns.operName;
            this.Viewer.lueReporter.Properties.FilterColumn = string.Concat(new string[]
	        {
		        EntityCodeOperator.Columns.operCode, 
		        "|", 
		        EntityCodeOperator.Columns.operName, 
		        "|", 
		        EntityCodeOperator.Columns.pyCode, 
		        "|", 
		        EntityCodeOperator.Columns.wbCode
	        });
            this.Viewer.lueReporter.Properties.IsUseShowColumn = true;

            if (GlobalDic.DataSourceEmployee != null && GlobalDic.DataSourceEmployee.Count > 0)
            {
                this.Viewer.lueReporter.Properties.DataSource = GlobalDic.DataSourceEmployee.ToArray();
            }
            this.Viewer.lueReporter.Properties.SetSize();

            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                dicEventType = proxy.Service.GetEventDicparm(29);
                if (dicEventType != null)
                {
                    foreach (var dic in dicEventType)
                    {
                        Viewer.cboEventType.Properties.Items.Add(dic.Value);
                    }
                }
            }

            DateTime dtmNow = DateTime.Now;
            Viewer.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteEnd.DateTime = dtmNow;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal void Query()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = Viewer.dteStart.Text.Trim();
            string endDate = Viewer.dteEnd.Text.Trim();
            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }

                dicParm.Add(Function.GetParm("reportDate", beginDate + "|" + endDate));
            }

            if (!string.IsNullOrEmpty(Viewer.ucDept.DeptName) && !string.IsNullOrEmpty(Viewer.ucDept.DeptVo.deptCode))
            {
                dicParm.Add(Function.GetParm("deptCode", Viewer.ucDept.DeptVo.deptCode));
            }

            if (!string.IsNullOrEmpty(Viewer.lueReporter.Text))
            {
                dicParm.Add(Function.GetParm("reporter", Viewer.lueReporter.Text));
            }

            if (!string.IsNullOrEmpty(Viewer.cboEventType.Text))
            {
                string typeCode = dicEventType.FirstOrDefault(q => q.Value == Viewer.cboEventType.Text).Key.Trim();
                dicParm.Add(Function.GetParm("eventId", dicEventType.FirstOrDefault(q => q.Value == Viewer.cboEventType.Text).Key.Trim()));
            }

            if (!string.IsNullOrEmpty(Viewer.cboLevel.Text))
            {
                dicParm.Add(Function.GetParm("level", Viewer.cboLevel.Text));
            }

            try
            {
                uiHelper.BeginLoading(Viewer);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    dateScope = beginDate + " ~ " + endDate;
                    datasource = proxy.Service.GetEventListAll(dicParm);
                    Viewer.gcReport.DataSource = datasource;
                    Viewer.lblTip.Text = "事件数：" + datasource.Count.ToString();
                }
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
            }
        }

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityEventDisplay GetRowObject()
        {
            if (Viewer.gvReport.FocusedRowHandle < 0) return null;
            return Viewer.gvReport.GetRow(Viewer.gvReport.FocusedRowHandle) as EntityEventDisplay;
        }
        #endregion

        #region EditEvent
        /// <summary>
        /// EditEvent
        /// </summary>
        internal void EditEvent()
        {
            EntityEventDisplay vo = GetRowObject();

            if (vo != null) this.PopupForm(vo);
        }
        #endregion

        #region DelEvent
        /// <summary>
        /// DelEvent
        /// </summary>
        internal void DelEvent()
        {
            EntityEventDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                if (DialogBox.Msg("确定是否删除当前记录？？", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        if (proxy.Service.DelEvent(Function.Dec(vo.rptId)) > 0)
                        {
                            DialogBox.Msg("删除不良事件记录成功！");
                            this.Query();
                        }
                        else
                        {
                            DialogBox.Msg("删除不良事件记录失败。");
                        }
                    }
                }
            }
        }
        #endregion

        #region PopupForm
        /// <summary>
        /// PopupForm
        /// </summary>
        /// <param name="vo"></param>
        void PopupForm(EntityEventDisplay vo)
        {
            if (vo == null)
            {
                vo = new EntityEventDisplay();
                vo.isNew = true;
            }
            //vo.eventId = Viewer.EventId;
            frmEventEdit frm = new frmEventEdit(vo);

            switch (vo.eventId)
            {
                case "11":
                    Viewer.Text = "医疗安全不良事件";
                    break;
                case "12":
                    Viewer.Text = "医疗器械不良事件";
                    break;
                case "13":
                    Viewer.Text = "药品不良事件";
                    break;
                case "14":
                    Viewer.Text = "护理不良事件";
                    break;
                case "15":
                    Viewer.Text = "输血不良事件记录";
                    break;
                case "16":
                    Viewer.Text = "输血不良事件回报";
                    break;
                case "17":
                    Viewer.Text = "职业暴露登记";
                    break;
                case "18":
                    Viewer.Text = "护理质量异常指标监测报告";
                    break;
                case "19":
                    Viewer.Text = "护理安全(不良)事件(新)";
                    break;
                case "20":
                    Viewer.Text = "护理皮肤损害安全（不良）事件";
                    break;
                case "21":
                    Viewer.Text = "护理皮肤损害（院外）事件";
                    break;
                default:
                    break;
            }

            frm.Text = Viewer.Text;

            frm.ShowDialog();
            if (frm.IsSave)
            {
                this.Query();
            }
        }
        #endregion
    }
}
