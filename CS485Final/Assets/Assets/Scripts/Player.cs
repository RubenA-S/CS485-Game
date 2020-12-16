using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Variables

    // player
    public float maxHealth;
    public float health;

    public float movementSpeed;
    Animation anim;

    public float attackTimer;
    private float currentAttackTimer;

    private bool moving;
    private bool attacking;
    private bool followingEnemy;

    private float damage;
    public float minDamage;
    public float maxDamage;

    private bool attacked;

    // pmr
    public GameObject playerMovePoint;
    private Transform pmr;
    private bool triggeringPMR;

    // enemy
    private bool triggeringEnemy;
    private GameObject attackingEnemy;

    private Text healthText;
    private Image healthBar;

    private AudioSource audio;

    void Start()
    {
        movementSpeed = 0.05f;
        attackTimer = 1.5f;
        minDamage = 10;
        maxDamage = 25;
        maxHealth = 100;

        health = maxHealth;

        currentAttackTimer = attackTimer;

        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
        pmr.GetComponent<BoxCollider>().enabled = false;
        anim = GetComponent<Animation>();

        healthText = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        healthBar = transform.Find("Canvas").Find("Image2").GetComponent<Image>();

        audio = GetComponent<AudioSource>();
    }

    //Functions
    void Update()
    {
        healthText.text = Mathf.RoundToInt(health).ToString();
        healthBar.fillAmount = Mathf.RoundToInt(health) / maxHealth;

        //cameraFollow();

        //Player Movement
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float hitDistance = 1.0f;

        if(playerPlane.Raycast(ray, out hitDistance))
        {
            Vector3 mousePosition = ray.GetPoint(hitDistance);
            if(Input.GetMouseButtonDown(0))
            {
                moving = true;
                triggeringPMR = false;
                pmr.transform.position = mousePosition;
                pmr.GetComponent<BoxCollider>().enabled = true;

                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.tag == "Enemy")
                    {
                        attackingEnemy = hit.collider.gameObject;
                        followingEnemy = true;
                    }
                    else
                    {
                        attackingEnemy = null;
                        followingEnemy = false;
                    }
                }
            }
        }

        if (moving)
            Move();
        else
            Idle();

        if(triggeringPMR)
        {
            moving = false;
        }

        if (triggeringEnemy)
            Attack();

        if(attacked)
        {
            currentAttackTimer -= 1 * Time.deltaTime;
        }

        if(currentAttackTimer <= 0)
        {
            currentAttackTimer = attackTimer;
            attacked = false;
        }

        if (health <= 0)
        {
            Death();
        }

        //out of combat health regeneration
        if(!attackingEnemy && health < maxHealth)
        {
            health += 5 * Time.deltaTime;
        }

    }

    public void Idle()
    {
        anim.CrossFade("knight_idle");
    }

    public void Move()
    {
        if(followingEnemy)
        {
            if(!triggeringEnemy)
            {
                transform.position = Vector3.MoveTowards(transform.position, attackingEnemy.transform.position, movementSpeed);
                this.transform.LookAt(attackingEnemy.transform);
            }
            
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
            this.transform.LookAt(pmr.transform);
        }
       
        anim.CrossFade("knight_walk");
            
    }

    public void Attack()
    {
        if(!attacked)
        {
            damage = UnityEngine.Random.Range(minDamage, maxDamage);

            if(attackingEnemy)
            {
            	audio.Play();
                attackingEnemy.GetComponent<Enemy>().health -= damage;
            }

            attacked = true;
        }

        if (attackingEnemy)
        {
            transform.LookAt(attackingEnemy.transform);

            attackingEnemy.GetComponent<Enemy>().aggro = true;

            anim.CrossFade("knight_attack");
        }
        else
            Idle();

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PMR")
        {
            triggeringPMR = true;
        }

        if(other.tag == "Enemy")
        {
            triggeringEnemy = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PMR")
        {
            triggeringPMR = false;
        }

        if (other.tag == "Enemy")
        {
            triggeringEnemy = false;
        }
    }

    void Death()
    {
    	Application.LoadLevel(0);
    }
}
