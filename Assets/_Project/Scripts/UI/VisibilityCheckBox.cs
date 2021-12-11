using UnityEngine;
using FoodChain.Life;
using UnityEngine.UI;

namespace FoodChain.UI
{
    public class VisibilityCheckBox : MonoBehaviour
    {
        [SerializeField] private string organismTag;
        [SerializeField] private Toggle toggle;

        public void ShowPanels(bool value)
        {
            var organisms = OrganismDatabase.FindMembersByTag(organismTag);
            foreach (var org in organisms)
            {
                org.ShowPanel(value);
            }
        } 
    }
}