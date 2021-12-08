using System;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class ForagerData
    {
        public event Action<PercentPack> OnEnergyChanged = delegate { };
        public event Action Starved = delegate { };

        public float CurrentEnergy { get { return _currentEnergyPercent; } }
        public float EnergyCostPerSecond { get { return _energyCostPerSecond; } }
        public float HungerEnergyThreshold { get { return _hungerEnergyThreshold; } }
        public bool IsHungry { get { return (CurrentEnergy < HungerEnergyThreshold && _ticker.IsFinished); } }
        public string PreyTag { get { return _preyTag; } }
        public int[] PhasePreference { get { return _preyPhasePreference; } }
        public Organism Target { get; set; }

        private float _currentEnergyPercent;
        private float _energyCostPerSecond;
        private float _hungerEnergyThreshold;
        private string _preyTag;
        private int[] _preyPhasePreference;
        
        private Ticker _ticker;
        
        public ForagerData(float nEPS, float nHungerThreshold, string nPreyTag, int[] nPhasePref)
        {
            _currentEnergyPercent = 1f;
            _energyCostPerSecond = Helpers.MustBePositivePercentage(nEPS);
            _hungerEnergyThreshold = Helpers.MustBePositivePercentage(nHungerThreshold);
            _preyTag = nPreyTag;
            _preyPhasePreference = nPhasePref;
            Refresh();
            _ticker.OnTick += UpdateEnergy;
        }
        
        public void AddEnergy(float value)
        {
            _currentEnergyPercent += Helpers.MustBePositivePercentage(value);
            OnEnergyChanged(new PercentPack(CurrentEnergy, 1));
        }
        
        public void SubtractEnergy(float value)
        {
            _currentEnergyPercent -= Helpers.MustBePositivePercentage(value);
            OnEnergyChanged(new PercentPack(CurrentEnergy, 1));
            if (_currentEnergyPercent <= 0f) Starved();
        }
        
        public void Cleanup()
        {
            _ticker.Cleanup();
            OnEnergyChanged = null;
        }
        
        public void UpdateEnergy()
        {
            OnEnergyChanged(new PercentPack(CurrentEnergy, 1));
        }

        public void Refresh()
        {
            if (_ticker == null)
            {
                _ticker = new Ticker(1);
                return;
            }
            SubtractEnergy(EnergyCostPerSecond);
            _ticker.Refresh();
        }

        public void Tick()
        {
            if (_ticker.IsFinished) Refresh();
            _ticker.Tick();
        }
    }
}