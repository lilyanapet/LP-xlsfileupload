using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace App_Plugins.XlsFileUpload.Application.Models
{
    [TableName("CompanyInvetoryAudit")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class CompanyInvetoryAudit
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        //this is for audit 
        [Column("ImportedDate")]
        public string ImportedDate { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("UserName")]
        public string UserName { get; set; }

        [Column("RecordsCount")]
        public int RecordsCount { get; set; }
    }
}
