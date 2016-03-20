using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 6f;

    Vector3 _movement;
    Animator _animator;
    Rigidbody _playeRigidbody;
    int _floorMask;
    float _camRayLength = 100f;

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
        _animator = GetComponent<Animator>();
        _playeRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var horizontalAxis = Input.GetAxisRaw("Horizontal");
        var verticalAxis = Input.GetAxisRaw("Vertical");

        Move(horizontalAxis, verticalAxis);
        Turning();
        Animating(horizontalAxis, verticalAxis);
    }

    private void Move(float horizontalAxis, float verticalAxis)
    {
        _movement.Set(horizontalAxis, 0, verticalAxis);

        _movement = _movement.normalized * Speed * Time.deltaTime;

        _playeRigidbody.MovePosition(transform.position + _movement);
    }

    private void Turning()
    {
        var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorRaycastHit;

        if (!Physics.Raycast(camRay, out floorRaycastHit, _camRayLength, _floorMask)) return;

        var playerToMouse = floorRaycastHit.point - transform.position;
        playerToMouse.y = 0f;

        var newRotationQuaternion = Quaternion.LookRotation(playerToMouse);
        _playeRigidbody.MoveRotation(newRotationQuaternion);
    }

    private void Animating(float horizontalAxis, float verticalAxis)
    {
        var walking = horizontalAxis != 0f || verticalAxis != 0f;

        _animator.SetBool("IsWalking", walking);
    }
}
