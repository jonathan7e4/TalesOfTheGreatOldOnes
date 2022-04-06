using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeSystem : MonoBehaviour
{
    public float hp = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float attackStrength) 
    {
        Debug.Log("Me dieron");
        hp -= attackStrength;
    }
}
