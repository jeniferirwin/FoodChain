using UnityEngine;
using UnityEngine.InputSystem;

namespace FoodChain.Player
{
    public enum Compass
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public class CameraControl : MonoBehaviour
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
            // ABSTRACTION
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
            // For example, to pan north, we actually tilt the input value
            // northeast. To move west, tilt the input northwest, and so on.
            
            Compass dir = GetInputDirection(x,y);

            if (dir == Compass.North || dir == Compass.South)
                return new Vector3(y, 0, y).normalized;

            if (dir == Compass.West || dir == Compass.East)
                return new Vector3(x, 0, -x).normalized;

            if (dir == Compass.NorthEast || dir == Compass.SouthWest)
                return new Vector3(x, 0, 0).normalized;
            
            if (dir == Compass.NorthWest || dir == Compass.SouthEast)
                return new Vector3(0, 0, y).normalized;

            return Vector3.zero;
        }
        
        private Compass GetInputDirection(float x, float y)
        {
            if (x == 0 && y > 0) return Compass.North;
            if (x > 0 && y > 0) return Compass.NorthEast;
            if (x > 0 && y == 0) return Compass.East;
            if (x > 0 && y < 0) return Compass.SouthEast;
            if (x == 0 && y < 0) return Compass.South;
            if (x < 0 && y < 0) return Compass.SouthWest;
            if (x < 0 && y == 0) return Compass.West;
            if (x < 0 && y > 0) return Compass.NorthWest;
            return Compass.North;
        }

        private void Update()
        {
            if (movement == Vector3.zero) return;
            controller.SimpleMove(movement * moveSpeed);
        }
    }
}
