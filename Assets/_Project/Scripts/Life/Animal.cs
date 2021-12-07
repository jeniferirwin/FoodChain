using System.Collections.Generic;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class Animal : Organism, ICanFeed
    {
        [SerializeField] protected GameObject offspringPrefab;
        [SerializeField] protected string foodSourceTag;
        [SerializeField] [Range(0, 2)] protected int[] foodSourcePhasePreference;
        [SerializeField] [Range(0f, 1f)] protected float energyUsePerSecond;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyMinimum;
        [SerializeField] [Range(0f, 1f)] protected float reproductiveEnergyUse;
        [SerializeField] [Range(0f, 1f)] protected float foragingEnergyThreshold;

        protected Organism _target = null;
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
            if (_target.Aggressor != null && _target.Aggressor != this.gameObject)
            {
                Debug.Log($"Entity {gameObject.name} has stopped foraging. Target already belongs to {_target.Aggressor.gameObject.name}.");
                _target = null;
                return;
            }
            _target.StartBeingEaten(gameObject);
            _moveSpeed = Vector3.Distance(_target.transform.position, gameObject.transform.position);
        }

        protected void Reproduce()
        {
            _reproductionTicker.Refresh();
            _currentEnergyLevel -= ReproductiveEnergyUse;
            var pos = transform.position;
            var xOffset = Random.Range(1f, 2f);
            var zOffset = Random.Range(1f, 2f);
            var offspringPos = new Vector3(pos.x + xOffset, pos.y, pos.z + zOffset);
            GameObject.Instantiate(offspringPrefab, offspringPos, Quaternion.identity);
        }

        protected void OnTriggerStay(Collider other)
        {
            if (_target == null) return;
            if (other.gameObject != _target.gameObject) return;
            var org = other.gameObject.GetComponent<ICanBeEaten>();
            var energy = org.EnergyPercentValue;
            _currentEnergyLevel += energy;
            org.FinishBeingEaten();
            _target = null;
        }

        protected Organism FindClosestFoodSource()
        {
            for (int i = 0; i < 3; i++)
            {
                var sources = OrganismDatabase.FindAvailableMembersByPhase(foodSourceTag, foodSourcePhasePreference[i]);
                if (sources.Count > 0)
                {
                    var source = FindClosestFoodSourceInList(sources);
                    Debug.Log($"{this.gameObject.name} wants to eat {source.gameObject.name}...");
                    return source;
                }
            }
            return null;
        }

        protected Organism FindClosestFoodSourceInList(List<Organism> sources)
        {
            Organism closest = null;
            foreach (var source in sources)
            {
                if (closest == null)
                {
                    closest = source;
                    continue;
                }
                var currentDist = Vector3.Distance(gameObject.transform.position, source.gameObject.transform.position);
                var closestDist = Vector3.Distance(gameObject.transform.position, closest.gameObject.transform.position);
                if (currentDist < closestDist)
                {
                    closest = source;
                }
            }
            if (closest == null) return null;
            return closest;
        }
    }
}
