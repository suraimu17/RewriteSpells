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
            //�߂��p�̃��X�g
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //�C���X�^���X
            BaseMagicObject instance = (BaseMagicObject)prefab.Clone();

            //�������W��target�ɒ���
            instance.transform.position = param.target;
            instance.transform.rotation = param.originRoot.rotation;

            //������
            instance.Init(param);

            //���X�g�ɒǉ����Ė߂�
            magicObjectList.Add(instance);
            return magicObjectList;
        }
    }
}
