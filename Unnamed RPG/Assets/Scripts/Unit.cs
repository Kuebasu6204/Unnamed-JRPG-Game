using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public int maxHealth;
    public int health;
    public int attack;
    public int heal;
    public int speed;
    public bool isActive = false;
    public BattleHUD unitHUD;
    public Button selectionButton;
    

    public abstract bool takeDamage(int damage);
    public abstract void updateHUD();

}
