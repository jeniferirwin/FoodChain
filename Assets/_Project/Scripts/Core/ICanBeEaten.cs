using UnityEngine;

namespace FoodChain.Core
{
    public interface ICanBeEaten
    {
        public int CurrentLifePhase { get; }
        public float EnergyPercentValue { get; }
        public GameObject Aggressor { get; set; }
        public void Die();
    }
}
