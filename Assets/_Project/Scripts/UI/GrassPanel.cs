using UnityEngine;
using FoodChain.Core;

namespace FoodChain.UI
{
    public class GrassPanel : MonoBehaviour
    {
        [SerializeField] private FillBar lifeBar;
        
        private IHaveGrassPanel grass;

        private void OnEnable()
        {
            grass = transform.GetComponentInParent<IHaveGrassPanel>();
            if (grass != null)
            {
                SubscribeEvents();
            }
        }
        
        private void SubscribeEvents()
        {
            grass.OnAgeTicked += UpdateLife;
            grass.OnAgeUp += UpdateLifePhase;
        }
        
        private void UnsubscribeEvents()
        {
            grass.OnAgeTicked -= UpdateLife;
            grass.OnAgeUp -= UpdateLifePhase;
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void UpdateLife(PercentPack life)
        {
            lifeBar.UpdateFill(life.current, life.max);
        }

        private void UpdateLifePhase(int phase)
        {
            lifeBar.UpdateLifePhase(grass.CurrentLifePhase);
            Debug.Log($"Grass updated with phase {grass.CurrentLifePhase}");
        }
    }
}