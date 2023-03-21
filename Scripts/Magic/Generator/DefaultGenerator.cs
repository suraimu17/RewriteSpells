using System.Collections.Generic;

using UnityEngine;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;


namespace CM.Magic.Generator
{
    public class DefaultGenerator : MonoBehaviour, IMagicObjectGenerator
    {
        //End�Ő������邩�ǂ���
        [SerializeField] private bool isEndGenerate;

        [SerializeField] private float forwardOffset;

        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param)
        {
            //�߂��p�̃��X�g
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //�C���X�^���X
            BaseMagicObject instance = null;

            //�ʒu��ύX
            if (!isEndGenerate)
            {
                instance = (BaseMagicObject)prefab.Clone();
                instance.transform.position = param.originRoot.position;
                instance.transform.rotation = param.originRoot.rotation;
            }
            else
            {
                //endRoot��null�Ȃ琶�����Ȃ�
                if (param.endRoot == null) return magicObjectList;
                instance = (BaseMagicObject)prefab.Clone();
                instance.transform.position = param.endRoot.Value;
                instance.transform.rotation = param.originRoot.rotation;
            }
            //offset�K�p
            Vector3 fowardOffset = param.originRoot.forward * forwardOffset;
            instance.transform.position = instance.transform.position + fowardOffset;

            //������
            instance.Init(param);

            //���X�g�ɒǉ����Ė߂�
            magicObjectList.Add(instance);
            return magicObjectList;
        }
    }
}

