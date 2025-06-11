using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class BasicFlyingEnemy : EnemyBase
{
    public override bool CheckForObstructions()
    {
        return base.CheckForObstructions();
    }

    public override void Die(bool goal = false)
    {
        base.Die(goal);
    }

    public override NavMeshAgent GetAgent()
    {
        return base.GetAgent();
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
    }

    protected override void AnimateMovement()
    {
        base.AnimateMovement();
    }

    protected override bool CalculateNewPath()
    {
        return base.CalculateNewPath();
    }

    protected override void DebugPath()
    {
        base.DebugPath();
    }

    protected override bool FindPathObstruction()
    {
        return base.FindPathObstruction();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
