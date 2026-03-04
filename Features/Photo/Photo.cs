using System.ComponentModel.DataAnnotations;

namespace maspire_angular.Features.Photo
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
