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

	//　アイテムデータベース
	[SerializeField]
	public ItemDataBase itemDataBase;
	//　アイテム数管理
	public Dictionary<Item, int> numOfItem = new Dictionary<Item, int>();

	public void Start()
	{
		for (int i = 0; i < itemDataBase.GetItemLists().Count; i++)
		{
			//　アイテム数を適当に設定
			numOfItem.Add(itemDataBase.GetItemLists()[i], i);

			
				Text Item_text = Item_objetct.GetComponent<Text>();
				//　確認の為データ出力
				Item_text.text= itemDataBase.GetItemLists()[i].GetItemName() + ": " + itemDataBase.GetItemLists()[i].GetInformation();

			
		}
	}



	public void OnClickObj()
	{
		isClick = true;
		if(isClick==true)
		{
			Debug.Log("入力を検知");


			Text Item_text = Item_objetct.GetComponent<Text>();
		//	Item_text.text = itemDataBase.GetItemLists()[i].GetItemName() + ": " + itemDataBase.GetItemLists()[i].GetInformation();
			isClick = false;

		}
	  
	}

	//　名前でアイテムを取得
	public Item GetItem(string searchName)
	{
		return itemDataBase.GetItemLists().Find(itemName => itemName.GetItemName() == searchName);
	}

	
}
