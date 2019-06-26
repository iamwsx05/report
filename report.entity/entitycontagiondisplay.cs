using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;

namespace Report.Entity
{
    [DataContract, Serializable]
    public class EntityContagionDisplay : BaseDataContract
    {
        [DataMember]
        public int check { get; set; }

        [DataMember]
        public string rptId { get; set; }

        [DataMember]
        public string registerCode { get; set; }

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
        public string reportId { get; set; }

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
        public bool isNew { get; set; }

        [DataMember]
        public string SH { get; set; }

        [DataMember]
        public string SHR { get; set; }

        [DataMember]
        public string SHSJ { get; set; }

        [DataMember]
        public string reportDept { get; set; }

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
            public string reportId = "reportId";
            public string reportOperCode = "reportOperCode";
            public string reportOperName = "reportOperName";
            public string patNo = "patNo";
            public string patName = "patName";
            public string patSex = "patSex";
            public string patAge = "patAge";
            public string patBirthDay = "patBirthDay";
            public string contactTel = "contactTel";
            public string deptName = "deptName";
        }
    }

    [DataContract, Serializable]
    public class EntityAidsCheck : BaseDataContract
    {
        //
        [DataMember]
        public string REQNO { get; set; }
        //白细胞计数
        [DataMember]
        public string BXBJS { get; set; }
        //总淋巴细胞计数
        [DataMember]
        public string ZLBXBJS { get; set; }
        //血小板计数
        [DataMember]
        public string XXBJS { get; set; }
        //血红蛋白
        [DataMember]
        public string XHDB { get; set; }
        //血糖
        [DataMember]
        public string XT { get; set; }
        //谷丙转氨酶
        [DataMember]
        public string GBZAM { get; set; }
        //谷草转氨酶
        [DataMember]
        public string GCZAM { get; set; }
        //总胆红素
        [DataMember]
        public string ZDHS { get; set; }
        //血肌酐
        [DataMember]
        public string XJG { get; set; }
        //血尿素氮
        [DataMember]
        public string XNST { get; set; }
        //CD44细胞计数
        [DataMember]
        public string CD4XBJS { get; set; }
        //CD8细胞计数
        [DataMember]
        public string CD8XBJS { get; set; }
        //病毒载量
        [DataMember]
        public string BDZL { get; set; }

        //梅毒螺旋体抗原血清学实验（TPPA/ELISA）
        [DataMember]
        public string MD_TPPA_ELISA { get; set; }
        //梅毒滴度
        [DataMember]
        public string MD_DD { get; set; }

        //非梅毒螺旋体抗原血清学实验（RPR、TRUST）
        [DataMember]
        public string MD_RPR_TRUST { get; set; }
        //滴度
        [DataMember]
        public int iDD { get; set; }
        //乙肝表面抗原（HBsAg）
        [DataMember]
        public string HBsAg { get; set; }
        //乙肝e抗原（HBeAg）
        [DataMember]
        public string HBeAg { get; set; }
        //丙肝 HCV_IGG
        [DataMember]
        public string HCV_IGG { get; set; }
        //丙肝—HCV-IGM
        [DataMember]
        public string HCV_IGM { get; set; }
        //孕周
        [DataMember]
        public string YZ { get; set; }
        //计数
        [DataMember]
        public int count { get; set; }

        #region 4-I
        //        4——I
        //快速血浆反应素环状片试验（RPR）X133 
        //阴性X134 
        //阳性X135
        public string I_RPR { get; set; }
        //检测时间X137
        public string I_RPRTIME { get; set; }
        //甲苯胺红不加热血清试验（TRUST）X138
        //阴性X139
        //阳性X140
        //检测时间X142
        public string I_TRUST { get; set; }
        public string I_TRUSTTIME { get; set; }
        //梅毒螺旋体颗粒凝集试验（TPPA）X145
        //阴性X146
        //阳性X147
        //检测时间X149
        public string I_TPPA { get; set; }
        public string I_TPPATIME { get; set; }
        //酶联免疫吸附试验（ELISA）X150
        //阴性X151
        //阳性X152
        //检测时间X154
        public string I_ELISA { get; set; }
        public string I_ELISATIME { get; set; }
        //免疫层析法-快速体测（RT）X155
        //阴性X156
        //阳性X157
        //检测时间X159
        public string I_RT { get; set; }
        public string I_RTTIME { get; set; }
        //梅毒螺旋体IgM抗体检测：
        //未检测X166
        //检测阳性X167
        //检测阴性X168
        //检测时间X169
        public string I_IGM { get; set; }
        public string I_IGMTIME { get; set; }
        //暗视野显微镜梅毒螺旋体检测：
        //未检测X170
        //检测X171
        //（检测到梅毒螺旋体：
        //否X172
        //是X173
        //检测时间X174
        public string I_MD { get; set; }
        public string I_MDTIME { get; set; }
        #endregion
    }
}
