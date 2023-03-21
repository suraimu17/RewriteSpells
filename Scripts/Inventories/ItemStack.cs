using UnityEngine;

using CM.Magic;

namespace Inventories
{
    //�A�C�e���f�[�^�^
    public class ItemStack
    {
        //�R���X�g���N�^
        public ItemStack(string code, ItemType type)
        {
            this.code = code;
            this.type = type;
            castable = null;
        }
        public ItemStack(string code, ItemType type, ICastable castable)
        {
            this.code = code;
            this.type = type;
            this.castable = castable;
        }

        //�f�R���X�g���N�^
        ~ItemStack()
        {
            //castable��null����Ȃ���component�Ȃ�폜
            if (castable != null && castable is Component) GameObject.Destroy(((Component)castable).gameObject);
        }


        //�s�σp�����[�^
        public readonly string code;                //�A�C�e���R�[�h
        public readonly ItemType type;              //�^�C�v
        public readonly ICastable castable;         //�g�������̌���(���@�n�V�X�e���̎g���܂킵���\) - �g���Ȃ��Ȃ�null
    }



    //�^�C�venum
    public enum ItemType
    {
        POTION,     //�|�[�V����
        SPELL,      //�X�y��
        SUPPORT,    //�T�|�[�g
        NONE        //���̑� �f�ރA�C�e���Ȃ�
    }
}
