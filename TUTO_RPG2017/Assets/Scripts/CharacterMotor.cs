using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMotor : MonoBehaviour {

    // Scripts playerinventory
    PlayerInventory playerInv;

    // Animations du perso
    Animation animations;

    // Vitesse de déplacement
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    // Variables concernant l'attaque
    public float attackCooldown;
    private bool isAttacking;
    private float currentCooldown;
    public float attackRange;
    private GameObject rayHit;

    // Inputs
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumpSpeed;
    CapsuleCollider playerCollider;

    // Le personnage est-il mort ?
    public bool isDead = false;

    // Spells
    [Header("Paramètres des sorts")]
    public int totalSpell;
    private GameObject raySpell;
    private GameObject spellHolderImg;
    private int currentSpell = 1;

    // lightning Spell
    [Header("Paramètres du sort de foudre")]
    public GameObject lightningSpellGO;
    public float lightningSpellCost;
    public float lightningSpellSpeed;
    public int lightningSpellID;
    public Sprite lightningSpellImage;

    // heal spell
    [Header("Paramètres du sort de heal")]
    public GameObject healSpellGO;
    public float healSpellCost;
    public float healSpellAmount;
    public int healSpellID;
    public Sprite healSpellImage;

    void Start () {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
        playerInv = gameObject.GetComponent<PlayerInventory>();
        rayHit = GameObject.Find("RayHit");
        raySpell = GameObject.Find("RaySpell");
        spellHolderImg = GameObject.Find("SpellHolderImg");
    }

    bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.08f, layermask:9);
    }
	
	void Update () {

        if (!isDead)
        {
            // si on avance
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);

                if (!isAttacking)
                {
                    animations.Play("walk");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Spell();
                }
            }

            // Si on sprint
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                animations.Play("run");
            }

            // si on recule
            if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);

                if (!isAttacking)
                {
                    animations.Play("walk");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Spell();
                }
            }

            // rotation à gauche 
            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
            }

            // rotation à droite 
            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }

            // Si on avance pas et que on recule pas non plus
            if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
            {
                if (!isAttacking)
                {
                    animations.Play("idle");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    Spell();
                }
            }

            // Si on saute
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                // Préparation du saut (Nécessaire en C#)
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
                v.y = jumpSpeed.y;

                // Saut
                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }
        }

        // système cooldown
        if (isAttacking)
        {
            currentCooldown -= Time.deltaTime;
        }

        if(currentCooldown <= 0)
        {
            currentCooldown = attackCooldown;
            isAttacking = false;
        }

        // changement de sort avec la molette
        // back
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            if (currentSpell <= totalSpell && currentSpell != 1) {
                currentSpell -= 1;
            }
        }

        // forward
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentSpell >= 0 && currentSpell != totalSpell)
            {
                currentSpell += 1;
            }
        }

        // changement d'image en fonction du sort selectionné
        // Note : condition à recopier pour chaque sort, permet d'afficher la bonne image en fonction du sort selectionné

        if(currentSpell == lightningSpellID)
        {
            spellHolderImg.GetComponent<Image>().sprite = lightningSpellImage;
        }

        if (currentSpell == healSpellID)
        {
            spellHolderImg.GetComponent<Image>().sprite = healSpellImage;
        }

    }

    // Fonction d'attaque
    public void Attack()
    {
        if (!isAttacking)
        {
            animations.Play("attack");

            RaycastHit hit;

            if(Physics.Raycast(rayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
            {
                Debug.DrawLine(rayHit.transform.position, hit.point, Color.red);

                if (hit.transform.tag == "Enemy") {
                    hit.transform.GetComponent<enemyAi>().ApplyDammage(playerInv.currentDamage);
                }

            }
            isAttacking = true;
        }
        
    }

    // Fonction de sorts
    public void Spell()
    {
        // sort de projectile de foudre
        if (currentSpell == lightningSpellID && !isAttacking && playerInv.currentMana >= lightningSpellCost)
        {
            animations.Play("attack");
            GameObject theSpell = Instantiate(lightningSpellGO, raySpell.transform.position, transform.rotation);
            theSpell.GetComponent<Rigidbody>().AddForce(transform.forward * lightningSpellSpeed);
            playerInv.currentMana -= lightningSpellCost;
            isAttacking = true;
        }

        // sort de soin
        if (currentSpell == healSpellID && !isAttacking && playerInv.currentMana >= healSpellCost && playerInv.currentHealth < playerInv.maxHealth)
        {
            animations.Play("attack");
            Instantiate(healSpellGO, raySpell.transform.position, transform.rotation);
            playerInv.currentMana -= healSpellCost;
            playerInv.currentHealth += healSpellAmount;
            isAttacking = true;
        }

    }
}
