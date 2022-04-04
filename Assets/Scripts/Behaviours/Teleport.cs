using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform playerTransform;
    Transform iaTransform;
    Rigidbody2D rb;
    public float offset;
    public static Teleport instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log(instance == null);
        iaTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            TeleportToPLayer();
        }
    }

    public void TeleportToPLayer()
    {
        rb.velocity = Vector3.zero;

        Vector3 playerPosition = playerTransform.position;
        Vector3 target;

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
            
            target = new Vector3(x, y, playerPosition.z);

            cast = Physics2D.OverlapCircle(target, GetComponent<CircleCollider2D>().radius, LayerMask.GetMask("Obstacle"));

        } while (cast && i++<10);

        iaTransform.position = target;
    }
}
