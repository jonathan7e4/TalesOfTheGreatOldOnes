using UnityEngine;

[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(Dash))]
[RequireComponent(typeof(Flank))]

public class SkullAIController : MonoBehaviour
{
    public static SkullAIController instance;

    Animator animator;
    Transform playerTransform;
    Teleport teleport;
    Dash dash;
    Flank flank;

    public float damage = 30f;
    public float attackCooldown = 2f;
    public float attackCooldownTimer;

    public int consecutiveAttacks = 4;
    public int consecutiveAttacksCounter;

    [HideInInspector]
    public enum State
    {
        Dashing,
        Waiting,
        Attack,
        FollowingPlayer,
        Resting
    }
    public State currentState = State.Resting;


    void Start()
    {
        animator = GetComponent<Animator>();
        instance = this;

        teleport = GetComponent<Teleport>();
        dash = GetComponent<Dash>();
        flank = GetComponent<Flank>();

        teleport.InitBehaviourData();
        dash.InitBehaviourData();
        flank.InitBehaviourData();

        attackCooldownTimer = 0f;
        consecutiveAttacksCounter = consecutiveAttacks;
    }

    void Update()
    {
        if ( playerTransform == null && PlayerController.instance != null ) playerTransform = PlayerController.instance.transform;

        switch (currentState)
        {
            case State.Dashing:
                DashingUpdateLogic();
                break;
            case State.Resting:
                RestingUpdateLogic();
                break;
            case State.Attack:
                Attack();
                break;
            case State.FollowingPlayer:
                FollowPlayer();
                break;
            default:
                break;
        }
    }

    private void FollowPlayer()
    {
        UpdateIdleAnimation();

        flank.UpdateBehaviour();

        if (attackCooldownTimer <= 0f) {
            flank.StopBehaviour();

            currentState = State.Resting;
        }
        else
            attackCooldownTimer -= Time.deltaTime;
    }

    void RestingUpdateLogic()
    {
        UpdateIdleAnimation();

        flank.UpdateDistanceToPlayer();

        if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer || flank.distanceToPlayer.magnitude < flank.minDistToPlayer)
        {
            currentState = State.FollowingPlayer;
        }
        else if (attackCooldownTimer <= 0f)
        {
            attackCooldownTimer = attackCooldown;
            consecutiveAttacksCounter = consecutiveAttacks;
            currentState = State.Attack;
        }
        else
            attackCooldownTimer -= Time.deltaTime;
    }


    private void UpdateIdleAnimation()
    {
        Vector2 enemyToPlayer = playerTransform.position - transform.position;

        if ( Mathf.Abs( enemyToPlayer.x ) > Mathf.Abs( enemyToPlayer.y ) )
        {
            if ( enemyToPlayer.x > 0 ) animator.SetFloat( "Horizontal", 1 );
            else animator.SetFloat( "Horizontal", -1 );
            animator.SetFloat( "Vertical", 0 );
        }
        else
        {
            if ( enemyToPlayer.y > 0 ) animator.SetFloat( "Vertical", 1 );
            else animator.SetFloat( "Vertical", -1 );
            animator.SetFloat( "Horizontal", 0 );
        }
    }


    void Attack()
    {
        consecutiveAttacksCounter--;

        teleport.StartBehaviour();

        UpdateIdleAnimation();

        dash.StartBehaviour();

        currentState = State.Dashing;
    }

    void DashingUpdateLogic()
    {
        if (!dash.dashing)
        {
            UpdateIdleAnimation();

            if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer || flank.distanceToPlayer.magnitude < flank.minDistToPlayer)
                currentState = State.FollowingPlayer;
            else if (consecutiveAttacksCounter == 0)
                currentState = State.Resting;
            else
                currentState = State.Attack;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && dash.dashing) {
            PlayerController.instance.lifeSystem.TakeDamage(damage);

            Shake.instance.ShakeIt();

            dash.dashing = false;
            dash.StopBehaviour();
        }
    }
}