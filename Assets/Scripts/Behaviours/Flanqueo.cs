using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flanqueo : MonoBehaviour
{   // INITIALIZED ON Start()
    AstarPath pathController;
    Rigidbody2D rb;
    Pathfinding.Seeker seeker;
    // COROUTINES
    Coroutine seekPathRoutine;
    // PUBLIC ATTRIBUTES
    public float speed = 3f;
    public float shootReloadTime;
    public float minDistToPlayer = 4f;
    public float maxDistToPlayer = 8f;
    public Transform playerTransform;
    public GameObject projectile;
    public Vector2 m_distanceToPlayer;
    // PRIVATE ATTRIBUTES
    bool lookingAPath = false;
    bool followingPath;
    float timeFollowingPathCount;
    Vector2 lastPathNodePos;


    void Start()
    {
        pathController = FindObjectOfType<AstarPath>();

        rb = GetComponent<Rigidbody2D>();

        seeker = GetComponent<Pathfinding.Seeker>();
    }


    bool NotTooClose()
    {
        return m_distanceToPlayer.magnitude > minDistToPlayer;
    }


    bool NotTooFarAway()
    {
        return m_distanceToPlayer.magnitude < maxDistToPlayer;
    }


    public bool AroundPlayer()
    {
        return NotTooFarAway() && NotTooClose();
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
            target = -m_distanceToPlayer.normalized * (minDistToPlayer + offset) + (Vector2) playerTransform.position;
            target2 = target;
            playerToTarget = target - (Vector2) playerTransform.position;
            playerToTarget2 = playerToTarget;

            int it = 0;

            while (cast1 && cast2)
            {
                cast1 = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));
                cast2 = Physics2D.CircleCast(target2, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));

                if (it++ > 180 / 15) break;

                playerToTarget = RotateVector2(playerToTarget, 15f);
                target = (Vector2)playerTransform.position + playerToTarget;

                playerToTarget2 = RotateVector2(playerToTarget2, -15f);
                target2 = (Vector2)playerTransform.position + playerToTarget2;
            }

            offset *= 1.1f;

        } while (cast1 && cast2 && offset < maxOffset);
        
        if (cast1 && !cast2) return target2;

        if (!cast1 && cast2) return target;

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
            target = -m_distanceToPlayer.normalized * (minDistToPlayer + offset) + (Vector2) playerTransform.position;
            target2 = target;

            aiToTarget = target - (Vector2) transform.position;
            aiToTarget2 = aiToTarget;

            int it = 0;

            while (cast1 && cast2)
            {

                cast1 = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));
                cast2 = Physics2D.CircleCast(target2, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));

                if (it++ > 180 / 15) break;

                aiToTarget = RotateVector2(aiToTarget, 15f);
                target = (Vector2) transform.position + aiToTarget;

                aiToTarget2 = RotateVector2(aiToTarget2, -15f);
                target2 = (Vector2) transform.position + aiToTarget2;
            }

            var distanceTargetToPlayer = Vector2.Distance(playerTransform.position, target);
            var distanceTarget2ToPlayer = Vector2.Distance(playerTransform.position, target2);

            if (distanceTargetToPlayer < minDistToPlayer || distanceTarget2ToPlayer < minDistToPlayer)
            {
                cast1 = true;
                cast2 = true;
            }

            offset *= 1.1f;

        } while (cast1 && cast2 && offset < maxOffset);

        if (cast1 && !cast2) return target2;

        if (!cast1 && cast2) return target;

        return Vector2.Distance(transform.position, target) < Vector2.Distance(transform.position, target2) ? target : target2;
    }


    Vector2 GetPositionToShootPlayer()
    {
        return GetPositionToApproachPlayer();
    }


    void StartPath(Vector2 start, Vector2 end, Pathfinding.OnPathDelegate callback)
    {
        lookingAPath = true;
        seeker.StartPath(start, end, (Pathfinding.Path p) => {
            lookingAPath = false;
            callback(p);
        });
    }


    public void FollowPlayerLogicUpdate()
    {
        m_distanceToPlayer = playerTransform.position - transform.position;

        Vector2 target;

        if (followingPath && timeFollowingPathCount >= 3f && !PositionIsAroundPlayer(Vector2.Distance(lastPathNodePos, (Vector2) playerTransform.position)))
        {
            try { StopCoroutine(seekPathRoutine); } catch ( System.NullReferenceException ) {}

            target = GetPositionToAvoidPlayer();
            StartPath(transform.position, target, OnPathFound);
        }
        else if (m_distanceToPlayer.magnitude < minDistToPlayer && !lookingAPath && !followingPath)
        {
            target = GetPositionToAvoidPlayer();
            StartPath(transform.position, target, OnPathFound);
        }
        else if (!lookingAPath && !followingPath)
        {
            target = GetPositionToApproachPlayer();
            StartPath(transform.position, target, OnPathFound);
        }
    }


    void StopSeekingPath()
    {
        if (rb.velocity != Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            StopCoroutine(seekPathRoutine);
        }
    }


    void FinishFollowingPath()
    {
        followingPath = false;
        rb.velocity = Vector2.zero;
    }


    IEnumerator FollowPath(List<Vector3> path)
    {
        timeFollowingPathCount = 0f;

        const float minToReachNode = 0.1f;
        followingPath = true;
        rb.velocity = (path[0] - transform.position).normalized * speed;

        foreach (var node in path)
        {
            while (Vector2.Distance(transform.position, node) > minToReachNode)
            {
                timeFollowingPathCount += Time.deltaTime;
                Vector2 newVelocity = node - transform.position;
                newVelocity.Normalize();
                rb.velocity = newVelocity * speed;
                yield return null;
            }
        }

        FinishFollowingPath();
    }


    void PrintPath(List<Vector3> path)
    {
#if UNITY_EDITOR
        Vector2 lastPos = transform.position;

        foreach (var node in path)
        {
            Vector2 current = new Vector2(node.x, node.y);
            Debug.DrawLine(lastPos, current, Color.green, 1f);
            lastPos = current;
        }
#endif
    }


    void OnPathFound(Pathfinding.Path p)
    {
        lastPathNodePos = p.vectorPath[p.vectorPath.Count - 1];

        if (seekPathRoutine != null) StopSeekingPath();

        seekPathRoutine = StartCoroutine(FollowPath(p.vectorPath));
    }


    bool PositionIsAroundPlayer(float positionDistToPlayer)
    {
        return positionDistToPlayer < maxDistToPlayer && positionDistToPlayer > minDistToPlayer;
    }


   

    bool CanShotToPlayer()
    {
        Vector2 toPlayer = (playerTransform.position - rb.transform.position).normalized;
        Vector2 perp = AIUtils.GetPerpendicular(toPlayer) * projectile.GetComponent<CircleCollider2D>().radius;

        bool firstCast = !Physics2D.Linecast((Vector2) rb.transform.position + perp, (Vector2) playerTransform.position + perp, LayerMask.GetMask("Obstacle"));
        bool secondCast = !Physics2D.Linecast((Vector2) rb.transform.position - perp, (Vector2) playerTransform.position - perp, LayerMask.GetMask("Obstacle"));

        return firstCast && secondCast;
    }


    public void ShootLogicUpdate()
    {
        shootReloadTime -= Time.deltaTime;

        if (CanShotToPlayer())
        {
            if (shootReloadTime <= 0)
            {
                var toPlayer = m_distanceToPlayer.normalized;

                var proj = Instantiate(projectile, (Vector2) transform.position + toPlayer * 1f, Quaternion.identity);

                Vector3 playerPosition = playerTransform.position;
                Vector2 playerVelocity = playerTransform.GetComponent<Rigidbody2D>().velocity;
                Vector3 aiPosition = transform.position;
                float projectileSpeed = projectile.GetComponent<ProyectileMovement>().speed;

                var target = AIUtils.GetPlayerPredictiveTarget( playerPosition, playerVelocity, aiPosition, projectileSpeed );

                proj.GetComponent<Rigidbody2D>().velocity = (target - (Vector2) transform.position).normalized * proj.GetComponent<ProyectileMovement>().speed;

                shootReloadTime = 3f;
            }
        }
        else if (!lookingAPath && !followingPath)
        {
            var target = GetPositionToShootPlayer();
            StartPath(transform.position, target, OnPathFound);
        }
    }


    private void Update()
    {
        m_distanceToPlayer = playerTransform.position - transform.position;
    }
}