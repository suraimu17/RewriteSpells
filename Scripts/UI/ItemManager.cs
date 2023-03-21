using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
	public static ItemManager instance;
	public GameObject Item_objetct=null;
	private bool isClick = false;
	

	void Awake()
	{
		if (instance == null)
			instance = this;
	}

	//�@�A�C�e���f�[�^�x�[�X
	[SerializeField]
	public ItemDataBase itemDataBase;
	//�@�A�C�e�����Ǘ�
	public Dictionary<Item, int> numOfItem = new Dictionary<Item, int>();

	public void Start()
	{
		for (int i = 0; i < itemDataBase.GetItemLists().Count; i++)
		{
			//�@�A�C�e������K���ɐݒ�
			numOfItem.Add(itemDataBase.GetItemLists()[i], i);

			
				Text Item_text = Item_objetct.GetComponent<Text>();
				//�@�m�F�̈׃f�[�^�o��
				Item_text.text= itemDataBase.GetItemLists()[i].GetItemName() + ": " + itemDataBase.GetItemLists()[i].GetInformation();

			
		}
	}



	public void OnClickObj()
	{
		isClick = true;
		if(isClick==true)
		{
			Debug.Log("���͂����m");


			Text Item_text = Item_objetct.GetComponent<Text>();
		//	Item_text.text = itemDataBase.GetItemLists()[i].GetItemName() + ": " + itemDataBase.GetItemLists()[i].GetInformation();
			isClick = false;

		}
	  
	}

	//�@���O�ŃA�C�e�����擾
	public Item GetItem(string searchName)
	{
		return itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName);
	}

	
}
