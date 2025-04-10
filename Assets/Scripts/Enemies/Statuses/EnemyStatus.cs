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

    public Sprite icon;

    public EnemyStatus(
    bool? isTimed = null,
    float? statusTime = null,
    string type = null,
    bool? stackable = null,
    Sprite icon = null)
    {
        this.isTimed = isTimed ?? false;
        this.statusTime = statusTime ?? 1f;
        this.type = type ?? "";
        this.stackable = stackable ?? false;
        this.icon = icon;
    }

    public virtual EnemyStatus SetStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log("status applied");
        return this;
    }

    public virtual void RemoveStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log("status removed");
    }
}
