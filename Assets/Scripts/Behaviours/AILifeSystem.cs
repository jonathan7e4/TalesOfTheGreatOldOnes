using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeSystem : MonoBehaviour
{
    public float maxHp = 20f;
    public float hp;
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

    public void getHealed() {
        if (hp < maxHp) {
            hp = Mathf.Min(hp += maxHp / 5, maxHp);
            Debug.Log("Me curaron");
        }
    }
}
