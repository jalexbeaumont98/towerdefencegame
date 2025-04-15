using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    public bool isTimed;
    public float statusTime = 1f;

    public string type = "";
    public bool stackable;

    public Sprite sprite;



    public virtual EnemyStatus SetStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log("status applied");
        return this;
    }

    public virtual void RemoveStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log("status removed");
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
