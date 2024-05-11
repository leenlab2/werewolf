using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        // Perform raycast
        if (Physics.SphereCast(originRay, sphereRadius, out hit, maxPlayerReach, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out Item item) && item != detectedItem)
            {
                detectedItem = item;
                OnItemHitChange?.Invoke(detectedItem);
            }
        }
    }
}
