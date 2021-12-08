using UnityEngine;
using UnityEngine.UI;
using FoodChain.Core;

namespace FoodChain.UI
{
    public class AnimalPanel : MonoBehaviour
    {
        [SerializeField] private FillBar energyBar;
        [SerializeField] private FillBar spawnBar;
        [SerializeField] private FillBar lifeBar;

        private IHaveAnimalPanel animal;

        private void OnEnable()
        {
            animal = transform.GetComponentInParent<IHaveAnimalPanel>();
            if (animal == null)
            {
                this.enabled = false;
                return;
            }

            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            animal.OnAgeTicked += UpdateLife;
            animal.OnAgeUp += UpdateLifePhase;
            animal.OnReproductionTimerChanged += UpdateSpawn;
            animal.OnEnergyChanged += UpdateEnergy;
        }

        private void UnsubscribeEvents()
        {
            animal.OnAgeTicked -= UpdateLife;
            animal.OnAgeUp -= UpdateLifePhase;
            animal.OnReproductionTimerChanged -= UpdateSpawn;
            animal.OnEnergyChanged -= UpdateEnergy;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void UpdateLifePhase(int phase)
        {
            lifeBar.UpdateLifePhase(animal.CurrentLifePhase);
        }

        private void UpdateEnergy(PercentPack energy)
        {
            energyBar.UpdateFill(energy.current, energy.max);
        }

        private void UpdateSpawn(PercentPack spawn)
        {
            spawnBar.UpdateFill(spawn.current, spawn.max);
        }

        private void UpdateLife(PercentPack life)
        {
            lifeBar.UpdateFill(life.current, life.max);
        }
    }
}