using UnityEngine;

using CM.Magic;

namespace Inventories
{
    [RequireComponent(typeof(IInventoryIndexProvider))]
    public class ItemActioner : MonoBehaviour
    {
        //対象のインベントリ
        [SerializeField] private Inventory inventory;
        [SerializeField] private Caster caster;

        private IInventoryIndexProvider provider;


        //初期化
        private void Start()
        {
            provider = GetComponent<IInventoryIndexProvider>();
        }



        //アクション(外部から呼び出す)
        public bool Action()
        {
            if (inventory.maxSlot <= provider.GetIndex()) return false;
            ItemStack item = inventory.GetItemStack(provider.GetIndex());
            if(item != null && item.castable != null)
            {
                float t = caster.Cast(item.castable);
                if (t > 0)
                {
                    //ポーションなら消費を行う
                    Debug.Log(item.type);
                    if(item.type == ItemType.POTION)
                    {
                        inventory.SetItemStack(null, provider.GetIndex());
                    }
                    return true;
                }
            }
            return false;
        }
    }



    //IndexのProvider
    public interface IInventoryIndexProvider
    {
        public int GetIndex();
    }
}
