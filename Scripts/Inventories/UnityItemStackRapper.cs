using System;

using UnityEngine;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories
{
    //ItemStack�̃��b�v�p
    [Serializable]
    public class UnityItemStackRapper
    {
        //���̓p�����[�^
        [SerializeField] private string code;
        [SerializeField] private ItemType type;

        //���x��(�O���ˑ�)
        private int level;


        //�O���ˑ��p�����[�^�̕ύX
        public void ChangeCastableLevel(int level)
        {
            this.level = level;
        }


        //ItemStack�̎擾
        private ItemStack _item;
        public ItemStack item
        {
            get
            {
                if (_item == null)
                {
                    ICastable castable = null;

                    switch (type)
                    {
                        case ItemType.SPELL:
                            castable = MagicLoader.loader.GetSpell(code, level);
                            break;
                        case ItemType.POTION:
                            castable = MagicLoader.loader.GetSpell(code, level);
                            break;
                    }
                    _item = new ItemStack(code, type, castable);
                }
                return _item;
            }
        }
    }

}