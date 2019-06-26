using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityEventDisplay : BaseDataContract
    {
        [DataMember]
        public int check { get; set; }

        [DataMember]
        public string rptId { get; set; }

        [DataMember]
        public string reportTime { get; set; }

        [DataMember]
        public string reportOper
        {
            get { return reportOperCode + " " + reportOperName; }
            set { ;}
        }

        [DataMember]
        public string reportOperCode { get; set; }

        [DataMember]
        public string reportOperName { get; set; }

        [DataMember]
        public string reportDeptName { get; set; }

        [DataMember]
        public string reportType { get; set; }

        [DataMember]
        public string eventCode { get; set; }

        [DataMember]
        public string eventName { get; set; }

        [DataMember]
        public string patNo { get; set; }

        [DataMember]
        public string patName { get; set; }

        [DataMember]
        public string patSex { get; set; }

        [DataMember]
        public string patAge { get; set; }

        [DataMember]
        public string patBirthDay { get; set; }

        [DataMember]
        public string contactTel { get; set; }

        [DataMember]
        public string deptName { get; set; }

        [DataMember]
        public string eventId { get; set; }

        [DataMember]
        public bool isNew { get; set; }

        [DataMember]
        public string owerDeptCode { get; set; }

        [DataMember]
        public string pubRoleId { get; set; }

        //护理不良事件 安全小组签名
        [DataMember]
        public string XZQM { get; set; }
        //护理不良事件 护长签名
        [DataMember]
        public string HCQM { get; set; }
        //护理不良事件 护理签名
        [DataMember]
        public string HLQM { get; set; }

        [DataMember]
        public string eventLevel { get; set; }

        [DataMember]
        public string eventTime { get; set; }


        /// <summary>
        /// Columns
        /// </summary>
        public static EnumCols Columns = new EnumCols();

        /// <summary>
        /// EnumCols
        /// </summary>
        public class EnumCols
        {
            public string check = "check";
            public string rptId = "rptId";
            public string reportTime = "reportTime";
            public string reportType = "reportType";
            public string reportOperCode = "reportOperCode";
            public string reportOperName = "reportOperName";
            public string reportDeptName = "reportDeptName";
            public string eventCode = "eventCode";
            public string eventName = "eventName";
            public string patNo = "patNo";
            public string patName = "patName";
            public string patSex = "patSex";
            public string patAge = "patAge";
            public string patBirthDay = "patBirthDay";
            public string contactTel = "contactTel";
            public string deptName = "deptName";
            public string eventId = "eventId";
        }

    }
}
