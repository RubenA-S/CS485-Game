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

    public float maxDamage;
    public float minDamage;
    public float damage;

    private Text healthText;
    private Image healthBar;

    //Functions
    void Start()
    {
        maxHealth = 100;
        movementSpeed = 0.025f;
        player = GameObject.FindWithTag("Player");

        attackTimer = 2;
        minDamage = 5;
        maxDamage = 10;

        health = maxHealth;


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

        if(aggro)
        {
            FollowPlayer();
        }

        if (triggeringPlayer)
            Attack();

        if(health <= 0)
        {
            Death();
        }
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
		Destroy(this);
        Destroy(gameObject);
    }
}
