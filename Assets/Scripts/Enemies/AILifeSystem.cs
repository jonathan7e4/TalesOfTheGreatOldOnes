using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeSystem : MonoBehaviour
{
    public float maxHp;
    [HideInInspector]
    public float hp;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float attackStrength)
    {
        hp -= attackStrength;
        Debug.Log("Ouch (" + hp + ")");
    }

    public void GetHealed(float hpHealed)
    {
        if (hp < maxHp)
        {
            hp = Mathf.Min(hp += hpHealed, maxHp);
            Debug.Log("Healed" + hp + ")");
        }
    }

    public bool HasFullHp()
    {
        return hp == maxHp;
    }
}