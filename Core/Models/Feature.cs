using System.ComponentModel.DataAnnotations;

namespace angular_vega.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        // translations for multi-language support
        public ICollection<FeatureTranslation> Translations { get; set; }
    }
}