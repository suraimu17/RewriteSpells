using UnityEngine;

using Game.Manager;

namespace Inventories
{
    public class ItemLevelSetter : MonoBehaviour
    {
        [SerializeField] private PickableItem item;
        

        public void Start()
        {
            FloorManager manager = FindObjectOfType<FloorManager>();
            item.unityItemStack.ChangeCastableLevel(manager.currentFloor);
            Destroy(this);
        }
    }
}
