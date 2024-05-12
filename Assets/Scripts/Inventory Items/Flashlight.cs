using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Flashlight : Item
{
    [SerializeField] private float maxBatteryLife = 100f;
    [SerializeField] private float batteryDrainRate = 1f;
    [SerializeField] private float batteryRechargeRate = 1f;

    private Image batteryFill;
    private GameObject crank;

    public bool isOn = false;
    private bool recharging = false;
    private Light pointLight;

    private float batteryLife;

    public static event Action<bool> OnFlashlightCharge;

    private void Start()
    {
        pointLight = GetComponentInChildren<Light>();
        batteryLife = maxBatteryLife;

        GameObject HUD = GameObject.Find("HUD");
        Transform battery = HUD.transform.Find("Battery");
        battery.gameObject.SetActive(false);
        batteryFill = battery.GetChild(0).GetComponent<Image>();
        crank = HUD.transform.Find("Crank").gameObject;
    }

    public void Toggle()
    {
        isOn = !isOn;

        if (batteryLife > 0 && isOn)
        {
            pointLight.enabled = true;
        } else if (!isOn)
        {
            pointLight.enabled = false;
        } else if (batteryLife <= 0 && isOn)
        {
            pointLight.enabled = false;
            RaiseItemError();
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
            crank.SetActive(true);
            recharging = true;
            OnFlashlightCharge?.Invoke(true);
        } else
        {
            crank.SetActive(false);
            recharging = false;
            OnFlashlightCharge?.Invoke(false);
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

        batteryFill.transform.parent.gameObject.SetActive(true);
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
            crank.SetActive(true);
        }

        if (recharging)
        {
            batteryLife += batteryRechargeRate * Time.deltaTime;
        }

        batteryFill.fillAmount = batteryLife / maxBatteryLife;
    }
}
