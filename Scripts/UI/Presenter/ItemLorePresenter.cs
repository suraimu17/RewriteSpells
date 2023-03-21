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

        //�R�[�h�̑Ή��t��
        private Dictionary<string, Item> codeToItemMeta = new Dictionary<string, Item>();

        private void Start()
        {
            //�����𐶐�
            foreach (Item item in dataBase.GetItemLists())
            {
                codeToItemMeta.Add(item.name, item);    //ScriptableObject�̃t�@�C����
            }

            inventory.observable
                .Subscribe(e =>
                {
                    ItemStack item = inventory.GetItemStack(0);

                    //null��������text����ɂ���
                    if (item == null)
                    {
                        text.text = "";
                        return;
                    }
                    
                    //�R�s�y
                    Item MyItem = codeToItemMeta[item.code];
                    text.text = MyItem.MyItemName + "\n" + MyItem.Information + "\n" + MyItem.kindOfItem1;

                    //Spell��������
                    if (item.type == ItemType.SPELL)
                    {
                        Spell spell = item.castable as Spell;
                        text.text += "\n���x��: " + spell.level +  "\n�}�i: " + spell.manaCost + "\n�L���X�g: " + spell.delayTime + "(s)"+ "\n���L���X�g: " + spell.coolTime + "(s)";
                    }
                    //Potion
                    else if (item.type == ItemType.POTION)
                    {
                        Spell spell = item.castable as Spell;
                        text.text += "\n���x��: " + spell.level + "\n�L���X�g�^�C��: " + spell.delayTime;
                    }
                    //support
                    else if (item.type == ItemType.SUPPORT)
                    {
                        Support support = MagicLoader.loader.GetSupport(item.code);
                        text.text += "\n���x���␳: " + support.addLevel * 100 + "%\n�}�i�␳: " + support.addManaCost * 100 + "%\n�L���X�g�␳: " + support.addDelayTime * 100 + "%\n���L���X�g�␳: " + support.addCoolTime * 100 + "(s)";
                        Destroy(support.gameObject);
                    }
                });
        }
    }
}
