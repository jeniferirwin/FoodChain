using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class OrganismData
    {
        public float EnergyPercentValue { get { return _energyPercentValue; } }
        public int MainColorSlot { get { return _mainColorSlot; } }

        protected float _energyPercentValue;
        protected int _mainColorSlot;

        public OrganismData(float nEPV, int nMainColorSlot)
        {
            _energyPercentValue = Helpers.MustBePositivePercentage(nEPV);
            _mainColorSlot = Helpers.MustBePositive(nMainColorSlot);
        }
    }
}