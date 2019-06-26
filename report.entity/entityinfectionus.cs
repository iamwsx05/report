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
    [Entity(TableName = "rptInfectionus")]
    public class EntityInfectionus : BaseDataContract
    {
        public EntityInfectionus(){}
        public static EntityInfectionus.EnumCols Columns;

        //报告ID
        [DataMember]
        [Entity(FieldName = "rptid", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
        public decimal rptId { get; set; }

        //报告时间
        [DataMember]
        [Entity(FieldName = "reportTime", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
        public string reportTime { get; set; }

        //科室名称
        [DataMember]
        [Entity(FieldName = "deptCode", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
        public string deptCode { get; set; }

        //患者住院号
        [DataMember]
        [Entity(FieldName = "inpatNo", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
        public string inpatNo { get; set; }

        //患者姓名
        [DataMember]
        [Entity(FieldName = "patName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
        public string patName { get; set; }

        //患者性别
        [DataMember]
        [Entity(FieldName = "patSex", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
        public string patSex { get; set; }

        //患者年龄
        [DataMember]
        [Entity(FieldName = "patAge", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 7)]
        public string patAge { get; set; }

        //患者出生日期
        [DataMember]
        [Entity(FieldName = "birthDay", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 8)]
        public DateTime? birthDay { get; set; }

        //床号
        [DataMember]
        [Entity(FieldName = "bedNo", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
        public string bedNo { get; set; }

        //入院日期
        [DataMember]
        [Entity(FieldName = "dateIn", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 10)]
        public DateTime? dateIn { get; set; }

        //入院诊断
        [DataMember]
        [Entity(FieldName = "inDiagnosis", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 11)]
        public string inDiagnosis { get; set; }

        //切口类型
        [DataMember]
        [Entity(FieldName = "incisionType", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 12)]
        public int incisionType { get; set; }

        //感染部位 1
        [DataMember]
        [Entity(FieldName = "infectionSite01", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
        public string infectionSite01 { get; set; }

        //感染部位 2
        [DataMember]
        [Entity(FieldName = "infectionSite02", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 14)]
        public string infectionSite02 { get; set; }

        //感染日期 1
        [DataMember]
        [Entity(FieldName = "infectionDate01", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 15)]
        public string infectionDate01 { get; set; }

        //感染日期 2
        [DataMember]
        [Entity(FieldName = "infectionDate02", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 14)]
        public string infectionDate02 { get; set; }

        //易感因素
        [DataMember]
        [Entity(FieldName = "infectionReason", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 16)]
        public string infectionReason { get; set; }

        //主管医师
        [DataMember]
        [Entity(FieldName = "doctId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 17)]
        public string doctId { get; set; }

        //记录时间
        [DataMember]
        [Entity(FieldName = "recordDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 18)]
        public DateTime? recordDate { get; set; }

        //状态
        [DataMember]
        [Entity(FieldName = "status", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 19)]
        public int status { get; set; }

        //上报者姓名
        [DataMember]
        [Entity(FieldName = "reporterName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 21)]
        public string reporterName { get; set; }

        //上报者工号
        [DataMember]
        [Entity(FieldName = "reporterId", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 20)]
        public string reporterId { get; set; }

        //手术名称
        [DataMember]
        [Entity(FieldName = "operationName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 22)]
        public string operationName { get; set; }

        //手术日期
        [DataMember]
        [Entity(FieldName = "operationDate", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 23)]
        public string operationDate { get; set; }

        //科室
        [DataMember]
        public string deptName { get; set; }
        //主管医生
        [DataMember]
        public string doctName { get; set; }
        //是否通过审核
        [DataMember]
        public string isPass { get; set;}
        //标本名称
        [DataMember]
        public string sampleName { get; set; }
        //送检日期
        [DataMember]
        public string checkDate { get; set; }
        //病原体名称
        [DataMember]
        public string pathogenyName { get; set; }
        //多种耐药菌名称
        [DataMember]
        public string drugName { get; set; }
        //切口类型0
        [DataMember]
        public int incisionType01 { get; set; }
        ////切口类型1
        [DataMember]
        public int incisionType02 { get; set; }
        ////切口类型2
        [DataMember]
        public int incisionType03 { get; set; }
        ////切口类型2
        [DataMember]
        public int incisionType04 { get; set; }


        /// <summary>
        /// EnumCols
        /// </summary>
        public class EnumCols
        {
            public string rptId = "rptId";
            public string reportTime = "reportTime";
            public string deptCode = "deptCode";
            public string inpatNo = "inpatNo";
            public string patName = "patName";
            public string patSex = "patSex";
            public string patAge = "patAge";
            public string birthDay = "birthDay";
            public string bedNo = "bedNo";
            public string dateIn = "dateIn";
            public string inDiagnosis = "inDiagnosis";
            public string incisionType = "incisionType";
            public string infectionSite01 = "infectionSite01";
            public string infectionSite02 = "infectionSite02";
            public string infectionDate01 = "infectionDate01";
            public string infectionDate02 = "infectionDate02";
            public string infectionReason = "infectionReason";
            public string doctId = "doctId";
            public string recordDate = "recordDate";
            public string status = "status";
            public string reportName = "reportName";
            public EnumCols() { }
        }
    }

    [DataContract, Serializable]
    [Entity(TableName = "rptPathogeny")]
    public class EntityPathogeny : BaseDataContract
    {
        public EntityPathogeny() { }
        public static EntityPathogeny.EnumCols Columns;

        [DataMember]
        [Entity(FieldName = "serNo", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
        public decimal serNo { get; set; }
        //报告ID
        [DataMember]
        [Entity(FieldName = "rptId", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 2)]
        public decimal rptId { get; set; }
        //标本名称
        [DataMember]
        [Entity(FieldName = "sampleName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
        public string sampleName { get; set; }

        //送检日期
        [DataMember]
        [Entity(FieldName = "checkDate", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
        public string checkDate { get; set; }

        //病原体名称
        [DataMember]
        [Entity(FieldName = "pathogenyName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
        public string pathogenyName { get; set; }

        //多种耐药菌名称
        [DataMember]
        [Entity(FieldName = "drugName", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
        public string drugName { get; set; }

       // public static EnumCols Columns = new EnumCols();

        /// <summary>
        /// EnumCols
        /// </summary>
        public class EnumCols
        {
            public string serNo = "serNo";
            public string rptId = "rptId";
            public string sampleName = "sampleName";
            public string checkDate = "checkDate";
            public string pathogenyName = "pathogenyName";
            public string drugName = "drugName";

            public EnumCols() { }
        }
    }


    [DataContract, Serializable]
    public class EntityDeptList : BaseDataContract
    {
        public EntityDeptList() { }

        [DataMember]
        public string deptCode { get; set; }

        [DataMember]
        public string deptName { get; set; }

    }
}
