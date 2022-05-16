using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : AIBehaviour
{
    Transform playerTransform;
    Transform iaTransform;
    Rigidbody2D rb;
    public float offset;


    public void TeleportToPLayer()
    {
        rb.velocity = Vector3.zero;

        Vector3 playerPosition = playerTransform.position;
        Vector3 target = Vector3.zero;

        bool cast;
        int i=0;

        do
        {
            var x = playerPosition.x;
            var y = playerPosition.y;

            var dice = Random.Range(0, 4);
            switch (dice)
            {
                case 0:
                    x += offset;
                    break;

                case 1:
                    x -= offset;
                    break;

                case 2:
                    y += offset;
                    break;

                case 3:
                    y -= offset;
                    break;
            }

            bool targetOutOfBounds = x >= 19 || x <= -19 || y >= 22.5 || y <= -22.5;

            if (targetOutOfBounds)
            {
                cast = true;
            }
            else {
                target = new Vector3(x, y, playerPosition.z);

                Vector2 targetToPlayer = playerPosition - target;

                cast = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, targetToPlayer, targetToPlayer.magnitude,  LayerMask.GetMask("Obstacle"));
            }

        } while (cast && i++<10);

        if(target != Vector3.zero)
        {
            Debug.DrawLine(target, playerPosition, Color.green, 1f);
            iaTransform.position = target;
        }
        else
            Debug.DrawLine(target, playerPosition, Color.red, 1f);

        rb.velocity = Vector3.zero;
    }


    public override void InitBehaviourData()
    {
        iaTransform = gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void StartBehaviour()
    {
        TeleportToPLayer();
    }

    public override void StopBehaviour()
    {

    }

    public override void UpdateBehaviour()
    {
        
    }

    void Update()
    {
        if (playerTransform == null && PlayerController.instance != null) playerTransform = PlayerController.instance.transform;
    }
}
