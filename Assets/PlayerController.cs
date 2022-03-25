using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    KeyboardStatus keyboardStatus = new KeyboardStatus();
    public Animator animator;

    float speed = 4f;
    float currentSpeed = 0f;
    float acceleration = 200f;


    class KeyboardStatus
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool keyPressed;


        public void SetKeyPressed()
        {
            keyPressed = up || down || left || right;
        }
    }


    void SetCurrentSpeed()
    {
        keyboardStatus.SetKeyPressed();

        if (keyboardStatus.keyPressed)
        {
            currentSpeed = Mathf.Min(speed, currentSpeed + acceleration * Time.deltaTime);
            animator.SetFloat("Speed", 1);
        }
        else
        {
            currentSpeed = 0f;
            animator.SetFloat("Speed", 0);
        }
    }


    Vector2 GetPlayerDirection()
    {
        Vector2 playerDirection = Vector2.zero;

        if (keyboardStatus.up)
        {
            playerDirection += Vector2.up;
            animator.SetFloat("Vertical", 1);
            animator.SetFloat("Horizontal", 0);
        }
        else if (keyboardStatus.down)
        {
            playerDirection += Vector2.down;
            animator.SetFloat("Vertical", -1);
            animator.SetFloat("Horizontal", 0);
        }

        if (keyboardStatus.left)
        {
            playerDirection += Vector2.left;
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
        }
        else if (keyboardStatus.right)
        {
            playerDirection += Vector2.right;
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 0);
        }

        playerDirection.Normalize();

        return playerDirection;
    }


    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) keyboardStatus.up = true;
        else if (Input.GetKeyUp(KeyCode.W)) keyboardStatus.up = false;

        if (Input.GetKeyDown(KeyCode.S)) keyboardStatus.down = true;
        else if (Input.GetKeyUp(KeyCode.S)) keyboardStatus.down = false;

        if (Input.GetKeyDown(KeyCode.A)) keyboardStatus.left = true;
        else if (Input.GetKeyUp(KeyCode.A)) keyboardStatus.left = false;

        if (Input.GetKeyDown(KeyCode.D)) keyboardStatus.right = true;
        else if (Input.GetKeyUp(KeyCode.D)) keyboardStatus.right = false;
    }


    void Update()
    {
        HandleInput();

        SetCurrentSpeed();

        Vector2 playerDirection = GetPlayerDirection();

        rigidBody2D.velocity = playerDirection * currentSpeed;
    }


    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
}