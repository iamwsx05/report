using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;

namespace Report.Entity
{
    /// <summary>
    /// 查询参数
    /// </summary>
    [DataContract, Serializable]
    public class EntityQueryParm : BaseDataContract
    {
        [DataMember]
        public string eventId { get; set; }

        [DataMember]
        public string reportId { get; set; }

        [DataMember]
        public string startDate { get; set; }

        [DataMember]
        public string endDate { get; set; }

        [DataMember]
        public string deptCode { get; set; }

        [DataMember]
        public string cardNo { get; set; }

        [DataMember]
        public string patName { get; set; }
    }
}
