using Common.Controls;
using Common.Entity;
using Common.Utils;
using DevExpress.XtraReports.UI;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    /// <summary>
    /// 不良事件控制类
    /// </summary>
    public class ctlAdverseEvent : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmAdverseEvent Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmAdverseEvent)child;
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 所属科室编码
        /// </summary>
        string OwerDeptCode { get; set; }
        string ZYBLroleQuery { get; set; }
        string RoleQuery { get; set; }
        string RoleEdit { get; set; }
        string RoleDel { get; set; }

        /// <summary>
        /// 全局角色
        /// </summary>
        string PubRoleId { get; set; }

        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            switch (Viewer.EventId)
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
            if (Viewer.EventId == "18")
                Viewer.gvReport.ViewCaption = Viewer.Text;
            else
                Viewer.gvReport.ViewCaption = Viewer.Text + "表格";
            DateTime dtmNow = DateTime.Now;
            Viewer.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteDateEnd.DateTime = dtmNow;

            #region 参数
            using (ProxyEntityFactory proxy = new ProxyEntityFactory())
            {
                List<EntityRptEventParm> lstEventParmData = EntityTools.ConvertToEntityList<EntityRptEventParm>(proxy.Service.SelectFullTable(new EntityRptEventParm()));
                if (lstEventParmData != null)
                {
                    if (lstEventParmData.Any(t => t.eventId == Viewer.EventId && t.keyId == "pubRoleId"))
                    {
                        this.PubRoleId = lstEventParmData.FirstOrDefault(t => t.eventId == this.Viewer.EventId && t.keyId == "pubRoleId").keyValue;
                    }
                }
            }

            this.OwerDeptCode = string.Empty;
            foreach (EntityCodeDepartment item in GlobalLogin.objLogin.lstDept)
            {
                this.OwerDeptCode += "'" + item.deptCode + "',";
            }
            if (this.OwerDeptCode != string.Empty) this.OwerDeptCode = this.OwerDeptCode.TrimEnd(',');

            this.RoleEdit = string.Empty;
            this.RoleDel = string.Empty;
            this.RoleQuery = string.Empty;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                this.RoleDel = proxy.Service.GetEventRoleDel(GlobalLogin.objLogin.EmpNo);
                this.RoleEdit = proxy.Service.GetEventRoleEdit(GlobalLogin.objLogin.EmpNo);
                this.RoleQuery = proxy.Service.GetEventRoleQuery(GlobalLogin.objLogin.EmpNo);
                this.ZYBLroleQuery = proxy.Service.GetZYBLEventRoleLimt(GlobalLogin.objLogin.EmpNo);
            }

            #endregion

            this.Query();
        }
        #endregion

        #region Query
        /// <summary>
        /// Query
        /// </summary>
        internal void Query()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = Viewer.dteDateStart.Text.Trim();
            string endDate = Viewer.dteDateEnd.Text.Trim();
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
            if (Viewer.txtCardNo.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("cardNo", Viewer.txtCardNo.Text.Trim()));
            }
            if (Viewer.txtPatName.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("patName", Viewer.txtPatName.Text.Trim()));
            }
            if (this.PubRoleId != string.Empty && GlobalLogin.objLogin.lstRoleID.IndexOf(this.PubRoleId) >= 0)
            {
                //isPub = true;
            }
            if (this.RoleDel != string.Empty || this.RoleQuery != string.Empty)
            {
            }
            else if ((Viewer.EventId == "14" || Viewer.EventId == "18" ||
                Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21") &&
                (this.RoleEdit != string.Empty))//护理部 护理安全事件
            {
            }
            else if (Viewer.EventId == "17" && this.ZYBLroleQuery != string.Empty)
            {
            }
            else if ((this.RoleEdit != string.Empty) || (Viewer.EventId == "14" || Viewer.EventId == "18" ||
                Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21"))
            {
                dicParm.Add(Function.GetParm("areaStr", this.OwerDeptCode));
                dicParm.Add(Function.GetParm("selfId", GlobalLogin.objLogin.EmpNo));
            }
            else  //查看自己的事件
            {
                dicParm.Add(Function.GetParm("limitId", GlobalLogin.objLogin.EmpNo));
            }

            try
            {
                uiHelper.BeginLoading(Viewer);
                if (dicParm.Count > 0)
                {
                    dicParm.Add(Function.GetParm("eventId", Viewer.EventId));
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        List<EntityEventDisplay> dataSource = proxy.Service.GetEventList(dicParm);
                        Viewer.gcReport.DataSource = dataSource;
                    }
                }
                else
                {
                    DialogBox.Msg("请输入查询条件。");
                }
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
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
            else
            {
                if (GetFieldValueStr(gv, e.RowHandle, EntityEventDisplay.Columns.reportType) == "跟踪报告")
                {
                    e.Appearance.ForeColor = Color.Crimson;
                }
                else if (Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21")
                {
                    e.Appearance.ForeColor = Color.FromArgb(0, 0, 0);
                }
                else
                {
                    e.Appearance.ForeColor = Color.FromArgb(0, 92, 156);
                }
            }

            int hand = e.RowHandle;
            if (hand < 0) return;
            EntityEventDisplay vo = gv.GetRow(hand) as EntityEventDisplay;
            //护理安全表单审核
            if (vo.HLQM != "" && (Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21"))
                e.Appearance.ForeColor = Color.Green;
            else if (vo.XZQM != "" && (Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21"))
                e.Appearance.ForeColor = Color.Purple;
            else if (vo.HCQM != "" && (Viewer.EventId == "19" || Viewer.EventId == "20" || Viewer.EventId == "21"))
                e.Appearance.ForeColor = Color.FromArgb(0, 0, 156);

            gv.Invalidate();
        }
        #endregion

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

        #region NewEvent
        /// <summary>
        /// NewEvent
        /// </summary>
        internal void NewEvent()
        {
            this.PopupForm(null);
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
            vo.eventId = Viewer.EventId;
            vo.owerDeptCode = this.OwerDeptCode;
            vo.pubRoleId = this.PubRoleId;
            frmEventEdit frm = new frmEventEdit(vo);
            frm.Text = Viewer.Text;

            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                if (Function.Int(vo.rptId) > 0)
                {
                    string RoleEdit = proxy.Service.GetEventRoleEdit(GlobalLogin.objLogin.EmpNo);
                    string RoleDel = proxy.Service.GetEventRoleDel(GlobalLogin.objLogin.EmpNo);
                    frm.blbiSave.Enabled = false;
                    if (RoleEdit != string.Empty || RoleDel != string.Empty)
                    {
                        frm.blbiSave.Enabled = true;
                    }
                }
            }

            frm.ShowDialog();
            if (frm.IsSave)
            {
                this.Query();
            }
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

        #region XR
        /// <summary>
        /// GetXR
        /// </summary>
        /// <returns></returns>
        XtraReport GetXR(decimal rptId)
        {
            EntityRptEvent eventVo = null;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                eventVo = proxy.Service.GetEvent(rptId);
            }
            EntityFormDesign formVo = null;
            EntityEmrPrintTemplate printVo = null;
            using (ProxyFormDesign proxy = new ProxyFormDesign())
            {
                proxy.Service.GetForm((int)eventVo.formId, out formVo);
                if (formVo == null) return null;
                printVo = proxy.Service.GetFormPrintTemplate(1, formVo.Printtemplateid.ToString());
            }
            DataTable printDataSource = FormTool.GetPrintDataTable(formVo.Layout, eventVo.xmlData);
            if (printVo.templateFile != null && printVo.templateFile.Length > 0)
            {
                XtraReport xr = new XtraReport();
                MemoryStream stream = new MemoryStream(printVo.templateFile);
                xr.LoadLayout(stream);
                xr.DataSource = printDataSource;
                return xr;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Export
        /// <summary>
        /// Export
        /// </summary>
        internal void Export()
        {
            //EntityEventDisplay vo = GetRowObject();
            //if (vo != null && Function.Dec(vo.rptId) > 0)
            //{
            //    XtraReport xr = GetXR(Function.Dec(vo.rptId));
            //    if (xr != null && xr.DataSource != null)
            //    {
            //        xr.Name = Viewer.Text;
            //        uiHelper.Export(xr);
            //    }
            //}

            uiHelper.ExportToXls(Viewer.gvReport);
        }
        #endregion

        #region Print
        /// <summary>
        /// Print
        /// </summary>
        internal void Print()
        {
            EntityEventDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(vo.rptId)));
                frm.ShowDialog();
            }
        }
        #endregion

        #endregion
    }
}
