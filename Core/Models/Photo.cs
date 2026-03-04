using System.ComponentModel.DataAnnotations;

namespace angular_vega.Core.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
    }
}
