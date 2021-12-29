using UnityEngine;

namespace FoodChain.Core
{
    public class GameManager : MonoBehaviour
    {
        public static bool PlantPanelsShowing { get; set; }
        public static bool HerbivorePanelsShowing { get; set; }
        public static bool CarnivorePanelsShowing { get; set; }
        
        public ScriptableObject wolfOrgContainer;
        public ScriptableObject wolfAniContainer;
        
        private void Awake()
        {
            PlantPanelsShowing = true;
            HerbivorePanelsShowing = true;
            CarnivorePanelsShowing = true;
        }
    }
}