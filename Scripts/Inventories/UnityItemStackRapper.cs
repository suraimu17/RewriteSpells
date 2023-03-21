using System;

using UnityEngine;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories
{
    //ItemStackのラップ用
    [Serializable]
    public class UnityItemStackRapper
    {
        //入力パラメータ
        [SerializeField] private string code;
        [SerializeField] private ItemType type;

        //レベル(外部依存)
        private int level;


        //外部依存パラメータの変更
        public void ChangeCastableLevel(int level)
        {
            this.level = level;
        }


        //ItemStackの取得
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