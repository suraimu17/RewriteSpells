

namespace Inventories.Parameters
{
    //�X�y���p�p�����[�^�^
    public class ItemSpellParam
    {
        //�R���X�g���N�^   ������
        public ItemSpellParam(int level, int maxSlot)
        {
            this.level = level;
            this.supports = new ItemStack[maxSlot];
        }


        public readonly int level;                //���x��
        public readonly ItemStack[] supports;     //�T�|�[�g�ꗗ


        //�ǉ��֐�
        public bool addSupport(ItemStack item, int slot)
        {
            //�������ރA�C�e�����T�|�[�g�ŃX���b�g�ɋ󂫂�����Ƃ�
            if (item.type == ItemType.SUPPORT && supports[slot] == null)
            {
                supports[slot] = item;
                return true;
            }
            return false;
        }
    }

}
