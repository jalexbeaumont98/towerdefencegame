using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{

    public bool isTimed;
    public float statusTime = 1f;
    public float statusStrength;

    public string type = "";
    public bool stackable;
    public bool repeats;

    public bool isActive;

    public Damage damage;

    public Sprite sprite;



    public virtual EnemyStatus SetStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log("status set");
        ApplyStatus(enemy);

        return this;
    }

    protected virtual void ApplyStatus(EnemyBase enemy)
    {
        UnityEngine.Debug.Log(type + " status applied");
    }



    public virtual void RemoveStatus(EnemyBase enemy)
    {
        isActive = false;
        UnityEngine.Debug.Log(type + " status removed");
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public virtual void ModifyStatus(Dictionary<string, string> attributes)
    {
        foreach (var pair in attributes)
        {
            switch (pair.Key)
            {
                case "statusStrength":
                    if (float.TryParse(pair.Value, out float strength))
                        statusStrength += strength;
                    break;

                case "statusTime":
                    if (float.TryParse(pair.Value, out float time))
                        statusTime += time;
                    break;

                case "stackable":
                    if (bool.TryParse(pair.Value, out bool isStackable))
                        stackable = isStackable;
                    break;
                case "repeats":
                    if (bool.TryParse(pair.Value, out bool doesRepeat))
                        repeats = doesRepeat;
                    break;


                default:
                    Debug.LogWarning($"Unknown attribute: {pair.Key}");
                    break;
            }
        }
    }
}
