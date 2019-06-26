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
    /// 传染病上报控制类
    /// </summary>
    public class ctlContagion : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmContagion Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmContagion)child;
        }
        #endregion

        #region 变量.属性
        
        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            switch (Viewer.ReportId)
            {
                case "21":
                    Viewer.Text = "3-I.艾滋病病毒感染孕产妇/婚检妇女基本情况登记卡";
                    break;
                case "22":
                    Viewer.Text = "3-II.艾滋病病毒感染孕产妇妊娠及所生婴儿登记卡";
                    break;
                case "23":
                    Viewer.Text = "3-III.艾滋病病毒感染产妇及所生儿童随访登记卡";
                    break;
                case "24":
                    Viewer.Text = "4-I.梅毒感染孕产妇登记卡";
                    break;
                case "25":
                    Viewer.Text = "4-II.艾滋病病毒感染产妇及所生儿童随访登记卡";
                    break;
                case "26":
                    Viewer.Text = "4-III.梅毒感染产妇所生儿童随访登记卡";
                    break;
                default:
                    break;
            }
            Viewer.gvReport.ViewCaption = Viewer.Text + "表格";
            DateTime dtmNow = DateTime.Now;
            Viewer.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteDateEnd.DateTime = dtmNow;
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
            if (Viewer.ucDept.DeptVo != null && !string.IsNullOrEmpty(Viewer.ucDept.DeptVo.deptCode))
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
            try
            {
                uiHelper.BeginLoading(Viewer);
                if (dicParm.Count > 0)
                {
                    dicParm.Add(Function.GetParm("reportId", Viewer.ReportId));
                    using (ProxyContagion proxy = new ProxyContagion())
                    {
                        List<EntityContagionDisplay> dataSource = proxy.Service.GetContagionList(dicParm);
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
                //if (GetFieldValueStr(gv, e.RowHandle, EntityContagionDisplay.Columns.reportType) == "跟踪报告")
                //{
                //    e.Appearance.ForeColor = Color.Crimson;
                //}
                //else
                //{
                e.Appearance.ForeColor = Color.FromArgb(0, 92, 156);
                //}
            }
            gv.Invalidate();
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityContagionDisplay GetRowObject()
        {
            if (Viewer.gvReport.FocusedRowHandle < 0) return null;
            return Viewer.gvReport.GetRow(Viewer.gvReport.FocusedRowHandle) as EntityContagionDisplay;
        }
        #endregion

        #region NewReport
        /// <summary>
        /// NewReport
        /// </summary>
        internal void NewReport()
        {
            this.PopupForm(null);
        }
        #endregion

        #region EditReport
        /// <summary>
        /// EditReport
        /// </summary>
        internal void EditReport()
        {
            EntityContagionDisplay vo = GetRowObject();
            if (vo != null) this.PopupForm(vo);
        }
        #endregion

        #region PopupForm
        /// <summary>
        /// PopupForm
        /// </summary>
        /// <param name="vo"></param>
        void PopupForm(EntityContagionDisplay vo)
        {
            if (vo == null)
            {
                vo = new EntityContagionDisplay();
                vo.isNew = true;
            }
            vo.reportId = Viewer.ReportId;
            frmContagionEdit frm = new frmContagionEdit(vo);
            frm.Text = Viewer.Text;

            using (ProxyContagion proxy = new ProxyContagion())
            {
                if (Function.Int(vo.rptId) > 0)
                {
                    string Role = proxy.Service.GetContagionRole(GlobalLogin.objLogin.EmpNo);
                    frm.blbiSave.Enabled = false;
                    if (Role != string.Empty || vo.SHR == null)
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

        #region DelReport
        /// <summary>
        /// DelReport
        /// </summary>
        internal void DelReport()
        {
            EntityContagionDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                if (DialogBox.Msg("确定是否删除当前记录？？", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (ProxyContagion proxy = new ProxyContagion())
                    {
                        if (proxy.Service.DelContagion(Function.Dec(vo.rptId)) > 0)
                        {
                            DialogBox.Msg("删除传染病记录成功！");
                            this.Query();
                        }
                        else
                        {
                            DialogBox.Msg("删除传染病记录失败。");
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
            EntityRptContagion contagionVo = null;
            using (ProxyContagion proxy = new ProxyContagion())
            {
                contagionVo = proxy.Service.GetContagion(rptId);
            }
            EntityFormDesign formVo = null;
            EntityEmrPrintTemplate printVo = null;
            using (ProxyFormDesign proxy = new ProxyFormDesign())
            {
                proxy.Service.GetForm((int)contagionVo.formId, out formVo);
                if (formVo == null) return null;
                printVo = proxy.Service.GetFormPrintTemplate(1,formVo.Printtemplateid.ToString());
            }
            DataTable printDataSource = FormTool.GetPrintDataTable(formVo.Layout, contagionVo.xmlData);
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
            EntityContagionDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                XtraReport xr = GetXR(Function.Dec(vo.rptId));
                if (xr != null && xr.DataSource != null)
                {
                    xr.Name = Viewer.Text;
                    uiHelper.Export(xr);
                }
            }
        }
        #endregion

        #region Print
        /// <summary>
        /// Print
        /// </summary>
        internal void Print()
        {
            EntityContagionDisplay vo = GetRowObject();
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
