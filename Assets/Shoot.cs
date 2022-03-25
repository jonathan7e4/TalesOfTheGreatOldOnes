using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public static Shoot instance;
    public Transform target;
    public GameObject projectile;
    private void Awake()
    {
        instance = this;
    }

    public void ShootProjectile(Transform position)
    {
        StartCoroutine(BigShot(5f, position));
    }

    IEnumerator BigShot(float duration, Transform position)
    {
        float elapsedTime = 0.0f;

        Instantiate(projectile, position.position, Quaternion.identity);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
        }
        return null;
    }
}
