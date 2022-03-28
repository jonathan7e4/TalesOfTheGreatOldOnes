using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
 // Start is called before the first frame update
    AstarPath pathController;

    Pathfinding.Seeker seeker;

    Rigidbody2D rb;

    public Transform playerTransform;

    public float speed;

    public float shootReloadTime;

    Coroutine seekPathRoutine;

    public GameObject projectile;

    Coroutine timerRoutine;



    public Vector2 GetPlayerPredictiveTarget()
    {
        var playerVelocity = playerTransform.GetComponent<Rigidbody2D>().velocity;
        var projectileSpeed = projectile.GetComponent<ProyectileMovement>().speed;

        var dist = (playerTransform.position - transform.position).magnitude;
        var time = dist / projectileSpeed;

        return (Vector2)playerTransform.position + (playerVelocity * time);
    }



    void Start()
    {
        pathController = FindObjectOfType<AstarPath>();
        rb = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Pathfinding.Seeker>();

        Teleport.instance.TeleportToPLayer();

    }

    Vector2 GetPositionToApproachPlayer()
    {
        float offset = 1f;
        float maxOffset = maxDistToPlayer - minDistToPlayer;

        Vector2 target;
        Vector2 target2;
        Vector2 playerToTarget;
        Vector2 playerToTarget2;

        bool cast1 = true;
        bool cast2 = true;


        do
        {
            target = -m_distanceToPlayer.normalized * (minDistToPlayer + offset) + (Vector2)playerTransform.position;
            target2 = target;
            playerToTarget = target - (Vector2)playerTransform.position;
            playerToTarget2 = playerToTarget;

            Debug.DrawLine(playerTransform.position, target);

            int it = 0;

            while (cast1 && cast2)
            {
                cast1 = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));
                cast2 = Physics2D.CircleCast(target2, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));

                if (it++ > 180 / 15) break;
                Debug.DrawLine(playerTransform.position, target);
                Debug.DrawLine(playerTransform.position, target2);

                playerToTarget = RotateVector2(playerToTarget, 15f);
                target = (Vector2)playerTransform.position + playerToTarget;

                playerToTarget2 = RotateVector2(playerToTarget2, -15f);
                target2 = (Vector2)playerTransform.position + playerToTarget2;
            }

            offset *= 1.1f;

        } while (cast1 && cast2 && offset < maxOffset);


        if (cast1 && !cast2)
            return target2;

        if (!cast1 && cast2)
            return target;   

        // return closest
        return Vector2.Distance(transform.position, target) < Vector2.Distance(transform.position, target2) ? target : target2;
    }

    Vector2 RotateVector2(Vector2 v, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * v;
    }

    Vector2 GetPositionToAvoidPlayer()
    {
        Vector2 minDistanceFromPlayer = -m_distanceToPlayer.normalized * minDistToPlayer;

        float offset = 1f;
        float maxOffset = maxDistToPlayer - minDistToPlayer;

        Vector2 target;
        Vector2 target2;

        Vector2 aiToTarget;
        Vector2 aiToTarget2;

        bool cast1 = true;
        bool cast2 = true;

       do
        {
            target = -m_distanceToPlayer.normalized * (minDistToPlayer + offset) + (Vector2)playerTransform.position;
            target2 = target;

            aiToTarget = target - (Vector2) transform.position;
            aiToTarget2 = aiToTarget;

            Debug.DrawLine(transform.position, target);

            int it = 0;

            while (cast1 && cast2)
            {

                cast1 = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));
                cast2 = Physics2D.CircleCast(target2, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));

                if (it++ > 180 / 15) break;
                Debug.DrawLine(transform.position, target);
                Debug.DrawLine(transform.position, target2);

                aiToTarget = RotateVector2(aiToTarget, 15f);
                target = (Vector2) transform.position + aiToTarget;

                aiToTarget2 = RotateVector2(aiToTarget2, -15f);
                target2 = (Vector2) transform.position + aiToTarget2;
            }

            //este dentro del minimo, poner true

            var distanceTargetToPlayer = Vector2.Distance(playerTransform.position, target);
            var distanceTarget2ToPlayer = Vector2.Distance(playerTransform.position, target2);

            if (distanceTargetToPlayer < minDistToPlayer || distanceTarget2ToPlayer < minDistToPlayer)
            {
                cast1 = true;
                cast2 = true;
            }

            offset *= 1.1f;

        } while (cast1 && cast2 && offset < maxOffset);


        if (cast1 && !cast2)
            return target2;

        if (!cast1 && cast2)
            return target;

        return Vector2.Distance(transform.position, target) < Vector2.Distance(transform.position, target2) ? target : target2;
    }

    Vector2 GetPositionToShootPlayer()
    {
        return GetPositionToApproachPlayer();
    }

    bool CanShotToPlayer()
    {
        Vector2 toPlayer = (playerTransform.position - rb.transform.position).normalized;
        Vector2 perp = GetPerpendicular(toPlayer) * projectile.GetComponent<CircleCollider2D>().radius;

        bool firstCast = !Physics2D.Linecast((Vector2)rb.transform.position + perp, (Vector2)playerTransform.position + perp, LayerMask.GetMask("Obstacle"));
        bool secondCast = !Physics2D.Linecast((Vector2)rb.transform.position - perp, (Vector2)playerTransform.position - perp, LayerMask.GetMask("Obstacle"));

        //Debug.DrawLine((Vector2)rb.transform.position + perp, (Vector2)playerTransform.position + perp);   
        //Debug.DrawLine((Vector2)rb.transform.position - perp, (Vector2)playerTransform.position - perp);   

        return firstCast && secondCast;
    }


    Vector2 GetPerpendicular(Vector2 vector)
    {
        Vector2 result;
        result.x = vector.y;
        result.y = -vector.x;

        return result;
    }   


    void ShootLogicUpdate()
    {
        shootReloadTime -= Time.deltaTime;

        if (CanShotToPlayer())
        {

            if (shootReloadTime <= 0)
            {
                var toPlayer = m_distanceToPlayer.normalized;
                

                var proj = Instantiate(projectile, (Vector2)transform.position + toPlayer * 1f, Quaternion.identity);

                var target = GetPlayerPredictiveTarget();
                proj.GetComponent<Rigidbody2D>().velocity = (target - (Vector2)transform.position).normalized * proj.GetComponent<ProyectileMovement>().speed;

                shootReloadTime = 3f;
            }

        }else if (!lookingAPath && !followingPath) {
            var target = GetPositionToShootPlayer();
            StartPath(transform.position, target, OnPathFound);
        }
    }

    Vector2 lastPathTarget;

    bool CurrentPathIsNotValid( Vector2 destination, Vector2 lastNode )
    {
        return Vector2.Distance( destination, lastNode ) > MAX_DIST_TO_LAST_NODE;
    }

    float minDistToPlayer = 4f;
    float maxDistToPlayer = 8f;

    bool lookingAPath = false;
    const float MAX_DIST_TO_LAST_NODE = 2f;

    void StartPath(Vector2 start, Vector2 end, Pathfinding.OnPathDelegate callback) {
        lastPathTarget = end;
        lookingAPath = true;
        seeker.StartPath(start, end, (Pathfinding.Path p) => {
            lookingAPath = false;
            callback(p);
        });
    }

    void FollowPlayerLogicUpdate()
    {
        m_distanceToPlayer = playerTransform.position - transform.position;

        Vector2 target;

        if (m_distanceToPlayer.magnitude < minDistToPlayer && !lookingAPath && !followingPath)
        {
                                                                                    
            target = GetPositionToAvoidPlayer();
            StartPath(transform.position, target, OnPathFound);

        }
        else if (!lookingAPath && !followingPath)
        {
            // TODO: NO SEGUIR AL PLAYER, SEGUIR A UN PUNTO DENTRO DE LA ZONA DONDE ME QUIERO MANTENER
            target = GetPositionToApproachPlayer();

            ////debug.log("FollowPlayerLogicUpdate()\t-\tMESSAGE: lookingAPath = " + lookingAPath);

            StartPath(transform.position, target, OnPathFound);
            
        }
    }

    Vector2 m_distanceToPlayer;

    // Update is called once per frame
    void Update()
    {

        // UPDATE LOGIC VARIABLES
        m_distanceToPlayer = playerTransform.position - transform.position;

        // UPDATE LOGIC
        if(m_distanceToPlayer.magnitude < maxDistToPlayer && m_distanceToPlayer.magnitude > minDistToPlayer){

            //ShootLogicUpdate();
        }
        else {
           //FollowPlayerLogicUpdate();
        }

    }

    void StopSeekingPath()
    {
        if(rb.velocity != Vector2.zero){
            rb.velocity = Vector2.zero;
            StopCoroutine(seekPathRoutine);
        }
    }


    bool followingPath;


    IEnumerator FollowPath( List<Vector3> path )
    {
        const float minToReachNode = 0.1f;
        followingPath = true;
        rb.velocity = ( path[0] - transform.position ).normalized * speed;

        foreach ( var node in path )
        {
            while ( Vector2.Distance( transform.position, node ) > minToReachNode )
            {
                Vector2 newVelocity = node - transform.position;
                newVelocity.Normalize();
                rb.velocity = newVelocity * speed;
                yield return null;
            }
        }

        followingPath = false;
        rb.velocity = Vector2.zero;
    }


    void PrintPath( List<Vector3> path )
    {
#if UNITY_EDITOR
        Vector2 lastPos = transform.position;

        foreach ( var node in path )
        {
            Vector2 current = new Vector2( node.x, node.y );

            Debug.DrawLine( lastPos, current, Color.green, 1f );
            lastPos = current;
        }
#endif
    }


    Vector2 lastPathNodePos;


    void OnPathFound( Pathfinding.Path p )
    {
        lastPathNodePos = p.vectorPath[ p.vectorPath.Count - 1 ];


        if ( seekPathRoutine != null ) StopSeekingPath();

        seekPathRoutine = StartCoroutine( FollowPath( p.vectorPath ) );

        PrintPath( p.vectorPath );
    }
}