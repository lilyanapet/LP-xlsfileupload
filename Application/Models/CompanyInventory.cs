using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace App_Plugins.XlsFileUpload.Application.Models
{
    [TableName("CompanyInventory")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class CompanyInventory
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("CompanyClient")]
        public string CompanyClient { get; set; }
        [Column("CompanyName")]
        public string CompanyName { get; set; }
        [Column("IsOn")]
        public string IsOn { get; set; }
        [Column("BC")]
        public string BC { get; set; }
        [Column("Availability")]
        public string Availability { get; set; }
        [Column("CorporationStatus")]
        public string CorporationStatus { get; set; }
        [Column("DateOfIncorporation")]
        public string DateOfIncorporation { get; set; }
        [Column("Month")]
        public string Month { get; set; }
        [Column("FirstOrSecondHalf")]
        public int FirstOrSecondHalf { get; set; }
        [Column("PotentialStrikeOffDate")]
        public string PotentialStrikeOffDate { get; set; }
        [Column("ReservedDate")]
        public string ReservedDate { get; set; }
        [Column("ReservedBy")]
        public string ReservedBy { get; set; }
        [Column("SoldDate")]
        public string SoldDate { get; set; }
        [Column("SoldBy")]
        public string SoldBy { get; set; }
        [Column("CompanyStruckOffDate")]
        public string CompanyStruckOffDate { get; set; }
        [Column("Note")]
        public string Note { get; set; }

        [ForeignKey(typeof(CompanyInvetoryAudit), Name = "AuditId_CompanyInventory")]
        [IndexAttribute(IndexTypes.NonClustered, Name = "AuditId_CompanyInventory")]
        public virtual int AuditId { get; set; }


    }
}
