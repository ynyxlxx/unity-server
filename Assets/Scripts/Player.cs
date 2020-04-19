using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int id;
    public string username;
    public CharacterController controller;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;
    public float dashMultiplier = 2f;
    public float dashTime = 1f;

    private bool[] inputs;
    private float yVelocity = 0;
    private float dashTimer = 0f;
    private bool isDashing = false;

    private void Start () {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize (int _id, string _username) {
        id = _id;
        username = _username;

        inputs = new bool[5];
    }

    public void FixedUpdate () {
        Vector2 _inputDirection = Vector2.zero;
        if (inputs[0]) {
            _inputDirection.y += 1;
        }
        if (inputs[1]) {
            _inputDirection.y -= 1;
        }
        if (inputs[2]) {
            _inputDirection.x -= 1;
        }
        if (inputs[3]) {
            _inputDirection.x += 1;
        }

        Move (_inputDirection);
    }

    private void Move (Vector2 _inputDirection) {
        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded) {
            yVelocity = 0f;
            if (inputs[4]) {
                yVelocity = jumpSpeed;
            }

            if (inputs[5] && isDashing == false) {
                isDashing = true;
            }

        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;

        if (isDashing) {
            DashControll ();
            _moveDirection = new Vector3 (_moveDirection.x * dashMultiplier, _moveDirection.y, _moveDirection.z * dashMultiplier);
            controller.Move (_moveDirection);
        } else {
            controller.Move (_moveDirection);
        }

        ServerSend.PlayerPosition (this);
        ServerSend.PlayerRotation (this);
    }

    public void SetInput (bool[] _inputs, Quaternion _rotation) {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    private void DashControll () {
        if (dashTimer > dashTime) {
            dashTimer = 0;
            isDashing = false;
        } else {
            dashTimer += Time.deltaTime;
        }
    }
}