using Common.Controls;
using Report.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using weCare.Core.Entity;
using weCare.Core.Utils;

namespace Report.Ui.controller
{
    public class ctlYbUpLoadAuto : BaseController
    {
        List<EntityPatUpload> dataSourceF = null;
        List<EntityMzcf> dataSourceM = null;
        System.Timers.Timer timer;
        System.Timers.Timer timer1;
        string lastStatusF = string.Empty;
        string lastStatusM = string.Empty;

        #region 
        public void Init(int flg)
        {
            timer = new System.Timers.Timer();
            timer1 = new System.Timers.Timer();
        }
        #endregion

        #region QueryFirst
        /// <summary>
        /// flg 1.病案首页 2.出院小结
        /// </summary>
        internal void QueryFirst(int flg)
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = string.Empty;
            string endDate = string.Empty;
            DateTime dtmNow = DateTime.Now;
            string DTETYPE = ctlUploadSbPublic.strReadXML("YBFIRSTPARAM", "FDTETYPE", "AnyOne");
            string DTESTAR = ctlUploadSbPublic.strReadXML("YBFIRSTPARAM", "FDTESTAR", "AnyOne");
            string DTEEND = ctlUploadSbPublic.strReadXML("YBFIRSTPARAM", "FDTEEND", "AnyOne");
            string STATUS = ctlUploadSbPublic.strReadXML("YBFIRSTPARAM", "FSTATUS", "AnyOne");

            if (DTETYPE == "2")
            {
                beginDate = DTESTAR;
                endDate = DTEEND;
            }
            else if (DTETYPE == "1")
            {
                beginDate = dtmNow.ToString("yyyy-MM-dd");
                endDate = dtmNow.ToString("yyyy-MM-dd");
            }

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParm.Add(Function.GetParm("queryDate", beginDate + "|" + endDate));
            }
            try
            {
                if (dicParm.Count > 0)
                {
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        dataSourceF = proxy.Service.GetPatList(dicParm, flg);
                    }
                }
            }
            finally
            {

            }
        }
        #endregion

        #region 门诊处方项目明细查询
        /// <summary>
        /// </summary>
        internal void QueryMzcf()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = string.Empty;
            string endDate = string.Empty;
            DateTime dtmNow = DateTime.Now;
            string DTETYPE = ctlUploadSbPublic.strReadXML("YBMZCFPARAM", "MDTETYPE", "AnyOne");
            string DTESTAR = ctlUploadSbPublic.strReadXML("YBMZCFPARAM", "MDTESTAR", "AnyOne");
            string DTEEND = ctlUploadSbPublic.strReadXML("YBMZCFPARAM", "MDTEEND", "AnyOne");
            string STATUS = ctlUploadSbPublic.strReadXML("YBMZCFPARAM", "MSTATUS", "AnyOne");

            if (DTETYPE == "2")
            {
                beginDate = DTESTAR;
                endDate = DTEEND;
            }
            else if (DTETYPE == "1")
            {
                beginDate = dtmNow.ToString("yyyy-MM-dd");
                endDate = dtmNow.ToString("yyyy-MM-dd");
            }

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParm.Add(Function.GetParm("queryDate", beginDate + "|" + endDate));
            }
            try
            {
                if (dicParm.Count > 0)
                {
                    using (ProxyUploadSb proxy = new ProxyUploadSb())
                    {
                        dataSourceM = proxy.Service.GetPatMzchList(dicParm);

                        if (proxy.Service.SaveUpPatMzcf(dataSourceM) >= 0)
                        {
                            List<EntityMzcf> data = new List<EntityMzcf>();
                            EntityMzcf vo = null;
                            for (int i = 0; i < dataSourceM.Count; i++)
                            {
                                vo = dataSourceM[i];

                                if (vo.lstCfMsg == null)
                                {
                                   vo.lstCfMsg = proxy.Service.GetPatMzcfMsgList(vo.CFH);
                                }
                                data.Add(vo);
                            }
                            dataSourceM = data;
                        }
                    }
                }
            }
            finally
            {
            }
        }
        #endregion

        #region 首页数据上传
        /// <summary>
        /// 数据上传
        /// </summary>
        public void MthFirstPageUpload()
        {
            long lngRes = -1;

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

                using (ProxyUploadSb proxy = new ProxyUploadSb())
                {
                    dataSourceF = proxy.Service.GetPatFirstInfo(dataSourceF);
                }

                lngRes = ctlUploadSbPublic.lngFunSP3_3021(ref dataSourceF, extraVo, ref strValue);
                using (ProxyUploadSb proxy = new ProxyUploadSb())
                {
                    if (proxy.Service.SavePatFirstPage(dataSourceF) >= 0)
                    {
                        lngRes = 1;
                    }
                }
            }
        }
        #endregion

        #region 出院小结数据上传
        /// <summary>
        /// 数据上传
        /// </summary>
        public void MthCyxjUpload()
        {
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

                lngRes = ctlUploadSbPublic.lngFunSP3_3022(ref dataSourceF, extraVo, ref strValue);
                using (ProxyUploadSb proxy = new ProxyUploadSb())
                {
                    if (proxy.Service.SavePatFirstPage(dataSourceF) >= 0)
                    {
                        lngRes = 1;
                    }
                }
            }
        }
        #endregion

        
    }
}
