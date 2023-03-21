using System;
using System.Collections.Generic;

using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Inventories;

namespace CM.UI.Presenter
{
    //インベントリのプレゼンター
    public class InventoryPresenter : MonoBehaviour
    {
        //変換用変数
        [Header("Converter")]
        [SerializeField] private ItemDataBase dataBase; //データベース 中身はリスト

        //Model
        [Header("Model")]
        [SerializeField] private Inventories.Inventory inventory;

        //View
        [Header("View")]
        [SerializeField] private GameObject grid;  //対象のグリッド 仮実装(複数に対応するためとあくまでslotの親であるという点が重要
        private Slot[] slots;                      //Viewのスロット配列


        //コードの対応付け
        private Dictionary<string, Item> codeToItemMeta = new Dictionary<string, Item>();





        public void Awake()
        {
            //イベント登録
            IDisposable disposable = null;
            disposable = grid.UpdateAsObservable().Subscribe(_ =>
            {
                //初期化
                Init();

                //イベントを破棄(2F以降呼び出さないように)
                disposable.Dispose();
            });
        }




        //初期化
        private void Init()
        {
            //辞書を生成
            foreach (Item item in dataBase.GetItemLists())
            {
                codeToItemMeta.Add(item.name, item);    //ScriptableObjectのファイル名
            }

            //Modelのイベント登録(inventory)
            inventory.observable
                .Where(e => e.NewValue != e.OldValue)       //アイテムが変わった時だけ
                .Subscribe(e => OnReplaceInventory(e)).AddTo(this);

            //Viewのイベント登録(slot),slotの初期化
            slots = grid.GetComponentsInChildren<Slot>();
            for(int i = 0; i < slots.Length; i++)
            {
                //スロット取得
                Slot slot = slots[i];

                //イベント登録
                slot.beginDrag.Subscribe(slot => OnBeginDrag(slot)).AddTo(this);
                slot.drop.Subscribe(slot => OnDrop(slot)).AddTo(this);
                slot.endDrag.Subscribe(slot => OnEndDrag(slot)).AddTo(this);

                //UIの初期化
                ItemStack item = inventory.GetItemStack(i);
                if (item != null) slot.SetItem(codeToItemMeta[item.code]);
            }
        }



        //インベントリ変更時
        private void OnReplaceInventory(CollectionReplaceEvent<ItemStack> events)
        {
            //割り込み判定
            //現在の該当スロットのアイテムとそこに新しく居れたアイテムとで
            //食い違いが起こる場合は割り込まれているためアイコンの更新をやめる
            if (inventory.GetItemStack(events.Index) != events.NewValue) return;

            //ItemMetaを取得
            Item item = null;
            if(events.NewValue != null)
            {
                if(codeToItemMeta.ContainsKey(events.NewValue.code)) item = codeToItemMeta[events.NewValue.code];
            }

            //スロットに設定
            slots[events.Index].SetItem(item);
        }







        /*
         * 
         * 
         * この下仮実装
         * 大幅に変えるべき部分 いろいろごちゃごちゃしすぎ
         * 
         */
        //仮実装 - ItemStackを一時保持(どのPresenterでもただ１つ
        private static ItemStack tempItem;
        private static Slot tempSlot;   //いるか微妙 Dragの制限にだけ使っている
        private static int preIndex;
        private static Inventories.Inventory preInventory;

        //ドラッグ開始
        private void OnBeginDrag(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);
            tempItem = inventory.GetItemStack(index);
            tempSlot = slot;
            preIndex = index;
            preInventory = inventory;
        }

        //ドロップ(ドラッグされる側)
        private void OnDrop(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);

            //ドラッグされる側のアイテムを保持
            ItemStack temp = inventory.GetItemStack(index);

            //アイテムを変更 失敗したらUIを元にもどしてやる
            //tempItemを変更しないことでOnEndDrag側で勝手に戻るようにする(処理は完全にOnReplaceと同じなため任せる
            if (!inventory.SetItemStack(tempItem, index))
            {
                OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, temp, temp));
                return;
            }


            //tempの保持を変更する
            tempItem = temp;
            tempSlot = slot;
            preIndex = index;
            preInventory = inventory;
        }

        //ドロップ(ドラッグする側)
        private void OnEndDrag(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);

            //tempが変わってなければDropが失敗してるので戻す(戻さないと同じアイテムをSetしてもイベントが走らないので)
            if(tempItem == inventory.GetItemStack(index))
            {
                this.OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, tempItem, tempItem));
            }

            //ドラッグする側を更新する 失敗するのは入れ替え先が元に入れられない時
            if(!inventory.SetItemStack(tempItem, index))
            {
                //Dropのほうも元に戻さないといけない この時slotsが分からないのでslotに直接戻す(別のslotsのslotの可能性有)
                //更にアイテムも戻す必要があるがインベントリが分からないので戻せない
                //のでpreInventoryもいる
                Item itemMeta = null;
                if (tempItem != null && codeToItemMeta.ContainsKey(tempItem.code)) itemMeta = codeToItemMeta[tempItem.code];
                tempSlot.SetItem(itemMeta);
                preInventory.SetItemStack(tempItem, preIndex);


                //Drag側を戻す
                ItemStack item = inventory.GetItemStack(index);
                this.OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, item, item));
            }


            //tempを空にしておく
            tempItem = null;
            tempSlot = null;
            preIndex = -1;
            preInventory = null;
        }
    }



    //仮実装 拡張メソッド
    public static class SlotExtender
    {

        //スロットリストを取得
        public static int GetSlotIndex(this Slot slot, Slot[] slots)
        {
            //対象Slotを検索する idexが必要なためforで検索
            for (int i = 0; i < slots.Length; i++)
            {
                //参照比較
                if (slot == slots[i])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
