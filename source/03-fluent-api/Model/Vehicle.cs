using System.Collections.Generic;

namespace _03_fluent_api.Model
{
    public interface IVehicle
    {
        string VehicleId { get; }
        IShift Shift { get; }        
    }

    public class Vehicle : IVehicle
    {
        public Vehicle(string vehicleId)
        {
            VehicleId = vehicleId;
        }

        public IShift Shift { get; private set; } = new Shift();
        public string VehicleId { get; }

        internal void SetShift(IShift shift)
        {
            this.Shift = shift;
        }

        public override string ToString() => $"Vehicle #{this.VehicleId} ({this.Shift})";
    }

    public class VehicleCollection : HashSet<IVehicle>
    { }
}
