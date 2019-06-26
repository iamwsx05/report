using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityOperationReport : BaseDataContract
    {
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember]
        public int  XH { get; set; }

        public decimal sequenceid { get; set; }
        /// <summary>
        /// 日期时间
        /// </summary>
        [DataMember]
        public string RQSJ { get; set; }
        /// <summary>
        /// 通知时间
        /// </summary>
        [DataMember]
        public string TZSJ { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        [DataMember]
        public string FJH { get; set; }
        /// <summary>
        /// 病区
        /// </summary>
        [DataMember]
        public string BQ { get; set; }
        /// <summary>
        /// 床号
        /// </summary>
        [DataMember]
        public string CH { get; set; }
        /// <summary>
        /// 住院号
        /// </summary>
        [DataMember]
        public string ZYH { get; set; }
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
        /// 主要诊断
        /// </summary>
        [DataMember]
        public string ZYZD { get; set; }
        /// <summary>
        /// 手术名称
        /// </summary>
        [DataMember]
        public string SSMC { get; set; }
        /// <summary>
        /// 手术部位
        /// </summary>
        [DataMember]
        public string SSBW { get; set; }
        /// <summary>
        /// 手术类型
        /// </summary>
        [DataMember]
        public string SSLX { get; set; }
        /// <summary>
        /// 手术分级
        /// </summary>
        [DataMember]
        public string SSFJ { get; set; }
        /// <summary>
        /// 主刀医师
        /// </summary>
        [DataMember]
        public string ZDYS { get; set; }
        /// <summary>
        /// 职称
        /// </summary>
        [DataMember]
        public string ZC { get; set; }
        /// <summary>
        /// 助手
        /// </summary>
        [DataMember]
        public string ZS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string BZ { get; set; }
    }
}
