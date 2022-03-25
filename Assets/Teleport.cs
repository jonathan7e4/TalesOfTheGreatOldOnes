using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform playerTransform;
    Transform iaTransform;
    public float verticalOffset;
    public float horizontalOffset;
    public static Teleport instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        iaTransform = GetComponent<Transform>();

    }

    public void TeleportToPLayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 target;
        bool cast;

        do
        {
            var x = 0f;
            var y = 0f;

            var dice = Random.Range(1, 5);
            Debug.Log(dice);
            switch (dice)
            {
                case 1:
                    x = playerPosition.x + horizontalOffset;
                    break;

                case 2:
                    x = playerPosition.x - horizontalOffset;
                    break;

                case 3:
                    y = playerPosition.y + verticalOffset;
                    break;

                case 4:
                    y = playerPosition.y - verticalOffset;
                    break;
            }
                                        //default z
            target = new Vector3(x, y, 1);

            cast = Physics2D.CircleCast(target, GetComponent<CircleCollider2D>().radius, Vector2.right, LayerMask.GetMask("Obstacle"));
            

        } while (cast);

        iaTransform.position = target;
    }
}
