using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileMovement : MonoBehaviour
{
    public float speed;
    public float directionRotationSpeed;


    Transform player;
    Rigidbody2D rb;
    Vector2 proyectileToPlayer;
    // Start is called before the first frame update
    public float fieldView;
    public float lifeSpan;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>().transform;

        proyectileToPlayer = player.position - rb.transform.position;

        //rb.velocity = proyectileToPlayer.normalized * speed;

    }

    
    // Update is called once per frame
    void Update()
    {
        return;

#pragma warning disable CS0162
        if (lifeSpan <= 0f){
#pragma warning restore CS0162
            Destroy(gameObject);
        }
        else{
            Vector2 dirToPlayer =  (player.position - rb.transform.position).normalized;
            Vector2 currentDirection = rb.velocity.normalized;

            // Vector con la nueva direccion, NO est� normalizado, tiene que medir "speed" metros por segundo
            Vector2 dir = currentDirection + dirToPlayer * Time.deltaTime * directionRotationSpeed;
            dir.Normalize();

            float dot = Vector2.Dot(dir, dirToPlayer);
            float angle = 1 - fieldView/90;

            if(dot < angle)
            {
                return;
            }

            rb.velocity = dir * speed;
        }
        lifeSpan -= Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       Destroy(gameObject);
        if (collision.collider.tag == "Player") {
            Shake.instance.ShakeIt();
        }
       
    }
}
