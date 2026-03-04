using System.ComponentModel.DataAnnotations;

namespace maspire_angular.Features.Feature
{
    public class FeatureTranslation
    {
        public int Id { get; set; }

        [Required]
        public int FeatureId { get; set; }

        [Required]
        [StringLength(10)]
        public string Language { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        // navigation
        public Feature Feature { get; set; }
    }
}