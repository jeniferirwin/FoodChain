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
        [SerializeField] private static ScriptableObject panelControlSO;

        private IHaveAnimalPanel animal;
        private IPanelControl<AnimalPanel> panelControl;

        private void Start()
        {
            animal = transform.GetComponentInParent<IHaveAnimalPanel>();
            panelControl = panelControlSO as IPanelControl<AnimalPanel>;
            if (animal == null)
            {
                this.enabled = false;
                return;
            }

            SubscribeEvents();
            UpdateLifePhase(animal.CurrentLifePhase);
        }
        
        private void SubscribeEvents()
        {
            animal.OnAgeTicked += UpdateLife;
            animal.OnAgeUp += UpdateLifePhase;
            animal.OnReproductionCooldownUpdated += UpdateSpawn;
            animal.OnEnergyUpdated += UpdateEnergy;
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