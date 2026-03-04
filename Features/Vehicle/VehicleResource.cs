using maspire_angular.Features.Feature;
using maspire_angular.Features.Photo;
using System.Collections.ObjectModel;

namespace maspire_angular.Features.Vehicle
{
    public class VehicleResource
    {
        public int Id { get; set; }
        public KeyValuePairResource Model { get; set; }
        public KeyValuePairResource Make { get; set; }
        public bool IsRegistered { get; set; }
        public ContactResource Contact { get; set; }
        public DateTime lastUpdate { get; set; }
        public ICollection<KeyValuePairResource> Features { get; set; }
        public ICollection<PhotoResource> Photos { get; set; }
        public VehicleResource()
        {
            Features = new Collection<KeyValuePairResource>();
            Photos = new Collection<PhotoResource>();
        }
    }
}