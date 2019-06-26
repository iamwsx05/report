using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityEventStat : BaseDataContract
    {
        /// <summary>
        /// 统计月份
        /// </summary>
        [DataMember]
        public string TJYF { get; set; }
        /// <summary>
        /// I级事件
        /// </summary>
        [DataMember]
        public int IJSJ { get; set; }
        /// <summary>
        /// II级事件
        /// </summary>
        [DataMember]
        public int IIJSJ { get; set; }
        /// <summary>
        /// III级事件
        /// </summary>
        [DataMember]
        public int IIIJSJ { get; set; }
        /// <summary>
        /// IV级事件
        /// </summary>
        [DataMember]
        public int IVJSJ { get; set; }
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
        /// 骨外科
        /// </summary>
        [DataMember]
        public int GWK { get; set; }
        /// <summary>
        /// 神经外科
        /// </summary>
        [DataMember]
        public int SJWK { get; set; }
        /// <summary>
        /// 妇产科
        /// </summary>
        [DataMember]
        public int FCK { get; set; }
        /// <summary>
        /// 儿科
        /// </summary>
        [DataMember]
        public int EK { get; set; }
        /// <summary>
        /// 重症医学科
        /// </summary>
        [DataMember]
        public int CZYXK { get; set; }
        /// <summary>
        /// 急诊科
        /// </summary>
        [DataMember]
        public int JZK { get; set; }
        /// <summary>
        /// 麻醉科
        /// </summary>
        [DataMember]
        public int MZK { get; set; }
        /// <summary>
        /// 中医康复科
        /// </summary>
        [DataMember]
        public int ZYKFK { get; set; }
        /// <summary>
        /// 口腔科
        /// </summary>
        [DataMember]
        public int KJK { get; set; }
        /// <summary>
        /// 检验科
        /// </summary>
        [DataMember]
        public int JYK { get; set; }
        /// <summary>
        /// 病理科
        /// </summary>
        [DataMember]
        public int BLK { get; set; }
        /// <summary>
        /// 放射科
        /// </summary>
        [DataMember]
        public int FSK { get; set; }
        /// <summary>
        /// 超声科
        /// </summary>
        [DataMember]
        public int CSK { get; set; }
        /// <summary>
        /// 药剂科
        /// </summary>
        [DataMember]
        public int YJK { get; set; }
        /// <summary>
        /// 眼科
        /// </summary>
        [DataMember]
        public int YK { get; set; }
        /// <summary>
        /// 耳鼻喉科
        /// </summary>
        [DataMember]
        public int EBHK { get; set; }
        /// <summary>
        /// 第二门部
        /// </summary>
        [DataMember]
        public int DEMZ { get; set; }
        /// <summary>
        /// 第三门部
        /// </summary>
        [DataMember]
        public int DSMZ { get; set; }
        /// <summary>
        /// 门诊部
        /// </summary>
        [DataMember]
        public int MZ { get; set; }
        /// <summary>
        /// 信息传递错误事件
        /// </summary>
        [DataMember]
        public int XXZDCW { get; set; }
        /// <summary>
        /// 治疗错误事件
        /// </summary>
        [DataMember]
        public int ZLCW { get; set; }
        /// <summary>
        /// 方法/技术错误事件
        /// </summary>
        [DataMember]
        public int FFJSCW { get; set; }
        /// <summary>
        /// 药物调剂分发错误事件
        /// </summary>
        [DataMember]
        public int YWTJFFCW { get; set; }
        /// <summary>
        /// 输血事件
        /// </summary>
        [DataMember]
        public int SXSJ { get; set; }
        /// <summary>
        /// 设备器械使用事件
        /// </summary>
        [DataMember]
        public int SBQXSYSJ { get; set; }
        /// <summary>
        /// 导管操作
        /// </summary>
        [DataMember]
        public int DGCZ { get; set; }
        /// <summary>
        /// 医疗技术检查事件
        /// </summary>
        [DataMember]
        public int YLJSJCSJ { get; set; }
        /// <summary>
        /// 基础护理事件
        /// </summary>
        [DataMember]
        public int JCHLSJ { get; set; }
        /// <summary>
        /// 营养与饮食事件
        /// </summary>
        [DataMember]
        public int YYYYSSJ { get; set; }
        /// <summary>
        /// 物品运送事件
        /// </summary>
        [DataMember]
        public int WPYSSJ { get; set; }
        /// <summary>
        /// 放射安全事件
        /// </summary>
        [DataMember]
        public int FSAQSJ { get; set; }
        /// <summary>
        /// 诊疗记录事件
        /// </summary>
        [DataMember]
        public int ZLJLSJ { get; set; }
        /// <summary>
        /// 知情同意事件
        /// </summary>
        [DataMember]
        public int ZQTYSJ { get; set; }
        /// <summary>
        /// 非预期事件
        /// </summary>
        [DataMember]
        public int FYQSJ { get; set; }
        /// <summary>
        /// 医护安全事件
        /// </summary>
        [DataMember]
        public int YHAQSJ { get; set; }

        /// <summary>
        /// 不作为事件
        /// </summary>
        [DataMember]
        public int BZWSJ { get; set; }
        /// <summary>
        /// 并发症
        /// </summary>
        [DataMember]
        public int BFZ { get; set; }
        /// <summary>
        /// 其它事件
        /// </summary>
        [DataMember]
        public int QTSJ { get; set; }
        /// <summary>
        /// 小计
        /// </summary>
        [DataMember]
        public int XJ { get; set; }
    }
}
