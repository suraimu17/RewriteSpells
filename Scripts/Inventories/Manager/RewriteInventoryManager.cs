using UnityEngine;
using UniRx;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories.Manager
{
    //Rewrite関連のInventoryセットアップ
    public class RewriteInventoryManager : MonoBehaviour
    {
        //それぞれのInventory
        [SerializeField] private Inventory spellInventory;
        [SerializeField] private Inventory supportInventory;


        //イベント登録
        private void Start()
        {
            //スペルを置いた時
            spellInventory.observable
                .Where(e => e.NewValue != e.OldValue)
                .Where(e => e.NewValue != null)                 //nullも無視する
                .Where(e => e.NewValue.type == ItemType.SPELL)  //アイテムがスペルの時だけ
                .Subscribe(e => OnSetSpell(e.NewValue))         //セットされたアイテムを渡す
                .AddTo(this);

            //スペルを取った時
            spellInventory.observable
                .Where(e => e.NewValue != e.OldValue)
                .Where(e => e.OldValue != null)                 //OldNullも無視する
                .Where(e => e.OldValue.type == ItemType.SPELL)  //取り除かれたのがSpellの時
                .Subscribe(e => OnRemoveSpell(e.OldValue))      //取り除かれるときだから前の値を渡して置く
                .AddTo(this);

            //各Inventoryに追加条件式
            spellInventory.setCondition = (item, index) => 
            { 
                if (item == null) return true; 
                return item.type == ItemType.SPELL; 
            };
            supportInventory.setCondition = (item, index) =>
            {
                if (item == null) return true;
                return item.type == ItemType.SUPPORT && spellInventory.GetItemStack(0) != null;
            };
        }



        //スペルスロットにスペルがセットされた時
        private void OnSetSpell(ItemStack spellItem)
        {
            //itemからspellを取得
            Spell spell = spellItem.castable as Spell;

            //spellからサポートリストを取得してアイテムを生成、slotに入れる
            for (int i = 0; i < spell.supportList.Count && i < supportInventory.maxSlot; i++)
            {
                ItemStack item = null;
                if(spell.supportList[i] != null) item = new ItemStack(spell.supportList[i].name, ItemType.SUPPORT);
                supportInventory.SetItemStack(item, i);
            }

            //spellのリストを初期化
            spell.supportList.Clear();
        }


        //スペルを取り除いた時
        private void OnRemoveSpell(ItemStack spellItem)
        {
            //itemからspellを取得
            Spell spell = spellItem.castable as Spell;

            //サポートを追加
            for (int i = 0; i < supportInventory.maxSlot; i++)
            {
                ItemStack item = supportInventory.GetItemStack(i);
                if (item == null) spell.supportList.Add(null);
                else spell.supportList.Add(MagicLoader.loader.GetSupport(item.code));
                supportInventory.SetItemStack(null, i);
            }

            //spellをReload
            spell.Reload();
        }

    }
}
