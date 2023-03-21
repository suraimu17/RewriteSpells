using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="Item",menuName="Items/item")]       //?A?Z?b?g???j???[???N???G?C?g???AItem??????????
public class Item : ScriptableObject    
{
    public static Item Instance;

    public void Awake()
    {
        if(Instance==null)
        Instance = this;
    }

    public enum KindOfItem
    {
        アイテム,
        リライトアイテム,
        リライトスペル,
        ポーション
    }


    //?@?A?C?e????????
    [SerializeField]
    public KindOfItem kindOfItem;
    //?@?A?C?e????????
    [SerializeField]
    public string information;
    [SerializeField]                
    public string itemName;        //?A?C?e???????O??????
    [SerializeField]
    public Sprite itemImage;       //?A?C?e?????A?C?R????????
    

  

    public string MyItemName { get => itemName;  }               //?J?v?Z????
    public Sprite MyItemImage { get => itemImage; }              //?J?v?Z???? 
    public string Information { get => information; set => information = value; }
    public KindOfItem kindOfItem1 { get => kindOfItem; set => kindOfItem = value; }

    public KindOfItem GetKindOfItem()
    {
        return kindOfItem;
    }

    public Sprite GetItemImage()
    {
        return itemImage;
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetInformation()
    {
        return information;
    }
}
