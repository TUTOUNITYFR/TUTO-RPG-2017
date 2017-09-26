using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAi : MonoBehaviour {

    //Distance entre le joueur et l'ennemi
    private float Distance;

    // Cible de l'ennemi
    public Transform Target;

    //Distance de poursuite
    public float chaseRange = 10;

    // Portée des attaques
    public float attackRange = 2.2f;

    // Cooldown des attaques
    public float attackRepeatTime = 1;
    private float attackTime;

    // Montant des dégâts infligés
    public float TheDammage;

    // Agent de navigation
    private UnityEngine.AI.NavMeshAgent agent;

    // Animations de l'ennemi
    private Animation animations;

    // Vie de l'ennemi
    public float enemyHealth;
    private bool isDead = false;

    void Start () {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animations = gameObject.GetComponent<Animation>();
        attackTime = Time.time;
    }



    void Update () {

        if (!isDead)
        {

            // On cherche le joueur en permanence
            Target = GameObject.Find("Player").transform;

            // On calcule la distance entre le joueur et l'ennemi, en fonction de cette distance on effectue diverses actions
            Distance = Vector3.Distance(Target.position, transform.position);

            // Quand l'ennemi est loin = idle
            if (Distance > chaseRange)
            {
                idle();
            }

            // Quand l'ennemi est proche mais pas assez pour attaquer
            if (Distance < chaseRange && Distance > attackRange)
            {
                chase();
            }

            // Quand l'ennemi est assez proche pour attaquer
            if (Distance < attackRange)
            {
                attack();
            }

        }
    }

    // poursuite
    void chase()
    {
        animations.Play("walk");
        agent.destination = Target.position;
    }

    // Combat
    void attack()
    {
        // empeche l'ennemi de traverser le joueur
        agent.destination = transform.position;

        //Si pas de cooldown
        if (Time.time > attackTime) {
            animations.Play("hit");
            Target.GetComponent<PlayerInventory>().ApplyDamage(TheDammage);
            Debug.Log("L'ennemi a envoyé " + TheDammage + " points de dégâts");
            attackTime = Time.time + attackRepeatTime;
        }
    }

    // idle
    void idle()
    {
        animations.Play("idle");
    }

    public void ApplyDammage(float TheDammage)
    {
        if (!isDead)
        {
            enemyHealth = enemyHealth - TheDammage;
            print(gameObject.name + "a subit " + TheDammage + " points de dégâts.");

            if(enemyHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        isDead = true;
        animations.Play("die");
        Destroy(transform.gameObject, 5);
    }
}
