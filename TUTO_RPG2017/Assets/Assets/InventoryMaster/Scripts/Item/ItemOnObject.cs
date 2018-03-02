using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemOnObject : MonoBehaviour                   //Saves the Item in the slot
{
    public Item item;                                       //Item 
    private Text text;                                      //text for the itemValue
    private Image image;

    public ItemDataBaseList itemDatabase;
    private Item currentItem;
    public int itemPrice;

    void Update()
    {
        text.text = "" + item.itemValue;                     //sets the itemValue         
        image.sprite = item.itemIcon;
        GetComponent<ConsumeItem>().item = item;
    }

    void Start()
    {
        currentItem = itemDatabase.getItemByID(item.itemID);
        itemPrice = currentItem.itemValue;

        image = transform.GetChild(0).GetComponent<Image>();
        transform.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;                 //set the sprite of the Item 
        text = transform.GetChild(1).GetComponent<Text>();                                  //get the text(itemValue GameObject) of the item
    }
}
