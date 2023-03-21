using UnityEngine;


namespace Inventories
{
    public class PickableItem : MonoBehaviour
    {
        [SerializeField] UnityItemStackRapper _item;

        public UnityItemStackRapper unityItemStack => _item;
        public ItemStack item
        {
            get { return _item.item; }
        }
    }
}
