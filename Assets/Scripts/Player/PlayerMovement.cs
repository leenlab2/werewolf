using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speedBase;
    [SerializeField] private float mouseSensitivity;

    private Rigidbody playerBody;
    private Vector2 walkingDirection;
    private Vector2 cameraDirection;
    private float _speed;

    private void Start()
    {
        playerBody = _player.GetComponentInChildren<Rigidbody>();

        walkingDirection = Vector2.zero;
        cameraDirection = Vector2.zero;
        _speed = _speedBase;
    }

    public void UpdatePlayerVelocity(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            walkingDirection = ctx.ReadValue<Vector2>();
        }
        else if (ctx.canceled)
        { 
            walkingDirection = Vector2.zero;
        }
    }

    public void UpdateCameraDirection(InputAction.CallbackContext ctx)
    {
        cameraDirection = ctx.ReadValue<Vector2>() * mouseSensitivity;
    }

    public void Run(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _speed = 2.5f * _speedBase;
        }
        else if (ctx.canceled)
        {
            _speed = _speedBase;
        }
    }

    public void Sneak(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _speed = 0.3f * _speedBase;
        }
        else if (ctx.canceled)
        {
            _speed = _speedBase;
        }
    }

    private void FixedUpdate()
    {
        // Move player
        Vector3 movement = _player.transform.TransformDirection(new Vector3(walkingDirection.x, 0, walkingDirection.y)) * _speed;
        playerBody.velocity = movement;

        _player.transform.Rotate(new Vector3(0, cameraDirection.x, 0));

        // Move Camera
        Transform camera = Camera.main.transform;
        float x_rotation = Mathf.Clamp(camera.localRotation.eulerAngles.x - cameraDirection.y, -60f, 360f);
        camera.localRotation = Quaternion.Euler(x_rotation, camera.localRotation.y, camera.localRotation.z);
    }
}
