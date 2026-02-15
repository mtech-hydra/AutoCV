using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoCV.Web.Data
{
    [Table("errand")]
    public class JobAdEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = "";

        [Column("status")]
        public string Status { get; set; } = "";

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
