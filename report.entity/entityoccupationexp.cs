using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityOccupationexp : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        //编号 regCode
        [DataMember]
        public string regCode { get; set; }
        //姓名 Name
        [DataMember]
        public string Name { get; set; }
        //性别 Sex
        [DataMember]
        public string Sex { get; set; }
        //年龄 Age
        [DataMember]
        public string Age { get; set; }
        //科室 deptName
        [DataMember]
        public string deptName { get; set; }
        //工龄 GL
        [DataMember]
        public string GL { get; set; }
        //工作类别 GZLB
        [DataMember]
        public string GZLB { get; set; }
        //暴露方式 BLFF
        [DataMember]
        public string BLFF { get; set; }
        //发生时间 FSSJ
        [DataMember]
        public string FSSJ { get; set; }
        //发生地点 FSDT
        [DataMember]
        public string FSDT { get; set; }
        //发生时执行何种操作 FSSCZ
        [DataMember]
        public string FSSCZ { get; set; }
        //暴露由谁造成 BLZC
        [DataMember]
        public string BLZC { get; set; }
        //暴露时戴手套 BLDSD
        [DataMember]
        public string BLDSD { get; set; }
        //血源患者 XYHZ
        [DataMember]
        public string XYHZ { get; set; }
    }
}
