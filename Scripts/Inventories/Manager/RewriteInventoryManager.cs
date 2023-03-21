using UnityEngine;
using UniRx;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories.Manager
{
    //Rewrite�֘A��Inventory�Z�b�g�A�b�v
    public class RewriteInventoryManager : MonoBehaviour
    {
        //���ꂼ���Inventory
        [SerializeField] private Inventory spellInventory;
        [SerializeField] private Inventory supportInventory;


        //�C�x���g�o�^
        private void Start()
        {
            //�X�y����u������
            spellInventory.observable
                .Where(e => e.NewValue != e.OldValue)
                .Where(e => e.NewValue != null)                 //null����������
                .Where(e => e.NewValue.type == ItemType.SPELL)  //�A�C�e�����X�y���̎�����
                .Subscribe(e => OnSetSpell(e.NewValue))         //�Z�b�g���ꂽ�A�C�e����n��
                .AddTo(this);

            //�X�y�����������
            spellInventory.observable
                .Where(e => e.NewValue != e.OldValue)
                .Where(e => e.OldValue != null)                 //OldNull����������
                .Where(e => e.OldValue.type == ItemType.SPELL)  //��菜���ꂽ�̂�Spell�̎�
                .Subscribe(e => OnRemoveSpell(e.OldValue))      //��菜�����Ƃ�������O�̒l��n���Ēu��
                .AddTo(this);

            //�eInventory�ɒǉ�������
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



        //�X�y���X���b�g�ɃX�y�����Z�b�g���ꂽ��
        private void OnSetSpell(ItemStack spellItem)
        {
            //item����spell���擾
            Spell spell = spellItem.castable as Spell;

            //spell����T�|�[�g���X�g���擾���ăA�C�e���𐶐��Aslot�ɓ����
            for (int i = 0; i < spell.supportList.Count && i < supportInventory.maxSlot; i++)
            {
                ItemStack item = null;
                if(spell.supportList[i] != null) item = new ItemStack(spell.supportList[i].name, ItemType.SUPPORT);
                supportInventory.SetItemStack(item, i);
            }

            //spell�̃��X�g��������
            spell.supportList.Clear();
        }


        //�X�y������菜������
        private void OnRemoveSpell(ItemStack spellItem)
        {
            //item����spell���擾
            Spell spell = spellItem.castable as Spell;

            //�T�|�[�g��ǉ�
            for (int i = 0; i < supportInventory.maxSlot; i++)
            {
                ItemStack item = supportInventory.GetItemStack(i);
                if (item == null) spell.supportList.Add(null);
                else spell.supportList.Add(MagicLoader.loader.GetSupport(item.code));
                supportInventory.SetItemStack(null, i);
            }

            //spell��Reload
            spell.Reload();
        }

    }
}
