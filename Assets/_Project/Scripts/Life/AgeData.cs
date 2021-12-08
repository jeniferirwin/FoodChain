using System;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class AgeData
    {
        public event Action<int> OnAgeUp = delegate { };
        public event Action<PercentPack> OnAgeTicked = delegate { };
        public event Action OnLifeOver = delegate { };

        public Vector3 CurrentPhaseScale { get { return _phaseScales[_currentPhase]; } }
        public Material CurrentPhaseMaterial { get { return _phaseMaterials[_currentPhase]; } }
        public int CurrentPhase { get { return _currentPhase; } }
        public float CurrentPhaseRemaining { get { return _ticker.Remaining; } }
        public float CurrentPhaseLength { get { return _phaseLengths[_currentPhase]; } }
        public PercentPack PhasePercent { get { return new PercentPack(CurrentPhaseRemaining, CurrentPhaseLength); } }
        public bool IsPhaseFinished { get { return _ticker.IsFinished; } }
        public bool CanBreed { get { return (CurrentPhase == 1); }}

        protected float[] _phaseLengths = new float[3];
        protected Vector3[] _phaseScales = new Vector3[3];
        protected Material[] _phaseMaterials = new Material[3];
        protected int _currentPhase;
        protected Ticker _ticker;

        public AgeData(float[] nPhaseLengths, Vector3[] nPhaseScales, Material[] nPhaseMaterials)
        {
            _currentPhase = 0;
            _phaseLengths = nPhaseLengths;
            _phaseScales = nPhaseScales;
            _phaseMaterials = nPhaseMaterials;
            Refresh();
        }
        
        public void SubscribeEvents()
        {
            _ticker.OnFinished += AgeUp;
            _ticker.OnTick += BubbleTick;
        }
        
        public void BubbleTick() => OnAgeTicked(new PercentPack(CurrentPhaseRemaining, CurrentPhaseLength));
        public void Tick() => _ticker.Tick();
        
        public void Cleanup()
        {
            _ticker.Cleanup();
            OnAgeUp = null;
            OnAgeTicked = null;
            OnLifeOver = null;
        }

        private void Refresh()
        {
            if (_ticker != null) _ticker.Cleanup();
            if (_currentPhase >= 3)
            {
                Cleanup();
                return;
            }
            _ticker = new Ticker(_phaseLengths[_currentPhase]);
            _ticker.OnFinished += AgeUp;
            _ticker.OnTick += BubbleTick;
        }
        
        protected void AgeUp()
        {
            _currentPhase++;
            OnAgeUp(_currentPhase);
            Refresh();
        }
    }
}