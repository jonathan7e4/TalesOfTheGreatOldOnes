using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeSystem : MonoBehaviour
{
    public float maxHp = 1000f;
    public float hp;

    public HealthBar healthBar;

    void Start()
    {
        hp = maxHp;        
        healthBar.SetMaxHealth(maxHp);
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
        healthBar.SetHealth(hp);
    }

    public void getHealed() {
        if (hp < maxHp) {
            hp = Mathf.Min(hp += maxHp / 5, maxHp);
            healthBar.SetHealth(hp);
            //Debug.Log("Me curaron");
        }
    }
}
