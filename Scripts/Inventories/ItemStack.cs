using UnityEngine;

using CM.Magic;

namespace Inventories
{
    //アイテムデータ型
    public class ItemStack
    {
        //コンストラクタ
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

        //デコンストラクタ
        ~ItemStack()
        {
            //castableがnullじゃなくてcomponentなら削除
            if (castable != null && castable is Component) GameObject.Destroy(((Component)castable).gameObject);
        }


        //不変パラメータ
        public readonly string code;                //アイテムコード
        public readonly ItemType type;              //タイプ
        public readonly ICastable castable;         //使った時の効果(魔法系システムの使いまわしが可能) - 使えないならnull
    }



    //タイプenum
    public enum ItemType
    {
        POTION,     //ポーション
        SPELL,      //スペル
        SUPPORT,    //サポート
        NONE        //その他 素材アイテムなど
    }
}
