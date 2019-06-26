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
using System.Text;

namespace Report.Ui
{
    public class ctlFirstPageUpload : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmSbFirstPageUp Viewer = null;
        EntityPatUpload vo = null;
        EntityDGExtra exVo = null;
        List<EntityPatUpload> dataSource = null;
        List<EntityPatUpload> lstVo = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmSbFirstPageUp)child;
        }
        #endregion

        #region 方法

        internal void Export()
        {
            uiHelper.ExportToXls(Viewer.gvData);
        }

        internal void Print()
        {
            uiHelper.Print(Viewer.gcData);
        }
        

        #region Query
        /// <summary>
        /// </summary>
        internal void Query()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = string.Empty;
            string endDate = string.Empty;
            beginDate = Viewer.dteStart.Text.Trim();
            endDate = Viewer.dteEnd.Text.Trim();

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParm.Add(Function.GetParm("queryDate", beginDate + "|" + endDate));
            }
            if (Viewer.txtFindJzjlh.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("JZJLH", Viewer.txtFindJzjlh.Text.Trim()));
            }
            if (Viewer.txtCardNo.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("cardNo", Viewer.txtCardNo.Text.Trim()));
            }
            if(Viewer.chkSZ.Checked == true)
            {
                dicParm.Add(Function.GetParm("chkStat", Viewer.chkSZ.CheckState.ToString()));
            }
            try
            {
                uiHelper.BeginLoading(Viewer);
                if (dicParm.Count > 0)
                {
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        dataSource = proxy.Service.GetPatList(dicParm, Viewer.rdoType.SelectedIndex + 1);
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
        internal void Edit()
        {
            vo = GetRowObject();
        }

        internal void Apply()
        {
            string msg = string.Empty;
            int failCount = 0;
            int successCount = 0;

            if (Viewer.rdoType.SelectedIndex == 0)//病案首页
                MthFirstPageUpload();
            else if (Viewer.rdoType.SelectedIndex == 1)//出院小结
                MthCyxjUpload();
            foreach (EntityPatUpload item in lstVo)
            {
                if (item.Issucess == -1)
                {
                    failCount++;
                    msg += item.FailMsg + "\n";
                }
                else if (item.Issucess == 1)
                    successCount++;
            }
            msg = "上传成功：" + successCount.ToString() + "   上传失败：" + failCount.ToString() + "\n\n" + msg;
            MessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
            this.Query();
        }

       

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public void MthInit()
        {
            DateTime dtmNow = DateTime.Now;
            Viewer.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteEnd.DateTime = dtmNow;
            exVo = new EntityDGExtra();
            exVo.YYBH = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
            exVo.FWSJGDM = ctlUploadSbPublic.strReadXML("DGCSZYYB", "FWSJGDM", "AnyOne");
            exVo.JBR = GlobalLogin.objLogin.EmpNo;// 操作员工号
            string strPwd = ctlUploadSbPublic.strReadXML("DGCSZYYB", "PASSWORDZY", "AnyOne");
        }
        #endregion

        #region 首页数据上传
        /// <summary>
        /// 数据上传
        /// </summary>
        public void MthFirstPageUpload()
        {
            long lngRes = -1;
            try
            {
                uiHelper.BeginLoading(Viewer);

                string strUser = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                string strPwd = ctlUploadSbPublic.strReadXML("DGCSZYYB", "PASSWORDZY", "AnyOne");
                lngRes = ctlUploadSbPublic.lngUserLoin(strUser, strPwd, false);
                if (lngRes > 0)
                {
                    EntityDGExtra extraVo = new EntityDGExtra();
                    extraVo.YYBH = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                    extraVo.JBR = ctlUploadSbPublic.strReadXML("DGCSZYYB", "JBR", "AnyOne"); ;// 操作员工号
                    extraVo.FWSJGDM = ctlUploadSbPublic.strReadXML("DGCSZYYB", "FWSJGDM", "AnyOne");
                    System.Text.StringBuilder strValue = null;
                    lstVo = GetLstRowObject();
                    
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        lstVo = proxy.Service.GetPatFirstInfo(lstVo);
                    }
                    
                    lngRes = ctlUploadSbPublic.lngFunSP3_3021(ref lstVo, extraVo, ref strValue);
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        if (proxy.Service.SavePatFirstPage(lstVo) >= 0)
                        {
                            lngRes = 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
            }
        }
        #endregion

        #region 出院小结数据上传
        /// <summary>
        /// 数据上传
        /// </summary>
        public void MthCyxjUpload()
        {
            try
            {
                uiHelper.BeginLoading(Viewer);
                long lngRes = 1;

                string strUser = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                string strPwd = ctlUploadSbPublic.strReadXML("DGCSZYYB", "PASSWORDZY", "AnyOne");
                lngRes = ctlUploadSbPublic.lngUserLoin(strUser, strPwd, false);
                if (lngRes > 0)
                {
                    EntityDGExtra extraVo = new EntityDGExtra();
                    extraVo.YYBH = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                    extraVo.JBR = ctlUploadSbPublic.strReadXML("DGCSZYYB", "JBR", "AnyOne");// 操作员工号
                    System.Text.StringBuilder strValue = null;
                    lstVo = GetLstRowObject();
                    lngRes = ctlUploadSbPublic.lngFunSP3_3022(ref lstVo, extraVo, ref strValue);
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        if (proxy.Service.SavePatFirstPage(lstVo) >= 0)
                        {
                            lngRes = 1;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ExceptionLog.OutPutException(ex);
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
            }
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityPatUpload GetRowObject()
        {
            if (Viewer.gvData.FocusedRowHandle < 0) return null;
            return Viewer.gvData.GetRow(Viewer.gvData.FocusedRowHandle) as EntityPatUpload;
        }
        #endregion

        #region
        List<EntityPatUpload> GetLstRowObject()
        {
            List<EntityPatUpload> data = new List<EntityPatUpload>();
            EntityPatUpload  vo = null;
            string value = string.Empty;

            int[] rownumber = this.Viewer.gvData.GetSelectedRows();//获取选中行号；
            for (int i = 0; i < rownumber.Length; i++)
            {
                vo = Viewer.gvData.GetRow(rownumber[i]) as EntityPatUpload;
                data.Add(vo);
            }
            return data;
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

            int hand = e.RowHandle;
            if (hand < 0) return;
            EntityPatUpload vo = gv.GetRow(hand) as EntityPatUpload;
            if (vo.SZ == "已上传" )
                e.Appearance.ForeColor = Color.FromArgb(0, 0, 156);

            gv.Invalidate();
        }
        #endregion

        #endregion

      
    }
}

