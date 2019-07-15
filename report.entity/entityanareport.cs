using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityAnaRegister1 : BaseDataContract
    {
        /// <summary>
        /// AnaId  AnaID
        /// </summary>
        [DataMember]
        public decimal AnaId { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 入手术室日期时间
        /// </summary>
        [DataMember]
        public string RSSSRQSJ { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string KS { get; set; }
        /// <summary>
        /// 床位
        /// </summary>
        [DataMember]
        public string CW { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        [DataMember]
        public string ZYH { get; set; }
        /// <summary>
        /// 麻醉编号
        /// </summary>
        [DataMember]
        public string MZBH { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public string XB { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public string NL { get; set; }
        /// <summary>
        /// 术前诊断
        /// </summary>
        [DataMember]
        public string SQZD { get; set; }
        /// <summary>
        /// 手术名称
        /// </summary>
        [DataMember]
        public string SSMC { get; set; }
        /// <summary>
        /// 麻醉分级
        /// </summary>
        [DataMember]
        public string MZFJ { get; set; }
        /// <summary>
        /// 手术分级
        /// </summary>
        [DataMember]
        public string SSFJ { get; set; }
        /// <summary>
        /// 是否麻醉前讨论
        /// </summary>
        [DataMember]
        public string SFMZQTL { get; set; }
        /// <summary>
        /// 麻醉方式
        /// </summary>
        [DataMember]
        public string MZFS { get; set; }
        /// <summary>
        /// 镇痛方式
        /// </summary>
        [DataMember]
        public string ZTFS { get; set; }
        /// <summary>
        /// 是否心肺复苏
        /// </summary>
        [DataMember]
        public string SFXFFS { get; set; }
        /// <summary>
        /// 是否进入复苏室
        /// </summary>
        [DataMember]
        public string SFJRFSS { get; set; }
        /// <summary>
        /// 离室Steward>=4分
        /// </summary>
        [DataMember]
        public string LSSF { get; set; }
        /// <summary>
        /// 是否入ICU
        /// </summary>
        [DataMember]
        public string SFJICU { get; set; }
        /// <summary>
        /// 麻醉效果
        /// </summary>
        [DataMember]
        public string MZXG { get; set; }
        /// <summary>
        /// 是否麻醉非预期相关事件
        /// </summary>
        [DataMember]
        public string SFMZFYQXGSJ { get; set; }
        /// <summary>
        /// 麻醉用时
        /// </summary>
        [DataMember]
        public string MZYS { get; set; }
        /// <summary>
        /// 手术用时
        /// </summary>
        [DataMember]
        public string SSYS { get; set; }
        /// <summary>
        /// 主刀医师
        /// </summary>
        [DataMember]
        public string ZDYS { get; set; }
        /// <summary>
        /// 一助医师
        /// </summary>
        [DataMember]
        public string YZYS { get; set; }
        /// <summary>
        /// 器械护士
        /// </summary>
        [DataMember]
        public string QXHS { get; set; }
        /// <summary>
        /// 巡回护士
        /// </summary>
        [DataMember]
        public string XHHS { get; set; }
        /// <summary>
        /// 麻醉医师
        /// </summary>
        [DataMember]
        public string MZYS2 { get; set; }

        /// <summary>
        /// 麻醉开始时间
        /// </summary>
        [DataMember]
        public DateTime? MZKSSJ { get; set; }

        /// <summary>
        /// 麻醉结束时间
        /// </summary>
        [DataMember]
        public DateTime? MZJSSJ { get; set; }

        /// <summary>
        /// 手术开始时间
        /// </summary>
        [DataMember]
        public DateTime? SSKSSJ { get; set; }

        /// <summary>
        /// 手术结束时间
        /// </summary>
        [DataMember]
        public DateTime? SSJSSJ { get; set; }

    }

    [DataContract, Serializable]
    public class EntityAnaRegister2 : BaseDataContract
    {
        /// <summary>
        /// AnaId
        /// </summary>
        [DataMember]
        public decimal AnaId { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 镇痛开始时间
        /// </summary>
        [DataMember]
        public string ZTKSSJ { get; set; }
        /// <summary>
        /// 停泵时间
        /// </summary>
        [DataMember]
        public string TBSJ { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string KS { get; set; }
        /// <summary>
        /// 床位
        /// </summary>
        [DataMember]
        public string CW { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// <summary>
        /// 手术名称
        /// </summary>
        [DataMember]
        public string SSMC { get; set; }
        /// <summary>
        /// 镇痛方式
        /// </summary>
        [DataMember]
        public string ZTFS { get; set; }
        /// <summary>
        /// VAS评分
        /// </summary>
        [DataMember]
        public string VASPF { get; set; }
        /// <summary>
        /// 是否并发症
        /// </summary>
        [DataMember]
        public string SFBFZ { get; set; }
        /// <summary>
        /// 麻醉医师
        /// </summary>
        [DataMember]
        public string MZYS { get; set; }

    }

    [DataContract, Serializable]
    public class EntityAnaStat1 : BaseDataContract
    {
        /// <summary>
        /// 月份
        /// </summary>
        [DataMember]
        public string YF { get; set; }
        /// <summary>
        /// 麻醉总例数   椎管内麻醉
        /// </summary>
        [DataMember]
        public int MZZLS_ZGNMZ { get; set; }
        /// <summary>
        /// 麻醉总例数   插管全麻
        /// </summary>
        [DataMember]
        public int MZZLS_CGMZ { get; set; }
        /// <summary>
        /// 麻醉总例数   非插管全麻
        /// </summary>
        [DataMember]
        public int MZZLS_FCGMZ { get; set; }
        /// <summary>
        /// 麻醉总例数   复合麻醉
        /// </summary>
        [DataMember]
        public int MZZLS_FHMZ { get; set; }
        /// <summary>
        /// 麻醉总例数   其他
        /// </summary>
        [DataMember]
        public int MZZLS_QT { get; set; }
        /// <summary>
        /// 麻醉总例数   合计
        /// </summary>
        [DataMember]
        public int MZZLS_HJ { get; set; }
        /// <summary>
        /// 术后镇痛例数  硬膜外镇痛
        /// </summary>
        [DataMember]
        public int SHZTLS_YMWZT { get; set; }
        /// <summary>
        /// 术后镇痛例数  静脉镇痛
        /// </summary>
        [DataMember]
        public int SHZTLS_JMZT { get; set; }
        /// <summary>
        /// 术后镇痛例数  神经阻滞
        /// </summary>
        [DataMember]
        public int SHZTLS_SJZZ { get; set; }
        /// <summary>
        /// 术后镇痛例数  其他
        /// </summary>
        [DataMember]
        public int SHZTLS_QT { get; set; }
        /// <summary>
        /// 术后镇痛例数  合计
        /// </summary>
        [DataMember]
        public int SHZTLS_HJ { get; set; }
        /// <summary>
        /// 手术室外麻醉	分娩镇痛
        /// </summary>
        [DataMember]
        public int SSSWMZ_FMZT { get; set; }
        /// <summary>
        /// 手术室外麻醉	无痛胃肠镜
        /// </summary>
        [DataMember]
        public int SSSWMZ_WTWCJ { get; set; }
        /// <summary>
        /// 手术室外麻醉	无痛人流
        /// </summary>
        [DataMember]
        public int SSSWMZ_WTRL { get; set; }
        /// <summary>
        /// 手术室外麻醉	其他
        /// </summary>
        [DataMember]
        public int SSSWMZ_QT { get; set; }
        /// <summary>
        /// 手术室外麻醉	合计
        /// </summary>
        [DataMember]
        public int SSSWMZ_HJ { get; set; }
        /// <summary>
        /// 血管穿刺例数	中心静脉 //wsx_gz 1201
        /// </summary>
        [DataMember]
        public int XGCCLS_ZXJM { get; set; }
        /// <summary>
        /// 血管穿刺例数	动脉
        /// </summary>
        [DataMember]
        public int XGCCLS_DM { get; set; }
        /// <summary>
        /// 手术类别例数	择期
        /// </summary>
        [DataMember]
        public int SSLBLS_ZQ { get; set; }
        /// <summary>
        /// 手术类别例数	急诊
        /// </summary>
        [DataMember]
        public int SSLBLS_JZ { get; set; }
        /// <summary>
        /// 手术类别例数	醉后手术取消
        /// </summary>
        [DataMember]
        public int SSLBLS_ZHSSQX { get; set; }
        /// <summary>
        /// 输血例数	<400ml
        /// </summary>
        [DataMember]
        public int SXLS_X400 { get; set; }
        /// <summary>
        /// 输血例数	>=400ml
        /// </summary>
        [DataMember]
        public int SXLS_D400 { get; set; }
        /// <summary>
        /// 输血例数	术中输血
        /// </summary>
        [DataMember]
        public int SXLS_SZSX { get; set; }
        /// <summary>
        /// 输血例数	自体输血
        /// </summary>
        [DataMember]
        public int SXLS_ZTSX { get; set; }
        /// <summary>
        /// 气管插管管理例数	拔管出手术室
        /// </summary>
        [DataMember]
        public int QGCGGLLS_BGCSSS { get; set; }
        /// <summary>
        /// 气管插管管理例数	带管出手术室
        /// </summary>
        [DataMember]
        public int QGCGGLLS_DGCSSS { get; set; }
        /// <summary>
        /// 麻醉复苏管理例数	进入复苏室
        /// </summary>
        [DataMember]
        public int MZFSGLLS_JRFSS { get; set; }
        /// <summary>
        /// 麻醉复苏管理例数	离室steward>=4分
        /// </summary>
        [DataMember]
        public int MZFSGLLS_LSSF { get; set; }
        /// <summary>
        /// 心肺复苏例数	h内心跳骤停
        /// </summary>
        [DataMember]
        public int XFFSLS_HNXTZT { get; set; }
        /// <summary>
        /// 心肺复苏例数	复苏成功
        /// </summary>
        [DataMember]
        public int XFFSLS_FSCG { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	意识障碍
        /// </summary>
        [DataMember]
        public int MZFYQSJ_YSZA { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	氧饱和度降低
        /// </summary>
        [DataMember]
        public int MZFYQSJ_YBHDJD { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	使用催醒药
        /// </summary>
        [DataMember]
        public int MZFYQSJ_SYCXY { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	低体温
        /// </summary>
        [DataMember]
        public int MZFYQSJ_DTW { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	入PACU>3h
        /// </summary>
        [DataMember]
        public int MZFYQSJ_RPACU3H { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	非计划入ICU
        /// </summary>
        [DataMember]
        public int MZFYQSJ_FJHRICU { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	气管插管非计划二次
        /// </summary>
        [DataMember]
        public int MZFYQSJ_QGCGFJHEC { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	过敏
        /// </summary>
        [DataMember]
        public int MZFYQSJ_GM { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	误吸
        /// </summary>
        [DataMember]
        public int MZFYQSJ_WX { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	昏迷
        /// </summary>
        [DataMember]
        public int MZFYQSJ_HM { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	24h内死亡
        /// </summary>
        [DataMember]
        public int MZFYQSJ_24NSW { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	神经损伤
        /// </summary>
        [DataMember]
        public int MZFYQSJ_SJSS { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	声音嘶哑
        /// </summary>
        [DataMember]
        public int MZFYQSJ_SYSY { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	中心静脉穿刺
        /// </summary>
        [DataMember]
        public int MZFYQSJ_ZXJMCC { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	动静脉穿刺
        /// </summary>
        [DataMember]
        public int MZFYQSJ_JDMCC { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	其他
        /// </summary>
        [DataMember]
        public int MZFYQSJ_QT { get; set; }
        /// <summary>
        /// 麻醉非预期相关事件例数	合计
        /// </summary>
        [DataMember]
        public int MZFYQSJ_HJ { get; set; }
        /// <summary>
        /// 麻醉效果管理例数	I
        /// </summary>
        [DataMember]
        public int MZXGGLLS_I { get; set; }
        /// <summary>
        /// 麻醉效果管理例数	II
        /// </summary>
        [DataMember]
        public int MZXGGLLS_II { get; set; }
        /// <summary>
        /// 麻醉效果管理例数	III
        /// </summary>
        [DataMember]
        public int MZXGGLLS_III { get; set; }
        /// <summary>
        /// 麻醉效果管理例数	IV
        /// </summary>
        [DataMember]
        public int MZXGGLLS_IV { get; set; }
        /// <summary>
        /// 麻醉效果管理例数	麻醉方式更改
        /// </summary>
        [DataMember]
        public int MZXGGLLS_MZFSGG { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	I
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_I { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	II
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_II { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	III
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_III { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	IV
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_IV { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	V
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_V { get; set; }
        /// <summary>
        /// 麻醉分级管理例数ASA	合计
        /// </summary>
        [DataMember]
        public int MZFJGLLSASA_HJ { get; set; }
        /// <summary>
        /// 在岗医师	主任医师
        /// </summary>
        [DataMember]
        public int ZGYS_ZRYS { get; set; }
        /// <summary>
        /// 在岗医师	副主任医师
        /// </summary>
        [DataMember]
        public int ZGYS_FZRYS { get; set; }
        /// <summary>
        /// 在岗医师	主治医师
        /// </summary>
        [DataMember]
        public int ZGYS_ZZYS { get; set; }
        /// <summary>
        /// 在岗医师	住院医师
        /// </summary>
        [DataMember]
        public int ZGYS_ZYYS { get; set; }
        /// <summary>
        /// 在岗医师	合计
        /// </summary>
        [DataMember]
        public int ZGYS_HJ { get; set; }

        /// <summary>
        /// 进入ICU 合计
        /// </summary>
        [DataMember]
        public int JRICU_HJ { get; set; }
    }

    [DataContract, Serializable]
    public class EntityAnaStat2 : BaseDataContract
    {
        /// <summary>
        /// 月份
        /// </summary>
        [DataMember]
        public string YF { get; set; }
        /// <summary>
        /// 麻醉科医患比(麻醉科固定在岗(本院)医师总数比麻醉总数)
        /// </summary>
        [DataMember]
        public string MZKYHB { get; set; }
        /// <summary>
        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	I
        /// </summary>
        [DataMember]
        public string ASAFJMZHZBL_I { get; set; }
        /// <summary>
        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	II
        /// </summary>
        [DataMember]
        public string ASAFJMZHZBL_II { get; set; }
        /// <summary>
        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	III
        /// </summary>
        [DataMember]
        public string ASAFJMZHZBL_III { get; set; }
        /// <summary>
        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	IV
        /// </summary>
        [DataMember]
        public string ASAFJMZHZBL_IV { get; set; }
        /// <summary>
        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	V
        /// </summary>
        [DataMember]
        public string ASAFJMZHZBL_V { get; set; }
        /// <summary>
        /// 急诊非择期麻醉比例(急诊非择期手术所实施的麻醉数比麻醉总数)
        /// </summary>
        [DataMember]
        public string JZFZQMZBL { get; set; }
        /// <summary>
        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	椎管内麻醉
        /// </summary>
        [DataMember]
        public string MZFSBL_ZGNMZ { get; set; }
        /// <summary>
        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	插管全麻
        /// </summary>
        [DataMember]
        public string MZFSBL_CGMZ { get; set; }
        /// <summary>
        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	非插管全麻
        /// </summary>
        [DataMember]
        public string MZFSBL_FCGMZ { get; set; }
        /// <summary>
        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	复合麻醉
        /// </summary>
        [DataMember]
        public string MZFSBL_FHMZ { get; set; }
        /// <summary>
        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	其他
        /// </summary>
        [DataMember]
        public string MZFSBL_QT { get; set; }
        /// <summary>
        /// 麻醉开始后手术取消率(手术取消的数比麻醉总数)
        /// </summary>
        [DataMember]
        public string MZKSHSSQXL { get; set; }
        /// <summary>
        /// 麻醉后监测治疗室(PACU)转出延迟率(入PACU超过3ZGNMZHYZSJBFZFSLPACU患者总数)
        /// </summary>
        [DataMember]
        public string MZHJKZLSZCYCL { get; set; }
        /// <summary>
        /// PACU入室低体温率(PACU入室低体温患者数比入PACU患者总数)
        /// </summary>
        [DataMember]
        public string PACURSDTWL { get; set; }
        /// <summary>
        /// 非计划转入ICU率(非计划转入ICU患者数比转入ICU患者总数)
        /// </summary>
        [DataMember]
        public string FJHZRICUL { get; set; }
        /// <summary>
        /// 非计划二次气管插管率(非计划计划二次气管插管患者数比术后气管插管拔除患者总数)
        /// </summary>
        [DataMember]
        public string FJHECQGCGL { get; set; }
        /// <summary>
        /// 麻醉开始后24小时内死亡率(麻醉开始后24小时内死亡患者数比麻醉患者总数)
        /// </summary>
        [DataMember]
        public string MZKSH24SWL { get; set; }
        /// <summary>
        /// 麻醉开始后24小时内心跳骤停率(麻醉开始后24小时内心跳骤停患者数比麻醉患者总数)
        /// </summary>
        [DataMember]
        public string MZKSH24XTZTL { get; set; }
        /// <summary>
        /// 术中自体血输注率(接受400ml及以上自体血(包括自体全血及自体血红细胞)输注患者数比接受400ml及以上输血治疗的患者总数
        /// </summary>
        [DataMember]
        public string SZZTSXZL { get; set; }
        /// <summary>
        /// 麻醉期间严重过敏反应发生率(麻醉期间严重过敏反应发生例数比麻醉总例数)
        /// </summary>
        [DataMember]
        public string MZQJYZGMFYFSL { get; set; }
        /// <summary>
        /// 椎管内麻醉后严重神经并发症发生率(椎管内麻醉后严重神经并发症发生例数比椎管内麻醉总例数)
        /// </summary>
        [DataMember]
        public string ZGNMZHYZSJBFZFSL { get; set; }
        /// <summary>
        /// 中心静脉穿刺严重并发症发生率(中心静脉穿刺严重并发症发生例数比中心静脉穿刺总例数)
        /// </summary>
        [DataMember]
        public string ZXJMCCYZBFZFSL { get; set; }
        /// <summary>
        /// 全麻气管插管拔管后声音嘶哑发生率(全麻气管插管拔管后声音嘶哑发生例数比全麻气管插管总例数)
        /// </summary>
        [DataMember]
        public string QMQGCGBGHSYSYFSL { get; set; }
        /// <summary>
        /// 麻醉后新发昏迷发生率(麻醉后新发昏迷发生例数比麻醉总例数)	
        /// </summary>
        [DataMember]
        public string MZHXFHMFSL { get; set; }

    }

    #region AnaStatTemp
    /// <summary>
    /// 统计编辑.临时
    /// </summary>
    [DataContract, Serializable]
    public class EntityAnaStatTemp : BaseDataContract
    {
        [DataMember]
        public string Fmonth { get; set; }

        [DataMember]
        public int Field1 { get; set; }

        [DataMember]
        public int Field2 { get; set; }

        [DataMember]
        public int Field3 { get; set; }

        [DataMember]
        public int Field4 { get; set; }
    }

    #endregion
}
