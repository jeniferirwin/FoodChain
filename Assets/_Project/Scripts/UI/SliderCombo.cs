using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FoodChain.UI
{
    public class SliderCombo : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;
        
        private void Start()
        {
            OnSliderChanged();
        }

        public void OnSliderChanged()
        {
            text.text = PercentString(slider.value);
        }
        
        private string PercentString(float value)
        {
            return $"{value * 100}%";
        }
    }
}