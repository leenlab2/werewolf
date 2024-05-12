using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableDetector : MonoBehaviour
{
    [SerializeField] private float maxPlayerReach;
    [SerializeField] private LayerMask interactableLayer;

    private RaycastHit hit;
    private Item detectedItem;
    private float sphereRadius = 0.2f;

    public static event Action<Item> OnItemHitChange;

    private void FixedUpdate()
    {
        // Define the ray we are using to detect objects
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray originRay = new Ray(origin, direction);

        Debug.DrawRay(origin, direction * maxPlayerReach, Color.red);

        detectedItem = null;
        // Perform raycast
        if (Physics.SphereCast(originRay, sphereRadius, out hit, maxPlayerReach, interactableLayer))
        {
            if (hit.transform.parent.TryGetComponent(out Item item))
            {
                detectedItem = item;
                OnItemHitChange?.Invoke(detectedItem);
            }
        }
    }

    public void InteractWithItem(InputAction.CallbackContext ctx)
    {
        if (detectedItem != null && ctx.performed)
        {
            Debug.Log("Interacting with item " + detectedItem.gameObject.name);
            detectedItem.PickUp();
        }
    }
}
