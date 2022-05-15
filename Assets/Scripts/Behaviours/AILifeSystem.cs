using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeSystem : MonoBehaviour
{
    public float maxHp = 1000f;
    public float hp;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float attackStrength) 
    {
        //Debug.Log("Me dieron");
        hp -= attackStrength;
    }

    public void getHealed() {
        if (hp < maxHp) {
            hp = Mathf.Min(hp += maxHp / 5, maxHp);
            //Debug.Log("Me curaron");
        }
    }
}
