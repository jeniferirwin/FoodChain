using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
        [SerializeField] private float zoomSpeed;

        private CharacterController controller;
        private Vector3 movement;
        private CinemachineVirtualCamera vCam;
        private float zoomDir;
        private bool fromKeys;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            vCam = GameObject.FindGameObjectWithTag("VirtualCam").GetComponent<CinemachineVirtualCamera>();
            if (vCam == null)
            {
                Debug.LogError("Can't find the virtual camera!");
            }
        }

        public void OnKeyZoom(InputAction.CallbackContext context)
        {
            zoomDir = context.ReadValue<float>();
            if (context.started || context.performed)
            {
                fromKeys = true;
                Debug.Log("Started or performed.");
            }
            else if (context.canceled)
            {
                fromKeys = false;
                Debug.Log("Canceled.");
            }
        }

        public void OnMouseZoom(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                zoomDir = 0;
                return;
            }
            var value = context.ReadValue<Vector2>();
            zoomDir = value.y;
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

            Compass dir = GetInputDirection(x, y);

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

        // ABSTRACTION
        private void HandleZoom()
        {
            if (zoomDir != 0)
            {
                var curSize = vCam.m_Lens.OrthographicSize;
                var moddedSpeed = zoomSpeed;
                if (fromKeys)
                {
                    moddedSpeed /= 12;
                    Debug.Log($"Modding speed: {moddedSpeed}");
                }
                if (zoomDir > 0 && curSize <= 27)
                    vCam.m_Lens.OrthographicSize += (moddedSpeed * Time.deltaTime);
                else if (zoomDir < 0 && curSize >= 5)
                    vCam.m_Lens.OrthographicSize -= (moddedSpeed * Time.deltaTime);
            }
        }

        private void HandleMovement()
        {
            if (movement == Vector3.zero) return;
            controller.SimpleMove(movement * moveSpeed);
        }

        private void Update()
        {
            HandleZoom();
            HandleMovement();
        }
    }
}
