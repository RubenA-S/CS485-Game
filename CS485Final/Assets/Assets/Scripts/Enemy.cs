using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //Variables
    public float maxHealth;
    public float health;
    public float movementSpeed;

    Animation anim;

    private GameObject player;

    private bool triggeringPlayer;
    public bool aggro;

    public float attackTimer;
    private float _attackTimer;
    private bool attacked;

    public int aggroDistance;

    public float maxDamage;
    public float minDamage;
    public float damage;

    private Text healthText;
    private Image healthBar;

    public bool isDead;

    private int deathAnimation;

    //healthbar to be toggled on and off
    public Canvas CanvasObject; // Assign in inspector

    //Functions
    void Start()
    {
        maxHealth = 100;
        movementSpeed = 0.025f;
        player = GameObject.FindWithTag("Player");

        //distance between player and the enemy that will initiate combat
        aggroDistance = 10;

        //random number used to determine what death animation will be used
        deathAnimation = UnityEngine.Random.Range(1, 10);

        attackTimer = 2;
        minDamage = 5;
        maxDamage = 10;

        health = maxHealth;
        isDead = false;

        //CanvasObject = GetComponent<Canvas>();
        CanvasObject.GetComponent<Canvas>().enabled = false;


        _attackTimer = attackTimer;

        anim = GetComponent<Animation>();
        Idle();
		
		healthText = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        healthBar = transform.Find("Canvas").Find("Image2").GetComponent<Image>();
    }

    void Update()
    {
		healthText.text = Mathf.RoundToInt(health).ToString();
        healthBar.fillAmount = Mathf.RoundToInt(health) / maxHealth;

        if (health < 0)
            health = 0;

        if (health < 1 && health > 0)
            health = 1;

        if (health <= 0 && !isDead)
        {
            Death();
            //anim.CrossFade("BRB_archer_10_death_A");
        }

        if (!isDead)
        {
            //enemy aggros if player is inside aggro distance
            if (Vector3.Distance(player.transform.position, transform.position) <= aggroDistance)
            {
                if(aggro == false)
                {
                    player.GetComponent<Player>().inCombat += 1;
                }
                aggro = true;
                
                CanvasObject.GetComponent<Canvas>().enabled = true;
            }

            if (aggro)
            {
                FollowPlayer();
            }

            if (triggeringPlayer)
                Attack();

            if (health > maxHealth)
                health = maxHealth;

            
        }
        
        /*
        else
        {
            if (health <= 0)
            {
                if (health < 0)
                    health = 0;
                Death();
            }
        }
        */



    }

    public void Idle()
    {
        anim.CrossFade("undead_idle");
    }

    public void FollowPlayer()
    {
        if (!triggeringPlayer)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed);
            this.transform.LookAt(player.transform);

            anim.CrossFade("undead_walk");
        }

        if (_attackTimer <= 0)
        {
            attacked = false;
            _attackTimer = attackTimer;
        }

        if (attacked)
            _attackTimer -= 1 * Time.deltaTime;

        //Attack();
    }

    public void stopFollowingPlayer()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, transform.position, movementSpeed);
        //this.transform.LookAt(player.transform);

        anim.CrossFade("undead_idle");
    }


    public void Attack()
    {
        this.transform.LookAt(player.transform);

        if (!attacked)
        {
            damage = UnityEngine.Random.Range(minDamage, maxDamage);
            player.GetComponent<Player>().health -= damage;

            attacked = true;
        }

        anim.CrossFade("undead_attack");

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            triggeringPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggeringPlayer = false;
        }
    }

    void Death()
    {
        /*
        Destroy(this);
        Destroy(gameObject);

        
        player.GetComponent<Player>().triggeringEnemy = false;
        player.GetComponent<Player>().attackingEnemy = null;
        player.GetComponent<Player>().followingEnemy = false;
        */








        
        //anim.CrossFade("undead_idle");
        
        stopFollowingPlayer();

        isDead = true;
        aggro = false;
        triggeringPlayer = false;
        attacked = false;

        GetComponent<BoxCollider>().enabled = false;

        switch (deathAnimation)
        {
            case 10:
                anim.CrossFade("BRB_worker_10_death_B");
                break;
            case 9:
                anim.CrossFade("BRB_worker_10_death_A");
                break;
            case 8:
                anim.CrossFade("BRB_spearman_10_death_B");
                break;
            case 7:
                anim.CrossFade("BRB_spearman_10_death_A");
                break;
            case 6:
                anim.CrossFade("BRB_mage_10_death_B");
                break;
            case 5:
                anim.CrossFade("BRB_mage_10_death_A");
                break;
            case 4:
                anim.CrossFade("BRB_infantry_10_death_B");
                break;
            case 3:
                anim.CrossFade("BRB_infantry_10_death_A");
                break;
            case 2:
                anim.CrossFade("BRB_archer_10_death_B");
                break;
            case 1:
                anim.CrossFade("BRB_archer_10_death_A");
                break;
            default:
                anim.CrossFade("BRB_archer_10_death_A");
                break;
        }

        //removes the healthbar after death
        CanvasObject.GetComponent<Canvas>().enabled = false;
        //CanvasObject.enabled = !CanvasObject.enabled;

        player.GetComponent<Player>().inCombat -= 1;

        //player.GetComponent<Player>().maxHealth += 10;
        //player.GetComponent<Player>().Level += 1;
        player.GetComponent<Player>().Exp += 100;

        //Destroy(this);
        //Destroy(gameObject);

        /*
        player.GetComponent<Player>().triggeringEnemy = false;
        player.GetComponent<Player>().attackingEnemy = null;
        player.GetComponent<Player>().followingEnemy = false;
        */
    }
}
