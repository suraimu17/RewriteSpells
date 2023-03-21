using System.Collections.Generic;

using UnityEngine;

using UniRx;

using CM.Magic;



namespace Inventories.Manager
{
    public class HotBarInventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Caster caster;

        private List<ICastable> casting = new List<ICastable>();

        private void Start()
        {
            //クールタイム中のアイテムを動かせないように
            inventory.setCondition = (item, index) =>
            {
                //ホットバーにセットしようとしたとき
                if (item != null)
                {
                    if (casting.Contains(item.castable)) return false;
                }

                //ホットバーに別のホットバーからセットしようとしたとき
                if (inventory.GetItemStack(index) != null)
                {
                    if (casting.Contains(inventory.GetItemStack(index).castable)) return false;
                }

                return true;
            };


            //イベント登録
            caster.onCastEvent.Subscribe(e =>
            {
                Spell spell = e.castable as Spell;
                if (spell.coolTime <= 0) return;
                casting.Add(e.castable);

                spell.onEndCoolTime.Subscribe(e =>
                    casting.Remove(spell));
            });
        }
    }
}
