using System.Collections.Generic;

using UnityEngine;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;


namespace CM.Magic.Generator
{
    public class DefaultGenerator : MonoBehaviour, IMagicObjectGenerator
    {
        //Endで生成するかどうか
        [SerializeField] private bool isEndGenerate;

        [SerializeField] private float forwardOffset;

        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param)
        {
            //戻す用のリスト
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //インスタンス
            BaseMagicObject instance = null;

            //位置を変更
            if (!isEndGenerate)
            {
                instance = (BaseMagicObject)prefab.Clone();
                instance.transform.position = param.originRoot.position;
                instance.transform.rotation = param.originRoot.rotation;
            }
            else
            {
                //endRootがnullなら生成しない
                if (param.endRoot == null) return magicObjectList;
                instance = (BaseMagicObject)prefab.Clone();
                instance.transform.position = param.endRoot.Value;
                instance.transform.rotation = param.originRoot.rotation;
            }
            //offset適用
            Vector3 fowardOffset = param.originRoot.forward * forwardOffset;
            instance.transform.position = instance.transform.position + fowardOffset;

            //初期化
            instance.Init(param);

            //リストに追加して戻す
            magicObjectList.Add(instance);
            return magicObjectList;
        }
    }
}

