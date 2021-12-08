using System;
namespace FoodChain.Core
{
    public interface IHaveGrassPanel
    {
        public event Action<PercentPack> OnAgeTicked;
        public event Action<int> OnAgeUp;
        public PercentPack LifePhasePercent { get; }
        public int CurrentLifePhase { get; }
    }
}