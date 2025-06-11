using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ExplosionBase : MonoBehaviour
{

    [Header("References")]
    //[SerializeField] private Rigidbody2D rb;


    [Header("Attributes")]
    [SerializeField] protected int damage;
    [SerializeField] protected Damage damageObj;
    [SerializeField] protected string dtype = "explosion";
    [SerializeField] protected float critChance = 0.3f;

    [SerializeField] protected List<GameObject> statuses;

    protected virtual void Start()
    {

        InitializeDamage();
    }

    protected void InitializeDamage()
    {
        damageObj = GameState.Instance.baseDamage.Clone();
        damageObj.damage = damage;
        damageObj.type = dtype;
        damageObj.critChance = critChance;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        else print("collision not null exp!");

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        if (enemy)
        {
            print("found an enemy exp!");
            enemy.TakeDamage(damageObj);

            foreach (GameObject status in statuses) enemy.AddStatus(status.GetComponent<EnemyStatus>());
            //Destroy(gameObject);
        }

    }

    protected virtual void CheckInsideBounds()
    {

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!isVisible) Destroy(gameObject);
    }

    public virtual void SetStatuses(List<GameObject> statuses)
    {
        this.statuses = statuses;
    }

    public virtual void ExplosionEnd() {
        Destroy(gameObject);
    }
}
