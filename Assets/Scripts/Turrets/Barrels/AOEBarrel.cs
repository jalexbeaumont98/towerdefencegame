using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBarrel : BarrelBase
{

    [SerializeField] protected Transform[] firepoints;
    
    public override void FinalShoot()
    {
        foreach (Transform fp in firepoints)
        {
            turret.ShootFromBarrel(fp);
        }

        
    }

}
