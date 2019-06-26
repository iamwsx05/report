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
    public class ctlSbMzchxmUpload : BaseController
    {
        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmSbMzcfxmdr Viewer = null;

        List<EntityMzcf> dataSource = null;
        //List<EntityMzcfMsg> lstMzcfMsg = null;
        List<EntityMzcf> lstVo = null;
        

        #region
        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmSbMzcfxmdr)child;
        }
        #endregion


        internal void Export()
        {
            uiHelper.ExportToXls(Viewer.gvData);
        }

        internal  void Print()
        {
            uiHelper.Print(Viewer.gcData);
        }
        

        #region 方法

        #region 上传
        internal void Init()
        {
            DateTime dtmNow = DateTime.Now;
            Viewer.dteStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            Viewer.dteEnd.DateTime = dtmNow;
        }

        internal void Apply()
        {
            string msg = string.Empty;
            int failCount = 0;
            int successCount = 0;
            MthMzcfxmdr();
            foreach (EntityMzcf item in lstVo)
            {
                if (item.IsSuccess == -1)
                {
                    failCount++;
                    msg += item.failMsg + "\n";
                }
                else if (item.IsSuccess == 1)
                    successCount++;
            }
            msg = "上传成功：" + successCount.ToString() + "   上传失败：" + failCount.ToString() + "\n\n" + msg;
            MessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        #endregion


      

        #region 删除
        internal void Delete()
        {
            string msg = string.Empty;
            int failCount = 0;
            int successCount = 0;
            MthMzcfxmdrDel();
            foreach (EntityMzcf item in lstVo)
            {
                if (item.IsSuccess == -1)
                {
                    failCount++;
                    msg += item.failMsg + "\n";
                }
                else if (item.IsSuccess == 1)
                    successCount++;
            }
            msg = "删除成功：" + successCount.ToString() + "   删除失败：" + failCount.ToString() + "\n\n" + msg;
            MessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        #endregion

        #region 首页数据上传
        /// <summary>
        /// 数据上传
        /// 0 自动上传 1 手动上传
        /// </summary>
        public void MthMzcfxmdr()
        {
            long lngRes = -1;

            string strUser = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
            string strPwd = ctlUploadSbPublic.strReadXML("DGCSZYYB", "PASSWORDZY", "AnyOne");
            lngRes = ctlUploadSbPublic.lngUserLoin(strUser, strPwd, false);
            if (lngRes > 0)
            {
                EntityDGExtra extraVo = new EntityDGExtra();
                extraVo.YYBH = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                extraVo.JBR = ctlUploadSbPublic.strReadXML("DGCSZYYB", "JBR", "AnyOne");// 操作员工号
                extraVo.FWSJGDM = ctlUploadSbPublic.strReadXML("DGCSZYYB", "FWSJGDM", "AnyOne");
                System.Text.StringBuilder strValue = null;
               
                lstVo = GetLstRowObject();
                lngRes = ctlUploadSbPublic.lngFunSP3_2002(ref lstVo, extraVo, ref strValue);

                using (ProxyUploadSb proxy = new ProxyUploadSb())
                {
                    if (proxy.Service.SaveUpPatMzcf(lstVo) >= 0)
                    {
                        lngRes = 1;
                    }
                }
            }
        }
        #endregion

        #region 门诊处方删除
        /// <summary>
        /// 门诊处方删除
        /// </summary>
        public void MthMzcfxmdrDel()
        {
            long lngRes = -1;

            string strUser = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
            string strPwd = ctlUploadSbPublic.strReadXML("DGCSZYYB", "PASSWORDZY", "AnyOne");
            lngRes = ctlUploadSbPublic.lngUserLoin(strUser, strPwd, false);
            if (lngRes > 0)
            {
                EntityDGExtra extraVo = new EntityDGExtra();
                extraVo.YYBH = ctlUploadSbPublic.strReadXML("DGCSZYYB", "YYBHZY", "AnyOne");
                extraVo.JBR = ctlUploadSbPublic.strReadXML("DGCSZYYB", "JBR", "AnyOne");// 操作员工号
                extraVo.FWSJGDM = ctlUploadSbPublic.strReadXML("DGCSZYYB", "FWSJGDM", "AnyOne");
                System.Text.StringBuilder strValue = null;
                lstVo = GetLstRowObject();

                lngRes = ctlUploadSbPublic.lngFunSP3_2003(lstVo, extraVo, ref strValue);

                using (ProxyUploadSb proxy = new ProxyUploadSb())
                {
                    if (proxy.Service.SaveUpPatMzcf(lstVo) >= 0)
                    {
                        lngRes = 1;
                    }
                }
            }
        }
        #endregion

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
            if (Viewer.txtCardNo.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("cardNo", Viewer.txtCardNo.Text.Trim()));
            }
            if (Viewer.chkSZ.Checked == true)
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
                        dataSource = proxy.Service.GetPatMzchList(dicParm);
                        Viewer.gcCfData.DataSource = dataSource;
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

        #region Edit
        internal void Edit()
        {
            EntityMzcf vo = null;
            vo = GetRowObject();

            for (int i = 0; i < dataSource.Count; i++ )
            {
                if (dataSource[i] == vo)
                {
                    if (dataSource[i].lstCfMsg == null)
                    {
                        using (ProxyUploadSb proxy = new ProxyUploadSb())
                        {
                            dataSource[i].lstCfMsg = proxy.Service.GetPatMzcfMsgList(vo.CFH);
                        }
                    }

                    Viewer.gcCfMsgData.DataSource = dataSource[i].lstCfMsg;
                    break;

                }
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
            int hand = e.RowHandle;
            if (hand < 0) return;
            EntityMzcf vo = gv.GetRow(hand) as EntityMzcf;
            if (vo.SZ == "已上传")
                e.Appearance.ForeColor = Color.FromArgb(0, 0, 156);
            gv.Invalidate();
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityMzcf GetRowObject()
        {
            if (Viewer.gvCfData.FocusedRowHandle < 0) return null;
            return Viewer.gvCfData.GetRow(Viewer.gvCfData.FocusedRowHandle) as EntityMzcf;
        }
        #endregion

        #region GetLstRowObject
        List<EntityMzcf> GetLstRowObject()
        {
            List<EntityMzcf> data = new List<EntityMzcf>();
            EntityMzcf vo = null;
            string value = string.Empty;

            int[] rownumber = this.Viewer.gvCfData.GetSelectedRows();//获取选中行号；
            for (int i = 0; i < rownumber.Length; i++)
            {
                vo = Viewer.gvCfData.GetRow(rownumber[i]) as EntityMzcf;
                if(vo.lstCfMsg == null)
                {
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        vo.lstCfMsg = proxy.Service.GetPatMzcfMsgList(vo.CFH);
                    }
                }
                data.Add(vo);
            }
            return data;
        }
        #endregion

        
    }
}
