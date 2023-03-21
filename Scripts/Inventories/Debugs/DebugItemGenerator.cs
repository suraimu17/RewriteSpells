using UnityEngine;

using CM.Magic.Temp;

namespace Inventories.Debugs
{
    public class DebugItemGenerator : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;

        private void Update()
        {
            inventory.AddItemStack(new ItemStack("SPL_FireBall", ItemType.SPELL, MagicLoader.loader.GetSpell("SPL_FireBall", 100)));
            inventory.AddItemStack(new ItemStack("SUP_Explode", ItemType.SUPPORT));
            inventory.AddItemStack(new ItemStack("SPL_Teleport", ItemType.SPELL));
            Destroy(this);
        }
    }
}