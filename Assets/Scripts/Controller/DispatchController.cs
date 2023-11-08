using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dispatch.Controller
{
    public class DispatchController : MonoBehaviour
    {
        private DispatchControls cameraActions;
        private InputAction movement;
        private Transform cameraTransform;
        
        // Horizontal Motion
        [SerializeField] private float maxSpeed = 5f;
        private float speed;
        [SerializeField] private float acceleration = 10f;
        [SerializeField] private float damping = 15f;

        // Vertical Motion
        [SerializeField] private float stepSize = 2f;
        [SerializeField] private float zoomDampening = 7.5f;
        [SerializeField] private float minHeight = 5f;
        [SerializeField] private float maxHeight = 50f;
        [SerializeField] private float zoomSpeed = 2f;

        // Rotation
        [SerializeField] private float maxRoationSpeed = 1f;

        // Screen Edge Motion
        [SerializeField] [Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
        [SerializeField] private bool useScreenEdge = true;

        // Used to update the position of the camera base object
        private Vector3 targetPosition;
        private float zoomHeight;

        // Used tp track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        // Tracks where the dragging action started
        Vector3 startDrag;
        
        // Spawn Height for Units
        [SerializeField] private float spawnHeight = 1f;

        private void Awake() {
            cameraActions = new DispatchControls();
            cameraTransform = this.GetComponentInChildren<Camera>().transform;
        }

        private void OnEnable() {
            zoomHeight = cameraTransform.localPosition.y;
            cameraTransform.LookAt(this.transform);

            // Updating last know Camera Position on Enable
            lastPosition = this.transform.position;
            movement = cameraActions.Camera.Movement;
            cameraActions.Camera.RotateCamera.performed += RotateCamera;
            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Enable();
        }

        private void OnDisable() {
            // Turns off when not in use
            cameraActions.Disable();
            cameraActions.Camera.RotateCamera.performed -= RotateCamera;
            cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
        }

        private void Update() {
            GetKeyboardMovement();
            if (useScreenEdge)
                CheckMouseAtScreenEdge();
            DragCamera();
            UpdateVelocity();
            UpdateCameraPosition();
            UpdateBasePosition();
        }


        /// <summary>
        /// Functions Below
        /// </summary>
        private void UpdateVelocity() {
            horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
            horizontalVelocity.y = 0;
            lastPosition = this.transform.position;
        }

        private void GetKeyboardMovement() {
            Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                                 + movement.ReadValue<Vector2>().y * GetCameraForward();

            inputValue = inputValue.normalized;

            if (inputValue.sqrMagnitude > 0.1f)
                targetPosition += inputValue;
        }

        private Vector3 GetCameraRight() {
            Vector3 right = cameraTransform.right;
            right.y = 0;
            return right;
        }

        private Vector3 GetCameraForward() {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            return forward;
        }

        private void UpdateBasePosition() {
            if (targetPosition.sqrMagnitude > 0.1f) {
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                transform.position += targetPosition * speed * Time.deltaTime;
            }
            else {
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                transform.position += horizontalVelocity * Time.deltaTime;
            }

            targetPosition = Vector3.zero;
        }

        private void RotateCamera(InputAction.CallbackContext inputValue) {
            if (!Mouse.current.middleButton.isPressed) return;

            float value = inputValue.ReadValue<Vector2>().x;
            transform.rotation = Quaternion.Euler(0f, value * maxRoationSpeed + transform.rotation.eulerAngles.y, 0f);
        }

        private void ZoomCamera(InputAction.CallbackContext inputValue) {
            float value = -inputValue.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(value) > 0.1f) {
                zoomHeight = cameraTransform.localPosition.y + value * stepSize;
                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                    zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition() {
            Vector3 zoomTarget =
                new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
            zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

            cameraTransform.localPosition =
                Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            cameraTransform.LookAt(this.transform);

        }

        private void CheckMouseAtScreenEdge() {
            Vector2 MousePosition = Mouse.current.position.ReadValue();
            Vector3 moveDirection = Vector3.zero;

            if (MousePosition.x < edgeTolerance * Screen.width) moveDirection += -GetCameraRight();
            else if (MousePosition.x > (1f - edgeTolerance) * Screen.width) moveDirection += GetCameraRight();

            if (MousePosition.y < edgeTolerance * Screen.height) moveDirection += -GetCameraForward();
            else if (MousePosition.y > (1f - edgeTolerance) * Screen.height) moveDirection += GetCameraForward();

            // Combining Keyboard Movement + Mouse Movement
            targetPosition += moveDirection;
        }

        private void DragCamera() {
            if (!Mouse.current.rightButton.isPressed)
                return;

            //create plane to raycast to
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float distance)) {
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    startDrag = ray.GetPoint(distance);
                else
                    targetPosition += startDrag - ray.GetPoint(distance);
            }
        }
    }
}
