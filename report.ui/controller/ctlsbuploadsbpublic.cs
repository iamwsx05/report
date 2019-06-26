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
using System.Runtime.InteropServices;
using System.Text;

namespace Report.Ui
{
    public class ctlUploadSbPublic : BaseController
    {

        #region 接口函数
        /**初始化，在调用DLL前，初始化调用环境变量。整个调用工程只需调用该函数一次即可。
         * LPTSTR svrIP：代理服务器的ＩＰ地址。
         * USHORT svrPort： 代理服务器的监听端口。
         * SndBufSize：socket发送缓存大小。
         * RecvBufSize：socket接收缓存大小。
         * Return:HRESULT 1表示成功；-11表示系统初始化失败。*/
        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "Initialize")]
        public static extern int Initialize(string svrIP, int svrPort, int SndBuffSize, int RecvBuffSize);

        /**创建实例
         * 创建一个功能调用实例。在进行一个新的功能调用前必须执行该操作，以取得调用的处理句柄。返回的句柄将成为其他功能调用的入口参数。
         * Return:HANDLE 大于0的LONG型值表示创建成功；-13表示创建失败。*/
        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "CreateInstace")]
        public static extern int CreateInstace();

        /**设置入参
         * 提供功能调用的参数组，比如功能号以及其他功能的调用参数。（功能号的paramName规定为“FN”）
         * HANDLE pDataHandle：功能调用的处理句柄，由接口函数CreateInstace()创建。
         * LPCTSTR paramName：参数名称。
         * LPCTSTR paramValue：参数值。
         * Return:HRESULT 成功返回1；失败返回-14，详细的错误信息可以通过调用GetSysMessage()取得
         */
        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "SetParam")]
        public static extern int SetParam(int pDataHandle, string paramName, string paramValue);

        /**数据集*/
        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "InsertDataSet")]
        public static extern int InsertDataSet(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "InsertRow")]
        public static extern int InsertRow(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "SetField")]
        public static extern int SetField(int pDataHandle, string fieldName, string fieldValue);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "EndRow")]
        public static extern int EndRow(int pDataHandle, int rowID);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "EndDataSet")]
        public static extern int EndDataSet(int pDataHandle, string name);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "Run")]
        public static extern int Run(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetParam")]
        public static extern int GetParam(int pDataHandle, string paramName, StringBuilder paramValue, int nMaxValueLenth);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "LocateDataSet")]
        public static extern int LocateDataSet(int pDataHandle, string Name);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetRowSize")]
        public static extern int GetRowSize(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "NextRow")]
        public static extern int NextRow(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetCurrentRow")]
        public static extern int GetCurrentRow(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetColSize")]
        public static extern int GetColSize(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetFieldValue")]
        public static extern int GetFieldValue(int pDataHandle, string name, StringBuilder value, int nMaxValueLenth);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "DestroyInstance")]
        public static extern int DestroyInstance(int pDataHandle);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "GetSysMessage")]
        public static extern int GetSysMessage(int pDataHandle, StringBuilder pMassage, int nMaxMessage);

        [DllImport("HNBridge.dll", CallingConvention = CallingConvention.Winapi, EntryPoint = "EncryptWithCipher")]
        public static extern int EncryptWithCipher(int pDataHandle, string PlainData, StringBuilder EncryptedData, int nMaxValueLenth);
        #endregion

        #region 病案首页上传[SP3_3021]
        /// <summary>
        /// 病案首页上传[SP3_3021]
        /// </summary>
        /// <param name="lstVo"></param>
        /// <param name="exVo"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static long lngFunSP3_3021(ref List<EntityPatUpload> lstVo, EntityDGExtra exVo, ref StringBuilder strValue)
        {
            long lngRes = -1;
            int intRet;
            int j = 0;
            int intH = 0 ;
            if (lstVo.Count > 0)
            {
                foreach (EntityPatUpload item in lstVo)
                {
                    intH = CreateInstace();
                    
                    if (intH > 0)
                    {
                        intRet = SetParam(intH, "FN", "SP3_3021");
                        intRet = SetParam(intH, "JBR", exVo.JBR);
                        intRet = SetParam(intH, "YYBH", exVo.YYBH);
                    }
                    try 
                    {
                        item.JBR = exVo.JBR;
                        
                        #region 入参
                        Log.Output("JZJLH:" + item.fpVo.JZJLH.ToString().Trim()//
                        + "\nFWSJGDM:" + exVo.FWSJGDM
                        +"\nFFBBHNEW:" + item.fpVo.FFBBHNEW.ToString().Trim() //
                        +"\nFFBNEW:" + item.fpVo.FFBNEW.ToString().Trim() 
                        +"\nFASCARD1:" + item.fpVo.FASCARD1.ToString().Trim() 
                        +"\nFTIMES:" + item.fpVo.FTIMES.ToString().Trim() 
                        +"\nFPRN:" + item.fpVo.FPRN.ToString().Trim() 
                        +"\nFNAME:" + item.fpVo.FNAME.ToString().Trim() 
                        +"\nFSEXBH:" + item.fpVo.FSEXBH.ToString().Trim() 
                        +"\nFSEX:" + item.fpVo.FSEX.ToString().Trim() //
                        +"\nFBIRTHDAY:" + item.fpVo.FBIRTHDAY.ToString() //
                        +"\nFAGE:" + item.fpVo.FAGE.ToString() 
                        +"\nfcountrybh:" + item.fpVo.fcountrybh.ToString() 
                        +"\nfcountry:" + item.fpVo.fcountry.ToString() 
                        +"\nfnationalitybh:" + item.fpVo.fnationalitybh.ToString() 
                        +"\nfnationality:" + item.fpVo.fnationality.ToString() 
                        +"\nFCSTZ:" + (Function.Dec(item.fpVo.FCSTZ) / 1000).ToString("0.00") 
                        +"\nFRYTZ:" + (Function.Dec(item.fpVo.FRYTZ) / 1000).ToString("0.00") 
                        +"\nFBIRTHPLACE:" + item.fpVo.FBIRTHPLACE.ToString().Trim() 
                        +"\nFNATIVE:" + item.fpVo.FNATIVE.ToString().Trim() 
                        +"\nFIDCard:" + item.fpVo.FIDCard 
                        +"\nFJOB:" + item.fpVo.FJOB.ToString().Trim() 
                        +"\nFSTATUSBH:" + item.fpVo.FSTATUSBH.ToString().Trim() 
                        +"\nFSTATUS:" + item.fpVo.FSTATUS.ToString().Trim() 
                        +"\nFCURRADDR:" + item.fpVo.FCURRADDR.ToString().Trim() 
                        +"\nFCURRTELE:" + item.fpVo.FCURRTELE 
                        +"\nFCURRPOST:" + item.fpVo.FCURRPOST.ToString().Trim() 
                        +"\nFHKADDR:" + item.fpVo.FHKADDR.ToString().Trim() 
                        +"\nFHKPOST:" + item.fpVo.FHKPOST.ToString().Trim() 
                        +"\nFDWNAME:" + item.fpVo.FDWNAME.ToString().Trim() 
                        +"\nFDWADDR:" + item.fpVo.FDWADDR 
                        +"\nFDWTELE:" + item.fpVo.FDWTELE.ToString().Trim() 
                        +"\nFDWPOST:" + item.fpVo.FDWPOST.ToString().Trim() 
                        +"\nFLXNAME:" + item.fpVo.FLXNAME.ToString().Trim() 
                        +"\nFRELATE:" + item.fpVo.FRELATE.ToString().Trim() 
                        +"\nFLXADDR:" + item.fpVo.FLXADDR 
                        +"\nFLXTELE:" + item.fpVo.FLXTELE.ToString().Trim() 
                        +"\nFRYTJBH:" + item.fpVo.FRYTJBH.ToString().Trim() 
                        +"\nFRYTJ:" + item.fpVo.FRYTJ.ToString().Trim() 
                        +"\nFRYDATE:" + item.fpVo.FRYDATE.ToString().Trim() 
                        +"\nFRYTIME:" + item.fpVo.FRYTIME.ToString() 
                        +"\nFRYTYKH:" + item.fpVo.FRYTYKH.ToString().Trim() 
                        +"\nFRYDEPT:" + item.fpVo.FRYDEPT.ToString().Trim() 
                        +"\nFRYBS:" + item.fpVo.FRYBS.ToString().Trim() 
                        +"\nFZKTYKH:" + item.fpVo.FZKTYKH.ToString().Trim() 
                        +"\nFZKDEPT:" + item.fpVo.FZKDEPT 
                        +"\nFZKTIME:" + item.fpVo.FZKTIME.ToString() 
                        +"\nFCYDATE:" + item.fpVo.FCYDATE.ToString().Trim() 
                        +"\nFCYTIME:" + item.fpVo.FCYTIME.ToString().Trim() 
                        +"\nFCYTYKH:" + item.fpVo.FCYTYKH.ToString().Trim() 
                        +"\nFCYDEPT:" + item.fpVo.FCYDEPT 
                        +"\nFCYBS:" + item.fpVo.FCYBS.ToString().Trim() 
                        +"\nFDAYS:" + item.fpVo.FDAYS.ToString().Trim() 
                        +"\nFMZZDBH:" + item.fpVo.FMZZDBH.ToString().Trim() 
                        +"\nFMZZD:" + item.fpVo.FMZZD.ToString().Trim() 
                        +"\nFMZDOCTBH:" + item.fpVo.FMZDOCTBH
                        +"\nFMZDOCT:" + item.fpVo.FMZDOCT.ToString().Trim() 
                        +"\nFJBFXBH:" + item.fpVo.FJBFXBH.ToString().Trim() 
                        +"\nFJBFX:" + item.fpVo.FJBFX.ToString().Trim() 
                        +"\nFYCLJBH:" + item.fpVo.FYCLJBH 
                        +"\nFYCLJ:" + item.fpVo.FYCLJ.ToString().Trim() 
                        +"\nFQJTIMES:" + item.fpVo.FQJTIMES.ToString().Trim() 
                        +"\nFQJSUCTIMES:" + item.fpVo.FQJSUCTIMES.ToString().Trim() 
                        +"\nFPHZD:" + item.fpVo.FPHZD.ToString().Trim() 
                        +"\nFPHZDNUM:" + item.fpVo.FPHZDNUM 
                        +"\nFPHZDBH:" + item.fpVo.FPHZDBH.ToString().Trim() 
                        +"\nFIFGMYWBH:" + item.fpVo.FIFGMYWBH.ToString().Trim() 
                        +"\nFIFGMYW:" + item.fpVo.FIFGMYW.ToString().Trim() 
                        +"\nFGMYW:" + item.fpVo.FGMYW.ToString().Trim() 
                        +"\nFBODYBH:" + item.fpVo.FBODYBH.ToString().Trim() 
                        +"\nFBODY:" + item.fpVo.FBODY 
                        +"\nFBLOODBH:" + item.fpVo.FBLOODBH.ToString().Trim() 
                        +"\nFBLOOD:" + item.fpVo.FBLOOD.ToString().Trim() 
                        +"\nFRHBH:" + item.fpVo.FRHBH.ToString().Trim() 
                        +"\nFRH:" + item.fpVo.FRH 
                        +"\nFKZRBH:" + item.fpVo.FKZRBH.ToString().Trim() 
                        +"\nFKZR:" + item.fpVo.FKZR.ToString().Trim() 
                        +"\nFZRDOCTBH:" + item.fpVo.FZRDOCTBH.ToString().Trim() 
                        +"\nFZRDOCTOR:" + item.fpVo.FZRDOCTOR.ToString().Trim() 
                        +"\nFZZDOCTBH:" + item.fpVo.FZZDOCTBH 
                        +"\nFZZDOCT:" + item.fpVo.FZZDOCT.ToString().Trim() 
                        +"\nFZYDOCTBH:" + item.fpVo.FZYDOCTBH.ToString().Trim() 
                        +"\nFZYDOCT:" + item.fpVo.FZYDOCT.ToString().Trim() 
                        +"\nFNURSEBH:" + item.fpVo.FNURSEBH.ToString().Trim() 
                        +"\nFNURSE:" + item.fpVo.FNURSE 
                        +"\nFJXDOCTBH:" + item.fpVo.FJXDOCTBH.ToString().Trim() 
                        +"\nFJXDOCT:" + item.fpVo.FJXDOCT.ToString().Trim() 
                        +"\nFSXDOCTBH:" + item.fpVo.FSXDOCTBH.ToString().Trim() 
                        +"\nFSXDOCT:" + item.fpVo.FSXDOCT 
                        +"\nFBMYBH:" + item.fpVo.FBMYBH.ToString().Trim() 
                        +"\nFBMY:" + item.fpVo.FBMY.ToString().Trim() 
                        +"\nFQUALITYBH:" + item.fpVo.FQUALITYBH.ToString().Trim() 
                        +"\nFQUALITY:" + item.fpVo.FQUALITY.ToString().Trim() 
                        +"\nFZKDOCTBH:" + item.fpVo.FZKDOCTBH 
                        +"\nFZKDOCT:" + item.fpVo.FZKDOCT.ToString().Trim() 
                        +"\nFZKNURSEBH:" + item.fpVo.FZKNURSEBH.ToString().Trim() 
                        +"\nFZKNURSE:" + item.fpVo.FZKNURSE.ToString().Trim() 
                        +"\nFZKRQ:" + item.fpVo.FZKRQ.ToString().Trim() 
                        +"\nFLYFSBH:" + item.fpVo.FLYFSBH 
                        +"\nFLYFS:" + item.fpVo.FLYFS.ToString().Trim() 
                        +"\nFYZOUTHOSTITAL:" + item.fpVo.FYZOUTHOSTITAL.ToString().Trim() 
                        +"\nFSQOUTHOSTITAL:" + item.fpVo.FSQOUTHOSTITAL.ToString().Trim() 
                        +"\nFISAGAINRYBH:" + item.fpVo.FISAGAINRYBH 
                        +"\nFISAGAINRY:" + item.fpVo.FISAGAINRY.ToString().Trim() 
                        +"\nFISAGAINRYMD:" + item.fpVo.FISAGAINRYMD.ToString().Trim() 
                        +"\nFRYQHMDAYS:" + item.fpVo.FRYQHMDAYS.ToString().Trim() 
                        +"\nFRYQHMHOURS:" + item.fpVo.FRYQHMHOURS.ToString().Trim() 
                        +"\nFRYQHMMINS:" + item.fpVo.FRYQHMMINS 
                        +"\nFRYQHMCOUNTS:" + item.fpVo.FRYQHMCOUNTS.ToString().Trim() 
                        +"\nFRYHMDAYS:" + item.fpVo.FRYHMDAYS.ToString().Trim() 
                        +"\nFRYHMHOURS:" + item.fpVo.FRYHMHOURS.ToString().Trim() 
                        +"\nFRYHMMINS:" + item.fpVo.FRYHMMINS.ToString().Trim() 
                        +"\nFRYHMCOUNTS:" + item.fpVo.FRYHMCOUNTS 
                        +"\nFSUM1:" + item.fpVo.FSUM1.ToString("0.00") 
                        +"\nFZFJE:" + item.fpVo.FZFJE.ToString("0.00") 
                        +"\nFZHFWLYLF:" + item.fpVo.FZHFWLYLF.ToString("0.00") 
                        +"\nFZHFWLCZF:" + item.fpVo.FZHFWLCZF.ToString("0.00") 
                        +"\nFZHFWLHLF:" + item.fpVo.FZHFWLHLF.ToString("0.00") 
                        +"\nFZHFWLQTF:" + item.fpVo.FZHFWLQTF.ToString("0.00") 
                        +"\nFZDLBLF:" + item.fpVo.FZDLBLF.ToString("0.00") 
                        +"\nFZDLSSSF:" + item.fpVo.FZDLSSSF.ToString("0.00") 
                        +"\nFZDLYXF:" + item.fpVo.FZDLYXF.ToString("0.00") 
                        +"\nFZDLLCF:" + item.fpVo.FZDLLCF.ToString("0.00") 
                        +"\nFZLLFFSSF:" + item.fpVo.FZLLFFSSF.ToString("0.00") 
                        +"\nFZLLFWLZWLF:" + item.fpVo.FZLLFWLZWLF.ToString("0.00") 
                        +"\nFZLLFSSF:" + item.fpVo.FZLLFSSF.ToString("0.00") 
                        +"\nFZLLFMZF:" + item.fpVo.FZLLFMZF.ToString("0.00") 
                        +"\nFZLLFSSZLF:" + item.fpVo.FZLLFSSZLF.ToString("0.00") 
                        +"\nFKFLKFF:" + item.fpVo.FKFLKFF.ToString("0.00") 
                        +"\nFZYLZF:" + item.fpVo.FZYLZF.ToString("0.00") 
                        +"\nFXYF:" + item.fpVo.FXYF.ToString("0.00") 
                        +"\nFXYLGJF:" + item.fpVo.FXYLGJF.ToString("0.00") 
                        +"\nFZCHYF:" + item.fpVo.FZCHYF.ToString("0.00") 
                        +"\nFZCYF:" + item.fpVo.FZCYF.ToString("0.00") 
                        +"\nFXYLXF:" + item.fpVo.FXYLXF.ToString("0.00") 
                        +"\nFXYLBQBF:" + item.fpVo.FXYLBQBF.ToString("0.00") 
                        +"\nFXYLQDBF:" + item.fpVo.FXYLQDBF.ToString("0.00") 
                        +"\nFXYLYXYZF:" + item.fpVo.FXYLYXYZF.ToString("0.00") 
                        +"\nFXYLXBYZF:" + item.fpVo.FXYLXBYZF.ToString("0.00") 
                        +"\nFHCLCJF:" + item.fpVo.FHCLCJF.ToString("0.00") 
                        +"\nFHCLZLF:" + item.fpVo.FHCLZLF.ToString("0.00") 
                        +"\nFHCLSSF:" + item.fpVo.FHCLSSF.ToString("0.00") 
                        +"\nFQTF:" + item.fpVo.FQTF.ToString("0.00") 
                        +"\nFBGLX:" + item.fpVo.FBGLX 
                        +"\nGMSFHM:" + item.fpVo.GMSFHM 
                        +"\nYYBH:" + exVo.YYBH 
                        +"\nFZYF:" + item.fpVo.FZYF.ToString("0.00") 
                        +"\nFZKDATE:" + item.fpVo.FZKDATE.ToString() 
                        +"\nFJOBBH:" + item.fpVo.FJOBBH.ToString() 
                        +"\nFZHFWLYLF01:" + item.fpVo.FZHFWLYLF01.ToString("0.00") 
                        +"\nFZHFWLYLF02:" + item.fpVo.FZHFWLYLF02.ToString("0.00") 
                        +"\nFZYLZDF:" + item.fpVo.FZYLZDF.ToString("0.00") 
                        +"\nFZYLZLF:" + item.fpVo.FZYLZLF.ToString("0.00") 
                        + "\nFZYLZLF01:" + item.fpVo.FZYLZLF01.ToString("0.00") 
                        +"\nFZYLZLF02:" + item.fpVo.FZYLZLF02.ToString("0.00")
                        +"\nFZYLZLF03:" + item.fpVo.FZYLZLF03.ToString("0.00") 
                        +"\nFZYLZLF04:" + item.fpVo.FZYLZLF04.ToString("0.00") 
                        +"\nFZYLZLF05:" + item.fpVo.FZYLZLF05.ToString("0.00") 
                        +"\nFZYLZLF06:" + item.fpVo.FZYLZLF06.ToString("0.00") 
                        +"\nFZYLQTF:" + item.fpVo.FZYLQTF.ToString("0.00") 
                        +"\nFZCLJGZJF:" + item.fpVo.FZCLJGZJF.ToString("0.00") 
                        +"\nFZYLQTF01:" + item.fpVo.FZYLQTF01.ToString("0.00")
                        + "\nFZYLQTF02:" + item.fpVo.FZYLQTF02.ToString("0.00"));
                        intRet = SetParam(intH, "JZJLH", item.fpVo.JZJLH.ToString().Trim());//
                        intRet = SetParam(intH, "FWSJGDM", exVo.FWSJGDM);
                        intRet = SetParam(intH, "FFBBHNEW", item.fpVo.FFBBHNEW.ToString().Trim());//
                        intRet = SetParam(intH, "FFBNEW", item.fpVo.FFBNEW.ToString().Trim());
                        intRet = SetParam(intH, "FASCARD1", item.fpVo.FASCARD1.ToString().Trim());
                        intRet = SetParam(intH, "FTIMES", item.fpVo.FTIMES.ToString().Trim());
                        intRet = SetParam(intH, "FPRN", item.fpVo.FPRN.ToString().Trim());
                        intRet = SetParam(intH, "FNAME", item.fpVo.FNAME.ToString().Trim());//
                        intRet = SetParam(intH, "FSEXBH", item.fpVo.FSEXBH.ToString().Trim());//
                        intRet = SetParam(intH, "FSEX", item.fpVo.FSEX.ToString().Trim());//
                        intRet = SetParam(intH, "FBIRTHDAY", item.fpVo.FBIRTHDAY.ToString());//
                        intRet = SetParam(intH, "FAGE", item.fpVo.FAGE.ToString());
                        intRet = SetParam(intH, "fcountrybh", item.fpVo.fcountrybh.ToString());
                        intRet = SetParam(intH, "fcountry", item.fpVo.fcountry.ToString());
                        intRet = SetParam(intH, "fnationalitybh", item.fpVo.fnationalitybh.ToString());
                        intRet = SetParam(intH, "fnationality", item.fpVo.fnationality.ToString());
                        intRet = SetParam(intH, "FCSTZ", (Function.Dec(item.fpVo.FCSTZ) / 1000).ToString("0.00"));
                        intRet = SetParam(intH, "FRYTZ", (Function.Dec(item.fpVo.FRYTZ) / 1000).ToString("0.00"));
                        intRet = SetParam(intH, "FBIRTHPLACE", item.fpVo.FBIRTHPLACE.ToString().Trim());
                        intRet = SetParam(intH, "FNATIVE", item.fpVo.FNATIVE.ToString().Trim());
                        intRet = SetParam(intH, "FIDCard", item.fpVo.FIDCard);
                        intRet = SetParam(intH, "FJOB", item.fpVo.FJOB.ToString().Trim());
                        intRet = SetParam(intH, "FSTATUSBH", item.fpVo.FSTATUSBH.ToString().Trim());
                        intRet = SetParam(intH, "FSTATUS", item.fpVo.FSTATUS.ToString().Trim());
                        intRet = SetParam(intH, "FCURRADDR", item.fpVo.FCURRADDR.ToString().Trim());
                        intRet = SetParam(intH, "FCURRTELE", item.fpVo.FCURRTELE);
                        intRet = SetParam(intH, "FCURRPOST", item.fpVo.FCURRPOST.ToString().Trim());
                        intRet = SetParam(intH, "FHKADDR", item.fpVo.FHKADDR.ToString().Trim());
                        intRet = SetParam(intH, "FHKPOST", item.fpVo.FHKPOST.ToString().Trim());
                        intRet = SetParam(intH, "FDWNAME", item.fpVo.FDWNAME.ToString().Trim());
                        intRet = SetParam(intH, "FDWADDR", item.fpVo.FDWADDR);
                        intRet = SetParam(intH, "FDWTELE", item.fpVo.FDWTELE.ToString().Trim());
                        intRet = SetParam(intH, "FDWPOST", item.fpVo.FDWPOST.ToString().Trim());
                        intRet = SetParam(intH, "FLXNAME", item.fpVo.FLXNAME.ToString().Trim());
                        intRet = SetParam(intH, "FRELATE", item.fpVo.FRELATE.ToString().Trim());
                        intRet = SetParam(intH, "FLXADDR", item.fpVo.FLXADDR);
                        intRet = SetParam(intH, "FLXTELE", item.fpVo.FLXTELE.ToString().Trim());
                        intRet = SetParam(intH, "FRYTJBH", item.fpVo.FRYTJBH.ToString().Trim());
                        intRet = SetParam(intH, "FRYTJ", item.fpVo.FRYTJ.ToString().Trim());
                        intRet = SetParam(intH, "FRYDATE", item.fpVo.FRYDATE.ToString().Trim());
                        intRet = SetParam(intH, "FRYTIME", item.fpVo.FRYTIME.ToString());
                        intRet = SetParam(intH, "FRYTYKH", item.fpVo.FRYTYKH.ToString().Trim());
                        intRet = SetParam(intH, "FRYDEPT", item.fpVo.FRYDEPT.ToString().Trim());
                        intRet = SetParam(intH, "FRYBS", item.fpVo.FRYBS.ToString().Trim());
                        intRet = SetParam(intH, "FZKTYKH", item.fpVo.FZKTYKH.ToString().Trim());
                        intRet = SetParam(intH, "FZKDEPT", item.fpVo.FZKDEPT);
                        intRet = SetParam(intH, "FZKTIME", item.fpVo.FZKTIME.ToString());
                        intRet = SetParam(intH, "FCYDATE", item.fpVo.FCYDATE.ToString().Trim());
                        intRet = SetParam(intH, "FCYTIME", item.fpVo.FCYTIME.ToString().Trim());
                        intRet = SetParam(intH, "FCYTYKH", item.fpVo.FCYTYKH.ToString().Trim());
                        intRet = SetParam(intH, "FCYDEPT", item.fpVo.FCYDEPT);
                        intRet = SetParam(intH, "FCYBS", item.fpVo.FCYBS.ToString().Trim());
                        intRet = SetParam(intH, "FDAYS", item.fpVo.FDAYS.ToString().Trim());
                        intRet = SetParam(intH, "FMZZDBH", item.fpVo.FMZZDBH.ToString().Trim());
                        intRet = SetParam(intH, "FMZZD", item.fpVo.FMZZD.ToString().Trim());
                        intRet = SetParam(intH, "FMZDOCTBH", item.fpVo.FMZDOCTBH);
                        intRet = SetParam(intH, "FMZDOCT", item.fpVo.FMZDOCT.ToString().Trim());
                        intRet = SetParam(intH, "FJBFXBH", item.fpVo.FJBFXBH.ToString().Trim());
                        intRet = SetParam(intH, "FJBFX", item.fpVo.FJBFX.ToString().Trim());
                        intRet = SetParam(intH, "FYCLJBH", item.fpVo.FYCLJBH);
                        intRet = SetParam(intH, "FYCLJ", item.fpVo.FYCLJ.ToString().Trim());
                        intRet = SetParam(intH, "FQJTIMES", item.fpVo.FQJTIMES.ToString().Trim());
                        intRet = SetParam(intH, "FQJSUCTIMES", item.fpVo.FQJSUCTIMES.ToString().Trim());
                        intRet = SetParam(intH, "FPHZD", item.fpVo.FPHZD.ToString().Trim());
                        intRet = SetParam(intH, "FPHZDNUM", item.fpVo.FPHZDNUM);
                        intRet = SetParam(intH, "FPHZDBH", item.fpVo.FPHZDBH.ToString().Trim());
                        intRet = SetParam(intH, "FIFGMYWBH", item.fpVo.FIFGMYWBH.ToString().Trim());
                        intRet = SetParam(intH, "FIFGMYW", item.fpVo.FIFGMYW.ToString().Trim());
                        intRet = SetParam(intH, "FGMYW", item.fpVo.FGMYW.ToString().Trim());
                        intRet = SetParam(intH, "FBODYBH", item.fpVo.FBODYBH.ToString().Trim());
                        intRet = SetParam(intH, "FBODY", item.fpVo.FBODY);
                        intRet = SetParam(intH, "FBLOODBH", item.fpVo.FBLOODBH.ToString().Trim());
                        intRet = SetParam(intH, "FBLOOD", item.fpVo.FBLOOD.ToString().Trim());
                        intRet = SetParam(intH, "FRHBH", item.fpVo.FRHBH.ToString().Trim());
                        intRet = SetParam(intH, "FRH", item.fpVo.FRH);
                        intRet = SetParam(intH, "FKZRBH", item.fpVo.FKZRBH.ToString().Trim());
                        intRet = SetParam(intH, "FKZR", item.fpVo.FKZR.ToString().Trim());
                        intRet = SetParam(intH, "FZRDOCTBH", item.fpVo.FZRDOCTBH.ToString().Trim());
                        intRet = SetParam(intH, "FZRDOCTOR", item.fpVo.FZRDOCTOR.ToString().Trim());
                        intRet = SetParam(intH, "FZZDOCTBH", item.fpVo.FZZDOCTBH);
                        intRet = SetParam(intH, "FZZDOCT", item.fpVo.FZZDOCT.ToString().Trim());
                        intRet = SetParam(intH, "FZYDOCTBH", item.fpVo.FZYDOCTBH.ToString().Trim());
                        intRet = SetParam(intH, "FZYDOCT", item.fpVo.FZYDOCT.ToString().Trim());
                        intRet = SetParam(intH, "FNURSEBH", item.fpVo.FNURSEBH.ToString().Trim());
                        intRet = SetParam(intH, "FNURSE", item.fpVo.FNURSE);
                        intRet = SetParam(intH, "FJXDOCTBH", item.fpVo.FJXDOCTBH.ToString().Trim());
                        intRet = SetParam(intH, "FJXDOCT", item.fpVo.FJXDOCT.ToString().Trim());
                        intRet = SetParam(intH, "FSXDOCTBH", item.fpVo.FSXDOCTBH.ToString().Trim());
                        intRet = SetParam(intH, "FSXDOCT", item.fpVo.FSXDOCT);
                        intRet = SetParam(intH, "FBMYBH", item.fpVo.FBMYBH.ToString().Trim());
                        intRet = SetParam(intH, "FBMY", item.fpVo.FBMY.ToString().Trim());
                        intRet = SetParam(intH, "FQUALITYBH", item.fpVo.FQUALITYBH.ToString().Trim());
                        intRet = SetParam(intH, "FQUALITY", item.fpVo.FQUALITY.ToString().Trim());
                        intRet = SetParam(intH, "FZKDOCTBH", item.fpVo.FZKDOCTBH);
                        intRet = SetParam(intH, "FZKDOCT", item.fpVo.FZKDOCT.ToString().Trim());
                        intRet = SetParam(intH, "FZKNURSEBH", item.fpVo.FZKNURSEBH.ToString().Trim());
                        intRet = SetParam(intH, "FZKNURSE", item.fpVo.FZKNURSE.ToString().Trim());
                        intRet = SetParam(intH, "FZKRQ", item.fpVo.FZKRQ.ToString().Trim());
                        intRet = SetParam(intH, "FLYFSBH", item.fpVo.FLYFSBH);
                        intRet = SetParam(intH, "FLYFS", item.fpVo.FLYFS.ToString().Trim());
                        intRet = SetParam(intH, "FYZOUTHOSTITAL", item.fpVo.FYZOUTHOSTITAL.ToString().Trim());
                        intRet = SetParam(intH, "FSQOUTHOSTITAL", item.fpVo.FSQOUTHOSTITAL.ToString().Trim());
                        intRet = SetParam(intH, "FISAGAINRYBH", item.fpVo.FISAGAINRYBH);
                        intRet = SetParam(intH, "FISAGAINRY", item.fpVo.FISAGAINRY.ToString().Trim());
                        intRet = SetParam(intH, "FISAGAINRYMD", item.fpVo.FISAGAINRYMD.ToString().Trim());
                        intRet = SetParam(intH, "FRYQHMDAYS", item.fpVo.FRYQHMDAYS.ToString().Trim());
                        intRet = SetParam(intH, "FRYQHMHOURS", item.fpVo.FRYQHMHOURS.ToString().Trim());
                        intRet = SetParam(intH, "FRYQHMMINS", item.fpVo.FRYQHMMINS);
                        intRet = SetParam(intH, "FRYQHMCOUNTS", item.fpVo.FRYQHMCOUNTS.ToString().Trim());
                        intRet = SetParam(intH, "FRYHMDAYS", item.fpVo.FRYHMDAYS.ToString().Trim());
                        intRet = SetParam(intH, "FRYHMHOURS", item.fpVo.FRYHMHOURS.ToString().Trim());
                        intRet = SetParam(intH, "FRYHMMINS", item.fpVo.FRYHMMINS.ToString().Trim());
                        intRet = SetParam(intH, "FRYHMCOUNTS", item.fpVo.FRYHMCOUNTS);
                        intRet = SetParam(intH, "FSUM1", item.fpVo.FSUM1.ToString("0.00"));
                        intRet = SetParam(intH, "FZFJE", item.fpVo.FZFJE.ToString("0.00"));
                        intRet = SetParam(intH, "FZHFWLYLF", item.fpVo.FZHFWLYLF.ToString("0.00"));
                        intRet = SetParam(intH, "FZHFWLCZF", item.fpVo.FZHFWLCZF.ToString("0.00"));
                        intRet = SetParam(intH, "FZHFWLHLF", item.fpVo.FZHFWLHLF.ToString("0.00"));
                        intRet = SetParam(intH, "FZHFWLQTF", item.fpVo.FZHFWLQTF.ToString("0.00"));
                        intRet = SetParam(intH, "FZDLBLF", item.fpVo.FZDLBLF.ToString("0.00"));
                        intRet = SetParam(intH, "FZDLSSSF", item.fpVo.FZDLSSSF.ToString("0.00"));
                        intRet = SetParam(intH, "FZDLYXF", item.fpVo.FZDLYXF.ToString("0.00"));
                        intRet = SetParam(intH, "FZDLLCF", item.fpVo.FZDLLCF.ToString("0.00"));
                        intRet = SetParam(intH, "FZLLFFSSF", item.fpVo.FZLLFFSSF.ToString("0.00"));
                        intRet = SetParam(intH, "FZLLFWLZWLF", item.fpVo.FZLLFWLZWLF.ToString("0.00"));
                        intRet = SetParam(intH, "FZLLFSSF", item.fpVo.FZLLFSSF.ToString("0.00"));
                        intRet = SetParam(intH, "FZLLFMZF", item.fpVo.FZLLFMZF.ToString("0.00"));
                        intRet = SetParam(intH, "FZLLFSSZLF", item.fpVo.FZLLFSSZLF.ToString("0.00"));
                        intRet = SetParam(intH, "FKFLKFF", item.fpVo.FKFLKFF.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZF", item.fpVo.FZYLZF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYF", item.fpVo.FXYF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLGJF", item.fpVo.FXYLGJF.ToString("0.00"));
                        intRet = SetParam(intH, "FZCHYF", item.fpVo.FZCHYF.ToString("0.00"));
                        intRet = SetParam(intH, "FZCYF", item.fpVo.FZCYF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLXF", item.fpVo.FXYLXF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLBQBF", item.fpVo.FXYLBQBF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLQDBF", item.fpVo.FXYLQDBF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLYXYZF", item.fpVo.FXYLYXYZF.ToString("0.00"));
                        intRet = SetParam(intH, "FXYLXBYZF", item.fpVo.FXYLXBYZF.ToString("0.00"));
                        intRet = SetParam(intH, "FHCLCJF", item.fpVo.FHCLCJF.ToString("0.00"));
                        intRet = SetParam(intH, "FHCLZLF", item.fpVo.FHCLZLF.ToString("0.00"));
                        intRet = SetParam(intH, "FHCLSSF", item.fpVo.FHCLSSF.ToString("0.00"));
                        intRet = SetParam(intH, "FQTF", item.fpVo.FQTF.ToString("0.00"));
                        if (item.STATUS != 1)
                            item.fpVo.FBGLX = "1";
                        else
                            item.fpVo.FBGLX = "2";
                        intRet = SetParam(intH, "FBGLX", item.fpVo.FBGLX.ToString());
                        intRet = SetParam(intH, "GMSFHM", item.fpVo.GMSFHM);
                        intRet = SetParam(intH, "YYBH", exVo.YYBH);
                        intRet = SetParam(intH, "FZYF", item.fpVo.FZYF.ToString("0.00"));
                        intRet = SetParam(intH, "FZKDATE", item.fpVo.FZKDATE.ToString());
                        intRet = SetParam(intH, "FJOBBH", item.fpVo.FJOBBH.ToString());
                        intRet = SetParam(intH, "FZHFWLYLF01", item.fpVo.FZHFWLYLF01.ToString("0.00"));
                        intRet = SetParam(intH, "FZHFWLYLF02", item.fpVo.FZHFWLYLF02.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZDF", item.fpVo.FZYLZDF.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF", item.fpVo.FZYLZLF.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF01", item.fpVo.FZYLZLF01.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF02", item.fpVo.FZYLZLF02.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF03", item.fpVo.FZYLZLF03.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF04", item.fpVo.FZYLZLF04.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF05", item.fpVo.FZYLZLF05.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLZLF06", item.fpVo.FZYLZLF06.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLQTF", item.fpVo.FZYLQTF.ToString("0.00"));
                        intRet = SetParam(intH, "FZCLJGZJF", item.fpVo.FZCLJGZJF.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLQTF01", item.fpVo.FZYLQTF01.ToString("0.00"));
                        intRet = SetParam(intH, "FZYLQTF02", item.fpVo.FZYLQTF02.ToString("0.00"));
                        
                        ++j;
                        #endregion

                        #region 数据集(病人转科情况)：BRZKQKSJJ
                        if (item.fpVo.lstZkVo != null && item.fpVo.lstZkVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstZkVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FZKTYKH", item.fpVo.lstZkVo[i].FZKTYKH);//
                                intRet = SetField(intH, "FZKDEPT", item.fpVo.lstZkVo[i].FZKDEPT);//
                                intRet = SetField(intH, "FZKDATE", item.fpVo.lstZkVo[i].FZKDATE);//
                                intRet = SetField(intH, "FZKTIME", item.fpVo.lstZkVo[i].FZKTIME);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstZkVo[i].FPRN);//
                                EndRow(intH, i);
                            }
                            intRet = EndDataSet(intH, "BRZKQKSJJ");
                        }

                        #endregion

                        #region 数据集(病人诊断信息):  BRZDXXSJJ
                        if (item.fpVo.lstZdVo != null && item.fpVo.lstZdVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstZdVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FZDLX", item.fpVo.lstZdVo[i].FZDLX);//
                                intRet = SetField(intH, "FICDVersion", item.fpVo.lstZdVo[i].FICDVersion);//
                                intRet = SetField(intH, "FICDM", item.fpVo.lstZdVo[i].FICDM);//
                                intRet = SetField(intH, "FJBNAME", item.fpVo.lstZdVo[i].FJBNAME);//
                                intRet = SetField(intH, "FRYBQBH", item.fpVo.lstZdVo[i].FRYBQBH);
                                intRet = SetField(intH, "FRYBQ", item.fpVo.lstZdVo[i].FRYBQ);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstZdVo[i].FPRN);//
                                EndRow(intH, i);
                            }
                            intRet = EndDataSet(intH, "BRZDXXSJJ");
                        }
                        #endregion

                        #region 数据集(病人手术信息):BRSSXXSJJ
                        if (item.fpVo.lstSsVo != null && item.fpVo.lstSsVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstSsVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FNAME", item.fpVo.lstSsVo[i].FNAME);//
                                intRet = SetField(intH, "FOPTIMES", item.fpVo.lstSsVo[i].FOPTIMES);//
                                intRet = SetField(intH, "FOPCODE", item.fpVo.lstSsVo[i].FOPCODE);//
                                intRet = SetField(intH, "FOP", item.fpVo.lstSsVo[i].FOP);//
                                intRet = SetField(intH, "FOPDATE", item.fpVo.lstSsVo[i].FOPDATE);//
                                intRet = SetField(intH, "FQIEKOUBH", item.fpVo.lstSsVo[i].FQIEKOUBH);//
                                intRet = SetField(intH, "FQIEKOU", item.fpVo.lstSsVo[i].FQIEKOU);//
                                intRet = SetField(intH, "FYUHEBH", item.fpVo.lstSsVo[i].FYUHEBH);//
                                intRet = SetField(intH, "FYUHE", item.fpVo.lstSsVo[i].FYUHE);//
                                intRet = SetField(intH, "FDOCBH", item.fpVo.lstSsVo[i].FDOCBH);//
                                intRet = SetField(intH, "FDOCNAME", item.fpVo.lstSsVo[i].FDOCNAME);//
                                intRet = SetField(intH, "FMAZUIBH", item.fpVo.lstSsVo[i].FMAZUIBH);//
                                intRet = SetField(intH, "FMAZUI", item.fpVo.lstSsVo[i].FMAZUI);//
                                intRet = SetField(intH, "FIFFSOP", item.fpVo.lstSsVo[i].FIFFSOP);//
                                intRet = SetField(intH, "FOPDOCT1BH", item.fpVo.lstSsVo[i].FOPDOCT1BH);//
                                intRet = SetField(intH, "FOPDOCT1", item.fpVo.lstSsVo[i].FOPDOCT1);//
                                intRet = SetField(intH, "FOPDOCT2BH", item.fpVo.lstSsVo[i].FOPDOCT2BH);//
                                intRet = SetField(intH, "FOPDOCT2", item.fpVo.lstSsVo[i].FOPDOCT2);//
                                intRet = SetField(intH, "FMZDOCTBH", item.fpVo.lstSsVo[i].FMZDOCTBH);//
                                intRet = SetField(intH, "FMZDOCT", item.fpVo.lstSsVo[i].FMZDOCT);//
                                intRet = SetField(intH, "FZQSSBH", item.fpVo.lstSsVo[i].FZQSSBH);//
                                intRet = SetField(intH, "FZQSS", item.fpVo.lstSsVo[i].FZQSS);//
                                intRet = SetField(intH, "FSSJBBH", item.fpVo.lstSsVo[i].FSSJBBH);//
                                intRet = SetField(intH, "FSSJB", item.fpVo.lstSsVo[i].FSSJB);//
                                intRet = SetField(intH, "FOPKSNAME", item.fpVo.lstSsVo[i].FOPKSNAME);//
                                intRet = SetField(intH, "FOPTYKH", item.fpVo.lstSsVo[i].FOPTYKH);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstSsVo[i].FPRN);//
                                EndRow(intH, i);
                            }
                            intRet = EndDataSet(intH, "BRSSXXSJJ");
                        }

                        #endregion

                        #region 数据集（妇婴卡）
                        if (item.fpVo.lstFyVo != null && item.fpVo.lstFyVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstFyVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FBABYNUM", item.fpVo.lstFyVo[i].FBABYNUM);//
                                intRet = SetField(intH, "FNAME", item.fpVo.lstFyVo[i].FNAME);//
                                intRet = SetField(intH, "FBABYSEXBH", item.fpVo.lstFyVo[i].FBABYSEXBH);//
                                intRet = SetField(intH, "FBABYSEX", item.fpVo.lstFyVo[i].FBABYSEX);//
                                intRet = SetField(intH, "FTZ", (Function.Dec(item.fpVo.lstFyVo[i].FTZ) / 1000).ToString("0.00"));//
                                intRet = SetField(intH, "FRESULTBH", item.fpVo.lstFyVo[i].FRESULTBH);//
                                intRet = SetField(intH, "FRESULT", item.fpVo.lstFyVo[i].FRESULT);//
                                intRet = SetField(intH, "FZGBH", item.fpVo.lstFyVo[i].FZGBH);//
                                intRet = SetField(intH, "FZG", item.fpVo.lstFyVo[i].FZG);//
                                intRet = SetField(intH, "FBABYSUC", item.fpVo.lstFyVo[i].FBABYSUC);//
                                intRet = SetField(intH, "FHXBH", item.fpVo.lstFyVo[i].FHXBH);//
                                intRet = SetField(intH, "FHX", item.fpVo.lstFyVo[i].FHX);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstFyVo[i].FPRN);//
                                EndRow(intH, i);
                            }
                            intRet = EndDataSet(intH, "FYKSJJ");
                        }

                        #endregion

                        #region 数据集（肿瘤卡）：ZLKSJJ
                        if (item.fpVo.lstZlVo != null && item.fpVo.lstZlVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstZlVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FFLFSBH", item.fpVo.lstZlVo[i].FFLFSBH);//
                                intRet = SetField(intH, "FFLFS", item.fpVo.lstZlVo[i].FFLFS);//
                                intRet = SetField(intH, "FFLCXBH", item.fpVo.lstZlVo[i].FFLCXBH);//
                                intRet = SetField(intH, "FFLCX", item.fpVo.lstZlVo[i].FFLCX);//
                                intRet = SetField(intH, "FFLZZBH", item.fpVo.lstZlVo[i].FFLZZBH);//
                                intRet = SetField(intH, "FFLZZ", item.fpVo.lstZlVo[i].FFLZZ);//
                                intRet = SetField(intH, "FYJY", item.fpVo.lstZlVo[i].FYJY);//
                                intRet = SetField(intH, "FYCS", item.fpVo.lstZlVo[i].FYCS);//
                                intRet = SetField(intH, "FYTS", item.fpVo.lstZlVo[i].FYTS);//
                                intRet = SetField(intH, "FYRQ1", item.fpVo.lstZlVo[i].FYRQ1);//
                                intRet = SetField(intH, "FYRQ2", item.fpVo.lstZlVo[i].FYRQ2);//
                                intRet = SetField(intH, "FQJY", item.fpVo.lstZlVo[i].FQJY);//
                                intRet = SetField(intH, "FQCS", item.fpVo.lstZlVo[i].FQCS);//
                                intRet = SetField(intH, "FQTS", item.fpVo.lstZlVo[i].FQTS);//
                                intRet = SetField(intH, "FQRQ1", item.fpVo.lstZlVo[i].FQRQ1);//
                                intRet = SetField(intH, "FQRQ2", item.fpVo.lstZlVo[i].FQRQ2);//
                                intRet = SetField(intH, "FZNAME", item.fpVo.lstZlVo[i].FZNAME);//
                                intRet = SetField(intH, "FZJY", item.fpVo.lstZlVo[i].FZJY);//
                                intRet = SetField(intH, "FZCS", item.fpVo.lstZlVo[i].FZCS);//
                                intRet = SetField(intH, "FZTS", item.fpVo.lstZlVo[i].FZTS);//
                                intRet = SetField(intH, "FZRQ1", item.fpVo.lstZlVo[i].FZRQ1);//
                                intRet = SetField(intH, "FZRQ2", item.fpVo.lstZlVo[i].FZRQ2);//
                                intRet = SetField(intH, "FHLFSBH", item.fpVo.lstZlVo[i].FHLFSBH);//
                                intRet = SetField(intH, "FHLFS", item.fpVo.lstZlVo[i].FHLFS);//
                                intRet = SetField(intH, "FHLFFBH", item.fpVo.lstZlVo[i].FHLFFBH);//
                                intRet = SetField(intH, "FHLFF", item.fpVo.lstZlVo[i].FHLFF);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstZlVo[i].FPRN);//
                                EndRow(intH, i);
                            }
                            intRet = EndDataSet(intH, "ZLKSJJ");
                        }

                        #endregion

                        #region 数据集（肿瘤化疗记录）：ZLHLJLSJJ
                        if (item.fpVo.lstHlVo != null && item.fpVo.lstHlVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstHlVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FHLRQ1", item.fpVo.lstHlVo[i].FHLRQ1);//
                                intRet = SetField(intH, "FHLDRUG", item.fpVo.lstHlVo[i].FHLDRUG);//
                                intRet = SetField(intH, "FHLPROC", item.fpVo.lstHlVo[i].FHLPROC);//
                                intRet = SetField(intH, "FHLLXBH", item.fpVo.lstHlVo[i].FHLLXBH);//
                                intRet = SetField(intH, "FHLLX", item.fpVo.lstHlVo[i].FHLLX);//
                                intRet = SetField(intH, "FPRN", item.fpVo.lstHlVo[i].FPRN);//
                                EndRow(intH, j);
                            }
                            intRet = EndDataSet(intH, "ZLHLJLSJJ");
                        }


                        #endregion

                        #region 数据集（病人诊断码附加编码）：BRZDMFJBMSJJ
                        if (item.fpVo.lstZdfjVo != null && item.fpVo.lstZdfjVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int k = 0; k < item.fpVo.lstZdfjVo.Count; k++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FZDLX", item.fpVo.lstZdfjVo[k].FZDLX);//
                                intRet = SetField(intH, "FICDM", item.fpVo.lstZdfjVo[k].FICDM);//
                                intRet = SetField(intH, "FFJICDM", item.fpVo.lstZdfjVo[k].FFJICDM);//
                                intRet = SetField(intH, "FFJJBNAME", item.fpVo.lstZdfjVo[k].FFJJBNAME);//
                                intRet = SetField(intH, "FFRYBQBH", item.fpVo.lstZdfjVo[k].FFRYBQBH);//
                                intRet = SetField(intH, "FFRYBQ", item.fpVo.lstZdfjVo[k].FFRYBQ);//
                                intRet = SetField(intH, "FPX", item.fpVo.lstZdfjVo[k].FPX);//
                                EndRow(intH, k);
                            }
                            intRet = EndDataSet(intH, "BRZDMFJBMSJJ");
                        }

                        #endregion

                        #region （新增）数据集（中医院病人附加信息）：ZYYBRFJXXSJJ
                        if (item.fpVo.lstZyVo != null && item.fpVo.lstZyVo.Count > 0)
                        {
                            intRet = InsertDataSet(intH);
                            for (int i = 0; i < item.fpVo.lstZyVo.Count; i++)
                            {
                                intRet = InsertRow(intH);
                                intRet = SetField(intH, "FPRN", item.fpVo.lstZyVo[i].FPRN);//
                                intRet = SetField(intH, "FZLLBBH", item.fpVo.lstZyVo[i].FZLLBBH);//
                                intRet = SetField(intH, "FZLLB", item.fpVo.lstZyVo[i].FZLLB);//
                                intRet = SetField(intH, "FZZZYBH", item.fpVo.lstZyVo[i].FZZZYBH);//
                                intRet = SetField(intH, "FZZZY", item.fpVo.lstZyVo[i].FZZZY);//
                                intRet = SetField(intH, "FRYCYBH", item.fpVo.lstZyVo[i].FRYCYBH);//
                                intRet = SetField(intH, "FRYCY", item.fpVo.lstZyVo[i].FRYCY);//
                                intRet = SetField(intH, "FMZZYZDBH", item.fpVo.lstZyVo[i].FMZZYZDBH);//
                                intRet = SetField(intH, "FMZZYZD", item.fpVo.lstZyVo[i].FMZZYZD);//
                                intRet = SetField(intH, "FSSLCLJBH", item.fpVo.lstZyVo[i].FSSLCLJBH);//
                                intRet = SetField(intH, "FSSLCLJ", item.fpVo.lstZyVo[i].FSSLCLJ);//
                                intRet = SetField(intH, "FSYJGZJBH", item.fpVo.lstZyVo[i].FSYJGZJBH);//
                                intRet = SetField(intH, "FSYJGZJ", item.fpVo.lstZyVo[i].FSYJGZJ);//
                                intRet = SetField(intH, "FSYZYSBBH", item.fpVo.lstZyVo[i].FSYZYSBBH);//
                                intRet = SetField(intH, "FSYZYSB", item.fpVo.lstZyVo[i].FSYZYSB);//
                                intRet = SetField(intH, "FSYZYJSBH", item.fpVo.lstZyVo[i].FSYZYJSBH);//
                                intRet = SetField(intH, "FSYZYJS", item.fpVo.lstZyVo[i].FSYZYJS);//
                                intRet = SetField(intH, "FBZSHBH", item.fpVo.lstZyVo[i].FBZSHBH);//
                                intRet = SetField(intH, "FBZSH", item.fpVo.lstZyVo[i].FBZSH);//
                                EndRow(intH, i);
                            }

                            intRet = EndDataSet(intH, "ZYYBRFJXXSJJ");
                        }

                        #endregion

                        intRet = Run(intH);
                        strValue = new StringBuilder(1024);
                        intRet = GetParam(intH, "FHZ", strValue, 1024);
                        if (strValue.ToString() == "1")
                        {
                            StringBuilder sbValue = new StringBuilder(1024);
                            intRet = GetParam(intH, "MSG", sbValue, 1024);
                            string strCGBZ = sbValue.ToString().Trim();//原因
                            if (strCGBZ == "执行成功！" || strCGBZ == "1")
                            {
                                //MessageBox.Show(strCGBZ, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                                lngRes = 1;
                                item.Issucess = 1;//1  上传成功
                            }
                        }
                        else
                        {
                            intRet = GetParam(intH, "MSG", strValue, 1024);
                            ExceptionLog.OutPutException(item.JZJLH + "-" + item.INPATIENTID + ":" + strValue.ToString());
                            if (strValue.ToString().Contains("已存在对应就诊记录号"))
                                item.Issucess = 1;
                            else
                                item.Issucess = -1; //-1 上传失败
                            item.FailMsg = "就诊记录号:" + item.JZJLH + " 住院号：" + item.INPATIENTID + "\n" + strValue.ToString();
                            //MessageBox.Show("错误信息:" + lngRes.ToString() + ":" + strValue.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLog.OutPutException(ex);
                    }
                    
                }
                DestroyInstance(intH);
            }
           
            return lngRes;
        }
        #endregion

        #region 出院小结上传
        public static long lngFunSP3_3022(ref List<EntityPatUpload> lstVo, EntityDGExtra exVo, ref StringBuilder strValue)
        {
            long lngRes = -1;
            int intRet;
            for (int i = 0; i < lstVo.Count;i++ )
            {
                int intH = CreateInstace();
                if (intH > 0)
                {
                    Log.Output("出院小结数据上传：");
                    Log.Output("FN:" + "SP3_3022"
                        + "\nJBR:" + exVo.JBR
                        + "\nYYBH:" + exVo.YYBH
                        + "\nJZJLH:" + lstVo[i].xjVo.JZJLH
                        + "\nMZH:" + lstVo[i].xjVo.MZH
                        + "\nZYH:" + lstVo[i].xjVo.ZYH
                        + "\nMZZD:" + lstVo[i].xjVo.MZZD
                        + "\nRYZD:" + lstVo[i].xjVo.RYZD
                        + "\nCYZD:" + lstVo[i].xjVo.CYZD
                        + "\nXM:" + lstVo[i].xjVo.XM
                        + "\nXB:" + lstVo[i].xjVo.XB
                        + "\nNL:" + lstVo[i].xjVo.NL
                        + "\nZY:" + lstVo[i].xjVo.ZY
                        + "\nJG:" + lstVo[i].xjVo.JG
                        + "\nRYRQ:" + lstVo[i].xjVo.RYRQ
                        + "\nCYRQ:" + lstVo[i].xjVo.CYRQ
                        + "\nZYTS:" + lstVo[i].xjVo.ZYTS
                        + "\nYJDZ:" + lstVo[i].xjVo.YJDZ
                        + "\nZLJG:" + lstVo[i].xjVo.ZLJG
                        + "\nYSQM:" + lstVo[i].xjVo.YSQM
                        + "\nRYQK:" + lstVo[i].xjVo.RYQK
                        + "\nRYHCLGC:" + lstVo[i].xjVo.RYHCLGC
                        + "\nCYSQK:" + lstVo[i].xjVo.CYSQK
                        + "\nGMSFHM:" + lstVo[i].xjVo.GMSFHM);
                    intRet = SetParam(intH, "FN", "SP3_3022");
                    intRet = SetParam(intH, "JBR", exVo.JBR);
                    intRet = SetParam(intH, "YYBH", exVo.YYBH);
                    intRet = SetParam(intH, "JZJLH", lstVo[i].xjVo.JZJLH);//
                    intRet = SetParam(intH, "MZH", lstVo[i].xjVo.MZH);//
                    intRet = SetParam(intH, "ZYH", lstVo[i].xjVo.ZYH);//
                    intRet = SetParam(intH, "MZZD", lstVo[i].xjVo.MZZD);
                    intRet = SetParam(intH, "RYZD", lstVo[i].xjVo.RYZD);
                    intRet = SetParam(intH, "CYZD", lstVo[i].xjVo.CYZD);
                    intRet = SetParam(intH, "XM", lstVo[i].xjVo.XM);
                    intRet = SetParam(intH, "XB", lstVo[i].xjVo.XB);
                    intRet = SetParam(intH, "NL", lstVo[i].xjVo.NL);
                    intRet = SetParam(intH, "ZY", lstVo[i].xjVo.ZY);
                    intRet = SetParam(intH, "JG", lstVo[i].xjVo.JG);
                    intRet = SetParam(intH, "RYRQ", lstVo[i].xjVo.RYRQ);
                    intRet = SetParam(intH, "CYRQ", lstVo[i].xjVo.CYRQ);
                    intRet = SetParam(intH, "ZYTS", lstVo[i].xjVo.ZYTS);
                    intRet = SetParam(intH, "YJDZ", lstVo[i].xjVo.YJDZ);
                    intRet = SetParam(intH, "ZLJG", lstVo[i].xjVo.ZLJG);
                    intRet = SetParam(intH, "CYYZ", lstVo[i].xjVo.CYYZ);
                    intRet = SetParam(intH, "YSQM", lstVo[i].xjVo.YSQM);
                    intRet = SetParam(intH, "RYQK", lstVo[i].xjVo.RYQK);
                    intRet = SetParam(intH, "RYHCLGC", lstVo[i].xjVo.RYHCLGC);
                    intRet = SetParam(intH, "CYSQK", lstVo[i].xjVo.CYSQK);
                    intRet = SetParam(intH, "GMSFHM", lstVo[i].xjVo.GMSFHM);
                    intRet = Run(intH);
                }
                strValue = new StringBuilder(1024);
                intRet = GetParam(intH, "FHZ", strValue, 1024);
                if (strValue.ToString() == "1")
                {
                    StringBuilder sbValue = new StringBuilder(1024);
                    intRet = GetParam(intH, "MSG", sbValue, 1024);
                    string strCGBZ = sbValue.ToString().Trim();//原因
                    if (strCGBZ == "执行成功！" || strCGBZ == "1")
                    {
                        lngRes = 1;
                        lstVo[i].JBR = exVo.JBR;
                        lstVo[i].Issucess = 1;//1 上传成功 
                    }
                }
                else
                {
                    lngRes = -1;
                    intRet = GetParam(intH, "MSG", strValue, 1024);
                    ExceptionLog.OutPutException(lstVo[i].JZJLH + "-" + lstVo[i].INPATIENTID + ":" + strValue.ToString());
                    lstVo[i].Issucess = -1; //-1 上传失败
                    lstVo[i].FailMsg = "就诊记录号:" + lstVo[i].JZJLH + " 住院号：" + lstVo[i].INPATIENTID + "\n" + strValue.ToString();
                }

                DestroyInstance(intH);
            }
            
            return lngRes;
        }
        #endregion

        #region 门诊处方项目明细上传
        public static long lngFunSP3_2002(ref List<EntityMzcf> lstVo, EntityDGExtra exVo, ref StringBuilder strValue)
        {
            long lngRes = -1;
            int intRet;
            for (int i = 0; i < lstVo.Count; i++)
            {
                int intH = CreateInstace();
                if (intH > 0)
                {
                    intRet = SetParam(intH, "FN", "SP3_2002");
                    intRet = SetParam(intH, "JBR", exVo.JBR);
                    lstVo[i].JBR = exVo.JBR;
                    intRet = SetParam(intH, "YYBH", exVo.YYBH);
                    intRet = InsertDataSet(intH);
                }
                else
                    continue;
                
                for (int j = 0; j < lstVo[i].lstCfMsg.Count; j++)
                {
                    intRet = InsertRow(intH);
                    intRet = SetField(intH, "ZYH", lstVo[i].ZYH);//
                    intRet = SetField(intH, "CFH", lstVo[i].CFH);//
                    intRet = SetField(intH, "GMSFHM", lstVo[i].GMSFHM);//
                    intRet = SetField(intH, "JZLB", lstVo[i].JZLB);//
                    intRet = SetField(intH, "FYRQ", lstVo[i].FYRQ);//8位
                    intRet = SetField(intH, "XMXH", lstVo[i].lstCfMsg[j].XMXH);//
                    intRet = SetField(intH, "XMBH", lstVo[i].lstCfMsg[j].XMBH);//
                    intRet = SetField(intH, "XMMC", lstVo[i].lstCfMsg[j].XMMC);//
                    intRet = SetField(intH, "JG", lstVo[i].lstCfMsg[j].JG);
                    intRet = SetField(intH, "MCYL", lstVo[i].lstCfMsg[j].MCYL);//
                    intRet = SetField(intH, "JE", lstVo[i].lstCfMsg[j].JE);
                    intRet = SetField(intH, "ZFBL", lstVo[i].lstCfMsg[j].ZFBL);//0<= X <= 1 无比例时，默认传0
                    intRet = SetField(intH, "YSGH", lstVo[i].YSGH);
                    intRet = SetField(intH, "BZ", lstVo[i].lstCfMsg[j].BZ);
                    EndRow(intH, i + 1);

                    //Log.Output("\r\n ZYH:" + lstVo[i].ZYH + "\r\n CFH:" + lstVo[i].CFH + " \r\n GMSFHM:" +
                    //    lstVo[i].GMSFHM + "\r\n JZLB:" + lstVo[i].JZLB + "\r\n FYRQ:" + lstVo[i].FYRQ + "\r\n XMXH:" +
                    //    lstVo[i].lstCfMsg[j].XMXH + "\r\n XMBH:" + lstVo[i].lstCfMsg[j].XMBH + "\r\n XMMC:" + 
                    //    lstVo[i].lstCfMsg[j].XMMC + "\r\n JG:" +
                    //    lstVo[i].lstCfMsg[j].JG + "\r\n MCYL:" + lstVo[i].lstCfMsg[j].MCYL + "\r\n JE:" +
                    //    lstVo[i].lstCfMsg[j].JE + "\r\n ZFBL:" + lstVo[i].lstCfMsg[j].ZFBL + "\r\n YSGH:" +
                    //    lstVo[i].YSGH + "\r\n BZ:" + lstVo[i].lstCfMsg[j].BZ);
                }
                intRet = EndDataSet(intH, "MZCFXMDR");
                intRet = Run(intH);

                strValue = new StringBuilder(1024);
                intRet = GetParam(intH, "FHZ", strValue, 1024);
                if (strValue.ToString() == "1")
                {
                    StringBuilder sbValue = new StringBuilder(1024);
                    intRet = GetParam(intH, "MSG", sbValue, 1024);
                    string strCGBZ = sbValue.ToString().Trim();//原因
                    if (strCGBZ == "执行成功！" || strCGBZ == "1")
                    {
                        intRet = GetParam(intH, "PH", strValue, 1024);//批号，	批号（PH）：由医保系统产生，作为本次传送的标识。
                        string strPH = strValue.ToString();
                        lstVo[i].PH = strPH;
                        intRet = GetParam(intH, "ZJE", strValue, 1024);//	总金额：本次传送的记帐处方项目汇总的医疗费用总额。每条传入的处方项目的“金额”字段合计出的总金额。
                        string strZJE = strValue.ToString();
                        lstVo[i].ZJE = strZJE;
                        lngRes = 1;
                        lstVo[i].IsSuccess = 1;
                    }
                }
                else
                {
                    intRet = GetParam(intH, "MSG", strValue, 1024);
                    ExceptionLog.OutPutException(strValue.ToString());
                    lstVo[i].IsSuccess = -1;
                    lstVo[i].failMsg = strValue.ToString();
                }
                DestroyInstance(intH);
            }
           
            return lngRes;
        }
        #endregion

        #region 门诊记帐处方项目删除
        public static long lngFunSP3_2003(List<EntityMzcf> lstVo, EntityDGExtra exVo, ref StringBuilder strValue)
        {
            long lngRes = -1;
            
            int intRet;
            for (int i = 0; i < lstVo.Count; i++)
            {
                int intH = CreateInstace();
                if (intH < 0)
                    continue;
                intRet = SetParam(intH, "FN", "SP3_2003");
                intRet = SetParam(intH, "YYBH", exVo.YYBH);
                intRet = SetParam(intH,"ZYH", lstVo[i].ZYH);
                intRet = SetParam(intH, "CFH", lstVo[i].CFH);
                intRet = SetParam(intH, "JBR", exVo.JBR);
                lstVo[i].JBR = exVo.JBR;
                intRet = Run(intH);
                strValue = new StringBuilder(1024);
                intRet = GetParam(intH, "FHZ", strValue, 1024);
                if (strValue.ToString() == "1")
                {
                    StringBuilder sbValue = new StringBuilder(1024);
                    intRet = GetParam(intH, "MSG", sbValue, 1024);
                    string strCGBZ = sbValue.ToString().Trim();//原因
                    if (strCGBZ == "执行成功！" || strCGBZ == "1")
                    {
                        lstVo[i].STATUS = 2;
                        lngRes = 1;
                        lstVo[i].IsSuccess = 1;
                    }
                }
                else
                {
                    intRet = GetParam(intH, "MSG", strValue, 1024);
                    ExceptionLog.OutPutException(strValue.ToString());
                    lstVo[i].IsSuccess = -1;
                    lstVo[i].failMsg = strValue.ToString();
                }
                DestroyInstance(intH);
            }

            return lngRes;
        }
        #endregion

        #region
        #region HISYB.XML读写操作
        /// <summary>
        /// XML文件名
        /// </summary>
        public static string XMLFile = Application.StartupPath + @"\HISYB.xml";
        /// <summary>
        /// 读操作
        /// </summary>
        /// <param name="parentnode"></param>
        /// <param name="childnode"></param>
        /// <param name="key"></param>
        public static string strReadXML(string parentnode, string childnode, string key)
        {
            string strRet = "";
            try
            {
                if (File.Exists(XMLFile))
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(XMLFile);

                    XmlNode xndP = xdoc.DocumentElement.SelectNodes(@"//" + parentnode)[0];
                    XmlNode xndC = xndP.SelectSingleNode(@"//" + childnode + @"[@key='" + key + @"']");

                    if (xndP != null)
                    {
                        strRet = xndC.Attributes["value"].Value.ToString().Trim();
                    }
                }
            }
            catch
            {
                strRet = "";
            }
            return strRet;
        }
        #endregion

        #region 医院登录
        /// <summary>
        /// 医院登录
        /// </summary>
        /// <param name="strUser">用户名</param>
        /// <param name="strPwd">口令</param>
        /// <param name="Blxml">判断是否读取xml用户名和密码</param>
        /// <returns></returns>
        public static long lngUserLoin(string strUser, string strPwd, bool Blxml)
        {
            //初始化
            lngInitialize();
            int intPtr = CreateInstace();
            string strHosCode = strUser;
            if (Blxml)
            {
                strUser = strReadXML("DGCSMZYB", "USERNAMEMZ", "AnyOne");//need modify 需要传进来
                strPwd = strReadXML("DGCSMZYB", "PASSWORDMZ", "AnyOne");
                strHosCode = strReadXML("DGCSMZYB", "YYBHMZ", "AnyOne");
            }
            int intRet;
            StringBuilder sbPwd = new StringBuilder(32);
            if (intPtr > 0)
            {
                intRet = EncryptWithCipher(intPtr, strPwd, sbPwd, 32);
                if (intRet < 0)
                {
                    MessageBox.Show("明文密码加密失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    DestroyInstance(intPtr);
                    return -1;
                }
                string strNewPwd = sbPwd.ToString();
                //医院登录
                intRet = SetParam(intPtr, "FN", "1");
                intRet = SetParam(intPtr, "YYBH", strHosCode);
                intRet = SetParam(intPtr, "USERID", strUser);
                intRet = SetParam(intPtr, "PWD", strNewPwd);
                intRet = SetParam(intPtr, "JBRLX", "1");
                intRet = SetParam(intPtr, "CLIENTTYPE", "HIS");
                intRet = SetParam(intPtr, "JBR", "001");
                intRet = Run(intPtr);
                if (intRet < 0)
                {
                    StringBuilder strRetValue1 = new StringBuilder(32);
                    intRet = GetSysMessage(intPtr, strRetValue1, 66);
                    MessageBox.Show(strRetValue1.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    DestroyInstance(intPtr);
                    return -1;
                }
                StringBuilder strRetValue = new StringBuilder(32);
                StringBuilder strRetMessage = new StringBuilder(1024);
                intRet = GetParam(intPtr, "FHZ", strRetValue, 32);
                intRet = GetParam(intPtr, "MSG", strRetMessage, 1024);
                if (strRetValue.ToString() == "EHIS9700")
                {
                    ExceptionLog.OutPutException("返回值：EHIS9700 \r\n" + "社保系统登录故障，请稍后重新登录.");
                    MessageBox.Show("社保系统登录故障，请稍后重新登录！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    DestroyInstance(intPtr);
                    return -1;
                }
                else if (strRetValue.ToString() != "1")
                {
                    ExceptionLog.OutPutException(strRetMessage.ToString());
                    MessageBox.Show(strRetMessage.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    DestroyInstance(intPtr);
                }
            }
            else
            {
                MessageBox.Show("创建实例失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                DestroyInstance(intPtr);
                return -1;
            }
            return 1;
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static long lngInitialize()
        {
            string svrIP = strReadXML("DGCSMZYB", "svrIPMZ", "AnyOne");
            string svrPort = strReadXML("DGCSMZYB", "svrPortMZ", "AnyOne");
            short shPort = 0;
            short.TryParse(svrPort, out shPort);
            string SndBufSize = strReadXML("DGCSMZYB", "SndBufSizeMZ", "AnyOne");
            string RecvBufSize = strReadXML("DGCSMZYB", "RecvBufSizeMZ", "AnyOne");
            int intSize = 0, intSize2 = 0;
            int.TryParse(SndBufSize, out intSize);
            int.TryParse(RecvBufSize, out intSize2);
            int intPtr = Initialize(svrIP, shPort, intSize, intSize2);
            if (intPtr < 0)
            {
                MessageBox.Show("初始化失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return -1;
            }
            return intPtr;
        }
        #endregion

        #endregion
    }
}
