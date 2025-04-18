using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatus : EnemyStatus
{

    private Coroutine poisonRoutine;

    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        
        return base.SetStatus(enemy);
    }

    protected override void ApplyStatus(EnemyBase enemy)
    {
        isActive = true;
        poisonRoutine = enemy.StartCoroutine(ApplyPoison(enemy));

        base.ApplyStatus(enemy);
    }


    public override void RemoveStatus(EnemyBase enemy)
    {
        base.RemoveStatus(enemy);
    }

     private IEnumerator ApplyPoison(EnemyBase enemy)
    {
        while (isActive && enemy != null && enemy != null)
        {
            // Apply poison effect 
            enemy.TakeDamage((int)statusStrength); 

            yield return new WaitForSeconds(statusTime); 
        }
    }
}
