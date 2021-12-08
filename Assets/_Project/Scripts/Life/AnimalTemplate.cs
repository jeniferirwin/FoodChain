using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    [CreateAssetMenu(menuName = "FoodChain/AnimalTemplate")]
    public class AnimalTemplate : ScriptableObject
    {
        public GameObject offspringPrefab;
        public float reproductionCooldown;
        public string foodSourceTag;
        [Range(0, 2)] public int[] foodSourcePhasePreference = new int[3];
        [Range(0f, 1f)] public float energyCostPerSecond;
        [Range(0f, 1f)] public float reproductiveEnergyMinimum;
        [Range(0f, 1f)] public float reproductiveEnergyCost;
        [Range(0f, 1f)] public float hungerEnergyThreshold;
    }
}