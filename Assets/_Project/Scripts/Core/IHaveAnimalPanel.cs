using System;
using UnityEngine;

namespace FoodChain.Core
{
    public interface IHaveAnimalPanel
    {
        public int CurrentLifePhase { get; }

        public event Action<PercentPack> OnEnergyUpdated;
        public event Action<PercentPack> OnReproductionCooldownUpdated;
        public event Action<PercentPack> OnAgeTicked;
        public event Action<int> OnAgeUp;
    }
}
