using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerSkills : MonoBehaviour {

    public GameObject UIpannel;
    public Text pointsText;

    public int availablePoints;
    public string openKey;

    private bool isOpen;
    private PlayerInventory playerinv;

	// Use this for initialization
	void Start () {
        playerinv = gameObject.GetComponent<PlayerInventory>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(openKey))
        {
            isOpen = !isOpen;
        }

        if (isOpen)
        {
            UIpannel.SetActive(true);
            pointsText.text = "Points disponibles : " + availablePoints;
        }
        else
        {
            UIpannel.SetActive(false);
        }
	}

    public void addHealthMax(float amountHp)
    {
        if(availablePoints >= 1)
        {
            playerinv.maxHealth += amountHp;
            playerinv.currentHealth += playerinv.maxHealth;
            availablePoints -= 1;
        }
    }
}
