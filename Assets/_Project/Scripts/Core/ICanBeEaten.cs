using UnityEngine;

namespace FoodChain.Core
{
    public interface ICanBeEaten
    {
        public float EnergyPercentValue { get; }
        public bool IsBeingEaten { get; }
        public void StartBeingEaten();
        public void FinishBeingEaten();
    }
}
