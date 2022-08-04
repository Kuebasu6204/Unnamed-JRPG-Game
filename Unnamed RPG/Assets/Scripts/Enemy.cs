using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{


    public override bool takeDamage(int damage)
    {
        this.health = health - damage;
        this.health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0)
        {
            return true;
        }
        else
            return false;
    }

    public override void updateHUD()
    {
        unitHUD.setHUD(this);
    }
}


