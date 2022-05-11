using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(MeleeAttack))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    Rigidbody2D rigidBody2D;
    PlayerDash dash;
    KeyboardStatus keyboardStatus = new KeyboardStatus();
    public Animator animator;

    public float speed = 4f;
    float currentSpeed = 0f;
    float acceleration = 200f;

    Vector2 facingDirection;

    public enum state
    {
        Normal,
        Running,
        Dashing
    }
    public state currentState = state.Normal;

    class KeyboardStatus
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool keyPressed;
        public bool shift;

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


    public Vector2 GetPlayerDirection()
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

    public Vector2 FacingDirection() { return facingDirection; }


    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            keyboardStatus.up = true;
            facingDirection = Vector2.up;
        }
        else if (Input.GetKeyUp(KeyCode.W)) keyboardStatus.up = false;

        if (Input.GetKeyDown(KeyCode.S)) { 
            keyboardStatus.down = true;
            facingDirection = Vector2.down;
        }
        else if (Input.GetKeyUp(KeyCode.S)) keyboardStatus.down = false;

        if (Input.GetKeyDown(KeyCode.A))
        {
            keyboardStatus.left = true;
            facingDirection = Vector2.left;
        }
        else if (Input.GetKeyUp(KeyCode.A)) keyboardStatus.left = false;

        if (Input.GetKeyDown(KeyCode.D)) { 
                keyboardStatus.right = true;
                facingDirection = Vector2.right;
            }
        else if (Input.GetKeyUp(KeyCode.D)) keyboardStatus.right = false;

        if (Input.GetKeyDown(KeyCode.LeftShift) && currentState != state.Dashing && StaminaSystem.instance.currentStamina > 25)
        {
            if (keyboardStatus.shift)
            {
                currentState = state.Running;
            }
            else
            {
                keyboardStatus.shift = true;
                dash.StartBehaviour();
                StaminaSystem.instance.currentStamina -= 25;
                currentState = state.Dashing;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            keyboardStatus.shift = false;
        }
    }


    void Update()
    {
        HandleInput();

        switch (currentState)
        {
            case state.Normal:
                normalUpdate();
                break;
            case state.Running:
                break;
            case state.Dashing:
                dashingUpdate();
                break;
        }
    }


    void Start()
    {
        instance = this;
        rigidBody2D = GetComponent<Rigidbody2D>();
        dash = GetComponent<PlayerDash>();

        dash.InitBehaviourData();
    }

    void normalUpdate()
    {
        SetCurrentSpeed();

        Vector2 playerDirection = GetPlayerDirection();

        rigidBody2D.velocity = playerDirection * currentSpeed;
    }

    void dashingUpdate()
    {
        if (!dash.dashing)
            currentState = state.Normal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Obstacle"))
        {
            currentState = state.Normal;
        }
    }
}