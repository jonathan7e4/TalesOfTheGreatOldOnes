using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    public static Dash instance;
    Rigidbody2D rb;
    Transform transform;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    IEnumerator MakeDash(float distance, float speed, float acceleration)
    {
        float traveledDistance = 0.0f;
        var initPosition = transform.position;

        rb.velocity = Vector2.right;

        while (traveledDistance < distance)
        {
            var currentSpeed = rb.velocity.magnitude;
            var newSpeed = Mathf.Min(speed, currentSpeed + acceleration * Time.deltaTime);

            rb.velocity = rb.velocity.normalized * newSpeed;

            var actualPosition = transform.position;
            traveledDistance = (actualPosition - initPosition).magnitude;
            yield return null;
        }

        while (rb.velocity.magnitude > 0f)
        {
            var currentSpeed = rb.velocity.magnitude;
            var newSpeed = Mathf.Max(0f, currentSpeed - acceleration * 2f * Time.deltaTime);

            rb.velocity = rb.velocity.normalized * newSpeed;
            yield return null;
        }
    }

    public void StartDash()
    {
        if (instance == null) { Awake();}
        StartCoroutine(MakeDash(2f, 16f, 32f));
    }
}
