using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("Vitalis")]
    public LayerMask vitalisLayerMask;

    private Vector2 mousePosition;

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnLeftClic(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, vitalisLayerMask) && hit.transform.tag == "Vitalis")
            {
                VitalisManager.instance.CollectVitalis(hit.transform.gameObject);
            }
        }
    }
}
