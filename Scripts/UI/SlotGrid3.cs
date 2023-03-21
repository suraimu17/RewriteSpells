using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGrid3 : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private int slotNumber = 3;

    [SerializeField]
    private Item[] allItems;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, this.transform);

            Slot slot = slotObj.GetComponent<Slot>();

            //スロットにアイテムをセットしたい
            if (i < allItems.Length)
            {
                slot.SetItem(allItems[i]);
            }
            //空のスロットにはヌルを表示
            else
            {
                slot.SetItem(null);
            }



        }
    }
}
