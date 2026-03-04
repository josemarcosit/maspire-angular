using System.ComponentModel.DataAnnotations;

namespace maspire_angular.Features.Feature
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public ICollection<FeatureTranslation> Translations { get; set; }
    }
}