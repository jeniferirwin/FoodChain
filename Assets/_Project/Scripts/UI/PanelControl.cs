using System.Collections.Generic;
using UnityEngine;
using FoodChain.Core;

namespace FoodChain.UI
{
    [CreateAssetMenu(menuName = "FoodChain/PanelControl")]
    public class PanelControl<T> : ScriptableObject
    {
        private static PanelCollection<IPanelControl<T>> panels;
        
        public void KickStart()
        {
            OnEnable();
        }

        private static void OnEnable()
        {
            panels = new PanelCollection<IPanelControl<T>>();
        }
        
        private static void Cleanup()
        {
            OnEnable();
        }
        
        public void TogglePanels(bool value)
        {
            if (value) panels.HidePanels();
            else panels.ShowPanels();
        }
    }

    public class PanelCollection<T> : IPanelCollection<T>
    {
        private List<T> panels = new List<T>();

        public void AddPanel(T panel)
        {
            if (panels.Contains(panel)) return;
            panels.Add(panel);
        }
        
        public void RemovePanel(T panel)
        {
            if (panels.Contains(panel)) panels.Remove(panel);
        }
        
        public void HidePanels()
        {
            foreach (var panel in panels)
            {
                if (panel.GetType() == typeof(GrassPanel))
                {
                    var grassPanel = panel as GrassPanel;
                    grassPanel.enabled = false;
                }
                else
                if (panel.GetType() == typeof(AnimalPanel))
                {
                    var animalPanel = panel as AnimalPanel;
                    animalPanel.enabled = false;
                }
            }
        }

        public void ShowPanels()
        {
            foreach (var panel in panels)
            {
                if (panel.GetType() == typeof(GrassPanel))
                {
                    var grassPanel = panel as GrassPanel;
                    grassPanel.enabled = true;
                }
                else
                if (panel.GetType() == typeof(AnimalPanel))
                {
                    var animalPanel = panel as AnimalPanel;
                    animalPanel.enabled = true;
                }
            }
        }
    }
}