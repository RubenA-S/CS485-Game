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
    public float currentAttackTimer;

    public bool moving;
    public bool attacking;
    public bool followingEnemy;

    public float damage;
    public float minDamage;
    public float maxDamage;

    public bool attacked;

    // pmr
    public GameObject playerMovePoint;
    public Transform pmr;
    public bool triggeringPMR;

    // enemy
    public bool triggeringEnemy;
    public GameObject attackingEnemy;

    //public Text healthText;
    //public Image healthBar;

    //public HealthBar healthBar;

    public AudioSource audio;

    //new things
    public GameObject target;

    //used to determine if in combat or not (number of attackers)
    public int inCombat;

    public bool shouldMove;
    public bool run;

    public int runModifier;

    public float Level;
    public float Exp;
    public float maxExp;

    public float levelUpModifier;

    void Start()
    {
        Level = 1;
        Exp = 0;
        maxExp = 100;

        levelUpModifier = 0.25f;

        Level = PlayerPrefs.GetFloat("playerLevel");
        Exp = PlayerPrefs.GetFloat("playerExperience");

        runModifier = 2;
        movementSpeed = 0.05f;
        attackTimer = 1.5f;
        minDamage = 25 + ((Level - 1) * (25 * levelUpModifier));// 10;
        maxDamage = 50 + ((Level - 1) * (50 * levelUpModifier));// 25;
        maxHealth = 100 + ((Level - 1) * (100 * levelUpModifier));

        health = maxHealth;

        currentAttackTimer = attackTimer;

        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
        pmr.GetComponent<BoxCollider>().enabled = false;
        anim = GetComponent<Animation>();

        //healthText = transform.Find("Canvas").Find("Text").GetComponent<Text>();
        //healthBar = transform.Find("Canvas").Find("Image2").GetComponent<Image>();

        //healthBar.SetHealth(health, maxHealth);

        //initialize player out of combat
        inCombat = 0;

        audio = GetComponent<AudioSource>();

        shouldMove = true;
        run = false;
    }

    //Functions
    void Update()
    {
        PlayerPrefs.SetFloat("playerLevel", Level);
        PlayerPrefs.SetFloat("playerExperience", Exp);

        //healthText.text = Mathf.RoundToInt(health).ToString();
        //healthBar.fillAmount = Mathf.RoundToInt(health) / maxHealth;

        //healthBar.SetHealth(health, maxHealth);

        //cameraFollow();

        if(Exp >= maxExp)
        {
            levelUp();
        }

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
                shouldMove = true;

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

                        target = hit.collider.gameObject;
                        triggeringEnemy = false;
                    }
                    else
                    {
                        attackingEnemy = null;
                        followingEnemy = false;


                        target = null;
                        triggeringEnemy = false;
                    }
                }
            }
        }

        //if you kill what you are targeting
        //then deselect the target
        if (attackingEnemy && attackingEnemy.GetComponent<Enemy>().health <= 0)
        {
            shouldMove = false;
            anim.CrossFade("knight_idle");

            attackingEnemy = null;
            followingEnemy = false;


            target = null;
            triggeringEnemy = false;
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

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            if (health < 0)
                health = (float)0;

            Death();
        }

        //out of combat health regeneration
        if(inCombat == 0 && health < maxHealth)
        {
            health += ((maxHealth * 0.01f) * Time.deltaTime);
        }

        //sets run to true when pushing left control
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            run = true;
        }

        //sets run to false when releasing left control
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            run = false;
        }

        //reset character when pushing escape
        if (Input.GetKey("escape"))
        {
            resetCharacter();
        }

    }

    public void Idle()
    {
        anim.CrossFade("knight_idle");
    }

    public void Move()
    {
        if(shouldMove)
        {
            if (followingEnemy)
            {
                if (!triggeringEnemy)
                {
                    if(run)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, attackingEnemy.transform.position, movementSpeed * runModifier);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, attackingEnemy.transform.position, movementSpeed);
                    }
                    
                    this.transform.LookAt(attackingEnemy.transform);
                }

            }
            if (!followingEnemy)
            {
                if (!triggeringEnemy)
                {
                    if(run)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed * runModifier);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
                    }
                        
                    this.transform.LookAt(pmr.transform);
                }

            }

            if(run)
            {
                anim.CrossFade("BRB_infantry_03_run");
            }
            else
            {
                anim.CrossFade("knight_walk");
            }
            
        }
        
       
        
            
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

            followingEnemy = false;

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
            shouldMove = false;
        }
        if(target != null)
        {
            if (other == target.GetComponent<Collider>())
            {
                //if (other.tag == "Enemy")
                triggeringEnemy = true;
                shouldMove = false;
            }
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
        //reset level and experience
        PlayerPrefs.SetFloat("playerLevel", 1);
        Level = 1;
        PlayerPrefs.SetFloat("playerExperience", 0);
        Exp = 0;

        Application.LoadLevel(0);
    }

    void levelUp()
    {
        Exp = Exp - maxExp;
        Level++;

        minDamage = minDamage + (25 * levelUpModifier);
        maxDamage = maxDamage + (50 * levelUpModifier);
        maxHealth = maxHealth + (100 * levelUpModifier);
    }

    void resetCharacter()
    {
        //reset level and experience
        PlayerPrefs.SetFloat("playerLevel", 1);
        Level = 1;
        PlayerPrefs.SetFloat("playerExperience", 0);
        Exp = 0;

        minDamage = 25 + ((Level - 1) * (25 * levelUpModifier));// 10;
        maxDamage = 50 + ((Level - 1) * (50 * levelUpModifier));// 25;
        maxHealth = 100 + ((Level - 1) * (100 * levelUpModifier));

        health = maxHealth;
    }
}
