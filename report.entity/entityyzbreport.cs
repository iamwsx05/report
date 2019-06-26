using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    public class EntityRptYzb : BaseDataContract
    {
        //序号
        [DataMember]
        public int XH { get; set; }

        //姓名
        [DataMember]
        public string XM { get; set; }

        //工号
        [DataMember]
        public string GH { get; set; }

        //科室
        [DataMember]
        public string KS {get;set;}

        //科室代码
        [DataMember]
        public string deptCode { get; set; }

        //总收入
        [DataMember]
        public decimal ZSR { get; set; }

        //药品收入
        [DataMember]
        public decimal YBSR { get; set; }

        //药比
        [DataMember]
        public string YB {get;set;}

        //材料收入
        [DataMember]
        public decimal CLSR {get;set;}

        //材料占比
        [DataMember]
        public string CLZB {get;set;}

        //抗菌药物收入
        [DataMember]
        public decimal KJYWSR { get; set; }

        //抗菌药物使用比
        [DataMember]
        public string KJYWSYB { get; set; }

        //基药收入
        [DataMember]
        public decimal JBYWSR { get; set; }

        //基药收入比
        [DataMember]
        public string JBYWSRB { get; set; }
    }
}
