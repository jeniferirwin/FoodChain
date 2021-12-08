using UnityEngine;
using FoodChain.Core;

namespace FoodChain.UI
{
    public class GrassPanel : MonoBehaviour
    {
        [SerializeField] private FillBar lifeBar;
        
        private IHaveGrassPanel grass;

        private void Start()
        {
            grass = transform.GetComponentInParent<IHaveGrassPanel>();
            if (grass != null) SubscribeEvents();
            UpdateLifePhase(grass.CurrentLifePhase);
        }
        
        private void SubscribeEvents()
        {
            grass.OnAgeTicked += UpdateLife;
            grass.OnAgeUp += UpdateLifePhase;
        }
        
        public void UpdateLife(PercentPack life)
        {
            lifeBar.UpdateFill(life.current, life.max);
        }

        private void UpdateLifePhase(int phase)
        {
            lifeBar.UpdateLifePhase(grass.CurrentLifePhase);
        }
    }
}