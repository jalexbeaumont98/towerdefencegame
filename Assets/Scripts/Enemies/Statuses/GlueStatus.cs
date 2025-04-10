using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlueStatus : EnemyStatus
{

    public float speedModifier = 2f;

    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        float speed = enemy.Speed;
        speed = speed/speedModifier;
        enemy.Speed = speed;
        
        return base.SetStatus(enemy);
    }

    public override void RemoveStatus(EnemyBase enemy)
    {
        float speed = enemy.Speed;
        speed = speed*speedModifier;
        enemy.Speed = speed;

        base.RemoveStatus(enemy);
    }
    
}
