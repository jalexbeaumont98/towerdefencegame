using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueStatus : EnemyStatus
{
    

    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        
        return base.SetStatus(enemy);
    }

    protected override void ApplyStatus(EnemyBase enemy)
    {
        float speed = enemy.Speed;
        speed = speed/statusStrength;
        enemy.Speed = speed;

        base.ApplyStatus(enemy);
    }

    public override void RemoveStatus(EnemyBase enemy)
    {
        float speed = enemy.Speed;
        speed = speed*statusStrength;
        enemy.Speed = speed;

        base.RemoveStatus(enemy);
    }

   
    
}
