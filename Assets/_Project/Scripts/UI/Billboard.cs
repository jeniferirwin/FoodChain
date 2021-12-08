using UnityEngine;

namespace FoodChain.UI
{
    public class Billboard : MonoBehaviour
    {
        private Camera lookCam;
        
        private void Start()
        {
            lookCam = Camera.main;
            if (lookCam == null) this.enabled = false;
        }
        
        private void LateUpdate()
        {
            transform.LookAt(lookCam.transform);
        }
    }
}