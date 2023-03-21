using System;
using System.Collections.Generic;

using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Inventories;

namespace CM.UI.Presenter
{
    //�C���x���g���̃v���[���^�[
    public class InventoryPresenter : MonoBehaviour
    {
        //�ϊ��p�ϐ�
        [Header("Converter")]
        [SerializeField] private ItemDataBase dataBase; //�f�[�^�x�[�X ���g�̓��X�g

        //Model
        [Header("Model")]
        [SerializeField] private Inventories.Inventory inventory;

        //View
        [Header("View")]
        [SerializeField] private GameObject grid;  //�Ώۂ̃O���b�h ������(�����ɑΉ����邽�߂Ƃ����܂�slot�̐e�ł���Ƃ����_���d�v
        private Slot[] slots;                      //View�̃X���b�g�z��


        //�R�[�h�̑Ή��t��
        private Dictionary<string, Item> codeToItemMeta = new Dictionary<string, Item>();





        public void Awake()
        {
            //�C�x���g�o�^
            IDisposable disposable = null;
            disposable = grid.UpdateAsObservable().Subscribe(_ =>
            {
                //������
                Init();

                //�C�x���g��j��(2F�ȍ~�Ăяo���Ȃ��悤��)
                disposable.Dispose();
            });
        }




        //������
        private void Init()
        {
            //�����𐶐�
            foreach (Item item in dataBase.GetItemLists())
            {
                codeToItemMeta.Add(item.name, item);    //ScriptableObject�̃t�@�C����
            }

            //Model�̃C�x���g�o�^(inventory)
            inventory.observable
                .Where(e => e.NewValue != e.OldValue)       //�A�C�e�����ς����������
                .Subscribe(e => OnReplaceInventory(e)).AddTo(this);

            //View�̃C�x���g�o�^(slot),slot�̏�����
            slots = grid.GetComponentsInChildren<Slot>();
            for(int i = 0; i < slots.Length; i++)
            {
                //�X���b�g�擾
                Slot slot = slots[i];

                //�C�x���g�o�^
                slot.beginDrag.Subscribe(slot => OnBeginDrag(slot)).AddTo(this);
                slot.drop.Subscribe(slot => OnDrop(slot)).AddTo(this);
                slot.endDrag.Subscribe(slot => OnEndDrag(slot)).AddTo(this);

                //UI�̏�����
                ItemStack item = inventory.GetItemStack(i);
                if (item != null) slot.SetItem(codeToItemMeta[item.code]);
            }
        }



        //�C���x���g���ύX��
        private void OnReplaceInventory(CollectionReplaceEvent<ItemStack> events)
        {
            //���荞�ݔ���
            //���݂̊Y���X���b�g�̃A�C�e���Ƃ����ɐV�������ꂽ�A�C�e���Ƃ�
            //�H���Ⴂ���N����ꍇ�͊��荞�܂�Ă��邽�߃A�C�R���̍X�V����߂�
            if (inventory.GetItemStack(events.Index) != events.NewValue) return;

            //ItemMeta���擾
            Item item = null;
            if(events.NewValue != null)
            {
                if(codeToItemMeta.ContainsKey(events.NewValue.code)) item = codeToItemMeta[events.NewValue.code];
            }

            //�X���b�g�ɐݒ�
            slots[events.Index].SetItem(item);
        }







        /*
         * 
         * 
         * ���̉�������
         * �啝�ɕς���ׂ����� ���낢�낲���Ⴒ���Ⴕ����
         * 
         */
        //������ - ItemStack���ꎞ�ێ�(�ǂ�Presenter�ł������P��
        private static ItemStack tempItem;
        private static Slot tempSlot;   //���邩���� Drag�̐����ɂ����g���Ă���
        private static int preIndex;
        private static Inventories.Inventory preInventory;

        //�h���b�O�J�n
        private void OnBeginDrag(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);
            tempItem = inventory.GetItemStack(index);
            tempSlot = slot;
            preIndex = index;
            preInventory = inventory;
        }

        //�h���b�v(�h���b�O����鑤)
        private void OnDrop(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);

            //�h���b�O����鑤�̃A�C�e����ێ�
            ItemStack temp = inventory.GetItemStack(index);

            //�A�C�e����ύX ���s������UI�����ɂ��ǂ��Ă��
            //tempItem��ύX���Ȃ����Ƃ�OnEndDrag���ŏ���ɖ߂�悤�ɂ���(�����͊��S��OnReplace�Ɠ����Ȃ��ߔC����
            if (!inventory.SetItemStack(tempItem, index))
            {
                OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, temp, temp));
                return;
            }


            //temp�̕ێ���ύX����
            tempItem = temp;
            tempSlot = slot;
            preIndex = index;
            preInventory = inventory;
        }

        //�h���b�v(�h���b�O���鑤)
        private void OnEndDrag(Slot slot)
        {
            int index = slot.GetSlotIndex(slots);

            //temp���ς���ĂȂ����Drop�����s���Ă�̂Ŗ߂�(�߂��Ȃ��Ɠ����A�C�e����Set���Ă��C�x���g������Ȃ��̂�)
            if(tempItem == inventory.GetItemStack(index))
            {
                this.OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, tempItem, tempItem));
            }

            //�h���b�O���鑤���X�V���� ���s����͓̂���ւ��悪���ɓ�����Ȃ���
            if(!inventory.SetItemStack(tempItem, index))
            {
                //Drop�̂ق������ɖ߂��Ȃ��Ƃ����Ȃ� ���̎�slots��������Ȃ��̂�slot�ɒ��ږ߂�(�ʂ�slots��slot�̉\���L)
                //�X�ɃA�C�e�����߂��K�v�����邪�C���x���g����������Ȃ��̂Ŗ߂��Ȃ�
                //�̂�preInventory������
                Item itemMeta = null;
                if (tempItem != null && codeToItemMeta.ContainsKey(tempItem.code)) itemMeta = codeToItemMeta[tempItem.code];
                tempSlot.SetItem(itemMeta);
                preInventory.SetItemStack(tempItem, preIndex);


                //Drag����߂�
                ItemStack item = inventory.GetItemStack(index);
                this.OnReplaceInventory(new CollectionReplaceEvent<ItemStack>(index, item, item));
            }


            //temp����ɂ��Ă���
            tempItem = null;
            tempSlot = null;
            preIndex = -1;
            preInventory = null;
        }
    }



    //������ �g�����\�b�h
    public static class SlotExtender
    {

        //�X���b�g���X�g���擾
        public static int GetSlotIndex(this Slot slot, Slot[] slots)
        {
            //�Ώ�Slot���������� idex���K�v�Ȃ���for�Ō���
            for (int i = 0; i < slots.Length; i++)
            {
                //�Q�Ɣ�r
                if (slot == slots[i])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
