using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityRiskAna : BaseDataContract
    {
        [DataMember]
        public int XH { get; set; }
        //科室
        [DataMember]
        public string KS { get; set; }
        //姓名
        [DataMember]
        public string XM { get; set; }
        //住院号
        [DataMember]
        public string ZYH { get; set; }
        //手术名称
        [DataMember]
        public string SSMC { get; set; }
        //手术日期
        [DataMember]
        public string SSRQ { get; set; }
        //切口清洁程度
        [DataMember]
        public string QKQJCD { get; set; }
        //手术类别
        [DataMember]
        public string SSLB { get; set; }
        //麻醉分级
        [DataMember]
        public string MZFJ { get; set; }
        //手术在3小时内完成
        [DataMember]
        public string SS3XSWC { get; set; }
        //超过3小时
        [DataMember]
        public string CG3XS { get; set; }
        //急诊手术
        [DataMember]
        public string JZSS { get; set; }
        //术前30-60分钟预防用药
        [DataMember]
        public string SQYFYY { get; set; }
        //植入物
        [DataMember]
        public string ZRW { get; set; }
        //抗菌药物种类
        [DataMember]
        public string KJYZL { get; set; }
        //超3小时追加
        [DataMember]
        public string C3XSZJ { get; set; }
        //出血超1500ml追加
        [DataMember]
        public string CXC1500 { get; set; }
        //NNIS分级
        [DataMember]
        public string NNIS { get; set; }
        //愈合
        [DataMember]
        public string YH { get; set; }
        //感染
        [DataMember]
        public string GR { get; set; }
    }
}
