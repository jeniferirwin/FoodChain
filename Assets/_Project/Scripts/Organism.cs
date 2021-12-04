using UnityEngine;

namespace FoodChain
{
    public abstract class Organism : MonoBehaviour, ICanBeEaten
    {
        [SerializeField] [Range(0f, 1f)] protected float energyPercentValue;
        [SerializeField] protected float reproductionCooldown;
        [SerializeField] protected float[] phaseLengths = new float[3];
        [SerializeField] private Vector3[] ageScales = new Vector3[3];
        [SerializeField] private Material[] ageMaterials = new Material[3];
        [SerializeField] private int mainColorSlot;

        protected int _currentPhase;
        protected float _phaseTicker;
        protected bool _isBeingEaten;
        private MeshRenderer _rend;

        // ENCAPSULATION

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
        
        public bool IsBeingEaten
        {
            get { return _isBeingEaten; }
            protected set { _isBeingEaten = value; }
        }
        

        protected virtual void Awake()
        {
            _currentPhase = 0;
            _phaseTicker = phaseLengths[_currentPhase];
            transform.localScale = ageScales[_currentPhase];
            IsBeingEaten = false;
            _rend = GetComponent<MeshRenderer>();
        }

        protected virtual void AgeUp()
        {
            if (_currentPhase < 2)
            {
                _currentPhase++;
                _phaseTicker = phaseLengths[_currentPhase];
                transform.localScale = ageScales[_currentPhase];
                _rend.materials[mainColorSlot] = ageMaterials[_currentPhase];
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
