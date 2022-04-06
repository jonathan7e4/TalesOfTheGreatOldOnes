using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float speedScale;


    Rigidbody2D rigidBody2D;

    
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space)) Shake.instance.ShakeIt();


        float maxYDistToPlayer = 1.5f;
        float maxXDistToPlayer = 3f;
        var camToPlayer = player.position - transform.position;

        rigidBody2D.velocity = camToPlayer * speedScale * Time.deltaTime * 100;

        
        Vector3 newPos = transform.position;

        if (camToPlayer.y > maxYDistToPlayer) newPos.y = player.position.y - maxYDistToPlayer;
        if (camToPlayer.x > maxXDistToPlayer) newPos.x = player.position.x - maxXDistToPlayer;
        if (camToPlayer.y < -maxYDistToPlayer) newPos.y = player.position.y + maxYDistToPlayer;
        if (camToPlayer.x < -maxXDistToPlayer) newPos.x = player.position.x + maxXDistToPlayer;

        transform.position = newPos;


        var upLeft = (Vector2)player.transform.position + Vector2.left * maxXDistToPlayer + Vector2.up * maxYDistToPlayer;
        var rightDown = (Vector2)player.transform.position + Vector2.right * maxXDistToPlayer + Vector2.down * maxYDistToPlayer;

        Debug.DrawLine(upLeft, rightDown, Color.red);
    }


    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
}