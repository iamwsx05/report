using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityYgAccessRecord : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 患者代码
        /// </summary>
        [DataMember]
        public string HZDM { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public string  XM { get; set; }
        /// <summary>
        /// 入科科室
        /// </summary>
        [DataMember]
        public string RKKS { get; set; }
        /// <summary>
        /// 入科时间
        /// </summary>
        [DataMember]
        public string RKSJ { get; set; }
        /// <summary>
        /// 出科时间
        /// </summary>
        [DataMember]
        public string CKSJ { get; set; }
    }

    [DataContract, Serializable]
    public class EntityYgInpatStat : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 患者代码
        /// </summary>
        [DataMember]
        public string HZDM { get; set; }

        /// <summary>
        /// 住院号
        /// </summary>
        [DataMember]
        public string ZYH { get; set; }
        /// <summary>
        /// 住院次数
        /// </summary>
        [DataMember]
        public string ZYCS { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// 性别
        /// </summary>
        [DataMember]
        public string XB { get; set; }
        /// 出生年月
        /// </summary>
        [DataMember]
        public string CSNY { get; set; }
        /// <summary>
        /// 入院科室
        /// </summary>
        [DataMember]
        public string RYKS { get; set; }
        /// <summary>
        /// 入院时间
        /// </summary>
        [DataMember]
        public string RYSJ { get; set; }
        /// <summary>
        /// 入院诊断
        /// </summary>
        [DataMember]
        public string RYZD { get; set; }
        /// <summary>
        /// 出院科室
        /// </summary>
        [DataMember]
        public string CYKS { get; set; }
        /// <summary>
        /// 出院时间 
        /// </summary>
        [DataMember]
        public string CYSJ { get; set; }
        /// <summary>
        /// 出院方式  
        /// </summary>
        [DataMember]
        public string CYFS { get; set; }
    }

    [DataContract, Serializable]
    public class EntityYgDrugStat : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 患者代码
        /// </summary>
        [DataMember]
        public string HZDM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }

        /// <summary>
        /// 送检科室
        /// </summary>
        [DataMember]
        public string SJKS { get; set; }
        /// <summary>
        /// 报告编号
        /// </summary>
        [DataMember]
        public string BGBH { get; set; }
        
        /// 标本名称
        /// </summary>
        [DataMember]
        public string BBMC { get; set; }
        /// 细菌名称
        /// </summary>
        [DataMember]
        public string XJMC { get; set; }
        /// <summary>
        /// 采样时间
        /// </summary>
        [DataMember]
        public string CYSJ { get; set; }
        /// <summary>
        /// 报告时间
        /// </summary>
        [DataMember]
        public string BGSJ { get; set; }
        /// <summary>
        /// 是否多种耐药菌
        /// </summary>
        [DataMember]
        public string SFDZNYJ { get; set; }
        /// <summary>
        /// 多种耐药菌种类
        /// </summary>
        [DataMember]
        public string DZNYJZL { get; set; }
    }

    [DataContract, Serializable]
    public class EntityYgOperationStat : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 患者代码
        /// </summary>
        [DataMember]
        public string HZDM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        [DataMember]
        public string IPNO { get; set; }

        /// <summary>
        /// 手术科室
        /// </summary>
        [DataMember]
        public string SSKS { get; set; }
        /// <summary>
        /// 手术名称
        /// </summary>
        [DataMember]
        public string SSMC { get; set; }

        /// 开始时间
        /// </summary>
        [DataMember]
        public string KSSJ { get; set; }
        /// 结束时间
        /// </summary>
        [DataMember]
        public string JSSJ { get; set; }
        /// <summary>
        /// 切口等级
        /// </summary>
        [DataMember]
        public string QKTJ { get; set; }
        /// <summary>
        /// 报告时间
        /// </summary>
        [DataMember]
        public string BGSJ { get; set; }
        /// <summary>
        /// ASA评分
        /// </summary>
        [DataMember]
        public string ASAPF { get; set; }
        /// <summary>
        /// 麻醉方式
        /// </summary>
        [DataMember]
        public string MZFS { get; set; }
        /// <summary>
        /// 手术医生
        /// </summary>
        [DataMember]
        public string SSYS { get; set; }
        /// <summary>
        /// 术前预防用药
        /// </summary>
        [DataMember]
        public string SQYFYY { get; set; }
        /// <summary>
        /// 术前0.5-2H用药
        /// </summary>
        [DataMember]
        public string SQ2HYY { get; set; }
    }

    [DataContract, Serializable]
    public class EntityYgMachineStat : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        /// <summary>
        /// 患者代码
        /// </summary>
        [DataMember]
        public string HZDM { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string XM { get; set; }

        /// <summary>
        /// 使用科室
        /// </summary>
        [DataMember]
        public string SYKS { get; set; }
        /// <summary>
        /// 器械种类
        /// </summary>
        [DataMember]
        public string JXZL { get; set; }

        /// 开始时间
        /// </summary>
        [DataMember]
        public string KSSJ { get; set; }
        /// 结束时间
        /// </summary>
        [DataMember]
        public string JSSJ { get; set; }
        /// <summary>
        /// 医嘱名称
        /// </summary>
        [DataMember]
        public string YCMC { get; set; }
    }

    [DataContract, Serializable]
    [Entity(TableName = "t_Alerted")]
    public class EntityAlert : BaseDataContract
    {
        [DataMember]
        [Entity(FieldName = "REGISTERID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 0)]
        public string registerId { get; set; }

        [DataMember]
        [Entity(FieldName = "INPATIENTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 1)]
        public string inPatientId { get; set; }

        [Entity(FieldName = "PATNAME", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
        public string patName { get; set; }

        [DataMember]
        [Entity(FieldName = "SEX", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
        public string sex { get; set; }

        [DataMember]
        [Entity(FieldName = "ORDERNAME", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
        public string orderName { get; set; }

        [DataMember]
        [Entity(FieldName = "ITEMNAME", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
        public string itemName { get; set; }

        [DataMember]
        [Entity(FieldName = "ITEMRESULT", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
        public string itemResult { get; set; }

        [DataMember]
        [Entity(FieldName = "APLICATIONID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 7)]
        public string aplicationId { get; set; }

        [DataMember]
        [Entity(FieldName = "APPLYDATE", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 8)]
        public DateTime applyDate { get; set; }

        [DataMember]
        [Entity(FieldName = "RECORDDATE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
        public string recordDate { get; set; }

        [DataMember]
        [Entity(FieldName = "TYPEINT", DbType = DbType.Int16, IsPK = false, IsSeq = false, SerNo = 10)]
        public int typeInt { get; set; }

        [DataMember]
        [Entity(FieldName = "DEPTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 11)]
        public string deptId { get; set; }

        [DataMember]
        [Entity(FieldName = "ORDERID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 12)]
        public string OrderId { get; set; }
        [DataMember]
        [Entity(FieldName = "CHECKITEMID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
        public string checkItemId { get; set; }
    }


    [DataContract, Serializable]
    public class EntityAlertDisplay : BaseDataContract
    {
        [DataMember]
        public string registerId { get; set; }
        [DataMember]
        public string inPatientId { get; set; }
        [DataMember]
        public string inCount { get; set; }
        [DataMember]
        public string patName { get; set; }
        [DataMember]
        public string sex { get; set; }
        [DataMember]
        public string inPatientDate { get; set; }
        [DataMember]
        public string deptName { get; set; }
        [DataMember]
        public string lisAlertInfo { get; set; }
        [DataMember]
        public string vsAlertInfo { get; set; }
        [DataMember]
        public string pacsAlertInfo { get; set; }
        [DataMember]
        public string orderAlertInfo { get; set; }
        [DataMember]
        public string recordDate { get; set; }
        [DataMember]
        public int XH { get; set; }

       
    }

    [DataContract, Serializable]
    public class EntityAlertStat : BaseDataContract
    {
        [DataMember]
        public string deptName { get; set; }
        [DataMember]
        public int XH { get; set; }
        [DataMember]
        public string vsInfo { get; set; }
        [DataMember]
        public string lisInfo { get; set; }
        [DataMember]
        public string pacsInfo { get; set; }
        [DataMember]
        public string orderInfo { get; set; }
        [DataMember]
        public List<EntityTypeStat> lstType { get; set; }
    }

    [DataContract, Serializable]
    public class EntityTypeStat : BaseDataContract
    {
        [DataMember]
        public int typeInt { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
