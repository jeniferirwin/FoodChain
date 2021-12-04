using UnityEngine;

namespace FoodChain
{
public interface ICanBeEaten
{
    public float EnergyPercentValue { get; }
    public bool IsBeingEaten { get; }
    public void StartBeingEaten();
    public void FinishBeingEaten();
}
}
