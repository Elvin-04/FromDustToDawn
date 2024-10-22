using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovements : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float edgeSensitivity = 50f;

    Vector2 mousePosition;

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (mousePosition.x < edgeSensitivity) MoveCamera(new Vector2(-1, 1));
        if (mousePosition.x > Screen.width - edgeSensitivity) MoveCamera(new Vector2(1, -1));
        if (mousePosition.y < edgeSensitivity) MoveCamera(new Vector2(-1, -1));
        if (mousePosition.y > Screen.height - edgeSensitivity) MoveCamera(new Vector2(1, 1));
    }

    void MoveCamera(Vector2 direction)
    {
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    }

}