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
        targets.OrderBy(x => x.gameObject.GetComponent<AILifeSystem>().hp); 

        for (int i=0; i < Mathf.Min(targets.Length, maxTargets); i++)
        {
            Collider2D enemy = targets[i];
            Debug.Log(enemy.gameObject.GetComponent<AILifeSystem>() == null);
            enemy.gameObject.GetComponent<AILifeSystem>().getHealed();

            Debug.DrawLine(healerPosition, (Vector2)targets[i].GetComponent<Transform>().position, Color.blue, 1f);
        }
    }
}
