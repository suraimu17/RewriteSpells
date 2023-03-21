using System.Collections.Generic;

using UnityEngine;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;

namespace CM.Magic.Generator
{
    public class MultiGenerator : MonoBehaviour, IMagicObjectGenerator
    {
        //End�Ő������邩�ǂ���
        [SerializeField] private bool isEndGenerate;

        //������
        [SerializeField] private int amount;

        //�^�񒆂̐����𖳎����邩
        [SerializeField] private bool ignoreMidGenerate;

        //�I�t�Z�b�g�̒���
        [SerializeField] private float offSetLength;

        //�ő�p�x
        [SerializeField] private float maxAngle;


        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param)
        {
            //�߂��p�̃��X�g
            List<BaseMagicObject> magicObjectList = new List<BaseMagicObject>();

            //�^�[�Q�b�g�܂ł̍��W(�����ʒu����̕����ɌŒ�
            Vector3 toTarget = (param.target - param.souce).normalized;

            //�x�N�g�����Ȃ���
            toTarget = Quaternion.Euler(0, -(maxAngle / 2), 0) * toTarget;


            //��������
            for (int i = 0; i < amount; i++)
            {
                if (ignoreMidGenerate && i == amount/2)
                {
                    //�x�N�g�����Ȃ���
                    toTarget = Quaternion.Euler(0, maxAngle / (amount - 1), 0) * toTarget;
                    continue;
                }

                BaseMagicObject instance;
                //�ʒu��ύX
                if (!isEndGenerate)
                {
                    instance = (BaseMagicObject)prefab.Clone();
                    instance.transform.position = param.originRoot.position + toTarget * offSetLength;
                    instance.transform.rotation = param.originRoot.rotation;

                    //�^�[�Q�b�g��ύX���ď�����
                    MagicTriggerParameter new_param = new MagicTriggerParameter(param.origin, param.originRoot, param.end, param.endRoot, param.souce, param.originRoot.position + toTarget);
                    instance.Init(new_param);
                }
                else
                {
                    //endRoot��null�Ȃ琶�����Ȃ�
                    if (param.endRoot == null) return magicObjectList;
                    instance = (BaseMagicObject)prefab.Clone();
                    instance.transform.position = param.endRoot.Value + toTarget* offSetLength;
                    instance.transform.rotation = param.originRoot.rotation;

                    //�^�[�Q�b�g��ύX���ď�����
                    MagicTriggerParameter new_param = new MagicTriggerParameter(param.origin, param.originRoot, param.end, param.endRoot, param.souce, param.endRoot.Value + toTarget);
                    instance.Init(new_param);
                }
                //���X�g�ɒǉ�
                magicObjectList.Add(instance);

                //�x�N�g�����Ȃ���
                toTarget = Quaternion.Euler(0, maxAngle / (amount - 1), 0) * toTarget;
            }

            //���X�g��Ԃ�
            return magicObjectList;
        }
    }
}
