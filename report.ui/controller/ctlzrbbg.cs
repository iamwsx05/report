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

namespace Report.Ui.controller
{
    public class ctlZrbbg : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmZrbbgAdvers Viewer = null;
        string OwerDeptCode { get; set; }

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmZrbbgAdvers)child;
        }
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
                case "31":
                    Viewer.Text = "中华人民共和国传染病报告卡";
                    break;
                case "32":
                    Viewer.Text = "传染病报告卡艾滋病性病附卡";
                    break;
                case "33":
                    Viewer.Text = "传染病报告卡(梅毒)附卡";
                    break;
                case "34":
                    Viewer.Text = "肿瘤病例报告卡";
                    break;
                default:
                    break;
            }
            Viewer.gvData.ViewCaption = Viewer.Text + "表格";
            DateTime dtmNow = DateTime.Now;
            Viewer.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteDateEnd.DateTime = dtmNow;

            this.OwerDeptCode = string.Empty;
            //foreach (EntityCodeDepartment item in GlobalLogin.objLogin.lstDept)
            //{
            //    this.OwerDeptCode += "'" + item.deptCode + "',";
            //}
            //if (this.OwerDeptCode != string.Empty) this.OwerDeptCode = this.OwerDeptCode.TrimEnd(',');
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                this.OwerDeptCode = proxy.Service.GetOwerDeptCode(GlobalLogin.objLogin.EmpNo);
            }

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
            //vo.owerDeptCode = this.OwerDeptCode;
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
            if (Viewer.txtRegCode.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("regCode", Viewer.txtRegCode.Text.Trim()));
            }

            if (!GlobalLogin.objLogin.lstRoleID.Contains("00") && 
                !GlobalLogin.objLogin.lstRoleID.Contains("34"))
            {
                dicParm.Add(Function.GetParm("areaStr", this.OwerDeptCode));
                dicParm.Add(Function.GetParm("selfId", GlobalLogin.objLogin.EmpNo));
            }

            try
            {
                uiHelper.BeginLoading(Viewer);
                if (dicParm.Count > 0)
                {
                    dicParm.Add(Function.GetParm("reportId", Viewer.ReportId));
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        List<EntityZrbbgDisplay> dataSource = proxy.Service.GetZrbbgList(dicParm);
                        Viewer.gcData.DataSource = dataSource;
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
                e.Appearance.ForeColor = Color.FromArgb(0, 92, 156);
            }

            int hand = e.RowHandle;
            if (hand < 0) return;
            EntityZrbbgDisplay vo = gv.GetRow(hand) as EntityZrbbgDisplay;
            if (vo.printFlg == 1)
                e.Appearance.ForeColor = Color.Red;
            
            gv.Invalidate();
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityZrbbgDisplay GetRowObject()
        {
            if (Viewer.gvData.FocusedRowHandle < 0) return null;
            return Viewer.gvData.GetRow(Viewer.gvData.FocusedRowHandle) as EntityZrbbgDisplay;
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
            string xmlData = string.Empty;
            EntityZrbbgDisplay vo = GetRowObject();

            if (vo != null)
            {
                this.PopupForm(vo);

                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    xmlData = proxy.Service.GetRegisterZrbygfk(vo.rptId);
                    bool isChecked = (!string.IsNullOrEmpty(xmlData) ? false : true);
                }
                if (!string.IsNullOrEmpty(xmlData))
                {
                    frmZrbygfk frm = new frmZrbygfk(vo.rptId);
                    frm.Show();
                }
            } 
        }
        #endregion

        #region EditRegisterYgfk
        /// <summary>
        /// 
        /// </summary>
        internal void EditRegisterYgfk()
        {
            string xmlData = string.Empty;
            EntityZrbbgDisplay vo = GetRowObject();

            if (vo != null)
            {
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    xmlData = proxy.Service.GetRegisterZrbygfk(vo.rptId);
                    bool isChecked = (!string.IsNullOrEmpty(xmlData) ? false : true);
                }
                frmZrbygfk frm = new frmZrbygfk(vo.rptId);
                frm.ShowDialog();
            } 
        }
        #endregion

        #region DelReport
        /// <summary>
        /// DelReport
        /// </summary>
        internal void DelReport()
        {
            EntityZrbbgDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                if (DialogBox.Msg("确定是否删除当前记录？？", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        if (proxy.Service.DelZrbbg(Function.Dec(vo.rptId)) > 0)
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

        #region Export
        /// <summary>
        /// Export
        /// </summary>
        internal void Export()
        {
            EntityZrbbgDisplay vo = GetRowObject();
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

        #region Exportlist
        /// <summary>
        /// Exportlist
        /// </summary>
        internal void Exportlist()
        {
            uiHelper.ExportToXls(Viewer.gvData);
        }
        #endregion

        #region Print
        /// <summary>
        /// Print
        /// </summary>
        internal void Print()
        {
            EntityZrbbgDisplay vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(vo.rptId)));
                frm.ShowDialog();
            }

            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    proxy.Service.UpdateZrbbgPrintFlg(Function.Dec(vo.rptId));
                }
            }

            this.Query();
        }
        #endregion

        #region PopupForm
        /// <summary>
        /// PopupForm
        /// </summary>
        /// <param name="vo"></param>
        void PopupForm(EntityZrbbgDisplay vo)
        {
            if (vo == null)
            {
                vo = new EntityZrbbgDisplay();
                vo.isNew = true;
            }
            vo.reportId = Viewer.ReportId;
            frmZyrbgkEdit frm = new frmZyrbgkEdit(vo);
            frm.Text = Viewer.Text;

            frm.Show();
            if (frm.IsSave)
            {
                this.Query();
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
            EntityRptZrbbg rptZrbbgVo = null;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                rptZrbbgVo = proxy.Service.GetZrbbg(rptId);
            }
            EntityFormDesign formVo = null;
            EntityEmrPrintTemplate printVo = null;
            using (ProxyFormDesign proxy = new ProxyFormDesign())
            {
                proxy.Service.GetForm((int)rptZrbbgVo.formId, out formVo);
                if (formVo == null) return null;
                printVo = proxy.Service.GetFormPrintTemplate(1,formVo.Printtemplateid.ToString());
            }
            DataTable printDataSource = FormTool.GetPrintDataTable(formVo.Layout, rptZrbbgVo.xmlData);
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

        #endregion

    }
}
