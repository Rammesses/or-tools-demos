using System;
using _03_fluent_api.Model;

namespace _03_fluent_api.Services
{
    public interface IDistanceCalculator
    {
        int GetDistanceBetween(ILocation start, ILocation end);
    }
}
