using System;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.Life
{
    public class ReproductiveData
    {
        public event Action<PercentPack> OnCooldownUpdate = delegate { };

        public GameObject OffspringPrefab { get { return _offspringPrefab; } }
        public float SpawnCooldown { get { return _spawnCooldown; } }
        public float SpawnEnergyMinimum { get { return _spawnEnergyMin; } }
        public float SpawnEnergyCost { get { return _spawnEnergyCost; } }
        public float SpawnCooldownRemaining { get { return _ticker.Remaining; } }
        public bool CanSpawn { get { return _ticker.IsFinished; } }

        private GameObject _offspringPrefab;
        private float _spawnCooldown;
        private float _spawnEnergyMin;
        private float _spawnEnergyCost;
        
        private Ticker _ticker;
        
        public ReproductiveData(GameObject prefab, float cooldown, float min, float cost)
        {
            _offspringPrefab = prefab;
            _spawnCooldown = Helpers.MustBePositive(cooldown);
            _spawnEnergyMin = Helpers.MustBePositivePercentage(min);
            _spawnEnergyCost = Helpers.MustBePositivePercentage(cost);
            _ticker = new Ticker(SpawnCooldown);
        }
        
        public void Tick()
        {
            if (_ticker.IsFinished) return;
            _ticker.Tick();
            OnCooldownUpdate(new PercentPack(SpawnCooldownRemaining, SpawnCooldown));
        }
        
        public void Refresh()
        {
            _ticker.Refresh();
            OnCooldownUpdate(new PercentPack(SpawnCooldownRemaining, SpawnCooldown));
        }
    }
}