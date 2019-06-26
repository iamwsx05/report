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
    public class ctlOutpatientInterview : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmOutpatientInterview Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmOutpatientInterview)child;
        }


        #region NewEvent
        /// <summary>
        /// NewEvent
        /// </summary>
        internal void NewEvent()
        {
            EntityOutpatientInterview vo = GetRowObject();
            if (vo != null)
                this.PopupForm(vo);
            else
                return;
        }
        #endregion

        #region EditInterview
        /// <summary>
        /// EditEvent
        /// </summary>
        internal void EditInterview()
        {
            EntityOutpatientInterview vo = GetRowObject();
            if (vo.rptId > 0)
            {
                if (vo != null) this.PopupForm(vo);
            }
            else
                this.NewEvent();
        }
        #endregion

        #region PopupForm
        /// <summary>
        /// PopupForm
        /// </summary>
        /// <param name="vo"></param>
        void PopupForm(EntityOutpatientInterview vo)
        {
            if (vo == null)
            {
                vo = new EntityOutpatientInterview();
                vo.isNew = true;
            }

            frmInterviewEdit frm = new frmInterviewEdit(vo);
            frm.Text = Viewer.Text;

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
        internal void DelInterview()
        {
            EntityOutpatientInterview vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                if (DialogBox.Msg("确定是否删除当前记录？？", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        if (proxy.Service.DelInterview(Function.Dec(vo.rptId)) > 0)
                        {
                            DialogBox.Msg("删除记录成功！");
                            this.Query();
                        }
                        else
                        {
                            DialogBox.Msg("删除记录失败。");
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
            EntityOutpatientInterview Vo = null;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                Vo = proxy.Service.GetInterviewVo(rptId);
            }
            EntityFormDesign formVo = null;
            EntityEmrPrintTemplate printVo = null;
            using (ProxyFormDesign proxy = new ProxyFormDesign())
            {
                proxy.Service.GetForm(64, out formVo);
                if (formVo == null) return null;
                printVo = proxy.Service.GetFormPrintTemplate(1, formVo.Printtemplateid.ToString());
            }
            DataTable printDataSource = FormTool.GetPrintDataTable(formVo.Layout, Vo.xmlData);
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
            EntityOutpatientInterview vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                XtraReport xr = GetXR(Function.Dec(vo.rptId));
                if (xr != null && xr.DataSource != null)
                {
                    xr.Name = Viewer.Text;
                    uiHelper.Export(xr);
                }
            }

            uiHelper.ExportToXls(Viewer.gvReport);
        }
        #endregion

        #region Print
        /// <summary>
        /// Print
        /// </summary>
        internal void Print()
        {
            EntityOutpatientInterview vo = GetRowObject();
            if (vo != null && Function.Dec(vo.rptId) > 0)
            {
                frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR(Function.Dec(vo.rptId)));
                frm.ShowDialog();
            }
        }
        #endregion
        #endregion


        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            DateTime dtmNow = DateTime.Now;
            Viewer.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteDateEnd.DateTime = dtmNow;

            this.Query();
        }

        #endregion

        //#region Query
        ///// <summary>
        ///// 
        ///// </summary>
        //internal void Query()
        //{
        //    List<EntityParm> dicParm = new List<EntityParm>();
        //    string beginDate = Viewer.dteDateStart.Text.Trim();
        //    string endDate = Viewer.dteDateEnd.Text.Trim();
        //    if (beginDate != string.Empty && endDate != string.Empty)
        //    {
        //        if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
        //        {
        //            DialogBox.Msg("开始时间不能大于结束时间。");
        //            return;
        //        }
        //        dicParm.Add(Function.GetParm("interviewDate", beginDate + "|" + endDate));
        //    }
          
        //    if (Viewer.txtCardNo.Text.Trim() != string.Empty)
        //    {
        //        dicParm.Add(Function.GetParm("cardNo", Viewer.txtCardNo.Text.Trim()));
        //    }
        //    if (Viewer.txtPatName.Text.Trim() != string.Empty)
        //    {
        //        dicParm.Add(Function.GetParm("patName", Viewer.txtPatName.Text.Trim()));
        //    }
           
        //    try
        //    {
        //        uiHelper.BeginLoading(Viewer);
        //        if (dicParm.Count > 0)
        //        {
        //            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
        //            {
        //                List<EntityOutpatientInterview> dataSource = proxy.Service.GetInterviewList(dicParm);
        //                Viewer.gcReport.DataSource = dataSource;
        //            }
        //        }
        //        else
        //            DialogBox.Msg("请输入查询条件。");
        //    }
        //    finally
        //    {
        //        uiHelper.CloseLoading(Viewer);
        //    }
        //}
        //#endregion

        #region Query
        /// <summary>
        /// 
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
                dicParm.Add(Function.GetParm("outDate", beginDate + "|" + endDate));
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
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        List<EntityOutpatientInterview> dataSource = proxy.Service.GetPatInterviewInfo(dicParm);
                        Viewer.gcReport.DataSource = dataSource;
                    }
                }
                else
                    DialogBox.Msg("请输入查询条件。");
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
                
            }

            gv.Invalidate();
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityOutpatientInterview GetRowObject()
        {
            if (Viewer.gvReport.FocusedRowHandle < 0) return null;
            return Viewer.gvReport.GetRow(Viewer.gvReport.FocusedRowHandle) as EntityOutpatientInterview;
        }
        #endregion

        #endregion
    }
}
