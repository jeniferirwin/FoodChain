using System;
using UnityEngine;

namespace FoodChain.Core
{
    public class Ticker
    {
        public event Action OnFinished = delegate { };
        public event Action OnTick = delegate { };

        private float _maximum;
        private float _remaining;
        private bool _finished;

        public bool IsFinished { get { return _finished; } }
        public float Maximum { get { return _maximum; } set { _maximum = value; } }
        public float Remaining { get { return _remaining; } }

        public Ticker(float value)
        {
            _maximum = value;    
            _remaining = value;
        }

        public void Refresh()
        {
            _remaining = _maximum;
            _finished = false;
        }
        
        public void Tick()
        {
            if (IsFinished) return;
            if (_remaining <= 0f)
            {
                _finished = true;
                OnFinished();
                return;
            }
            _remaining -= Time.deltaTime;
            OnTick();
        }
        
        public void Cleanup()
        {
            OnFinished = null;
            OnTick = null;           
        }
    }
}