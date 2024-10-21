using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;
    public float zoomSpeed = 5f;
    public float zoomSmoothing = 0.1f;
    public float minZoom = 0f;
    public float maxZoom = 6f;

    private float targetZoom;
    private float zoomVelocity = 0f;

    private void Start()
    {
        targetZoom = cameraTransform.position.z; 
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float scrollAmount = context.ReadValue<float>();

        targetZoom += scrollAmount * zoomSpeed * Time.deltaTime;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }

    private void Update()
    {
        float newZoom = Mathf.SmoothDamp(cameraTransform.position.z, targetZoom, ref zoomVelocity, zoomSmoothing);
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);
        cameraTransform.position = cameraTransform.position + cameraTransform.forward * (newZoom - cameraTransform.position.z);
    }
}
