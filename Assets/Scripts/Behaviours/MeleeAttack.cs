using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Transform playerTransform;
    public static MeleeAttack instance;
    public float attack;
    PlayerController playerController;
    Vector2 attackDir;

    void Start()
    {
        instance = this;
        playerTransform = GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();
        attackDir = Vector2.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StaminaSystem.instance.currentStamina = Mathf.Max(0, StaminaSystem.instance.currentStamina - 15f);
            Attack();
        }
    }

    public void Attack()
    {
        Vector2 playerPosition = playerTransform.position;

        float attackDistance = 0.75f;
        Vector2 newAttackDir = playerController.GetPlayerDirection();
        attackDir = newAttackDir== Vector2.zero? playerController.FacingDirection(): newAttackDir;

        Debug.DrawLine(playerPosition, playerPosition+attackDir * attackDistance, Color.green, 1f);
        float maxAngle = 45f;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(playerPosition, attackDistance, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            Vector2 enemyPosition = enemy.gameObject.GetComponent<Transform>().position;
            Vector2 playerToEnemy = enemyPosition - playerPosition;

            var angle = Vector2Extension.AngleBetweenVector2(attackDir, playerToEnemy.normalized);

            if (angle < maxAngle && angle > -maxAngle)
            {
                Debug.DrawLine(playerPosition, enemyPosition, Color.red, 1f);
                enemy.GetComponent<AILifeSystem>().TakeDamage(attack);
            }
        }
    }
}
