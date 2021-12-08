using System;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public abstract class Organism : MonoBehaviour, ICanBeEaten
    {
        public event Action<PercentPack> OnAgeTicked = delegate { };
        public event Action<int> OnAgeUp = delegate { };
        [SerializeField] [Range(0f, 1f)] protected float energyPercentValue;
        [SerializeField] protected float reproductionCooldown;
        [SerializeField] protected float[] phaseLengths = new float[3];
        [SerializeField] private Vector3[] ageScales = new Vector3[3];
        [SerializeField] private Material[] ageMaterials = new Material[3];
        [SerializeField] private int mainColorSlot;

        protected int _currentPhase;
        protected Ticker _phaseTicker;
        protected Ticker _reproductionTicker;
        protected GameObject _aggressor;
        private MeshRenderer _rend;

        // ENCAPSULATION
        
        public int CurrentLifePhase { get { return _currentPhase; } }
        
        public PercentPack LifePhasePercent
        {
            get
            {
                return new PercentPack(_phaseTicker.Remaining, _phaseTicker.Maximum);
            }
        }

        public float ReproductionCooldown
        {
            get { return reproductionCooldown; }
            set { reproductionCooldown = Helpers.MustBePositive(value); }
        }
        
        public float EnergyPercentValue
        {
            get { return energyPercentValue; }
            set { energyPercentValue = Helpers.MustBePercentage(value); }
        }
        
        public GameObject Aggressor
        {
            get { return _aggressor; }
            protected set
            {
                if (Aggressor == null)
                {
                    _aggressor = value;
                }
            }
        }
        
        protected virtual void Awake()
        {
            _currentPhase = 0;
            _phaseTicker = new Ticker(phaseLengths[_currentPhase]);
            OnAgeTicked?.Invoke(LifePhasePercent);
            OnAgeUp?.Invoke(CurrentLifePhase);
            transform.localScale = ageScales[_currentPhase];
            _aggressor = null;
            _rend = GetComponent<MeshRenderer>();
            OrganismDatabase.AddMember(gameObject);
        }

        protected virtual void Update()
        {
            RunTickers();
            CheckAging();
        }

        // ABSTRACTION
        protected virtual void RunTickers()
        {
            _phaseTicker.Tick();
            OnAgeTicked?.Invoke(LifePhasePercent);
        }

        public virtual void StartBeingEaten(GameObject aggressor)
        {
            Aggressor = aggressor;
        }

        public virtual void FinishBeingEaten() => Die();

        protected virtual void Die()
        {
            OrganismDatabase.RemoveMember(gameObject);
            Destroy(gameObject);
        }

        protected virtual void AgeUp()
        {
            if (_currentPhase < 2)
            {
                _currentPhase++;
                _phaseTicker = new Ticker(phaseLengths[_currentPhase]);
                transform.localScale = ageScales[_currentPhase];
                _rend.materials[mainColorSlot] = ageMaterials[_currentPhase];
                OnAgeUp?.Invoke(CurrentLifePhase);
            }
            else
            {
                Die();
            }
        }

        protected virtual void CheckAging()
        {
            if (_phaseTicker.IsFinished)
            {
                AgeUp();
                return;
            }
        }
    }
}
