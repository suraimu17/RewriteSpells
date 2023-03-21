using UnityEngine;


namespace Inventories.Input
{
    public class IndexInputSelector : MonoBehaviour, IInventoryIndexProvider
    {
        [SerializeField] private HotBarManager manager;

        public int GetIndex()
        {
            return manager.index;
        }
    }
}
