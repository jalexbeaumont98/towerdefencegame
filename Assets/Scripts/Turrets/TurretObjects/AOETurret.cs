using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOETurret : TurretBase
{
    protected override void Update()
    {

        if (updateTarget)
        {
            target = null;
            updateTarget = false;
        }
        
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (target != null)
        {

            RotateTowardsTarget();

            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / fireRate)
            {
                if (isWithinTargettingAngleRange)
                {
                    Shoot();
                    FindTarget();
                }

                timeUntilFire = 0;
            }
        }

        if (!CheckTargetIsInRange()) target = null;

    }

    protected override void RotateTowardsTarget()
    {

        isWithinTargettingAngleRange = true;

    }

    protected override void FindTarget()
    {

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length <= 0) return;

        target = GetFirst(hits);


    }

    protected override Transform GetFirst(RaycastHit2D[] hits)
    {

        foreach (RaycastHit2D hit in hits)
        {
            if (EnsureTargetCanBeTargeted(hit.transform)) return hit.transform;
        }

        return null;
    }

    public override void ShootFromBarrel(Transform fp)
    {
        if (!target) return;

        GameObject _bullet = Instantiate(bullet, fp.position, fp.rotation);
        ProjectileBase bulletCon = _bullet.GetComponent<ProjectileBase>();

        bulletCon.SetTarget(target, -fp.up);
    }



}
