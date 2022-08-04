using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYER, ENEMY, WIN, LOSE}
public enum Action { ATTACK, HEAL, OTHER}



public class BattleSystem : MonoBehaviour
{
    public List<Player> party;
    public List<Player> players;
    public List<Enemy> enemies;

    public List<Unit> everyone;

    public Text dialogueText;

    public List<BattleHUD> playerHUDs;
    public List<BattleHUD> enemyHUDs;

    public GameObject qte;
    public int turn;

    public Unit activeUnit;
    public Enemy targetEnemy;
    public Player targetPlayer;


    

    public BattleState state;
    public Action action;

    // Start is called before the first frame update
    private void Start()
    {
        //need to have them all be active before deactivating/activating again through code
        foreach (BattleHUD hud in playerHUDs)
        {
            hud.gameObject.SetActive(false);
        }

        foreach (BattleHUD hud in enemyHUDs)
        {
            hud.gameObject.SetActive(false);
        }
        //deactivate the buttons too

        int i = 0;
        foreach(Player p in party)
        {
            players.Add(p);
            i++;
            if (i >= 4)
            {
                break;
            }
        }
        StartBattle(players, enemies);
    }

    public void StartBattle(List<Player> players, List<Enemy> enemy)
    {
        state = BattleState.START;
        StartCoroutine(SetUpBattle());
        
    }

    //Sets up the screen for the battle system
    IEnumerator SetUpBattle()
    {
        //switch statement for the dialogue box
        int x = enemies.Count;
        switch (x)
        {
            case 1:
                dialogueText.text = "A lone " + enemies[0].unitName + " appears!";
                break;
            default:
                dialogueText.text = "A group of enemies attack!";
                break;
        }

        int i = 0;
        //foreach player and enemy respectively setup their HUD
        foreach(Player player in players)
        {
            
            player.selectionButton.gameObject.SetActive(false);//throws exception at this line
            
            everyone.Add(player);
            
            playerHUDs[i].gameObject.SetActive(true);
            
            playerHUDs[i].setHUD(player);
            
            player.unitHUD = playerHUDs[i];
            
            i++;

        }

        i = 0;

        foreach(Enemy enemy in enemies)
        {
            enemy.selectionButton.gameObject.SetActive(false);
            everyone.Add(enemy);
            enemyHUDs[i].gameObject.SetActive(true);
            enemyHUDs[i].setHUD(enemy);
            enemy.unitHUD = enemyHUDs[i];
            i++;
        }


        //sort everyone by their speed

        everyone.Sort((y, x) => x.speed.CompareTo(y.speed));



        //start the battle with the fastest unit going first

        yield return new WaitForSeconds(3f);


        turn = 0;

        if(everyone[turn].tag.Equals("Player"))
        {
            state = BattleState.PLAYER;
            activeUnit = everyone[turn];
            PlayerTurn();
            //start player turn
        }
        else if(everyone[turn].tag.Equals("Enemy"))
        {
            state = BattleState.ENEMY;
            activeUnit = everyone[turn];
            StartCoroutine(EnemyTurn());
        }
        else
        {
            Debug.Log("Uh oh");
            //something is wrong here
        }
        

    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose!";
    }

    public void OnAttack()
    {
        action = Action.ATTACK;
        if (state != BattleState.PLAYER)
            return;

        //attack has been selected and now if there are > 1 targets
        if(enemies.Count > 1)
        {
            dialogueText.text = "Who will " + activeUnit.unitName + " attack?";

            //setting up the buttons and the listeners for each button
            foreach (Enemy e in enemies)
            {
                Button b = e.selectionButton;
                b.gameObject.SetActive(true);
                b.onClick.AddListener(() => SelectingTargetEnemy(e, enemies));

            }
        }
        else
        {
            targetEnemy = enemies[0];
            StartCoroutine(Attack());
        }

        
    }

    //targeted unit = unit connected to specific button on click
    private void SelectingTargetEnemy(Enemy e, List<Enemy> enemies)
    {
        targetEnemy = e;

        //once enemy has been selected disable all buttons and then initiate attack
        foreach(Enemy enemy in enemies)
        {
            Button b = enemy.selectionButton;
            b.onClick.RemoveAllListeners();
            b.gameObject.SetActive(false);
        }
        StartCoroutine(Attack());
    }

    public void OnHeal()
    {
        action = Action.HEAL;
        if (state != BattleState.PLAYER)
            return;

        //attack has been selected and now if there are > 1 targets
        if (players.Count > 1)
        {
            //setting up the buttons and the listeners for each button
            foreach (Player p in players)
            {
                Button b = p.selectionButton;
                b.gameObject.SetActive(true);
                b.onClick.AddListener(() => SelectingTargetPlayer(p, players));

            }
        }
        else
        {
            targetPlayer = (Player)activeUnit;
            StartCoroutine(Heal());
        }
    }

    //targeted unit = unit connected to specific button on click
    private void SelectingTargetPlayer(Player p, List<Player> players)
    {
        targetPlayer = p;

        //once enemy has been selected disable all buttons and then initiate attack
        foreach (Player player in players)
        {
            Button b = player.selectionButton;
            b.onClick.RemoveAllListeners();
            b.gameObject.SetActive(false);
        }
        StartCoroutine(Heal());
    }

    // Player attacks enemy
    IEnumerator Attack()
    {

        Vector3 p = activeUnit.GetComponent<Transform>().position;
        Vector3 e = targetEnemy.GetComponent<Transform>().position;



        activeUnit.GetComponent<Transform>().position = new Vector3(e.x - 2, e.y, e.z);

        yield return new WaitForSeconds(1);

        GameObject prompt = Instantiate(qte, new Vector3(activeUnit.transform.position.x - 2.2f, activeUnit.transform.position.y + 2.5f, activeUnit.transform.position.z), Quaternion.identity);
        prompt.transform.SetParent(activeUnit.gameObject.transform, false);


        bool isDead;

        prompt.GetComponent<QTESystem>().onQTE();

        yield return new WaitUntil(prompt.GetComponent<QTESystem>().CheckIfDone);


        bool attackHits = prompt.GetComponent<QTESystem>().success;
        Destroy(prompt);

        if (attackHits)
        {
            isDead = targetEnemy.GetComponent<Unit>().takeDamage(activeUnit.GetComponent<Unit>().attack);
            targetEnemy.updateHUD();
            dialogueText.text = targetEnemy.GetComponent<Unit>().unitName + " recieved damage!";

            yield return new WaitForSeconds(1);

        }
        else
        {
            isDead = false;
            dialogueText.text = "Attack missed!";

            yield return new WaitForSeconds(1);
        }

        activeUnit.GetComponent<Transform>().position = p;
        dialogueText.text = "";


        yield return new WaitForSeconds(.5f);

        if (isDead)
        {

            enemies.Remove(targetEnemy);
            everyone.Remove(targetEnemy);

            if(enemies.Count <= 0)
            {
                state = BattleState.WIN;
                dialogueText.text = "YOU WIN!!!";
                yield break;
            }

            if (turn >= everyone.Count)
            {
                turn = -1;
            }

        }

        turn++;
        if (turn >= everyone.Count)
        {
            turn = 0;
        }

        if (everyone[turn].tag.Equals("Player"))
        {
            state = BattleState.PLAYER;
            activeUnit = everyone[turn];
            PlayerTurn();
        }
        else if (everyone[turn].tag.Equals("Enemy"))
        {
            state = BattleState.ENEMY;
            activeUnit = everyone[turn];
            StartCoroutine(EnemyTurn());
        }
        else
        {
            //something is wrong here
        }

    }

    IEnumerator Heal()
    {
        targetPlayer.GetComponent<Unit>().health += 10;//later on change it for a variable of some sort (for item or magic heal)
        targetPlayer.GetComponent<Unit>().health = Mathf.Clamp(targetPlayer.GetComponent<Unit>().health, 0, targetPlayer.GetComponent<Unit>().maxHealth);
        targetPlayer.updateHUD();
        //update the hud later

        dialogueText.text = activeUnit.unitName + " healed " + targetPlayer.unitName + "!";//later on add how much points were healed

        yield return new WaitForSeconds(1);

        turn++;
        if (turn >= everyone.Count)
        {
            turn = 0;
        }

        if (everyone[turn].tag.Equals("Player"))
        {
            state = BattleState.PLAYER;
            activeUnit = everyone[turn];
            PlayerTurn();
        }
        else if (everyone[turn].tag.Equals("Enemy"))
        {
            state = BattleState.ENEMY;
            activeUnit = everyone[turn];
            StartCoroutine(EnemyTurn());
        }
        else
        {
            //something is wrong here
        }
    }

    IEnumerator EnemyTurn()
    {
        Vector3 p;
        Vector3 e = activeUnit.GetComponent<Transform>().position;
        int rand;

        //randomly chooses player to attack
        if (players.Count > 1)
        {
            rand = Random.Range(0, players.Count - 1);
        }
        else
        {
            rand = 0;
        }
        targetPlayer = players[rand];
        p = targetPlayer.GetComponent<Transform>().position;


        activeUnit.GetComponent<Transform>().position = new Vector3(p.x + 2, p.y, p.z);

        yield return new WaitForSeconds(1);

        dialogueText.text = activeUnit.GetComponent<Unit>().unitName + " is attacking!!";

        yield return new WaitForSeconds(1);

        GameObject prompt = Instantiate(qte, new Vector3(targetPlayer.transform.position.x + 2.2f, targetPlayer.transform.position.y + 2.5f, targetPlayer.transform.position.z), Quaternion.identity);
        prompt.transform.SetParent(targetPlayer.gameObject.transform, false);

        prompt.GetComponent<QTESystem>().onQTE();

        yield return new WaitUntil(prompt.GetComponent<QTESystem>().CheckIfDone);

        bool isDead;

        bool dodgeSuccess = prompt.GetComponent<QTESystem>().success;
        Destroy(prompt);

        if(!dodgeSuccess)
        {
            isDead = targetPlayer.GetComponent<Unit>().takeDamage(activeUnit.GetComponent<Unit>().attack);
            targetPlayer.updateHUD();
            dialogueText.text = targetPlayer.GetComponent<Unit>().unitName + " recieved damage!!";

            yield return new WaitForSeconds(1);
        }
        else
        {
            isDead = false;
            dialogueText.text = targetPlayer.GetComponent<Unit>().name + " dodged!";

            yield return new WaitForSeconds(1);
        }
        

        activeUnit.GetComponent<Transform>().position = e;
        dialogueText.text = "";

        yield return new WaitForSeconds(.5f);

        if (isDead)
        {
            //remove from player list
            players.Remove(targetPlayer);
            turn = -1;

            //if all players are dead then its game over
            if(players.Count <= 0)
            {
                state = BattleState.LOSE;
                dialogueText.text = "YOU LOSE!!";
                yield break;
            }
            
        }

        turn++;
        if (turn >= everyone.Count)
        {
            turn = 0;
        }

        if (everyone[turn].tag.Equals("Player"))
        {
            state = BattleState.PLAYER;
            activeUnit = everyone[turn];
            PlayerTurn();
        }
        else if (everyone[turn].tag.Equals("Enemy"))
        {
            state = BattleState.ENEMY;
            activeUnit = everyone[turn];
            StartCoroutine(EnemyTurn());
        }
        else
        {
            //something is wrong here
        }

    }

}
