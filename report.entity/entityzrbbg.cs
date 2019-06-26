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
    [Entity(TableName = "rptZrbbg")]
    public class EntityRptZrbbg : BaseDataContract
    {
        public static EntityRptZrbbg.EnumCols Columns = new EnumCols();

        public EntityRptZrbbg() {}
        [DataMember]
        [Entity(FieldName = "rptId", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
        public decimal rptId { get; set; }
        [DataMember]
        [Entity(FieldName = "reportId", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 2)]
        public string reportId { get; set; }
        [DataMember]
        [Entity(FieldName = "registerCode", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 3)]
        public string registerCode { get; set; }
        [DataMember]
        [Entity(FieldName = "patName", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 4)]
        public string patName { get; set; }
        [DataMember]
        [Entity(FieldName = "parentName", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 5)]
        public string parentName { get; set; }
        [DataMember]
        [Entity(FieldName = "patSex", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 6)]
        public string patSex { get; set; }
        [DataMember]
        [Entity(FieldName = "birthday", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 7)]
        public DateTime? birthday { get; set; }
        [DataMember]
        [Entity(FieldName = "familyAddr", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 8)]
        public string familyAddr { get; set; }
        [DataMember]
        [Entity(FieldName = "contactAddr", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 9)]
        public string contactAddr { get; set; }
        [DataMember]
        [Entity(FieldName = "contactTel", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 10)]
        public string contactTel { get; set; }
        [DataMember]
        [Entity(FieldName = "diagnoseName", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 11)]
        public string diagnoseName { get; set; }
        [DataMember]
        [Entity(FieldName = "diagnoseDate", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 12)]
        public string diagnoseDate { get; set; }
        [DataMember]
        [Entity(FieldName = "infectiveDate", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 13)]
        public string infectiveDate { get; set; }
        [DataMember]
        [Entity(FieldName = "reportDate", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 14)]
        public string reportDate { get; set; }
        [DataMember]
        [Entity(FieldName = "skDate", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 15)]
        public string skDate { get; set; }
        [DataMember]
        [Entity(FieldName = "bcDate", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 16)]
        public string bcDate { get; set; }
        [DataMember]
        [Entity(FieldName = "reportOperCode", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 17)]
        public string reportOperCode { get; set; }
        [DataMember]
        [Entity(FieldName = "reportOperName", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 18)]
        public string reportOperName { get; set; }
        [DataMember]
        [Entity(FieldName = "reportDeptCode", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 19)]
        public string reportDeptCode { get; set; }
        [DataMember]
        [Entity(FieldName = "patNo", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 20)]
        public string patNo { get; set; }
        [DataMember]
        [Entity(FieldName = "patDeptCode", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 21)]
        public string patDeptCode { get; set; }
        [DataMember]
        [Entity(FieldName = "idCard", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 22)]
        public string idCard { get; set; }
        [DataMember]
        [Entity(FieldName = "formId", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 23)]
        public decimal formId { get; set; }
        [DataMember]
        [Entity(FieldName = "operCode", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 24)]
        public string operCode { get; set; }
        [DataMember]
        [Entity(FieldName = "recordDate", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 25)]
        public DateTime recordDate { get; set; }
        [DataMember]
        [Entity(FieldName = "status", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 26)]
        public decimal status { get; set; }
        [DataMember]
        [Entity(FieldName = "RQFL", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 27)]
        public string RQFL { get; set; }
        [DataMember]
        [Entity(FieldName = "BZ", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 28)]
        public string BZ { get; set; }
        [DataMember]
        [Entity(FieldName = "patType", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 29)]
        public int patType { get; set; }
        [Entity(FieldName = "printFlg", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 30)]
        public int printFlg { get; set; }
        [DataMember]
        public string xmlData { get; set; }

        public class EnumCols
        {
            public string rptId = "rptId";
            public string reportId = "reportId";
            public string registerCode = "registerCode";
            public string patName = "patName";
            public string parentName = "parentName";
            public string patSex  = "patSex";
            public string birthday = "birthday";
            public string familyAddr = "familyAddr";
            public string contactAddr = "contactAddr";
            public string contactTel  = "contactTel";
            public string diagnoseName = "diagnoseName";
            public string diagnoseDate = "diagnoseDate";
            public string infectiveDate = "infectiveDate";
            public string reportDate = "reportDate";
            public string skDate = "skDate";
            public string bcDate = "bcDate";
            public string RQFL = "RQFL";
            public string reportOperCode = "reportOperCode";
            public string reportOperName = "reportOperName";
            public string reportDeptCode = "reportDeptCode";
            public string patNo = "patNo";
            public string patDeptCode = "patDeptCode";
            public string idCard = "idCard";
            public string formId = "formId";
            public string operCode = "operCode";
            public string recordDate = "recordDate";
            public string status = "status";
            public string patType = "patType";
            public string printFlg = "printFlg";
            public string BZ = "BZ";
            public string xmlData = "xmlData";
        }
    }


    [DataContract, Serializable]
    [Entity(TableName = "rptZrbbgData")]
    public class EntityRptZrbbgData : BaseDataContract
    {
        public static EntityRptZrbbgData.EnumCols Columns = new EnumCols();

        [DataMember]
        [Entity(FieldName = "rptId", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
        public decimal rptId { get; set; }
        [DataMember]
        [Entity(FieldName = "xmlData", DbType = DbType.Xml, IsPK = false, IsSeq = false, SerNo = 2)]
        public string xmlData { get; set; }

        public class EnumCols
        {
            public string rptId = "rptId";
            public string xmlData = "xmlData";
        }
    }


    
    [Entity(TableName = "rptZrbbgParm"),DataContract]
    [Serializable]
    public class EntityRptZrbbgParm : BaseDataContract
    {
        public static EntityRptZrbbgParm.EnumCols Columns = new EnumCols();
        [DataMember]
        [Entity(FieldName = "flag", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 4)]
        public decimal flag { get; set; }
        [DataMember]
        [Entity(FieldName = "keyId", DbType = DbType.AnsiString, IsPK = true, IsSeq = false, SerNo = 2)]
        public string keyId { get; set; }
        [DataMember]
        [Entity(FieldName = "keyValue", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 3)]
        public string keyValue { get; set; }
        [DataMember]
        [Entity(FieldName = "reportId", DbType = DbType.AnsiString, IsPK = true, IsSeq = false, SerNo = 1)]
        public string reportId { get; set; }

        public class EnumCols
        {
            public string flag = "flag";
            public string keyId = "keyId";
            public string keyValue = "keyValue";
            public string reportId = "reportId";
        }
    }


    [DataContract, Serializable]
    public class EntityZrbbgDisplay : BaseDataContract
    {
        [DataMember]
        public decimal rptId   { get; set; }
        [DataMember]
        public string reportId { get; set; }
        [DataMember]
        public string registerCode { get; set; }
        [DataMember]
        public string patName { get; set; }
        [DataMember]
        public string parentName { get; set; }
        [DataMember]
        public string patSex { get; set; }
        [DataMember]
        public string birthday { get; set; }
        [DataMember]
        public string familyAddr { get; set; }
        [DataMember]
        public string contactAddr { get; set; }
        [DataMember]
        public string contactTel { get; set; }
        [DataMember]
        public string diagnoseName { get; set; }
        [DataMember]
        public string diagnoseDate { get; set; }
        [DataMember]
        public string infectiveDate { get; set; }
        [DataMember]
        public string reportDate { get; set; }
        [DataMember]
        public string skDate { get; set; }
        [DataMember]
        public string bcDate { get; set; }
        [DataMember]
        public string reportOperCode { get; set; }
        [DataMember]
        public string reportOperName { get; set; }
        [DataMember]
        public string reportDeptCode { get; set; }
        [DataMember]
        public string patNo { get; set; }
        [DataMember]
        public string idCard { get; set; }
        [DataMember]
        public string RQFL { get; set; }
        [DataMember]
        public string patDeptCode { get; set; }
        [DataMember]
        public string formId { get; set; }
        [DataMember]
        public string operCode { get; set; }
        [DataMember]
        public string recordDate { get; set; }
        [DataMember]
        public bool isNew { get; set; }
        [DataMember]
        public string owerDeptCode { get; set; }
        [DataMember]
        public string pubRoleId { get; set; }
        [DataMember]
        public string patAge { get; set; }
        [DataMember]
        public string LRR { get; set; }
        [DataMember]
        public string BZ { get; set; }
        [DataMember]
        public decimal printFlg { get; set; }
    }
}
