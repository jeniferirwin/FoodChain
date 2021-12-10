using UnityEngine;

namespace FoodChain.Core
{
    public interface IPanelCollection<T>
    {
        public void AddPanel(T panel);
        public void RemovePanel(T panel);
    }
}