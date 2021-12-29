using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FoodChain.UI
{
    public class TweakMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField grassReproInput;
        [SerializeField] private TMP_InputField deerReproInput;
        [SerializeField] private TMP_InputField wolfReproInput;

        [SerializeField] private TMP_InputField grassJuvInput;
        [SerializeField] private TMP_InputField deerJuvInput;
        [SerializeField] private TMP_InputField wolfJuvInput;

        [SerializeField] private TMP_InputField grassMatureInput;
        [SerializeField] private TMP_InputField deerMatureInput;
        [SerializeField] private TMP_InputField wolfMatureInput;

        [SerializeField] private TMP_InputField grassElderInput;
        [SerializeField] private TMP_InputField deerElderInput;
        [SerializeField] private TMP_InputField wolfElderInput;

        [SerializeField] private TMP_InputField grassEPVInput;
        [SerializeField] private TMP_InputField deerEPVInput;
        
        [SerializeField] private TMP_InputField deerEPSInput;
        [SerializeField] private TMP_InputField wolfEPSInput;

        [SerializeField] private TMP_InputField deerMinEnergyReproInput;
        [SerializeField] private TMP_InputField wolfMinEnergyReproInput;

        [SerializeField] private TMP_InputField deerReproCostInput;
        [SerializeField] private TMP_InputField wolfReproCostInput;

        [SerializeField] private TMP_InputField deerForageInput;
        [SerializeField] private TMP_InputField wolfForageInput;
        
        public void SaveUserValues()
        {

        }
    }
}
