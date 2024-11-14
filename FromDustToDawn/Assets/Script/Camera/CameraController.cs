using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float maxZoom = 15f;
    public float minZoom = 60f;

    [Header("Camera movements")]
    public float edge = 50f;
    public float movementSpeed = 5f;

    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    private Vector2 mousePosition;

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (UIManager.instance.buildingPanel.activeSelf) return;

        float scrollAmount = context.ReadValue<float>();
        Camera camera = GetComponent<Camera>();

        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - scrollAmount * zoomSpeed * Time.deltaTime, maxZoom, minZoom);
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;

        if (mousePosition.y > Screen.height - edge)
        {
            newPosition += new Vector3(Time.deltaTime * movementSpeed, 0, Time.deltaTime * movementSpeed);
        }
        if (mousePosition.y < edge)
        {
            newPosition -= new Vector3(Time.deltaTime * movementSpeed, 0, Time.deltaTime * movementSpeed);
        }
        if (mousePosition.x < edge)
        {
            newPosition += new Vector3(-(Time.deltaTime * movementSpeed), 0, Time.deltaTime * movementSpeed);
        }
        if (mousePosition.x > Screen.width - edge)
        {
            newPosition += new Vector3(Time.deltaTime * movementSpeed, 0, -(Time.deltaTime * movementSpeed));
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

        transform.position = newPosition;
    }
}