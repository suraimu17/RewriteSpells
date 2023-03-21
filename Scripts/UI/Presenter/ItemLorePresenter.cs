using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using UniRx;

using Inventories;

using CM.Magic;
using CM.Magic.Temp;

namespace CM.UI.Presenter
{
    public class ItemLorePresenter : MonoBehaviour
    {
        //Model
        [Header("Model")]
        [SerializeField] private Inventories.Inventory inventory;
        [SerializeField] private Player uiItemDetailPlayer;

        //View
        [Header("View")]
        [SerializeField] private Text text;
        [SerializeField] private ItemDataBase dataBase;

        //コードの対応付け
        private Dictionary<string, Item> codeToItemMeta = new Dictionary<string, Item>();

        private void Start()
        {
            //辞書を生成
            foreach (Item item in dataBase.GetItemLists())
            {
                codeToItemMeta.Add(item.name, item);    //ScriptableObjectのファイル名
            }

            inventory.observable
                .Subscribe(e =>
                {
                    ItemStack item = inventory.GetItemStack(0);

                    //nullだったらtextを空にする
                    if (item == null)
                    {
                        text.text = "";
                        return;
                    }
                    
                    //コピペ
                    Item MyItem = codeToItemMeta[item.code];
                    text.text = MyItem.MyItemName + "\n" + MyItem.Information + "\n" + MyItem.kindOfItem1;

                    //Spellだったら
                    if (item.type == ItemType.SPELL)
                    {
                        Spell spell = item.castable as Spell;
                        text.text += "\nレベル: " + spell.level +  "\nマナ: " + spell.manaCost + "\nキャスト: " + spell.delayTime + "(s)"+ "\nリキャスト: " + spell.coolTime + "(s)";
                    }
                    //Potion
                    else if (item.type == ItemType.POTION)
                    {
                        Spell spell = item.castable as Spell;
                        text.text += "\nレベル: " + spell.level + "\nキャストタイム: " + spell.delayTime;
                    }
                    //support
                    else if (item.type == ItemType.SUPPORT)
                    {
                        Support support = MagicLoader.loader.GetSupport(item.code);
                        text.text += "\nレベル補正: " + support.addLevel * 100 + "%\nマナ補正: " + support.addManaCost * 100 + "%\nキャスト補正: " + support.addDelayTime * 100 + "%\nリキャスト補正: " + support.addCoolTime * 100 + "(s)";
                        Destroy(support.gameObject);
                    }
                });
        }
    }
}
