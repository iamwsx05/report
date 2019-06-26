using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityStatInstrument
    {
        [DataMember]
        public int sortNo { get; set; }

        [DataMember]
        public string eventTime { get; set; }

        [DataMember]
        public string eventContent { get; set; }

        [DataMember]
        public string reportTime { get; set; }

        [DataMember]
        public string reportOperName { get; set; }

        [DataMember]
        public string deptName { get; set; }

        [DataMember]
        public string patientName { get; set; }

        [DataMember]
        public string webTime { get; set; }

        [DataMember]
        public string fcomment { get; set; }

    }
}
