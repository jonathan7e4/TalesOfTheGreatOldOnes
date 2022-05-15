using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileBehaviour : MonoBehaviour
{
    public float lifeSpan;
    [HideInInspector]
    public float damage;
    
    void Update()
    {
        if (lifeSpan <= 0f)
            Destroy(gameObject);

        lifeSpan -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") {
            Shake.instance.ShakeIt();
            PlayerController.instance.lifeSystem.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
