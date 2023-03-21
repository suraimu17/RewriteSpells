using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Item item;

    public Item MyItem { get => item; private set => item = value; }

    public GameObject Item_desc = null;


    public void SetItem(Item item)
    {
        /*
        MyItem = item;
        Debug.Log("‘•”õƒAƒCƒeƒ€‚Í" + MyItem.MyItemName+"‚Å‚·");
        Text Item_text = Item_desc.GetComponent<Text>();
        Item_text.text =  MyItem.MyItemName+"\n"+MyItem.Information+"\n"+MyItem.kindOfItem1;
        */
    }
}

