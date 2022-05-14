using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(MeleeAttack))]
[RequireComponent(typeof(StaminaSystem))]
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

    public enum state
    {
        Normal,
        Running,
        Dashing
    }
    public state currentState = state.Normal;

    public class KeyboardStatus
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


    public Vector2 FacingDirection()
    {
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        return new Vector2(horizontal, vertical);
    }


    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            keyboardStatus.up = true;
        }
        else if (Input.GetKeyUp(KeyCode.W)) keyboardStatus.up = false;

        if (Input.GetKeyDown(KeyCode.S)) { 
            keyboardStatus.down = true;
        }
        else if (Input.GetKeyUp(KeyCode.S)) keyboardStatus.down = false;

        if (Input.GetKeyDown(KeyCode.A))
        {
            keyboardStatus.left = true;
        }
        else if (Input.GetKeyUp(KeyCode.A)) keyboardStatus.left = false;

        if (Input.GetKeyDown(KeyCode.D)) { 
                keyboardStatus.right = true;
            }
        else if (Input.GetKeyUp(KeyCode.D)) keyboardStatus.right = false;

        if (Input.GetKeyDown(KeyCode.LeftShift) && currentState != state.Dashing)
        {
            keyboardStatus.shift = true;
            if (StaminaSystem.instance.currentStamina > 10)
            {
                dash.StartBehaviour();
                StaminaSystem.instance.currentStamina -= 15;
                currentState = state.Dashing;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            keyboardStatus.shift = false;
        }
    }


    void Update()
    {
        HandleInput();

        switch (currentState)
        {
            case state.Normal:
                NormalUpdate();
                break;
            case state.Running:
                RunningUpdate();
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


    void NormalUpdate()
    {
        SetCurrentSpeed();

        Vector2 playerDirection = GetPlayerDirection();

        rigidBody2D.velocity = playerDirection * currentSpeed * StaminaSystem.instance.staminaDebuff;
    }


    void RunningUpdate()
    {
        if (keyboardStatus.shift && StaminaSystem.instance.currentStamina > 0f)
        {
            SetCurrentSpeed();

            Vector2 playerDirection = GetPlayerDirection();

            rigidBody2D.velocity = playerDirection * currentSpeed * 2f;

            StaminaSystem.instance.currentStamina -= 20f * Time.deltaTime;
        }
        else
            currentState = state.Normal;
    }


    void dashingUpdate()
    {
        if (!dash.dashing)
            currentState = keyboardStatus.shift ? state.Running : state.Normal;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                currentState = state.Normal;
                break;
            case "Enemy":
                Destroy(gameObject);
                break;
        }
    }
}