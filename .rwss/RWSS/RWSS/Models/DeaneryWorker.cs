using RWSS.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWSS.Models
{
    public class DeaneryWorker
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public PositionCategory PositionCategory { get; set; }
    }
}
