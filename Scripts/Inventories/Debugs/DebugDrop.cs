
using UnityEngine;

using UniRx;

using Game.enemys;

namespace Inventories.Debugs
{
    public class DebugDrop : MonoBehaviour
    {
        [SerializeField] private EnemyStatus status;
        private Inventory inventory;

        [SerializeField] private ItemType type;
        [SerializeField] private string code;
        [SerializeField] private float random;


        public void Start()
        {
            inventory = GameObject.Find("HotBar").GetComponent<Inventory>();

            status._isDead.Where(b => b == true)
                .Subscribe(b =>
                {
                    if(Random.value < random)
                    {
                        ItemStack item = new ItemStack(code, type);
                        inventory.AddItemStack(item);
                    }
                }).AddTo(this);

        }
    }
}
