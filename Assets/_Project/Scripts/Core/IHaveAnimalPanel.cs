using System;
using UnityEngine;

namespace FoodChain.Core
{
    public interface IHaveAnimalPanel
    {
        public PercentPack EnergyPercent { get; }
        public PercentPack ReproductionPercent { get; }
        public PercentPack LifePhasePercent { get; }
        public int CurrentLifePhase { get; }

        public event Action<PercentPack> OnEnergyChanged;
        public event Action<PercentPack> OnReproductionTimerChanged;
        public event Action<PercentPack> OnAgeTicked;
        public event Action<int> OnAgeUp;
    }
}
