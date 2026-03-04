using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace maspire_angular.Features.Vehicle
{
    public class SaveVehicleResource
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        [Required]
        public ContactResource Contact { get; set; }
        public bool IsRegistered { get; set; }
        public ICollection<int> Features { get; set; }

        public SaveVehicleResource()
        {
            Features = new Collection<int>();
        }

    }
}