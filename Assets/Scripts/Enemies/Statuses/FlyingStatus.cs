using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingStatus : EnemyStatus
{
    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        enemy.SetFlying();
        
        return base.SetStatus(enemy);
    }

    public override void RemoveStatus(EnemyBase enemy)
    {
        enemy.SetFlying(false);

        base.RemoveStatus(enemy);
    }
}
