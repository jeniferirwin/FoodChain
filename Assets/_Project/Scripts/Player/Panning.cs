using UnityEngine;
using UnityEngine.InputSystem;

namespace FoodChain
{
    public class Panning : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private CharacterController controller;
        private Vector3 movement;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void OnWASD(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                movement = Vector3.zero;
                return;
            }
            var raw = context.ReadValue<Vector2>();
            var tilted = TiltInput(raw.x, raw.y);
            movement = tilted;
        }
        
        private Vector3 TiltInput(float x, float y)
        {
            // This is some tomfoolery that we have to do in order to
            // make the panning movement feel natural even though
            // we're viewing the board at a 45 degree angle. It all
            // boils down to 'whatever direction we're trying to pan
            // in, tilt the input one cardinal direction clockwise.'
            //
            // For example, to pan north, we actually have the input
            // go northeast. To move west, we have the input go northwest,
            // and so on.
            // 
            // This is the most hideous thing I've ever written, and I
            // feel like there MUST be a better way. But it *works*, and
            // since this isn't a long term project, I'll just... I'll
            // just let it be.
            
            if (x == 0 && y > 0) return new Vector3(y, 0, y).normalized;
            if (x > 0 && y > 0) return new Vector3(x, 0, 0).normalized;
            if (x > 0 && y == 0) return new Vector3(x, 0, -x).normalized;
            if (x > 0 && y < 0) return new Vector3(0, 0, y).normalized;
            if (x == 0 && y < 0) return new Vector3(y, 0, y).normalized;
            if (x < 0 && y < 0) return new Vector3(x, 0, 0).normalized;
            if (x < 0 && y == 0) return new Vector3(x, 0, -x).normalized;
            if (x < 0 && y > 0) return new Vector3(0, 0, y).normalized;
            return Vector3.zero;
        }

        private void Update()
        {
            if (movement == Vector3.zero) return;
            controller.SimpleMove(movement * moveSpeed);
        }
    }
}
