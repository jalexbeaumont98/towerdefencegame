using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FlameProjectile : ProjectileBase
{
    protected override void Start()
    {
        base.Start();

        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, 0);
        
        
        
    }

    

    protected override void HandlePostCollisionEnemy()
    {
        rb.velocity *= 0.5f;
    }
}
