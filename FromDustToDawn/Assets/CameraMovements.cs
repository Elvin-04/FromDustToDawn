using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovements : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float edgeSensitivity = 50f;

    Vector2 mousePosition;

    //top 1 1
    //right 1 -1
    //left -1 1
    //down -1 -1

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

    //void MoveCamera(Vector2 direction)
    //{
    //    transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    //}

    void MoveCamera(Vector2 direction)
    {
        Vector3 rightMovement = new Vector3(transform.right.x, 0, transform.right.z);
        rightMovement.Normalize();

        Vector3 forwardMovement = new Vector3(transform.forward.x, 0, transform.forward.z); 
        forwardMovement.Normalize();
        Vector3 movement = (rightMovement * direction.x + forwardMovement * direction.y) * moveSpeed * Time.deltaTime;

        transform.position += movement;
    }

}
