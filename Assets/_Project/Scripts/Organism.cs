using UnityEngine;

namespace FoodChain
{
    public abstract class Organism : MonoBehaviour, ICanBeEaten
    {
        [SerializeField] protected float[] phaseLengths = new float[3];
        [SerializeField] protected float spawnRate;
        [SerializeField] [Range(0f, 1f)] protected float energyPercentValue;

        protected int _currentPhase;
        protected float _phaseTicker;
        protected bool _isBeingEaten;

        // ENCAPSULATION

        public float SpawnRate
        {
            get { return spawnRate; }
            set { spawnRate = MustBePositive(value); }
        }
        
        public float EnergyPercentValue
        {
            get { return energyPercentValue; }
            set { energyPercentValue = MustBePercentage(value); }
        }
        
        public bool IsBeingEaten
        {
            get { return _isBeingEaten; }
            protected set { _isBeingEaten = value; }
        }
        
        protected static float MustBePositive(float value)
        {
            if (value > 0f)
                return value;
            else
                return 0f;
        }

        protected float MustBePercentage(float value)
        {
            if (value > 1f) return 1f;
            if (value < 0f) return 0f;
            return value;
        }

        protected virtual void AgeUp()
        {
            if (_currentPhase < 2)
            {
                _currentPhase++;
                _phaseTicker = phaseLengths[_currentPhase];
            }
            else
            {
                Die();
            }
        }

        public virtual void StartBeingEaten()
        {
            IsBeingEaten = true;
        }
        
        public virtual void FinishBeingEaten()
        {
            Die();
        }
        
        
        protected virtual void Die()
        {
            Destroy(gameObject);
        }
        
        protected virtual void Awake()
        {
            _phaseTicker = phaseLengths[_currentPhase];
            IsBeingEaten = false;
        }
        
        // ABSTRACTION
        protected virtual void HandleAging()
        {
            if (_phaseTicker > 0)
            {
                _phaseTicker -= Time.deltaTime;
            }
            else
            {
                AgeUp();
            }
        }
        
        protected virtual void Update()
        {
            HandleAging();
        }
    }
}
