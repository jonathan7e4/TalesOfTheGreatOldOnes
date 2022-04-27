using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : AIBehaviour
{   // INITIAL DATA
    Rigidbody2D rb;
    // PUBLIC ATTRIBUTES
    public float shootReloadTime;
    public float minDistToPlayer = 4f;
    public float maxDistToPlayer = 8f;
    public GameObject projectile;
    public Vector2 distanceToPlayer;
    // PRIVATE ATTRIBUTES
    Transform playerTransform;


    bool CanShotToPlayer()
    {
        Vector2 toPlayer = ( playerTransform.position - rb.transform.position ).normalized;
        Vector2 perp = PositionUtils.GetPerpendicular( toPlayer ) * projectile.GetComponent<CircleCollider2D>().radius;

        bool firstCast = !Physics2D.Linecast( (Vector2) rb.transform.position + perp, (Vector2) playerTransform.position + perp, LayerMask.GetMask( "Obstacle" ) );
        bool secondCast = !Physics2D.Linecast( (Vector2) rb.transform.position - perp, (Vector2) playerTransform.position - perp, LayerMask.GetMask( "Obstacle" ) );

        return firstCast && secondCast;
    }


    public void ShootLogicUpdate()
    {
        UpdateDistanceToPlayer();

        shootReloadTime -= Time.deltaTime;

        if ( CanShotToPlayer() )
        {
            if ( shootReloadTime <= 0 )
            {
                var toPlayer = distanceToPlayer.normalized;

                var projectile = Instantiate( this.projectile, (Vector2) transform.position + toPlayer * 1f, Quaternion.identity );

                Vector3 playerPosition = playerTransform.position;
                Vector2 playerVelocity = playerTransform.GetComponent<Rigidbody2D>().velocity;
                Vector3 aiPosition = transform.position;
                float projectileSpeed = this.projectile.GetComponent<ProyectileMovement>().speed;

                var target = PositionUtils.GetPlayerPredictiveTarget( playerPosition, playerVelocity, aiPosition, projectileSpeed );

                projectile.GetComponent<Rigidbody2D>().velocity = ( target - (Vector2) transform.position ).normalized * projectile.GetComponent<ProyectileMovement>().speed;

                shootReloadTime = 3f;
            }
        }
    }


    public void UpdateDistanceToPlayer()
    {
        distanceToPlayer = playerTransform.position - transform.position;
    }


    public override void InitBehaviourData()
    {
        playerTransform = PlayerController.instance.GetComponent<Transform>();

        rb = GetComponent<Rigidbody2D>();
    }


    public override void StartBehaviour()
    {
        ShootLogicUpdate();
    }


    public override void StopBehaviour()
    {
        
    }


    public override void UpdateBehaviour()
    {
        ShootLogicUpdate();
    }
}
