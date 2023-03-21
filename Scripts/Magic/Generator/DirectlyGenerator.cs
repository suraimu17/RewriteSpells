using System.Collections.Generic;

using UnityEngine;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;

namespace CM.Magic.Generator
{
    public class DirectlyGenerator : MonoBehaviour, IMagicObjectGenerator
    {
        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param)
        {
            //戻す用のリスト
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //インスタンス
            BaseMagicObject instance = (BaseMagicObject)prefab.Clone();

            //生成座標はtargetに直接
            instance.transform.position = param.target;
            instance.transform.rotation = param.originRoot.rotation;

            //初期化
            instance.Init(param);

            //リストに追加して戻す
            magicObjectList.Add(instance);
            return magicObjectList;
        }
    }
}
