using UnityEngine;

using UniRx;
using System.Collections.Generic;


using Inventories.Parameters;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories.Debugs
{
    public class DebugEquipment : MonoBehaviour
    {
        //対象インベントリ
        [SerializeField] private Inventory inventory;

        //対象キャスター
        [SerializeField] private Caster caster;
        [SerializeField] private GameObject spellsParent;

        //破棄用に持っておく
        List<Spell> spellList = new List<Spell>();
 
        //登録処理
        private void Start()
        {
            inventory.observable.Subscribe(_ => OnEquipUpdate());
        }


        //装備時(外からも呼べるようにする)
        public void OnEquipUpdate()
        {
            //旧コード

            /*
            //生成済みインスタンスをすべて破壊
            foreach(Spell spell in spellList)
            {
                Destroy(spell.gameObject);
            }
            spellList.Clear();

            //インベントリを全て取得
            for(int i = 0; i < inventory.maxSlot; i++)
            {
                ItemStack item = inventory.GetItemStack(i);
                if (item == null)
                {
                    caster.SetCastable(null, i);
                    continue;
                }
                if (item.type != ItemType.SPELL)
                {
                    caster.SetCastable(null, i);
                    continue;
                }
                ItemSpellParam param = (ItemSpellParam)item.parameter;


                //スペルを取得
                Spell spell = MagicLoader.loader.GetSpell(item.code, 0);

                //サポートを取得して入れる
                foreach (ItemStack sup in param.supports)
                {
                    if (sup == null) continue;
                    Support support = MagicLoader.loader.GetSupport(sup.code);
                    spell.AddSupport(support);
                }
                spell.Reload();

                //casterにセット
                caster.SetCastable(spell, i);

                //casterの子にする
                if (spellsParent != null) spell.gameObject.transform.parent = spellsParent.transform;
                else spell.gameObject.transform.parent = caster.transform;

                spellList.Add(spell);
            }
            */
        }
    }
}

