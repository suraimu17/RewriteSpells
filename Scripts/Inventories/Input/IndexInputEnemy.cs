using UnityEngine;


namespace Inventories.Input
{
    public class IndexInputEnemy : MonoBehaviour, IInventoryIndexProvider
    {
        public int index;

        public int GetIndex()
        {
            return index;
        }
    }
}
