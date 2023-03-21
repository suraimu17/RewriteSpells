

namespace Inventories.Parameters
{
    //スペル用パラメータ型
    public class ItemSpellParam
    {
        //コンストラクタ   仮実装
        public ItemSpellParam(int level, int maxSlot)
        {
            this.level = level;
            this.supports = new ItemStack[maxSlot];
        }


        public readonly int level;                //レベル
        public readonly ItemStack[] supports;     //サポート一覧


        //追加関数
        public bool addSupport(ItemStack item, int slot)
        {
            //差し込むアイテムがサポートでスロットに空きがあるとき
            if (item.type == ItemType.SUPPORT && supports[slot] == null)
            {
                supports[slot] = item;
                return true;
            }
            return false;
        }
    }

}
