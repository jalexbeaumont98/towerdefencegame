using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Damage
{
    public int damage;
    public string type;
    public float critChance;

    public Damage Clone()
    {
        return new Damage
        {
            damage = this.damage,
            type = this.type,
            critChance = this.critChance
        };
    }
}
