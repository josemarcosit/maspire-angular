using System.ComponentModel.DataAnnotations.Schema;

namespace maspire_angular.Features.Vehicle
{
    [Table("VehicleFeatures")]
    public class VehicleFeature
    {
        public int VehicleId { get; set; }
        public int FeatureId { get; set; }
        public Vehicle Vehicle { get; set; }
        public Feature.Feature Feature { get; set; }
    }
}