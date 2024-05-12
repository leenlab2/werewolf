using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : Item
{
    [SerializeField] private float batteryLife = 100f;
    [SerializeField] private float batteryDrainRate = 1f;
    [SerializeField] private float batteryRechargeRate = 1f;

    public bool isOn = false;
    private bool recharging = false;
    private Light pointLight;

    private void Start()
    {
        pointLight = GetComponentInChildren<Light>();
    }

    public void Toggle()
    {
        isOn = !isOn;

        if (batteryLife > 0 && isOn)
        {
            pointLight.enabled = true;
        } else
        {
            pointLight.enabled = false;
        }
    }

    public void DrainBattery()
    {
        if (isOn)
        {
            batteryLife -= batteryDrainRate * Time.deltaTime;
        }
    }

    public void RechargeBattery(bool charging)
    {
        if (!isOn && charging)
        {
            recharging = true;
        } else
        {
            recharging = false;
        }
    }

    public override void PickUp()
    {
        base.PickUp();
        // move flashlight to child of player, at attach transform
        Transform attachTransform = PlayerController.instance.attachTransform;
        transform.SetParent(attachTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.Find("Model").gameObject.layer = 0;
    }

    public override void Use()
    {
        base.Use();
        Toggle();
    }

    public void Update()
    {
        DrainBattery();

        if (batteryLife <= 0 && isOn)
        {
            Toggle();
        }

        if (recharging)
        {
            batteryLife += batteryRechargeRate * Time.deltaTime;
        }
    }
}
