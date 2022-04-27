using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Heal : AIBehaviour
{
    Transform aiTransform;
    public float maxDistance;
    public int maxTargets;
    public float healingPower;

    IEnumerator HealEnemies() 
    {
        Vector2 healerPosition = aiTransform.position;

        Collider2D[] targets = Physics2D.OverlapCircleAll(healerPosition, maxDistance, LayerMask.GetMask("Enemy"));
        targets.OrderByDescending(target => target.gameObject.GetComponent<AILifeSystem>().hp);

        for (int i = 0; i < maxTargets && i < targets.Length; i++)
        {
            Collider2D target = targets[i];

            if (target.gameObject == gameObject)
            {
                continue;
            }

            target.GetComponent<AILifeSystem>().getHealed(healingPower);

            Debug.DrawLine(healerPosition, (Vector2)target.GetComponent<Transform>().position, Color.blue, 1f);

            yield return null;
        }
    }

    public override void InitBehaviourData()
    {
        aiTransform = gameObject.transform;
    }

    public override void StartBehaviour()
    {
        StartCoroutine(HealEnemies());
    }

    public override void StopBehaviour()
    {
        
    }

    public override void UpdateBehaviour()
    {
        
    }
}
