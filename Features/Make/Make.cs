using maspire_angular.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;


namespace maspire_angular.Features.Make
{
    public class Make
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public ICollection<Model> Models { get; set; }
        public ICollection<Feature.Feature> Features { get; set; }

        public Make()
        {
            Models = new Collection<Model>();
            Features = new Collection<Feature.Feature>();
        }
    }
}