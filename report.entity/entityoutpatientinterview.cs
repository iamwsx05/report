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
    [Entity(TableName = "rptinterview")]
    public class EntityOutpatientInterview : BaseDataContract
    {
        public static EnumCols Columns = new EnumCols();

        [DataMember]
        [Entity(FieldName = "rptId", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
        public decimal rptId { get; set; }
        [DataMember]
        [Entity(FieldName = "INTERVIEWTIME", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 2)]
        public string interviewTime { get; set; }
        [DataMember]
        [Entity(FieldName = "INTERVIEWCODE", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 3)]
        public string interviewCode { get; set; }
        [DataMember]
        [Entity(FieldName = "PATNO", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 4)]
        public string patNo { get; set; }
        [DataMember]
        [Entity(FieldName = "PATNAME", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 5)]
        public string patName { get; set; }
        [DataMember]
        [Entity(FieldName = "PATSEX", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 6)]
        public string patSex { get; set; }
        [DataMember]
        [Entity(FieldName = "IDCARD", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 7)]
        public string idCard { get; set; }
        [DataMember]
        [Entity(FieldName = "BIRTHDAY", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 8)]
        public string birthday { get; set; }
        [DataMember]
        [Entity(FieldName = "CONTACTADDR", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 9)]
        public string contactAddr { get; set; }
        [DataMember]
        [Entity(FieldName = "CONTACTTEL", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 10)]
        public string contactTel { get; set; }
        [DataMember]
        [Entity(FieldName = "OUTDEPTCODE", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 11)]
        public string outDeptCode { get; set; }
        [DataMember]
        [Entity(FieldName = "OUTHOSPITALTIME", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 12)]
        public string outHospitalTime { get; set; }
        [DataMember]
        [Entity(FieldName = "RECORDDATE", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 13)]
        public string recordDate { get; set; }
        [DataMember]
        [Entity(FieldName = "STATUS", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 14)]
        public decimal status { get; set; }
        [DataMember]
        [Entity(FieldName = "XMLDATA", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 15)]
        public string xmlData { get; set; }
        [DataMember]
        [Entity(FieldName = "REGISTERID", DbType = DbType.AnsiString, IsPK = false, IsSeq = false, SerNo = 16)]
        public string registerid { get; set; }
        [DataMember]
        public bool isNew { get; set; }
        [DataMember]
        public string outDeptName { get; set; }
        [DataMember]
        public string inDeptName { get; set; }
        [DataMember]
        public string inDeptTime { get; set; }
        [DataMember]
        public string inCount { get; set; }
        [DataMember]
        public string interviewName{get; set; }
        [DataMember]
        public string doctName { get; set; }
        [DataMember]
        public string patAge { get; set; }


        public class EnumCols
        {
            public string rptId = "rptId";
            public string interviewTime = "interviewTime";
            public string interviewCode = "interviewCode";
            public string patNo = "patNo";
            public string patName = "patName";
            public string patSex = "patSex";
            public string idCard = "idCard";
            public string birthday = "birthday";
            public string contactAddr = "contactAddr";
            public string contactTel = "contactTel";
            public string outDeptCode = "outDeptCode";
            public string outHospitalTime = "outHospitalTime";
            public string recordDate = "recordDate";
            public string reportDate = "reportDate";
            public string status = "status";
            public string xmlData = "xmlData";
            public string registerid = "registerid";
        }

    }

    [DataContract, Serializable]
    [Entity(TableName = "rptInterViewParm")]
    public class EntityRptInterviewParm : BaseDataContract
    {
        [DataMember]
        [Entity(FieldName = "keyId", DbType = DbType.AnsiString, IsPK = true, IsSeq = false, SerNo = 1)]
        public string keyId { get; set; }
        [DataMember]
        [Entity(FieldName = "keyValue", DbType = DbType.AnsiString, IsPK = true, IsSeq = false, SerNo = 2)]
        public string keyValue { get; set; }
        [DataMember]
        [Entity(FieldName = "flag", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 3)]
        public decimal flag { get; set; }
    }
}
