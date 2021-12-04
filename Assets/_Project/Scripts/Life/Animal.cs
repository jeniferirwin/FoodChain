using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class Animal : Organism, ICanFeed
    {
        [SerializeField] protected GameObject offspringPrefab;
        [SerializeField] protected string foodSourceTag;
        [SerializeField] [Range(0, 2)] protected int foodSourcePhasePreference;
        [SerializeField] [Range(0f, 1f)] protected float energyUsePerSecond;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyMinimum;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyUse;
        [SerializeField] [Range(0f, 1f)] protected float foragingEnergyThreshold;

        protected GameObject _target = null;
        protected Ticker _reproductionTicker;
        protected Ticker _energyTicker;
        protected Ticker _forageTicker;
        protected float _currentEnergyLevel;
        protected float _moveSpeed;

        // ENCAPSULATION
        public bool IsFeeding { get; protected set; }

        public float ReproductiveEnergyMinimum
        {
            get { return reproductiveEnergyMinimum; }
            protected set { reproductiveEnergyMinimum = Helpers.MustBePercentage(value); }
        }
        
        public float ReproductiveEnergyUse
        {
            get { return reproductiveEnergyUse; }
            protected set { reproductiveEnergyUse = Helpers.MustBePercentage(value); }
        }

        public float EnergyUsePerSecond
        {
            get { return energyUsePerSecond; }
            protected set { energyUsePerSecond = Helpers.MustBePercentage(value); }
        }
        
        public float ForagingEnergyThreshold
        {
            get { return foragingEnergyThreshold; }
            protected set { foragingEnergyThreshold = Helpers.MustBePercentage(value); }
        }

        protected override void Awake()
        {
            _currentEnergyLevel = 1;
            base.Awake();
            _reproductionTicker = new Ticker(ReproductionCooldown);
            _forageTicker = new Ticker(1);
            _energyTicker = new Ticker(1);
        }
        
        protected override void RunTickers()
        {
            base.RunTickers();
            _energyTicker.Tick();
            if (_currentPhase == 1) _reproductionTicker.Tick();
            if (_currentEnergyLevel < ForagingEnergyThreshold) _forageTicker.Tick();
        }

        protected override void Update()
        {
            base.Update();

            if (_energyTicker.IsFinished)
                UseEnergy();

            if (_reproductionTicker.IsFinished && _currentEnergyLevel > ReproductiveEnergyMinimum)
                Reproduce();

            if (_currentEnergyLevel < ForagingEnergyThreshold && _forageTicker.IsFinished)
                Forage();
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

        protected void UseEnergy()
        {
            _currentEnergyLevel -= EnergyUsePerSecond;
            _energyTicker.Refresh();
            if (_currentEnergyLevel <= 0f)
                Die();
        }

        protected void Forage()
        {
            _target = FindClosestFoodSource();
            _forageTicker.Refresh();
            if (_target == null) return;
            _moveSpeed = Vector3.Distance(_target.transform.position, gameObject.transform.position);
        }

        protected void Reproduce()
        {
            _reproductionTicker.Refresh();
            _currentEnergyLevel -= ReproductiveEnergyUse;
            var pos = transform.position;
            var xOffset = Random.Range(1f,2f);
            var zOffset = Random.Range(1f,2f);
            var offspringPos = new Vector3(pos.x + xOffset, pos.y, pos.z + zOffset);
            GameObject.Instantiate(offspringPrefab,offspringPos,Quaternion.identity);
        }

        protected void OnTriggerStay(Collider other)
        {
            if (other.gameObject != _target) return;
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
