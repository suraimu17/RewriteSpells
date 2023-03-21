using UnityEngine;

public class GodCheckItem : MonoBehaviour
{
    [SerializeField] private Inventories.Inventory bagInventory;
    [SerializeField] private Inventories.Inventory hotbarInventory;

    public bool HaveSpecialItem()
    {

        for (int slotNum = 0; slotNum < hotbarInventory.maxSlot; slotNum++)
        {
            var item = hotbarInventory.GetItemStack(slotNum);

            if (item?.code == "HpUpMaterial")
            {
                hotbarInventory.SetItemStack(null, slotNum);
                return true;
            }
        }

        for (int slotNum = 0; slotNum < bagInventory.maxSlot; slotNum++)
        {
            var item = bagInventory.GetItemStack(slotNum);

            if (item?.code == "HpUpMaterial")
            {
                bagInventory.SetItemStack(null, slotNum);
                return true;
            }
        }

        return false;
    }
}
