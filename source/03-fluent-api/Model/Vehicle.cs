﻿using System.Collections.Generic;

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

        public IShift Shift { get; } = new Shift();
        public string VehicleId { get; }

        public void SetShift(IShift shift)
        {
            var newShift = new Shift(
                shift.Start < this.Shift.Start ? shift.Start : this.Shift.Start,
                shift.End > this.Shift.End ? shift.End : this.Shift.End);
        }
    }

    public class VehicleCollection : HashSet<IVehicle>
    { }
}
