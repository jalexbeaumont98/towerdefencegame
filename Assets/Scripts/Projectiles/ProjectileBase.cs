using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProjectileBase : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected BoxCollider2D col;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected GameObject impact_explosion;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected Animator anim;
    [SerializeField] public Sprite icon;
    [SerializeField] protected List<GameObject> statuses;
    [SerializeField] protected Damage damageObj;
    [SerializeField] protected Transform expIP; //explosion image point ie where to spawn it


    [Header("Attributes")]
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected int damage;
    [SerializeField] protected string dtype = "basic";
    [SerializeField] protected float critChance = 0.1f;
    [SerializeField] protected bool autoTargeting;

    [SerializeField] protected bool canHitFlying;
    [SerializeField] protected string upgradeNotes = "";

    [SerializeField] protected bool destroyAfterDistance = true;

    [SerializeField] protected float maxTravelDistance = 1000f;

    [SerializeField] protected bool destroyOnTower = false;

    [SerializeField] protected float destroyOnTowerDistance = 10f;

    [SerializeField] protected Vector3 spawnPosition;
    [SerializeField] protected bool destroyAfterAnimation = false;


    protected Transform target;
    protected bool velocitySet;
    protected Vector2 direction;

    protected float distanceTravelled = 0f;




    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;

        InitializeDamage();


        spawnPosition = transform.position;

        if (destroyAfterAnimation) StartCoroutine(DestroyAfterAnimation());

    }

    protected virtual void InitializeDamage()
    {
        damageObj = GameState.Instance.baseDamage.Clone();
        damageObj.damage = damage;
        damageObj.type = dtype;
        damageObj.critChance = critChance;

        if (damageObj.type != "basic") print("damge type is " + damageObj.type);
    }

    public virtual void SetTarget(Transform _target, Vector2 _direction)
    {
        target = _target;

        if (!autoTargeting)
        {
            direction = _direction;
        }
    }

    public virtual void SetStatuses(List<GameObject> statuses)
    {
        this.statuses = statuses;
    }

    public virtual float[] GetStats()
    {
        float[] stats = new float[3];

        stats[0] = damage;
        stats[1] = bulletSpeed;
        stats[2] = 0;

        if (autoTargeting) stats[2] = 1;


        return stats;


    }

    public virtual string GetUpgradeNotes()
    {
        return upgradeNotes;
    }

    public virtual float GetSpeed()
    {
        return bulletSpeed;
    }

    public virtual bool GetAutoTargeting()
    {
        return autoTargeting;
    }

    public virtual void Update()
    {




    }


    protected virtual void FixedUpdate()
    {

        CheckInsideBounds();


        distanceTravelled = Vector3.Distance(spawnPosition, transform.position);

        if (distanceTravelled >= maxTravelDistance * GameState.Instance.tileSize)
        {
            DestroyProjectile();
        }


        if (destroyOnTower && false) //WIP
        {

            Vector2 bulletCenter = (Vector2)transform.position;
            Vector2 bulletSize = col.size;  // Get the size of the BoxCollider2D

            // Perform a BoxCast in the bullet's movement direction
            RaycastHit2D hit = Physics2D.BoxCast(
                bulletCenter, bulletSize, 0f, direction, distanceTravelled, wallLayer
            );

            if (hit.collider != null)
            {
                if (distanceTravelled >= destroyOnTowerDistance * GameState.Instance.tileSize)
                {


                    DestroyProjectile();
                    return;
                }


            }


        }

        if (autoTargeting)
        {
            if (!target) return;

            direction = (target.position - transform.position).normalized;

            rb.velocity = direction * bulletSpeed;

        }

        else if (!velocitySet)
        {
            rb.velocity = direction * bulletSpeed;
            velocitySet = true;
        }



    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        if (enemy)
        {
            if (enemy.Flying && !canHitFlying) return;

            if (impact_explosion)
            {
                Vector3 position;
                
                if (expIP) position = expIP.position;

                else position = transform.position;

                Instantiate(impact_explosion, position, quaternion.identity).GetComponent<ExplosionBase>().SetStatuses(statuses);

            }

            enemy.TakeDamage(damageObj);

            foreach (GameObject status in statuses) enemy.AddStatus(status.GetComponent<EnemyStatus>());

            HandlePostCollisionEnemy();
        }
        /*

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            //Debug.Log("Bullet hit a wall!");

            if (destroyOnTower)
            {
                

                if (distanceTravelled >= destroyOnTowerDistance * GameState.Instance.tileSize)
                {
                    DestroyProjectile();
                }
            }

        }
        */

    }

    protected virtual void HandlePostCollisionEnemy()
    {
        DestroyProjectile();
    }

    protected virtual void CheckInsideBounds()
    {

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (isVisible) return;

        if (!GameState.Instance.levelBounds.Contains(new Vector2(transform.position.x, transform.position.y))) DestroyProjectile();

    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        DestroyProjectile();
    }

    protected virtual void DestroyProjectile()
    {

        Destroy(gameObject);
    }
}
