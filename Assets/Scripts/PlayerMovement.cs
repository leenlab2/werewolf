using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _speed = 0.1f;

    private Rigidbody playerBody;

    private void Start()
    {
        playerBody = _player.GetComponent<Rigidbody>();
    }

    public void MovePlayer(InputAction.CallbackContext ctx)
    {
        Vector2 directionInput = ctx.ReadValue<Vector2>();
        // Movement needs to be in local space axis not world space
        Vector3 movement = _player.transform.TransformDirection(new Vector3(directionInput.x, 0, directionInput.y)) * _speed;
        playerBody.velocity = movement;
    }
}
