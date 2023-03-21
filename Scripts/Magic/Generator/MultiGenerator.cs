using System.Collections.Generic;

using UnityEngine;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;

namespace CM.Magic.Generator
{
    public class MultiGenerator : MonoBehaviour, IMagicObjectGenerator
    {
        //Endで生成するかどうか
        [SerializeField] private bool isEndGenerate;

        //生成数
        [SerializeField] private int amount;

        //真ん中の生成を無視するか
        [SerializeField] private bool ignoreMidGenerate;

        //オフセットの長さ
        [SerializeField] private float offSetLength;

        //最大角度
        [SerializeField] private float maxAngle;


        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param)
        {
            //戻す用のリスト
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //ターゲットまでの座標(初期位置からの方向に固定
            Vector3 toTarget = (param.target - param.souce).normalized;

            //ベクトルを曲げる
            toTarget = Quaternion.Euler(0, -(maxAngle / 2), 0) * toTarget;


            //複数生成
            for (int i = 0; i < amount; i++)
            {
                if (ignoreMidGenerate && i == amount/2)
                {
                    //ベクトルを曲げる
                    toTarget = Quaternion.Euler(0, maxAngle / (amount - 1), 0) * toTarget;
                    continue;
                }

                BaseMagicObject instance;
                //位置を変更
                if (!isEndGenerate)
                {
                    instance = (BaseMagicObject)prefab.Clone();
                    instance.transform.position = param.originRoot.position + toTarget * offSetLength;
                    instance.transform.rotation = param.originRoot.rotation;

                    //ターゲットを変更して初期化
                    MagicTriggerParameter new_param = new MagicTriggerParameter(param.origin, param.originRoot, param.end, param.endRoot, param.souce, param.originRoot.position + toTarget);
                    instance.Init(new_param);
                }
                else
                {
                    //endRootがnullなら生成しない
                    if (param.endRoot == null) return magicObjectList;
                    instance = (BaseMagicObject)prefab.Clone();
                    instance.transform.position = param.endRoot.Value + toTarget* offSetLength;
                    instance.transform.rotation = param.originRoot.rotation;

                    //ターゲットを変更して初期化
                    MagicTriggerParameter new_param = new MagicTriggerParameter(param.origin, param.originRoot, param.end, param.endRoot, param.souce, param.endRoot.Value + toTarget);
                    instance.Init(new_param);
                }
                //リストに追加
                magicObjectList.Add(instance);

                //ベクトルを曲げる
                toTarget = Quaternion.Euler(0, maxAngle / (amount - 1), 0) * toTarget;
            }

            //リストを返す
            return magicObjectList;
        }
    }
}
