using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusData
{
    public EnemyStatus status;
    public int stackCount;

    public GameObject icon;

    public EnemyStatusData(EnemyStatus status, GameObject icon = null, int stackCount = 1)
    {
        this.status = status;
        this.stackCount = stackCount;
        this.icon = icon;
    }

    public EnemyStatusData CloneStatusData()
    {
        return new EnemyStatusData(this.status, this.icon, this.stackCount);
    }
}
