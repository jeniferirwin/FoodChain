using UnityEngine;
using System.Collections.Generic;

namespace FoodChain
{
    public class Animal : Organism, ICanFeed
    {
        [SerializeField] protected GameObject offspringPrefab;
        [SerializeField] protected string foodSourceTag;
        [SerializeField] [Range(0f, 1f)] protected float energyUsePerSecond;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyMinimum;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyUse;
        [SerializeField] [Range(0f, 1f)] protected float foragingEnergyThreshold;
        [SerializeField] protected float reproductionCooldown;

        protected GameObject _target = null;
        protected float _currentEnergyLevel;
        protected float _energyTicker;
        protected float _forageTicker;
        protected float _reproductionTicker;
        protected float _moveSpeed;
        protected bool _canReproduce;

        // ENCAPSULATION
        public bool IsFeeding { get; protected set; }

        public float ReproductiveEnergyMinimum
        {
            get { return reproductiveEnergyMinimum; }
            protected set { reproductiveEnergyMinimum = MustBePercentage(value); }
        }
        
        public float ReproductiveEnergyUse
        {
            get { return reproductiveEnergyUse; }
            protected set { reproductiveEnergyUse = MustBePercentage(value); }
        }

        public float EnergyUsePerSecond
        {
            get { return energyUsePerSecond; }
            protected set { energyUsePerSecond = MustBePercentage(value); }
        }
        
        public float ForagingEnergyThreshold
        {
            get { return foragingEnergyThreshold; }
            protected set { foragingEnergyThreshold = MustBePercentage(value); }
        }
        

        
        protected override void Update()
        {
            base.Update();
            if (EnergyTick()) UseEnergy();
            if (ReproductionTick()) _canReproduce = true;
            if (_canReproduce && _currentEnergyLevel > ReproductiveEnergyMinimum)
            {
                Reproduce();
            }
            if (_currentEnergyLevel < ForagingEnergyThreshold)
            {
                if (ForageTick())
                {
                    Forage();
                }
            }
        }
        
        protected virtual void Reproduce()
        {
            _canReproduce = false;
            _currentEnergyLevel -= ReproductiveEnergyUse;
            var pos = transform.position;
            var xOffset = Random.Range(1f,2f);
            var zOffset = Random.Range(1f,2f);
            var offspringPos = new Vector3(pos.x + xOffset, pos.y, pos.z + zOffset);
            GameObject.Instantiate(offspringPrefab,offspringPos,Quaternion.identity);
        }

        protected override void Awake()
        {
            _currentEnergyLevel = 1;
            base.Awake();
        }
        
        protected void FixedUpdate()
        {
            if (_target != null)
            {
                transform.LookAt(_target.transform);
                var direction = (_target.transform.position - transform.position).normalized;
                transform.Translate(direction * _moveSpeed * Time.fixedDeltaTime, Space.World);
            }
        }

        protected virtual bool EnergyTick()
        {
            if (_energyTicker > 0f)
            {
                _energyTicker -= Time.deltaTime;
                return false;
            }
            _energyTicker = 1f;
            return true;
        }
        
        protected virtual bool ForageTick()
        {
            if (_forageTicker > 0f)
            {
                _forageTicker -= Time.deltaTime;
                return false;
            }
            _forageTicker = 1f;
            return true;
        }

        protected virtual bool ReproductionTick()
        {
            if (_canReproduce) return false;
            if (_currentPhase != 1) return false;
            if (_reproductionTicker > 0f)
            {
                _reproductionTicker -= Time.deltaTime;
                return false;
            }
            _reproductionTicker = reproductionCooldown;
            return true;
        }

        protected virtual void UseEnergy()
        {
            _currentEnergyLevel -= EnergyUsePerSecond;
            Debug.Log($"{_currentEnergyLevel}");
            if (_currentEnergyLevel <= 0f)
            {
                Die();
            }
        }

        protected virtual void Forage()
        {
            _target = FindClosestFoodSource();
            if (_target == null) return;
            _moveSpeed = Vector3.Distance(_target.transform.position, gameObject.transform.position);
        }

        protected void OnTriggerStay(Collider other)
        {
            if (other.gameObject != _target) return;
            Debug.Log($"{gameObject.name} is trying to eat...");
            var org = other.gameObject.GetComponent<ICanBeEaten>();
            var energy = org.EnergyPercentValue;
            _currentEnergyLevel += energy;
            org.FinishBeingEaten();
            _target = null;
        }

        protected GameObject FindClosestFoodSource()
        {
            var sources = GameObject.FindGameObjectsWithTag(foodSourceTag);
            if (sources.Length == 0) return null;
            GameObject closest = null;
            foreach (var source in sources)
            {
                ICanBeEaten org;
                if (!source.TryGetComponent<ICanBeEaten>(out org)) continue;
                if (org.IsBeingEaten) continue;
                var distance = Vector3.Distance(transform.position, source.transform.position);
                if (closest == null)
                {
                    closest = source;
                }
                else
                {
                    if (distance < Vector3.Distance(transform.position, closest.transform.position))
                    {
                        closest = source;
                    }
                }
            }
            return closest;
        }
    }
}
