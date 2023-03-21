using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventories
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;


        private void OnTriggerEnter(Collider collider)
        {
            PickableItem pickbale = collider.GetComponent<PickableItem>();
            if (pickbale)
            {
                inventory.AddItemStack(pickbale.item);
                Destroy(pickbale.gameObject);
            }
        }
    }
}
