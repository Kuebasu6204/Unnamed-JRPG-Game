using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{

    public bool dead;

    

    public float timer = 0;
    public float maxTime = 5;

    
    
    // Start is called before the first frame update
    void Start()
    {
        timer = maxTime;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if(timer > 2 && timer < 4)
            {
                //attack * 2
            }
            else if(timer < 0)
            {
                //attack / 2
            }
            else
            {
                //attack
            }
        }
    }

    //method for taking damage
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
