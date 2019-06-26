using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;


namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityAnaedit3 : BaseDataContract
    {
        [DataMember]
        public string anaedit3Id { get; set; }

        [DataMember]
        public string dataInDate { get; set; }

        [DataMember]
        public string FMZT { get; set; }

        [DataMember]
        public string WTWCJ { get; set; }

        [DataMember]
        public string WTRL { get; set; }

        [DataMember]
        public string QT { get; set; }

        /// <summary>
        /// EnumCols
        /// </summary>
        public class EnumCols
        {
            public string anaedit3Id;
            public string dataInDate;
            public string FMZT;
            public string WTWCJ;
            public string WTRL;
            public string QT;
        }
    }
}
