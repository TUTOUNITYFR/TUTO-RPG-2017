using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItem : MonoBehaviour {

    CharacterMotor charMotor;
    PlayerInventory playerInv;
    Tooltip tooltip;

	// Use this for initialization
	void Start () {
        charMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMotor>();
        playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
    }
	
	public void sellItem(){
        if (Input.GetKey(KeyCode.LeftShift) && charMotor.isInShop)
        {
            playerInv.goldCoins += GetComponent<ItemOnObject>().itemPrice;
            tooltip.deactivateTooltip();
            Destroy(gameObject);
        }
    }
}
