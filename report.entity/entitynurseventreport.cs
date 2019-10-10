using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityNursEventSkin : BaseDataContract
    {
       #region  护理皮肤损害安全（不良）事件统计
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
       /// <summary>
       ///院外压力性损伤
       /// </summary>
       [DataMember]
        public int YWYLXSS { get; set; }
       /// <summary>
       ///院外失禁性皮炎（选择小便/大便/大小便）
       /// </summary>
       [DataMember]
        public int YWSJXPY { get; set; }
       /// <summary>
       ///院内压力性皮肤损伤（选择）
       /// </summary>
       [DataMember]
        public int YNYLXPFSS { get; set; }
       /// <summary>
       ///院内失禁性皮炎（选择小便/大便/大小便）
       /// </summary>
       [DataMember]
        public int YNSJXPY { get; set; }
       /// <summary>
       ///医源性皮肤损伤（选择）
       /// </summary>
       [DataMember]
        public int YYXPFSS { get; set; }
       /// <summary>
       ///（非医源性）意外皮肤损伤
       /// </summary>
       [DataMember]
        public int FYYXYWPFSS { get; set; }

       [DataMember]
       public int YWYLXSSFQ_HJ { get; set; }
       /// <summary>
       ///院外压力性损伤分期 Ⅰ期
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_I { get; set; }
       /// <summary>
       ///院外压力性损伤分期 Ⅱ期
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_II { get; set; }
       /// <summary>
       ///院外压力性损伤分期 Ⅲ期
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_III { get; set; }
       /// <summary>
       ///院外压力性损伤分期 Ⅳ期
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_IV { get; set; }
       /// <summary>
       ///院外压力性损伤分期 无法分期
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_WFFQ { get; set; }
       /// <summary>
       ///院外压力性损伤分期 深部组织损伤
       /// </summary>
       [DataMember]
       public int YWYLXSSFQ_SBZZSS { get; set; }

       [DataMember]
       public int YWYLXSSBW_HJ { get; set; }

       [DataMember]
       public int YNYLXSSFQ_HJ { get; set; }
       /// <summary>
       ///院内压力性损伤分期 Ⅰ期
       /// </summary>
       [DataMember]
       public int YNYLXSSFQ_I { get; set; }
       /// <summary>
       ///院内压力性损伤分期 Ⅱ期
       /// </summary>
       [DataMember]
        public int YNYLXSSFQ_II { get; set; }
       /// <summary>
       ///院内压力性损伤分期 Ⅲ期
       /// </summary>
        [DataMember]
        public int YNYLXSSFQ_III { get; set; }
       /// <summary>
       ///院内压力性损伤分期 Ⅳ期
       /// </summary>
       [DataMember]
       public int YNYLXSSFQ_IV { get; set; }
       /// <summary>
       ///院内压力性损伤分期 无法分期
       /// </summary>
       [DataMember]
       public int YNYLXSSFQ_WFFQ { get; set; }
       /// <summary>
       ///院内压力性损伤分期 深部组织损伤
       /// </summary>
        [DataMember]
       public int YNYLXSSFQ_SBZZSS { get; set; }
       /// <summary>
       ///院外压力性损伤部位 骶尾椎骨处
       /// </summary>
        [DataMember]
        public int YWYLXSSBW_DWZGC { get; set; }
       /// <summary>
       ///院外压力性损伤部位 坐骨处
       /// </summary>
       [DataMember]
       public int YWYLXSSBW_ZGC { get; set; }
        /// <summary>
       ///院外压力性损伤部位 股骨粗隆处
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_GGZLC { get; set; }
        /// <summary>
        ///院外压力性损伤部位 跟骨处
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_KGC { get; set; }
        /// <summary>
        ///院外压力性损伤部位 足踝处
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_ZWC { get; set; }
        /// <summary>
        ///院外压力性损伤部位 肩胛处
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_JJC { get; set; }
        /// <summary>
        ///院外压力性损伤部位 枕骨处
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_CGC { get; set; }
        /// <summary>
        ///院外压力性损伤部位 肘部
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_CB { get; set; }

        /// <summary>
        ///院外压力性损伤部位 其它部位
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_QTBW { get; set; }
        /// <summary>
        ///院外压力性损伤部位 多部位
        /// </summary>
        [DataMember]
        public int YWYLXSSBW_DBW { get; set; }
        /// <summary>
        ///院内压力性损伤部位 骶尾椎骨处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_DWZGC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 坐骨处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_ZGC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 股骨粗隆处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_GGZLC { get; set; }

        /// <summary>
        ///院内压力性损伤部位 跟骨处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_KGC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 足踝处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_ZWC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 肩胛处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_JJC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 枕骨处
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_CGC { get; set; }
        /// <summary>
        ///院内压力性损伤部位 肘部
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_CB { get; set; }
        /// <summary>
        ///院内压力性损伤部位 其它部位
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_QTBW { get; set; }
        /// <summary>
        ///院内压力性损伤部位 多部位
        /// </summary>
        [DataMember]
        public int YNYLXSSBW_DBW { get; set; }
        [DataMember]
        public int YNYLXSSBW_HJ { get; set; }

        #region 院外压力性损伤患者来源
        [DataMember]
        public int YWHZLY_JT { get; set; }
        [DataMember]
        public int YWHZLY_YLY { get; set; }
        [DataMember]
        public int YWHZLY_QTYY { get; set; }
        [DataMember]
        public int YWHZLY_QTLY { get; set; }
        [DataMember]
        public int YWHZLY_HJ { get; set; }
        #endregion
        #region 跟踪转归
        [DataMember]
        public int KZLG_ZY { get; set; }
        [DataMember]
        public int KZLG_HZ { get; set; }
        [DataMember]
        public int KZLG_WY { get; set; }
        [DataMember]
        public int KZLG_JZ { get; set; }
        [DataMember]
        public int KZLG_CY { get; set; }
        [DataMember]
        public int KZLG_ZY1 { get; set; }
        [DataMember]
        public int KZLG_SW { get; set; }
        [DataMember]
        public int KZLG_HJ { get; set; }
        #endregion

       #endregion
    }

    [DataContract, Serializable]
    public class EntityNursEventClass : BaseDataContract
    {
        #region  护理质量与安全（不良事件）类型统计
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        /// 查对不合格
        /// </summary>
        [DataMember]
        public int CDBHG { get; set; }
        /// <summary>
        /// 身份识别错误（患者身份查对）
        /// </summary>
        [DataMember]
        public int SFSBCW { get; set; }
        /// <summary>
        /// 使用药物错误（发生在患者身上）
        /// </summary>
        [DataMember]
        public int SYYWCW { get; set; }
        /// <summary>
        /// 标本丢失、损毁
        /// </summary>
        [DataMember]
        public int BBDSSH { get; set; }
        /// <summary>
        /// 急救设备器材药品不合格
        /// </summary>
        [DataMember]
        public int JQSBJCYPBHG { get; set; }
        /// <summary>
        /// 无菌物品不合格
        /// </summary>
        [DataMember]
        public int WJWPBHG { get; set; }
        /// <summary>
        /// 发放不合格的消毒或灭菌物品
        /// </summary>
        [DataMember]
        public int FFBHGXDHMJWP { get; set; }
        /// <summary>
        /// 贵重医疗器材损毁或丢失
        /// </summary>
        [DataMember]
        public int GCYLJCSH { get; set; }
        /// <summary>
        /// 发生召回灭菌物品事件
        /// </summary>
        [DataMember]
        public int FSZHMJWPSJ { get; set; }
        /// <summary>
        /// 包内器械物品配置错误影响手术进程
        /// </summary>
        [DataMember]
        public int BNJXWPPZCW { get; set; }
        /// <summary>
        /// 发生与灭菌器械相关的感染事件
        /// </summary>
        [DataMember]
        public int FSYMJJXXDSJ { get; set; }
        /// <summary>
        /// 药物外渗
        /// </summary>
        [DataMember]
        public int YWWS { get; set; }
        /// <summary>
        /// 药物渗出
        /// </summary>
        [DataMember]
        public int YWSC { get; set; }
        /// <summary>
        /// 输血反应
        /// </summary>
        [DataMember]
        public int SXFY { get; set; }
        /// <summary>
        /// 输液反应
        /// </summary>
        [DataMember]
        public int SYFY { get; set; }
        /// <summary>
        /// 非计划性拔管
        /// </summary>
        [DataMember]
        public int FJHXBG { get; set; }
        /// <summary>
        /// 跌倒
        /// </summary>
        [DataMember]
        public int TD { get; set; }
        /// <summary>
        /// 坠床
        /// </summary>
        [DataMember]
        public int ZC { get; set; }
        /// <summary>
        /// 走失
        /// </summary>
        [DataMember]
        public int ZS { get; set; }
        /// <summary>
        /// 误吸
        /// </summary>
        [DataMember]
        public int WX { get; set; }
        /// <summary>
        /// 足下垂/关节僵硬/肌肉萎缩
        /// </summary>
        [DataMember]
        public int CXZGJJYJRWS { get; set; }
        /// <summary>
        /// DVT/PET 
        /// </summary>
        [DataMember]
        public int DVTPET { get; set; }
        /// <summary>
        /// 新生儿烧伤、烫伤
        /// </summary>
        [DataMember]
        public int XSESS { get; set; }
        /// <summary>
        /// 新生儿鼻中隔压伤
        /// </summary>
        [DataMember]
        public int XSEBZGYS { get; set; }
        /// <summary>
        /// 产后出血
        /// </summary>
        [DataMember]
        public int CHCX { get; set; }
        /// <summary>
        /// 阴道分娩新生儿产伤
        /// </summary>
        [DataMember]
        public int YDFMXSECS { get; set; }
        /// <summary>
        /// 阴道分娩产妇尿潴留
        /// </summary>
        [DataMember]
        public int YDFMCFNCL { get; set; }
        /// <summary>
        /// 使用催产素并发症
        /// </summary>
        [DataMember]
        public int SYCCSBFZ { get; set; }
        /// <summary>
        /// 会阴裂伤
        /// </summary>
        [DataMember]
        public int HYLS { get; set; }
        /// <summary>
        /// 手术查对不合格
        /// </summary>
        [DataMember]
        public int SSCD { get; set; }
        /// <summary>
        /// 手术过程异物遗留
        /// </summary>
        [DataMember]
        public int SSGCYWYL { get; set; }
        /// <summary>
        /// 手术标本处理不合格
        /// </summary>
        [DataMember]
        public int SSBBCL { get; set; }
        /// <summary>
        /// 中心静脉导管相关血流感染
        /// </summary>
        [DataMember]
        public int ZXJMDGXLGR { get; set; }
        /// <summary>
        /// 使用呼吸机卧位不正确
        /// </summary>
        [DataMember]
        public int SYHXJWWBZQ { get; set; }
        /// <summary>
        /// 急诊分诊不合格
        /// </summary>
        [DataMember]
        public int JZFZBHG { get; set; }
        /// <summary>
        /// 运送中病情变化    
        /// </summary>
        [DataMember]
        public int YSZBQBH { get; set; }
        /// <summary>
        /// 擅自离院 
        /// </summary>
        [DataMember]
        public int SZLY { get; set; }
        /// <summary>
        /// 自残  
        /// </summary>
        [DataMember]
        public int ZC_1 { get; set; }
        /// <summary>
        /// 自杀   
        /// </summary>
        [DataMember]
        public int ZS_1 { get; set; }
        /// <summary>
        /// 猝死  
        /// </summary>
        [DataMember]
        public int CS { get; set; }
        /// <summary>
        /// 失窃    
        /// </summary>
        [DataMember]
        public int SQ { get; set; }
        /// <summary>
        /// 投诉纠纷 
        /// </summary>
        [DataMember]
        public int DSJF { get; set; }
        /// <summary>
        /// 暴力行为  
        /// </summary>
        [DataMember]
        public int BLXW { get; set; }

        /// <summary>
        /// 意外伤害
        /// </summary>
        [DataMember]
        public int YWSH { get; set; }
        /// <summary>
        /// 并发症
        /// </summary>
        [DataMember]
        public int BFZ { get; set; }
        /// <summary>
        /// 其他事件
        /// </summary>
        [DataMember]
        public int QTSJ { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        [DataMember]
        public int HJ { get; set; }

        #endregion
    }

    [DataContract, Serializable]
    public class EntityNursEventSubClass : BaseDataContract
    {
        #region 护理质量与安全（不良事件）细分类型统计
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        /// 非溶血反应
        /// </summary>
        [DataMember]
        public int SXFY_RXFY { get; set; }
        /// <summary>
        /// 溶血反应
        /// </summary>
        [DataMember]
        public int SXFY_FRXFY { get; set; }
        /// <summary>
        /// 发热
        /// </summary>
        [DataMember]
        public int SYFY_FR { get; set; }
        /// <summary>
        /// 静脉炎
        /// </summary>
        [DataMember]
        public int SYFY_JMY { get; set; }
        /// <summary>
        /// 过敏
        /// </summary>
        [DataMember]
        public int SYFY_GM { get; set; }
        /// <summary>
        /// 空气栓塞
        /// </summary>
        [DataMember]
        public int SYFY_XS { get; set; }
        /// <summary>
        /// 中心静脉导管
        /// </summary>
        [DataMember]
        public int FJHXBG_ZXJMDG { get; set; }
        /// <summary>
        /// 气管插管
        /// </summary>
        [DataMember]
        public int FJHXBG_QGCG { get; set; }
        /// <summary>
        /// 胃管
        /// </summary>
        [DataMember]
        public int FJHXBG_WG { get; set; }
        /// <summary>
        /// 尿管
        /// </summary>
        [DataMember]
        public int FJHXBG_NG { get; set; }
        /// <summary>
        /// 引流管
        /// </summary>
        [DataMember]
        public int FJHXBG_YLG { get; set; }
        /// <summary>
        /// 骨折
        /// </summary>
        [DataMember]
        public int YDFMXSECS_GZ { get; set; }
        /// <summary>
        /// 重度窒息
        /// </summary>
        [DataMember]
        public int YDFMXSECS_CDZX { get; set; }
        /// <summary>
        /// 臂丛神经损伤
        /// </summary>
        [DataMember]
        public int YDFMXSECS_BCSJSS { get; set; }
        /// <summary>
        /// 窒息
        /// </summary>
        [DataMember]
        public int WX_ZX { get; set; }
        /// <summary>
        /// 肺炎
        /// </summary>
        [DataMember]
        public int WX_FY { get; set; }
        /// <summary>
        /// 产房出血
        /// </summary>
        [DataMember]
        public int CHCX_CFCX { get; set; }
        /// <summary>
        /// 病房产后出血
        /// </summary>
        [DataMember]
        public int CHCX_BFCX { get; set; }
        /// <summary>
        /// 手术患者身份
        /// </summary>
        [DataMember]
        public int SSCD_HZSF { get; set; }
        /// <summary>
        /// 手术部位标识
        /// </summary>
        [DataMember]
        public int SSCD_SSBW { get; set; }
        /// <summary>
        /// 手术同意书内容
        /// </summary>
        [DataMember]
        public int SSCD_SSTYS { get; set; }
        /// <summary>
        /// TIME OUT
        /// </summary>
        [DataMember]
        public int SSCD_TIMEOUT { get; set; }
        /// <summary>
        /// 遗失
        /// </summary>
        [DataMember]
        public int SSBBCL_YS { get; set; }
        /// <summary>
        /// 留置不合格
        /// </summary>
        [DataMember]
        public int SSBBCL_LZ { get; set; }
        /// <summary>
        /// 漏送
        /// </summary>
        [DataMember]
        public int SSBBCL_LS { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        [DataMember]
        public int HJ { get; set; }
        #endregion
    }

    [DataContract, Serializable]
    public class EntityNursEventDept : BaseDataContract
    {
        #region 护理质量与安全（不良事件）上报科室统计
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        /// 内一区
        /// </summary>
        [DataMember]
        public int NYQ { get; set; }
        /// <summary>
        /// 内二区
        /// </summary>
        [DataMember]
        public int NEQ { get; set; }
        /// <summary>
        /// 神经外科
        /// </summary>
        [DataMember]
        public int SJWK { get; set; }
        /// <summary>
        /// 骨科
        /// </summary>
        [DataMember]
        public int GK { get; set; }
        /// <summary>
        /// 耳鼻喉科
        /// </summary>
        [DataMember]
        public int EBHK { get; set; }
        /// <summary>
        /// 眼科
        /// </summary>
        [DataMember]
        public int YK { get; set; }
        /// <summary>
        /// 中医科
        /// </summary>
        [DataMember]
        public int ZYK { get; set; }
        /// <summary>
        /// 普外科
        /// </summary>
        [DataMember]
        public int PWK { get; set; }
        /// <summary>
        /// 泌尿外科
        /// </summary>
        [DataMember]
        public int MNWK { get; set; }
        /// <summary>
        /// 手外科
        /// </summary>
        [DataMember]
        public int SWK { get; set; }
        /// <summary>
        /// 妇科
        /// </summary>
        [DataMember]
        public int FK { get; set; }
        /// <summary>
        /// 产科
        /// </summary>
        [DataMember]
        public int CK { get; set; }
        /// <summary>
        /// 普儿科
        /// </summary>
        [DataMember]
        public int PEK { get; set; }
        /// <summary>
        /// 新生儿科
        /// </summary>
        [DataMember]
        public int XSEK { get; set; }
        /// <summary>
        /// ICU
        /// </summary>
        [DataMember]
        public int ICU { get; set; }
        /// <summary>
        /// 手术室
        /// </summary>
        [DataMember]
        public int SSS { get; set; }
        /// <summary>
        /// 急诊科
        /// </summary>
        [DataMember]
        public int JZK { get; set; }
        /// <summary>
        /// 门诊部（包括放射、B超、口腔、分诊、专科门诊）
        /// </summary>
        [DataMember]
        public int MZB { get; set; }
        /// <summary>
        /// 第二门诊
        /// </summary>
        [DataMember]
        public int DEMZ { get; set; }
        /// <summary>
        /// 第三门诊
        /// </summary>
        [DataMember]
        public int DSMZ { get; set; }
        /// <summary>
        /// 多功能科（包括高压氧）
        /// </summary>
        [DataMember]
        public int DGNK { get; set; }
        /// <summary>
        /// 消毒供应室
        /// </summary>
        [DataMember]
        public int XDGYS { get; set; }
        /// <summary>
        /// 体检科
        /// </summary>
        [DataMember]
        public int TJK { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        [DataMember]
        public int HJ { get; set; }
        #endregion
    }

    [DataContract, Serializable]
    public class EntityNursEventRptClass : BaseDataContract
    {
        #region 护理质量与安全（不良事件）上报类型统计
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        /// Ⅰ型
        /// </summary>
        [DataMember]
        public int IJSJ { get; set; }
        /// <summary>
        /// Ⅱ型
        /// </summary>
        [DataMember]
        public int IIJSJ { get; set; }
        /// <summary>
        /// Ⅲ型
        /// </summary>
        [DataMember]
        public int IIIJSJ { get; set; }
        /// <summary>
        /// Ⅳ型
        /// </summary>
        [DataMember]
        public int IVJSJ { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        [DataMember]
        public int HJ { get; set; }
        #endregion
    }

    [DataContract, Serializable]
    public class EntityNursEventStrument : BaseDataContract
    {
        #region 全院护理上报安全事件汇总表

        public string reportTime { get; set; }
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        ///序号
        /// </summary>
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        ///科室
        /// </summary>
        [DataMember]
        public string KS { get; set; }
        /// <summary>
        /// 安全事件发生日期
        /// </summary>
        [DataMember]
        public string FSRQSJ { get; set; }
        /// <summary>
        /// 住院号/诊疗号
        /// </summary>
        [DataMember]
        public string ZYHZLH { get; set; }
        /// <summary>
        /// 床号
        /// </summary>
        [DataMember]
        public string CH { get; set; }
        /// <summary>
        ///姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public string NL { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string XB { get; set; }
        /// <summary>
        ///安全事件类型
        /// </summary>
        [DataMember]
        public string AQSJLX { get; set; }
        /// <summary>
        /// 安全事件等级
        /// </summary>
        [DataMember]
        public string AQSJDJ { get; set; }
        /// <summary>
        /// 安全事件经过
        /// </summary>
        [DataMember]
        public string AQSJJG { get; set; }
        /// <summary>
        /// 安全事件损害程度
        /// </summary>
        [DataMember]
        public string AQSJSHCD { get; set; }
        /// <summary>
        /// 上报者
        /// </summary>
        [DataMember]
        public string SBZ { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        [DataMember]
        public int HJ { get; set; }
        /// <summary>
        /// 当事人
        /// </summary>
        public string DSR { get; set; }
        /// <summary>
        /// 当事人职称
        /// </summary>
        public string DSRZC { get; set; }
        #endregion
    }

    [DataContract, Serializable]
    public class EntitySkinEventStrument : BaseDataContract
    {
        #region 皮肤上报安全事件汇总表
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        ///序号
        /// </summary>
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        ///科室
        /// </summary>
        [DataMember]
        public string KS { get; set; }
        /// <summary>
        /// 安全事件发生日期
        /// </summary>
        [DataMember]
        public string FSRQSJ { get; set; }
        /// <summary>
        /// 住院号/诊疗号
        /// </summary>
        [DataMember]
        public string ZYHZLH { get; set; }
        /// <summary>
        /// 床号
        /// </summary>
        [DataMember]
        public string CH { get; set; }
        /// <summary>
        ///姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public string NL { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string XB { get; set; }
        /// <summary>
        ///安全事件类型
        /// </summary>
        [DataMember]
        public string AQSJLX { get; set; }
        /// <summary>
        ///部位
        /// </summary>
        [DataMember]
        public string BW { get; set; }
        /// <summary>
        ///分期
        /// </summary>
        [DataMember]
        public string FQ { get; set; }
        /// <summary>
        /// 安全事件经过
        /// </summary>
        [DataMember]
        public string AQSJJG { get; set; }
        
        /// <summary>
        /// 上报者
        /// </summary>
        [DataMember]
        public string SBZ { get; set; }
       
        #endregion
    }
    
}
