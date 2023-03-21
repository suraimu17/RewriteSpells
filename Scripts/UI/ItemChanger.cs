using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChanger : MonoBehaviour
{
    //Dropdownを格納する変数
    [SerializeField] private Dropdown dropdown;
    //Cubeを格納する変数
    [SerializeField] private GameObject cube;



    // オプションが変更されたときに実行するメソッド
    public void ItemChanger1()
    {
        //DropdownのValueが0のとき（アイテムが選択されているとき）
        if (dropdown.value == 0)
        {
            cube.GetComponent<Image>().color = new Color(1,1,1,1);


            //アイテムのアイコンを表示
        }
        //DropdownのValueが1のとき（リライトスペルが選択されているとき）
        else if (dropdown.value == 1)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,0);

        }
        //DropdownのValueが2のとき（リライトアイテムが選択されているとき）
        else if (dropdown.value == 2)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,1);

        }
        //DropdownのValueが3のとき（ポーションが選択されているとき）
        else if (dropdown.value == 3)
        {
            cube.GetComponent<Image>().color = new Color(0,0,0,0);

        }
        else if (dropdown.value == 4)
        {
            cube.GetComponent<Image>().color = new Color(1,1,1,1);

        }

    }
}

