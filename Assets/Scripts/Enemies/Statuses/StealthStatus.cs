using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthStatus : EnemyStatus
{
    public override EnemyStatus SetStatus(EnemyBase enemy)
    {   
        enemy.SetStealth();
        
        return base.SetStatus(enemy);
    }

    public override void RemoveStatus(EnemyBase enemy)
    {
        enemy.SetStealth(false);

        base.RemoveStatus(enemy);
    }
}
