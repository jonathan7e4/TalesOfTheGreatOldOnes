using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : AIBehaviour
{
    public Transform playerTransform;
    public static Dash instance;
    Rigidbody2D rb;
    Transform transform;

    public float dashDistance;
    public float dashSpeed;
    public float acceleration;

    public float dashFactor;

    IEnumerator MakeDash(Vector2 direction, float distance, float speed, float acceleration)
    {
        float traveledDistance = 0.0f;
        var initPosition = transform.position;

        rb.velocity = direction.normalized;

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

    public override void InitBehaviourData()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    public override void StartBehaviour()
    {
        Vector2 toTarget = playerTransform.position - transform.position;
        StartCoroutine(MakeDash(toTarget, 4f, 12f*dashFactor, 16f*dashFactor));
    }

    public override void StopBehaviour()
    {
        
    }

    public override void UpdateBehaviour()
    {
        
    }
}
