using UnityEngine;

using UniRx;


namespace Inventories.Manager
{
    public class TrushInventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;

        private void Start()
        {
            inventory.observable
                .Where(e => e.NewValue != e.OldValue)
                .Where(e => e.NewValue != null)                 //null‚à–³Ž‹‚·‚é
                .Subscribe(e =>
                {
                    inventory.SetItemStack(null, e.Index);
                }); 
        }
    }

}

