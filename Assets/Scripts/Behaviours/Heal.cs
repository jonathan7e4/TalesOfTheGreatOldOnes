using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Heal : MonoBehaviour
{
    Transform aiTransform;
    public float maxDistance;
    public int maxTargets;
    // Start is called before the first frame update
    void Start()
    {
        aiTransform = GetComponent<Transform>();
    }

    public void HealEnemies() {
        if (aiTransform == null) {Start();}
        
        Vector2 healerPosition = aiTransform.position;

        Collider2D[] targets = Physics2D.OverlapCircleAll(healerPosition, maxDistance, LayerMask.GetMask("Enemy"));
        targets.OrderByDescending(x => x.gameObject.GetComponent<AILifeSystem>().hp);


        for (int i = 0; i < maxTargets; i++)
        {
            Collider2D target = targets[i];

            if (target.gameObject == gameObject)
                continue;

            target.GetComponent<AILifeSystem>().getHealed();

            Debug.DrawLine(healerPosition, (Vector2)target.GetComponent<Transform>().position, Color.blue, 1f);
        }
    }
}
