using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class PlayerSlot : Slot
{
    private Player player;

    public Player MyPlayer { get => player; private set => player = value; }

   // public GameObject Item_object = null; //Text�I�u�W�F�N�g


    //�@�A�C�e���f�[�^�x�[�X
    [SerializeField]
    public ItemDataBase itemDataBase;
    //�@�A�C�e�����Ǘ�
    public Dictionary<Item, int> numOfItem = new Dictionary<Item, int>();


    // Start is called before the first frame update
    protected override void Start()
    {
       
        base.Start();

        MyPlayer = FindObjectOfType<Player>();

        
    }

    public override void OnDrop(PointerEventData eventData)
    {

        base.OnDrop(eventData);
        player.SetItem(MyItem);

       
      //  Text Item_text = Item_object.GetComponent<Text>();
     //   Item_text.text = "�A�C�e����:";
    }
    

}
