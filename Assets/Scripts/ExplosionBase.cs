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

    [SerializeField] protected List<GameObject> statuses;
    
    protected virtual void Start()
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        if (enemy)
        {
            
            enemy.TakeDamage(damage);
            
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
