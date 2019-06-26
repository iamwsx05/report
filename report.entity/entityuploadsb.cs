using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using weCare.Core.Entity;
using System.Data;

namespace Report.Entity
{
    //#region 首页
    //[DataContract, Serializable]
    //public class EntityFirstPage : BaseDataContract
    //{
    //    [DataMember]
    //    public string JZJLH { get; set; }
    //    [DataMember]
    //    public string FWSJGDM { get; set; }
    //    [DataMember]
    //    public string FFBBHNEW { get; set; }
    //    [DataMember]
    //    public string FFBNEW { get; set; }
    //    [DataMember]
    //    public string FASCARD1 { get; set; }
    //    [DataMember]
    //    public int FTIMES { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //    [DataMember]
    //    public string FNAME { get; set; }
    //    [DataMember]
    //    public string FSEXBH { get; set; }
    //    [DataMember]
    //    public string FSEX { get; set; }
    //    [DataMember]
    //    public string FBIRTHDAY { get; set; }
    //    [DataMember]
    //    public string FAGE { get; set; }
    //    [DataMember]
    //    public string fcountrybh { get; set; }
    //    [DataMember]
    //    public string fcountry { get; set; }
    //    [DataMember]
    //    public string fnationalitybh { get; set; }
    //    [DataMember]
    //    public string fnationality { get; set; }
    //    [DataMember]
    //    public string FCSTZ { get; set; }
    //    [DataMember]
    //    public string FRYTZ { get; set; }
    //    [DataMember]
    //    public string FBIRTHPLACE { get; set; }
    //    [DataMember]
    //    public string FNATIVE { get; set; }
    //    [DataMember]
    //    public string FIDCard { get; set; }
    //    [DataMember]
    //    public string FJOB { get; set; }
    //    [DataMember]
    //    public string FSTATUSBH { get; set; }
    //    [DataMember]
    //    public string FSTATUS { get; set; }
    //    [DataMember]
    //    public string FCURRADDR { get; set; }
    //    [DataMember]
    //    public string FCURRTELE { get; set; }
    //    [DataMember]
    //    public string FCURRPOST { get; set; }
    //    [DataMember]
    //    public string FHKADDR { get; set; }
    //    [DataMember]
    //    public string FHKPOST { get; set; }
    //    [DataMember]
    //    public string FDWNAME { get; set; }
    //    [DataMember]
    //    public string FDWADDR { get; set; }
    //    [DataMember]
    //    public string FDWTELE { get; set; }
    //    [DataMember]
    //    public string FDWPOST { get; set; }
    //    [DataMember]
    //    public string FLXNAME { get; set; }
    //    [DataMember]
    //    public string FRELATE { get; set; }
    //    [DataMember]
    //    public string FLXADDR { get; set; }
    //    [DataMember]
    //    public string FLXTELE { get; set; }
    //    [DataMember]
    //    public string FRYTJBH { get; set; }
    //    [DataMember]
    //    public string FRYTJ { get; set; }
    //    [DataMember]
    //    public string FRYDATE { get; set; }
    //    [DataMember]
    //    public string FRYTIME { get; set; }
    //    [DataMember]
    //    public string FRYTYKH { get; set; }
    //    [DataMember]
    //    public string FRYDEPT { get; set; }
    //    [DataMember]
    //    public string FRYBS { get; set; }
    //    [DataMember]
    //    public string FZKTYKH { get; set; }
    //    [DataMember]
    //    public string FZKDEPT { get; set; }
    //    [DataMember]
    //    public string FZKTIME { get; set; }
    //    [DataMember]
    //    public string FCYDATE { get; set; }
    //    [DataMember]
    //    public string FCYTIME { get; set; }
    //    [DataMember]
    //    public string FCYTYKH { get; set; }
    //    [DataMember]
    //    public string FCYDEPT { get; set; }
    //    [DataMember]
    //    public string FCYBS { get; set; }
    //    [DataMember]
    //    public string FDAYS { get; set; }
    //    [DataMember]
    //    public string FMZZDBH { get; set; }
    //    [DataMember]
    //    public string FMZZD { get; set; }
    //    [DataMember]
    //    public string FMZDOCTBH { get; set; }
    //    [DataMember]
    //    public string FMZDOCT { get; set; }
    //    [DataMember]
    //    public string FJBFXBH { get; set; }
    //    [DataMember]
    //    public string FJBFX { get; set; }
    //    [DataMember]
    //    public string FYCLJBH { get; set; }
    //    [DataMember]
    //    public string FYCLJ { get; set; }
    //    [DataMember]
    //    public string FQJTIMES { get; set; }
    //    [DataMember]
    //    public string FQJSUCTIMES { get; set; }
    //    [DataMember]
    //    public string FPHZD { get; set; }
    //    [DataMember]
    //    public string FPHZDNUM { get; set; }
    //    [DataMember]
    //    public string FPHZDBH { get; set; }
    //    [DataMember]
    //    public string FIFGMYWBH { get; set; }
    //    [DataMember]
    //    public string FIFGMYW { get; set; }
    //    [DataMember]
    //    public string FGMYW { get; set; }
    //    [DataMember]
    //    public string FBODYBH { get; set; }
    //    [DataMember]
    //    public string FBODY { get; set; }
    //    [DataMember]
    //    public string FBLOODBH { get; set; }
    //    [DataMember]
    //    public string FBLOOD { get; set; }
    //    [DataMember]
    //    public string FRHBH { get; set; }
    //    [DataMember]
    //    public string FRH { get; set; }
    //    [DataMember]
    //    public string FKZRBH { get; set; }
    //    [DataMember]
    //    public string FKZR { get; set; }
    //    [DataMember]
    //    public string FZRDOCTBH { get; set; }
    //    [DataMember]
    //    public string FZRDOCTOR { get; set; }
    //    [DataMember]
    //    public string FZZDOCTBH { get; set; }
    //    [DataMember]
    //    public string FZZDOCT { get; set; }
    //    [DataMember]
    //    public string FZYDOCTBH { get; set; }
    //    [DataMember]
    //    public string FZYDOCT { get; set; }
    //    [DataMember]
    //    public string FNURSEBH { get; set; }
    //    [DataMember]
    //    public string FNURSE { get; set; }
    //    [DataMember]
    //    public string FJXDOCTBH { get; set; }
    //    [DataMember]
    //    public string FJXDOCT { get; set; }
    //    [DataMember]
    //    public string FSXDOCTBH { get; set; }
    //    [DataMember]
    //    public string FSXDOCT { get; set; }
    //    [DataMember]
    //    public string FBMYBH { get; set; }
    //    [DataMember]
    //    public string FBMY { get; set; }
    //    [DataMember]
    //    public string FQUALITYBH { get; set; }
    //    [DataMember]
    //    public string FQUALITY { get; set; }
    //    [DataMember]
    //    public string FZKDOCTBH { get; set; }
    //    [DataMember]
    //    public string FZKDOCT { get; set; }
    //    [DataMember]
    //    public string FZKNURSEBH { get; set; }
    //    [DataMember]
    //    public string FZKNURSE { get; set; }
    //    [DataMember]
    //    public string FZKRQ { get; set; }
    //    [DataMember]
    //    public string FLYFSBH { get; set; }
    //    [DataMember]
    //    public string FLYFS { get; set; }
    //    [DataMember]
    //    public string FYZOUTHOSTITAL { get; set; }
    //    [DataMember]
    //    public string FSQOUTHOSTITAL { get; set; }
    //    [DataMember]
    //    public string FISAGAINRYBH { get; set; }
    //    [DataMember]
    //    public string FISAGAINRY { get; set; }
    //    [DataMember]
    //    public string FISAGAINRYMD { get; set; }
    //    [DataMember]
    //    public string FRYQHMDAYS { get; set; }
    //    [DataMember]
    //    public string FRYQHMHOURS { get; set; }
    //    [DataMember]
    //    public string FRYQHMMINS { get; set; }
    //    [DataMember]
    //    public string FRYQHMCOUNTS { get; set; }
    //    [DataMember]
    //    public string FRYHMDAYS { get; set; }
    //    [DataMember]
    //    public string FRYHMHOURS { get; set; }
    //    [DataMember]
    //    public string FRYHMMINS { get; set; }
    //    [DataMember]
    //    public string FRYHMCOUNTS { get; set; }
    //    [DataMember]
    //    public decimal FSUM1 { get; set; }
    //    [DataMember]
    //    public decimal FZFJE { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLYLF { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLCZF { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLHLF { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLQTF { get; set; }
    //    [DataMember]
    //    public decimal FZDLBLF { get; set; }
    //    [DataMember]
    //    public decimal FZDLSSSF { get; set; }
    //    [DataMember]
    //    public decimal FZDLYXF { get; set; }
    //    [DataMember]
    //    public decimal FZDLLCF { get; set; }
    //    [DataMember]
    //    public decimal FZLLFFSSF { get; set; }
    //    [DataMember]
    //    public decimal FZLLFWLZWLF { get; set; }
    //    [DataMember]
    //    public decimal FZLLFSSF { get; set; }
    //    [DataMember]
    //    public decimal FZLLFMZF { get; set; }
    //    [DataMember]
    //    public decimal FZLLFSSZLF { get; set; }
    //    [DataMember]
    //    public decimal FKFLKFF { get; set; }
    //    [DataMember]
    //    public decimal FZYLZF { get; set; }
    //    [DataMember]
    //    public decimal FXYF { get; set; }
    //    [DataMember]
    //    public decimal FXYLGJF { get; set; }
    //    [DataMember]
    //    public decimal FZCHYF { get; set; }
    //    [DataMember]
    //    public decimal FZCYF { get; set; }
    //    [DataMember]
    //    public decimal FXYLXF { get; set; }
    //    [DataMember]
    //    public decimal FXYLBQBF { get; set; }
    //    [DataMember]
    //    public decimal FXYLQDBF { get; set; }
    //    [DataMember]
    //    public decimal FXYLYXYZF { get; set; }
    //    [DataMember]
    //    public decimal FXYLXBYZF { get; set; }
    //    [DataMember]
    //    public decimal FHCLCJF { get; set; }
    //    [DataMember]
    //    public decimal FHCLZLF { get; set; }
    //    [DataMember]
    //    public decimal FHCLSSF { get; set; }
    //    [DataMember]
    //    public decimal FQTF { get; set; }
    //    [DataMember]
    //    public string FBGLX { get; set; }
    //    [DataMember]
    //    public string GMSFHM { get; set; }
    //    [DataMember]
    //    public string YYBH { get; set; }
    //    [DataMember]
    //    public decimal FZYF { get; set; }
    //    [DataMember]
    //    public string FZKDATE { get; set; }
    //    [DataMember]
    //    public string FJOBBH { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLYLF01 { get; set; }
    //    [DataMember]
    //    public decimal FZHFWLYLF02 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZDF { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF01 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF02 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF03 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF04 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF05 { get; set; }
    //    [DataMember]
    //    public decimal FZYLZLF06 { get; set; }
    //    [DataMember]
    //    public decimal FZYLQTF { get; set; }
    //    [DataMember]
    //    public decimal FZYLQTF01 { get; set; }
    //    [DataMember]
    //    public decimal FZYLQTF02 { get; set; }
    //    [DataMember]
    //    public decimal FZCLJGZJF { get; set; }
    //    [DataMember]
    //    public string FZYID { get; set; }

    //    [DataMember]
    //    public List<EntityBrzkqk> lstZkVo { get; set; }
    //    [DataMember]
    //    public List<EntityBrzdxx> lstZdVo { get; set; }
    //    [DataMember]
    //    public List<EntityBrssxx> lstSsVo { get; set; }
    //    [DataMember]
    //    public List<EntityFyksj> lstFyVo { get; set; }
    //    [DataMember]
    //    public List<EntityZlksj> lstZlVo { get; set; }
    //    [DataMember]
    //    public List<EntityZlhljlsj> lstHlVo { get; set; }
    //    [DataMember]
    //    public List<EntityBrzdfjm> lstZdfjVo { get; set; }
    //    [DataMember]
    //    public List<EntityZyybrfjxx> lstZyVo { get; set; }
    //}
    //#endregion

    //#region 病人转科情况
    ///// <summary>
    ///// 数据集(病人转科情况)：BRZKQKSJJ
    ///// </summary>
    //public class EntityBrzkqk
    //{
    //    [DataMember]
    //    public string FZKTYKH { get; set; }
    //    [DataMember]
    //    public string FZKDEPT { get; set; }
    //    [DataMember]
    //    public string FZKDATE { get; set; }
    //    [DataMember]
    //    public string FZKTIME { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }

    //}
    //#endregion

    //#region 病人诊断信息
    ///// <summary>
    ///// 数据集(病人诊断信息):  BRZDXXSJJ
    ///// </summary>
    //public class EntityBrzdxx
    //{
    //    [DataMember]
    //    public string FZDLX { get; set; }
    //    [DataMember]
    //    public string FICDVersion { get; set; }
    //    [DataMember]
    //    public string FICDM { get; set; }
    //    [DataMember]
    //    public string FJBNAME { get; set; }
    //    [DataMember]
    //    public string FRYBQBH { get; set; }
    //    [DataMember]
    //    public string FRYBQ { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //}
    //#endregion

    //#region 病人手术信息
    ///// <summary>
    ///// 数据集(病人手术信息):BRSSXXSJJ
    ///// </summary>
    //public class EntityBrssxx
    //{
    //    [DataMember]
    //    public string FNAME { get; set; }
    //    [DataMember]
    //    public string FOPTIMES { get; set; }
    //    [DataMember]
    //    public string FOPCODE { get; set; }
    //    [DataMember]
    //    public string FOP { get; set; }
    //    [DataMember]
    //    public string FOPDATE { get; set; }
    //    [DataMember]
    //    public string FQIEKOUBH { get; set; }
    //    [DataMember]
    //    public string FQIEKOU { get; set; }
    //    [DataMember]
    //    public string FYUHEBH { get; set; }
    //    [DataMember]
    //    public string FYUHE { get; set; }
    //    [DataMember]
    //    public string FDOCBH { get; set; }
    //    [DataMember]
    //    public string FDOCNAME { get; set; }
    //    [DataMember]
    //    public string FMAZUIBH { get; set; }
    //    [DataMember]
    //    public string FMAZUI { get; set; }
    //    [DataMember]
    //    public string FIFFSOP { get; set; }
    //    [DataMember]
    //    public string FOPDOCT1BH { get; set; }
    //    [DataMember]
    //    public string FOPDOCT1 { get; set; }
    //    [DataMember]
    //    public string FOPDOCT2BH { get; set; }
    //    [DataMember]
    //    public string FOPDOCT2 { get; set; }
    //    [DataMember]
    //    public string FMZDOCTBH { get; set; }
    //    [DataMember]
    //    public string FMZDOCT { get; set; }
    //    [DataMember]
    //    public string FZQSSBH { get; set; }
    //    [DataMember]
    //    public string FZQSS { get; set; }
    //    [DataMember]
    //    public string FSSJBBH { get; set; }
    //    [DataMember]
    //    public string FSSJB { get; set; }
    //    [DataMember]
    //    public string FOPKSNAME { get; set; }
    //    [DataMember]
    //    public string FOPTYKH { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //}
    //#endregion

    //#region 妇婴卡
    ///// <summary>
    ///// 数据集（妇婴卡）FYKSJJ
    ///// </summary>
    //public class EntityFyksj
    //{
    //    [DataMember]
    //    public string FBABYNUM { get; set; }
    //    [DataMember]
    //    public string FNAME { get; set; }
    //    [DataMember]
    //    public string FBABYSEXBH { get; set; }
    //    [DataMember]
    //    public string FBABYSEX { get; set; }
    //    [DataMember]
    //    public string FTZ { get; set; }
    //    [DataMember]
    //    public string FRESULTBH { get; set; }
    //    [DataMember]
    //    public string FRESULT { get; set; }
    //    [DataMember]
    //    public string FZGBH { get; set; }
    //    [DataMember]
    //    public string FZG { get; set; }
    //    [DataMember]
    //    public string FBABYSUC { get; set; }
    //    [DataMember]
    //    public string FHXBH { get; set; }
    //    [DataMember]
    //    public string FHX { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //}
    //#endregion

    //#region 肿瘤卡
    ///// <summary>
    ///// 数据集（肿瘤卡）：ZLKSJJ
    ///// </summary>
    //public class EntityZlksj
    //{
    //    [DataMember]
    //    public string FFLFSBH { get; set; }
    //    [DataMember]
    //    public string FFLFS { get; set; }
    //    [DataMember]
    //    public string FFLCXBH { get; set; }
    //    [DataMember]
    //    public string FFLCX { get; set; }
    //    [DataMember]
    //    public string FFLZZBH { get; set; }
    //    [DataMember]
    //    public string FFLZZ { get; set; }
    //    [DataMember]
    //    public string FYJY { get; set; }
    //    [DataMember]
    //    public string FYCS { get; set; }
    //    [DataMember]
    //    public string FYTS { get; set; }
    //    [DataMember]
    //    public string FYRQ1 { get; set; }
    //    [DataMember]
    //    public string FYRQ2 { get; set; }
    //    [DataMember]
    //    public string FQJY { get; set; }
    //    [DataMember]
    //    public string FQCS { get; set; }
    //    [DataMember]
    //    public string FQTS { get; set; }
    //    [DataMember]
    //    public string FQRQ1 { get; set; }
    //    [DataMember]
    //    public string FQRQ2 { get; set; }
    //    [DataMember]
    //    public string FZNAME { get; set; }
    //    [DataMember]
    //    public string FZJY { get; set; }
    //    [DataMember]
    //    public string FZCS { get; set; }
    //    [DataMember]
    //    public string FZTS { get; set; }
    //    [DataMember]
    //    public string FZRQ1 { get; set; }
    //    [DataMember]
    //    public string FZRQ2 { get; set; }
    //    [DataMember]
    //    public string FHLFSBH { get; set; }
    //    [DataMember]
    //    public string FHLFS { get; set; }
    //    [DataMember]
    //    public string FHLFFBH { get; set; }
    //    [DataMember]
    //    public string FHLFF { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //}
    //#endregion

    //#region 肿瘤化疗记录
    ///// <summary>
    ///// 数据集（肿瘤化疗记录）：ZLHLJLSJJ
    ///// </summary>
    //public class EntityZlhljlsj
    //{
    //    public string FHLRQ1 { get; set; }
    //    [DataMember]
    //    public string FHLDRUG { get; set; }
    //    [DataMember]
    //    public string FHLPROC { get; set; }
    //    [DataMember]
    //    public string FHLLXBH { get; set; }
    //    [DataMember]
    //    public string FHLLX { get; set; }
    //    [DataMember]
    //    public string FPRN { get; set; }
    //}
    //#endregion

    //#region 病人诊断码附加编码
    ///// <summary>
    ///// 数据集（病人诊断码附加编码）：BRZDMFJBMSJJ
    ///// </summary>
    //public class EntityBrzdfjm
    //{
    //    [DataMember]
    //    public string FZDLX { get; set; }
    //    [DataMember]
    //    public string FICDM { get; set; }
    //    [DataMember]
    //    public string FFJICDM { get; set; }
    //    [DataMember]
    //    public string FFJJBNAME { get; set; }
    //    [DataMember]
    //    public string FFRYBQBH { get; set; }
    //    [DataMember]
    //    public string FFRYBQ { get; set; }
    //    [DataMember]
    //    public string FPX { get; set; }
    //}
    //#endregion

    //#region 中医院病人附加信息
    ///// <summary>
    ///// 数据集（中医院病人附加信息）：ZYYBRFJXXSJJ
    ///// </summary>
    //public class EntityZyybrfjxx
    //{
    //    [DataMember]
    //    public string FPRN { get; set; }
    //    [DataMember]
    //    public string FZLLBBH { get; set; }
    //    [DataMember]
    //    public string FZLLB { get; set; }
    //    [DataMember]
    //    public string FZZZYBH { get; set; }
    //    [DataMember]
    //    public string FZZZY { get; set; }
    //    [DataMember]
    //    public string FRYCYBH { get; set; }
    //    [DataMember]
    //    public string FRYCY { get; set; }
    //    [DataMember]
    //    public string FMZZYZDBH { get; set; }
    //    [DataMember]
    //    public string FMZZYZD { get; set; }
    //    [DataMember]
    //    public string FSSLCLJBH { get; set; }
    //    [DataMember]
    //    public string FSSLCLJ { get; set; }
    //    [DataMember]
    //    public string FSYJGZJBH { get; set; }
    //    [DataMember]
    //    public string FSYJGZJ { get; set; }
    //    [DataMember]
    //    public string FSYZYSBBH { get; set; }
    //    [DataMember]
    //    public string FSYZYSB { get; set; }
    //    [DataMember]
    //    public string FSYZYJSBH { get; set; }
    //    [DataMember]
    //    public string FSYZYJS { get; set; }
    //    [DataMember]
    //    public string FBZSHBH { get; set; }
    //    [DataMember]
    //    public string FBZSH { get; set; }
    //}
    //#endregion

    //#region 出院小结
    //[DataContract, Serializable]
    //public class EntityCyxj : BaseDataContract
    //{
    //    [DataMember]
    //    public string JZJLH { get; set; }
    //    [DataMember]
    //    public string MZH { get; set; }
    //    [DataMember]
    //    public string ZYH { get; set; }
    //    [DataMember]
    //    public string MZZD { get; set; }
    //    [DataMember]
    //    public string RYZD { get; set; }
    //    [DataMember]
    //    public string CYZD { get; set; }
    //    [DataMember]
    //    public string XM { get; set; }
    //    [DataMember]
    //    public string XB { get; set; }
    //    [DataMember]
    //    public string NL { get; set; }
    //    [DataMember]
    //    public string ZY { get; set; }
    //    [DataMember]
    //    public string JG { get; set; }
    //    [DataMember]
    //    public string RYRQ { get; set; }
    //    [DataMember]
    //    public string CYRQ { get; set; }
    //    [DataMember]
    //    public string ZYTS { get; set; }
    //    [DataMember]
    //    public string YJDZ { get; set; }
    //    [DataMember]
    //    public string ZLJG { get; set; }
    //    [DataMember]
    //    public string CYYZ { get; set; }
    //    [DataMember]
    //    public string YSQM { get; set; }
    //    [DataMember]
    //    public string RYHCLGC { get; set; }
    //    [DataMember]
    //    public string CYSQK { get; set; }
    //    [DataMember]
    //    public string GMSFHM { get; set; }
    //    [DataMember]
    //    public string RYSJ { get; set; }
    //    [DataMember]
    //    public string CYSJ { get; set; }
    //    [DataMember]
    //    public string RYQK { get; set; }
    //}
    //#endregion

    //[DataContract, Serializable]
    //[Entity(TableName = "t_upload")]
    //public class EntityPatUpload : BaseDataContract
    //{
    //    public static EntityPatUpload.EnumCols Columns;
    //    [DataMember]
    //    [Entity(FieldName = "SERNO", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
    //    public decimal SERNO { get; set; }
    //    //上传类型
    //    [DataMember]
    //    [Entity(FieldName = "UPLOADTYPE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
    //    public decimal UPLOADTYPE { get; set; }
    //    //姓名
    //    [DataMember]
    //    [Entity(FieldName = "PATNAME", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
    //    public string PATNAME { get; set; }
    //    //性别
    //    [DataMember]
    //    [Entity(FieldName = "PATSEX", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
    //    public string PATSEX { get; set; }
    //    //身份证号
    //    [DataMember]
    //    [Entity(FieldName = "IDCARD", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
    //    public string IDCARD { get; set; }
    //    //住院号
    //    [DataMember]
    //    [Entity(FieldName = "INPATIENTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
    //    public string INPATIENTID { get; set; }

    //    //科室CODE
    //    [DataMember]
    //    [Entity(FieldName = "INDEPTCODE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 7)]
    //    public string INDEPTCODE { get; set; }
    //    //操作人CODE
    //    [DataMember]
    //    [Entity(FieldName = "OPERCODE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 8)]
    //    public string OPERCODE { get; set; }
    //    //记录时间
    //    [DataMember]
    //    [Entity(FieldName = "RECORDDDATE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
    //    public string RECORDDDATE { get; set; }
    //    //状态
    //    [DataMember]
    //    [Entity(FieldName = "STATUS", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 10)]
    //    public Decimal STATUS { get; set; }
    //    //入院时间
    //    [DataMember]
    //    [Entity(FieldName = "INPATIENTDATE", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 11)]
    //    public DateTime? INPATIENTDATE { get; set; }
    //    //出院时间
    //    [DataMember]
    //    [Entity(FieldName = "OUTHOSPITALDATE", DbType = DbType.DateTime, IsPK = false, IsSeq = false, SerNo = 12)]
    //    public DateTime? OUTHOSPITALDATE { get; set; }
    //    //上传时间
    //    [DataMember]
    //    [Entity(FieldName = "UPLOADDATE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
    //    public string UPLOADDATE { get; set; }
    //    //上传时间
    //    [DataMember]
    //    [Entity(FieldName = "FID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 14)]
    //    public string FID { get; set; }
    //    //病案号
    //    [DataMember]
    //    [Entity(FieldName = "FPRN", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 15)]
    //    public string FPRN { get; set; }
    //    //住院次数
    //    [DataMember]
    //    [Entity(FieldName = "FTIMES", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 16)]
    //    public string FTIMES { get; set; }
    //    //出生日期
    //    [DataMember]
    //    [Entity(FieldName = "BIRTH", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 17)]
    //    public string BIRTH { get; set; }
    //    //出院科室
    //    [DataMember]
    //    [Entity(FieldName = "OUTDEPTCODE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 18)]
    //    public string OUTDEPTCODE { get; set; }
    //    //出院科室
    //    [DataMember]
    //    [Entity(FieldName = "JZJLH", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 19)]
    //    public string JZJLH { get; set; }
    //    //出院科室
    //    [DataMember]
    //    [Entity(FieldName = "REGISTERID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 20)]
    //    public string REGISTERID { get; set; }

    //    [DataMember]
    //    public string InDeptName { get; set; }
    //    [DataMember]
    //    public string OutDeptName { get; set; }
    //    [DataMember]
    //    public string JBR {get;set;}
    //    [DataMember]
    //    public string JBRXM { get; set; }
    //    [DataMember]
    //    public string SZ { get; set; }
    //    [DataMember]
    //    public string RYSJ { get; set; }
    //    [DataMember]
    //    public string CYSJ { get; set; }
    //    [DataMember]
    //    public int XH { get; set; }
    //    [DataMember]
    //    public int Issucess { get; set; }
    //    [DataMember]
    //    public string FailMsg { get; set; }
    //    [DataMember]
    //    public EntityFirstPage fpVo { get; set; }
    //    [DataMember]
    //    public EntityCyxj xjVo { get; set; }


    //    /// <summary>
    //    /// EnumCols
    //    /// </summary>
    //    public class EnumCols
    //    {
    //        public string SERNO = "SERNO";
    //        public string UPLOADTYPE = "UPLOADTYPE";
    //        public string PATNAME = "PATNAME";
    //        public string PATSEX = "PATSEX";
    //        public string IDCARD = "IDCARD";
    //        public string INPATIENTID = "INPATIENTID";
    //        public string DEPTCODE = "DEPTCODE";
    //        public string OPERCODE = "OPERCODE";
    //        public string RECORDDDATE = "RECORDDDATE";
    //        public string STATUS = "STATUS";
    //        public string INPATIENTDATE = "INPATIENTDATE";
    //        public string OUTHOSPITALDATE = "OUTHOSPITALDATE";
    //        public string UPLOADDATE = "UPLOADDATE";
    //        public string FID = "FID";
    //        public string FPRN = "FPRN";
    //        public string FTIMES = "FTIMES";
    //        public string BIRTH = "BIRTH";
    //        public string OUTDEPTCODE = "OUTDEPTCODE";
    //        public string JZJLH = "JZJLH";
    //        public string REGISTERID = "JZJLH";
    //        public EnumCols() { }
    //    }
    //}

    //#region 处方项目信息
    //[DataContract, Serializable]
    //[Entity(TableName = "t_mzcfupload")]
    //public class EntityMzcf : BaseDataContract
    //{
    //    [DataMember]
    //    public int XH { get; set; }
    //    [DataMember]
    //    public string MZH { get; set; }
    //    [DataMember]
    //    public string XM { get; set; }
    //    [DataMember]
    //    public string XB { get; set; }
    //    [DataMember]
    //    public string CSRQ { get; set; }
    //    [DataMember]
    //    public string JZYS { get; set; }
    //    [DataMember]
    //    public string JZKS { get; set; }
    //    [DataMember]
    //    public string SZ { get; set; }
    //    [DataMember]
    //    public string SZSJ { get; set; }

    //    //处方号
    //    [DataMember]
    //    public string CFH { get; set; }
    //    //GMSFHM
    //    [DataMember]
    //    public string GMSFHM { get; set; }
    //    //费用日期
    //    [DataMember]
    //    public string JZLB { get; set; }
    //    //费用日期
    //    [DataMember]
    //    public string FYRQ { get; set; }
    //    //医师工号
    //    [DataMember]
    //    public string YSGH { get; set; }

    //    //成功标志
    //    [DataMember]
    //    public int IsSuccess { get; set; }
    //    //失败信息
    //    [DataMember]
    //    public string failMsg { get; set; }
    //    //经办人姓名
    //    [DataMember]
    //    public string jbrxm { get; set; }

    //    [DataMember]
    //    public List<EntityMzcfMsg> lstCfMsg { get; set; }

    //    public static EntityPatUpload.EnumCols Columns;
    //    [DataMember]
    //    [Entity(FieldName = "SERNO", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
    //    public decimal SERNO { get; set; }
    //    //患者ID
    //    [DataMember]
    //    [Entity(FieldName = "PATIENTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 2)]
    //    public string PATIENTID { get; set; }
    //    //处方号
    //    [DataMember]
    //    [Entity(FieldName = "OUTPATRECIPEID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 3)]
    //    public string OUTPATRECIPEID { get; set; }
    //    //姓名
    //    [DataMember]
    //    [Entity(FieldName = "PATNAME", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 4)]
    //    public string PATNAME { get; set; }
    //    //性别
    //    [DataMember]
    //    [Entity(FieldName = "PATSEX", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
    //    public string PATSEX { get; set; }
    //    //出生日期
    //    [DataMember]
    //    [Entity(FieldName = "BIRTH", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
    //    public string BIRTH { get; set; }
    //    //身份证号
    //    [DataMember]
    //    [Entity(FieldName = "IDCARD", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 7)]
    //    public string IDCARD { get; set; }
    //    //医生工号
    //    [DataMember]
    //    [Entity(FieldName = "DOCTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 8)]
    //    public string DOCTID { get; set; }
    //    //科室ID
    //    [DataMember]
    //    [Entity(FieldName = "DEPTID", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 9)]
    //    public string DEPTID { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "STATUS", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 10)]
    //    public int STATUS { get; set; }
    //    //经办人id
    //    [DataMember]
    //    [Entity(FieldName = "JBR", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 11)]
    //    public string JBR { get; set; }
    //    //上传时间
    //    [DataMember]
    //    [Entity(FieldName = "UPLOADDATE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 12)]
    //    public string UPLOADDATE { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "RECORDDATE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 13)]
    //    public string RECORDDATE { get; set; }

    //    //上传时间
    //    [DataMember]
    //    [Entity(FieldName = "PH", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 14)]
    //    public string PH { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "ZJE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 15)]
    //    public string ZJE { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "ZYH", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 16)]
    //    public string ZYH { get; set; }

    //    /// <summary>
    //    /// EnumCols
    //    /// </summary>
    //    public class EnumCols
    //    {
    //        public string SERNO = "SERNO";
    //        public string PATIENTID = "PATIENTID";
    //        public string OUTPATRECIPEID = "OUTPATRECIPEID";
    //        public string PATNAME = "PATNAME";
    //        public string PATSEX = "PATSEX";
    //        public string BIRTH = "BIRTH";
    //        public string IDCARD = "IDCARD";
    //        public string DOCTID = "DOCTID";
    //        public string DEPTID = "DEPTID";
    //        public string STATUS = "STATUS";
    //        public string JBR = "JBR";
    //        public string UPLOADDATE = "UPLOADDATE";
    //        public string RECORDDATE = "RECORDDATE";
    //        public string PH = "PH";
    //        public string ZJE = "ZJE";
    //        public EnumCols() { }
    //    }

    //}
    //#endregion

    //#region 处方项目明细
    //[DataContract, Serializable]
    //public class EntityMzcfMsg : BaseDataContract
    //{
    //    [DataMember]
    //    public int XH { get; set; }
    //    [DataMember]
    //    public string LX { get; set; }
    //    [DataMember]
    //    public string GG { get; set; }
    //    [DataMember]
    //    public string DW { get; set; }
    //    [DataMember]
    //    public string ZJ { get; set; }
    //    //处方号
    //    [DataMember]
    //    public string CFH { get; set; }
    //    //项目序号
    //    [DataMember]
    //    public string XMXH { get; set; }
    //    //医院项目编号
    //    [DataMember]
    //    public string XMBH { get; set; }
    //    //项目名称
    //    [DataMember]
    //    public string XMMC { get; set; }
    //    //单价
    //    [DataMember]
    //    public string JG { get; set; }
    //    //数量
    //    [DataMember]
    //    public string MCYL { get; set; }
    //    //金额
    //    [DataMember]
    //    public string JE { get; set; }
    //    //自费比例
    //    [DataMember]
    //    public string ZFBL { get; set; }
    //    //备注
    //    [DataMember]
    //    public string BZ { get; set; }
    //}
    //#endregion

    //#region
    //[DataContract, Serializable]
    //public class EntityDGExtra : BaseDataContract
    //{
    //    // 摘要:
    //    //     参保地统筹区编码
    //    public string CBDTCQBM;
    //    //
    //    // 摘要:
    //    //     待遇享受标志
    //    public string DYXSBZ;
    //    //
    //    // 摘要:
    //    //     结束时间
    //    public DateTime EndTime;
    //    //
    //    // 摘要:
    //    //     发票编号
    //    public string FPHM;
    //    //
    //    // 摘要:
    //    //     公民身份号码
    //    public string GMSFHM;
    //    //
    //    // 摘要:
    //    //     经办人
    //    public string JBR;
    //    //
    //    // 摘要:
    //    //     就诊记录号
    //    public string JZJLH;
    //    //
    //    // 摘要:
    //    //     就诊类别
    //    public string JZLB;
    //    //
    //    // 摘要:
    //    //     结算序号
    //    public string SDYWH;
    //    //
    //    // 摘要:
    //    //     开始时间
    //    public DateTime StarTime;
    //    //
    //    // 摘要:
    //    //     原因
    //    public string YY;
    //    //
    //    // 摘要:
    //    //     医院编号
    //    public string YYBH;
    //    //
    //    // 摘要:
    //    //     住院号(住院记帐处方项目删除用)
    //    public string ZYH;
    //    //
    //    // 摘要:
    //    //     住院类别
    //    public string ZYLB;

    //    public string FWSJGDM;

    //}
    //#endregion

    //#region 自动任务参数
    //[DataContract, Serializable]
    //[Entity(TableName = "t_uploadparam")]
    //public class EntityUploadParam : BaseDataContract
    //{
    //    [DataMember]
    //    public int isNew { get; set; }

    //    //
    //    [DataMember]
    //    [Entity(FieldName = "PARAMID", DbType = DbType.Decimal, IsPK = true, IsSeq = false, SerNo = 1)]
    //    public decimal PARAMID { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "UPLOADTYPE", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 2)]
    //    public decimal upType { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "DTETYPE", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 3)]
    //    public decimal dateType { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "DTESTART", DbType = DbType.String, IsPK = true, IsSeq = false, SerNo = 4)]
    //    public string dteStart { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "DTEEND", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 5)]
    //    public string dteEnd { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "DTEEXCUTE", DbType = DbType.String, IsPK = false, IsSeq = false, SerNo = 6)]
    //    public string dteExecute { get; set; }
    //    //
    //    [DataMember]
    //    [Entity(FieldName = "STATUS", DbType = DbType.Decimal, IsPK = false, IsSeq = false, SerNo = 7)]
    //    public decimal onOffFlg { get; set; }

    //    /// <summary>
    //    /// EnumCols
    //    /// </summary>
    //    public class EnumCols
    //    {
    //        public string PARAMID = "PARAMID";
    //        public string UPLOADTYPE = "UPLOADTYPE";
    //        public string DTETYPE = "DTETYPE";
    //        public string DTESTART = "DTESTART";
    //        public string DTEEND = "DTEEND";
    //        public string DTEEXCUTE = "DTEEXCUTE";
    //        public string STATUS = "STATUS";
    //        public EnumCols() { }
    //    }
    //}


    //#endregion
}
