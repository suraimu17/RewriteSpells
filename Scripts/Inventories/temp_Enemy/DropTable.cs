using System.Collections.Generic;
using UnityEngine;

using Game.enemys;


namespace Inventories.Enemy
{
    [RequireComponent(typeof(EnemyStatus))]
    [RequireComponent(typeof(ILevelProvider))]
    public class DropTable : MonoBehaviour
    {
        //テーブルと確率
        [SerializeField] private List<PickableItem> table = new List<PickableItem>();
        [SerializeField] private List<float> random = new List<float>();

        //レベル
        private ILevelProvider level;

        private void Start()
        {
            level = GetComponent<ILevelProvider>();
        }



        //1個だけ出る
        public void Drop()
        {
            //指定回数抽選 出たらキャンセル
            for(int i = 0; i < table.Count; i++)
            {
                if(Random.value + level.Value/100 <= random[i])
                {
                    //インスタンス化とレベルの初期化
                    //生成時に元のプレハブの回転にし、座標はドロップ箇所にプレハブの座標を加算
                    GameObject itemObject = Instantiate(table[i].gameObject, gameObject.transform.position + table[i].transform.position, table[i].transform.rotation);

                    //レベルを設定して終了
                    PickableItem pItem = itemObject.GetComponent<PickableItem>();
                    pItem.unityItemStack.ChangeCastableLevel(level.Value);
                    break;
                }
            }
        }


    }
}
