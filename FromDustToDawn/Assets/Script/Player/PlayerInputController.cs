using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [Header("Vitalis")]
    public LayerMask vitalisLayerMask;

    private Vector2 mousePosition;

    public static PlayerInputController instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnLeftClic(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RaycastHit hit = MousePointRaycast(vitalisLayerMask);
            if(hit.collider != null && hit.transform.tag == "Vitalis")
                VitalisManager.instance.CollectVitalis(hit.transform.gameObject);
        }
    }

    public RaycastHit MousePointRaycast( LayerMask mask)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

        return hit;
    }
}
