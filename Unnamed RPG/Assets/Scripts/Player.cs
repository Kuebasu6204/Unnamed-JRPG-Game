using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    


    

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

    void Attack()
    {
        
    }
}
