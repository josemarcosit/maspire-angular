using maspire_angular.Features.Feature;
using System.Collections.ObjectModel;

namespace maspire_angular.Features.Make
{
    public class MakeResource : KeyValuePairResource
    {
        public ICollection<KeyValuePairResource> Models { get; set; }

        public MakeResource()
        {
            Models = new Collection<KeyValuePairResource>();
        }
    }
}