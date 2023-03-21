using System.Collections.Generic;

using UnityEngine;


namespace Inventories
{
    [RequireComponent(typeof(ILevelProvider))]
    public class DefaultItemGenerator : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private List<UnityItemStackRapper> itemList = new List<UnityItemStackRapper>();

        //ƒŒƒxƒ‹
        ILevelProvider level;

        private void Start()
        {
            level = GetComponent<ILevelProvider>();
            foreach(UnityItemStackRapper item in itemList)
            {
                item.ChangeCastableLevel(level.Value);
                inventory.AddItemStack(item.item);
            }
            Destroy(this);
        }
    }
}
