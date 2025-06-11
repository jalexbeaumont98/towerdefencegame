using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatus : EnemyStatus
{

    private Coroutine poisonRoutine;

    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        if (damage == null)
        {
            damage = GameState.Instance.baseDamage.Clone();
            damage.damage = (int)statusStrength;
            damage.type = "poison";
            damage.critChance = 0f;
        }
        
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
            enemy.TakeDamage(damage); 

            yield return new WaitForSeconds(statusTime); 
        }
    }
}
