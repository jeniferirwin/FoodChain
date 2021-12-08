using System;
using System.Collections.Generic;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class Animal : Organism, ICanFeed, IHaveAnimalPanel
    {
        public event Action<PercentPack> OnEnergyUpdated = delegate {};
        public event Action<PercentPack> OnReproductionCooldownUpdated = delegate {};

        [SerializeField] protected AnimalTemplate animalTemplate;

        public ForagerData Foraging { get { return _forageData; } }
        public ReproductiveData Reproduction { get { return _repData; } }
        protected ForagerData _forageData;
        protected ReproductiveData _repData;
        protected float _moveSpeed;

        public GameObject TargetObject { get { return Target.gameObject; } }

        public Organism Target
        { 
            get { return Foraging.Target; }
            set { Foraging.Target = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            AnimalTemplate _tmp = animalTemplate;
            _forageData = new ForagerData(_tmp.energyCostPerSecond, _tmp.hungerEnergyThreshold, _tmp.foodSourceTag, _tmp.foodSourcePhasePreference);
            _repData = new ReproductiveData(_tmp.offspringPrefab, _tmp.reproductionCooldown, _tmp.reproductiveEnergyMinimum, _tmp.reproductiveEnergyCost);
            Foraging.OnEnergyChanged += EnergyChanged;
            Reproduction.OnCooldownUpdate += ReproductionCooldownUpdated;
            Foraging.Starved += Die;
        }
        
        protected override void Update()
        {
            RunTickers();
            if (Foraging.IsHungry)
            {
                Forage();
                return;
            }
            if (Reproduction.CanSpawn && Foraging.CurrentEnergy > Reproduction.SpawnEnergyMinimum)
            {
                Reproduce();
            }
        }
        
        protected override void RunTickers()
        {
            base.RunTickers();
            Foraging.Tick();
            if (Age.CanBreed) Reproduction.Tick();
        }
        
        protected void FixedUpdate()
        {
            if (Target != null)
            {
                transform.LookAt(Target.transform);
                var direction = (Target.transform.position - transform.position).normalized;
                transform.Translate(direction * _moveSpeed * Time.fixedDeltaTime, Space.World);
            }
        }

        protected void Forage()
        {
            Target = FindClosestFoodSource();
            Foraging.Refresh();
            if (Target == null) return;
            if (Target.Aggressor != null && Target.Aggressor != this.gameObject)
            {
                Target = null;
                return;
            }
            Target.Aggressor = gameObject;
            _moveSpeed = Vector3.Distance(Target.transform.position, gameObject.transform.position);
        }

        protected void Reproduce()
        {
            var pos = transform.position;
            var xOffset = UnityEngine.Random.Range(1f, 2f);
            var zOffset = UnityEngine.Random.Range(1f, 2f);
            var offspringPos = new Vector3(pos.x + xOffset, pos.y, pos.z + zOffset);
            GameObject.Instantiate(Reproduction.OffspringPrefab, offspringPos, Quaternion.identity);
            Foraging.SubtractEnergy(Reproduction.SpawnEnergyCost);
            Reproduction.Refresh();
        }
        
        public void EnergyChanged(PercentPack pack) => OnEnergyUpdated(pack);
        public void ReproductionCooldownUpdated(PercentPack pack) => OnReproductionCooldownUpdated(pack);

        protected void OnTriggerStay(Collider other)
        {
            if (Target == null) return;
            if (other.gameObject != Target.gameObject) return;
            var org = other.gameObject.GetComponent<ICanBeEaten>();
            var energy = org.EnergyPercentValue;
            Foraging.AddEnergy(energy);
            org.Die();
            Target = null;
        }

        protected Organism FindClosestFoodSource()
        {
            for (int i = 0; i < 3; i++)
            {
                var sources = OrganismDatabase.FindAvailableMembersByPhase(Foraging.PreyTag, Foraging.PhasePreference[i]);
                if (sources.Count > 0)
                    return FindClosestFoodSourceInList(sources);
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
