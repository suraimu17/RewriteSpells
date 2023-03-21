using UnityEngine;

using CM.Magic;

namespace Inventories
{
    [RequireComponent(typeof(IInventoryIndexProvider))]
    public class ItemActioner : MonoBehaviour
    {
        //�Ώۂ̃C���x���g��
        [SerializeField] private Inventory inventory;
        [SerializeField] private Caster caster;

        private IInventoryIndexProvider provider;


        //������
        private void Start()
        {
            provider = GetComponent<IInventoryIndexProvider>();
        }



        //�A�N�V����(�O������Ăяo��)
        public bool Action()
        {
            if (inventory.maxSlot <= provider.GetIndex()) return false;
            ItemStack item = inventory.GetItemStack(provider.GetIndex());
            if(item != null && item.castable != null)
            {
                float t = caster.Cast(item.castable);
                if (t > 0)
                {
                    //�|�[�V�����Ȃ������s��
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



    //Index��Provider
    public interface IInventoryIndexProvider
    {
        public int GetIndex();
    }
}
