using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace FoodChain.Core
{
    public interface IPanelControl<T>
    {
        public void KickStart();
        public IPanelCollection<T> panels { get; }
    }
}