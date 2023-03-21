using UnityEngine;
using System;
using UniRx;

namespace Inventories
{
    public class Inventory : MonoBehaviour
    {
        //maxSlot
        [SerializeField] private int _maxSlot;
        public int maxSlot { get => _maxSlot; } //取得のみできるように(setはInspectorだけ)

        //Inventory配列 ReactiveCollection
        private ReactiveCollection<ItemStack> itemList;
        public IObservable<CollectionReplaceEvent<ItemStack>> observable => itemList.ObserveReplace();

        //Inventory追加の条件式
        public Func<ItemStack, int, bool> setCondition { private get; set; }


        //初期化 イベント登録は必ずStart
        private void Awake()
        {
            //初期化
            itemList = new ReactiveCollection<ItemStack>(new ItemStack[maxSlot]);

            //lockerがnullなら代入する
            if (setCondition == null) setCondition = (item, index) => { return true; };
        }


        //上から空いてるスロットに追加する
        public bool AddItemStack(ItemStack item)
        {
            for (int i = 0; i < maxSlot; i++)
            {
                if (itemList[i] == null)
                {
                    //itemがセットできるかチェック
                    if (!setCondition(item, i)) return false;
                    itemList[i] = item;
                    return true;
                }
            }
            return false;
        }


        //指定スロットのアイテムを上書きする
        public bool SetItemStack(ItemStack item, int index)
        {
            //itemSetのチェック
            if(!setCondition(item, index)) return false;

            if (maxSlot < index) return false;
            itemList[index] = item;
            return true;
        }


        //指定したスロットのアイテムを取得する
        public ItemStack GetItemStack(int index)
        {
            if (maxSlot < index) return null;
            return itemList[index];
        }
    }
}
