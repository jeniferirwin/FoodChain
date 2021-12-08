using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FoodChain.UI
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private TMP_Text text;
        
        public void UpdateLifePhase(int phase)
        {
            if (phase == 0) text.text = "Juvenile".ToUpper();
            else if (phase == 1) text.text = "Mature".ToUpper();
            else if (phase == 2) text.text = "Elder".ToUpper();
        }

        public void UpdateFill(float current, float max)
        {
            bar.fillAmount = current / max;
        }
    }
}