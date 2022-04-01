using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PAttack : MonoBehaviour
{
    public int maxHealth = 100;
    public int health = 100;

    public Slider healthBar;

    public int attack = 10;

    public float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = health;
        timer = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
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
}
