using System.Collections.Generic;
using UnityEngine;

using UniRx;

using CM.Magic.MagicObject;
using CM.Magic.Parameter;
using CM.Magic.Legacy;

namespace CM.Magic
{
    [RequireComponent(typeof(IMagicObjectGenerator))]
    public class MagicObjectController : MonoBehaviour
    {
        //��������X�y���I�u�W�F�N�g
        [Header("MagicObject")]
        [SerializeField] private BaseMagicObject magicObject;
        [SerializeField] private bool isNullController = false;         //NullController����p(magicObject�ɃZ�b�g����ĂĂ���������)
        public SpellObjectType spellObjectType => SpellObjectType.NONE.GetSpellObjectType(magicObject); //�^�C�v��Ԃ�

        //����
        private IMagicObjectGenerator generator;


        //���̃R���g���[���[ ����/���Ԍo��/���Ŏ� �Ɉڍs
        [Header("BaseController")]
        [SerializeField] private MagicObjectController onGenerateController;
        [SerializeField] private MagicObjectController onRemoveController;


        //�ǉ��R���g���[���[ ���I�ɕω�����
        private MagicObjectController onGenerateAddController;
        private MagicObjectController onRemoveAddController;




        //�������Ɉ�x�����ĂׂΗǂ�������
        private void Awake()
        {
            //generator�擾 �擾�Ɏ��s������G���[
            generator = GetComponent<IMagicObjectGenerator>();
            if(generator == null)
            {
                Debug.LogError("Not Found IMagicObjectGenerator Component");
                enabled = false;
                return;
            }

            //magicObject���A�N�e�B�u��
            if(magicObject) magicObject.gameObject.SetActive(false);
        }



        //�ǉ��R���g���[���[���폜����
        public void ClearController()
        {
            //�ǉ��R���g���[���[��S�Ĕj��
            onGenerateAddController = null;
            onRemoveAddController = null;

            if (isNullController) magicObject = null;

            //�Ȃ����Ă�����̂��Ăяo��
            if (onGenerateController != null) onGenerateController.ClearController();
            if (onRemoveController != null) onRemoveController.ClearController();
        }

        //�ēǂݍ���
        public void Reload(int level)
        {
            //magicObject�̃��x��������
            magicObject.Init(level);

            //�e�R���g���[���[��null�̎���magicObject�𕡐�����悤�ɂ��� �X��Reload�����s
            //Generate
            if (onGenerateController)
            {                                    
                //Null�R���g���[���[�̎�����
                if (onGenerateController.isNullController)
                {
                    onGenerateController.magicObject = magicObject;
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onGenerateController.onGenerateAddController = onGenerateAddController; //AddGenerate
                    if (onRemoveController) onGenerateController.onRemoveController = onRemoveController;                                                             //Remove
                    if (onRemoveAddController) onGenerateController.onRemoveAddController = onRemoveAddController;                                                    //AddRemove
                }

                //�����[�h
                onGenerateController.Reload(level);
            }
            //AddGenerate
            if (onGenerateAddController)
            {
                if (onGenerateAddController.isNullController)
                {
                    onGenerateAddController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onGenerateAddController.onGenerateController = onGenerateController;           //Generate
                    if (onRemoveController) onGenerateAddController.onRemoveController = onRemoveController;                                                           //Remove
                    if (onRemoveAddController) onGenerateAddController.onRemoveAddController = onRemoveAddController;                                                  //AddRemove
                }
                onGenerateAddController.Reload(level);
            }
            //Remove
            if (onRemoveController)      //�S�Ăɂ�����nullable�֎~
            {
                if (onRemoveController.isNullController)
                {
                    onRemoveController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onRemoveController.onGenerateController = onGenerateController;                //Generate
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onRemoveController.onGenerateAddController = onGenerateAddController;    //AddGenerate
                    if (onRemoveAddController && !onRemoveAddController.isNullController) onRemoveController.onRemoveAddController = onRemoveAddController;            //AddRemove
                }
                onRemoveController.Reload(level);
            }
            //RemoveAdd
            if (onRemoveAddController)
            {
                if (onRemoveAddController.isNullController)
                {
                    onRemoveAddController.magicObject = magicObject;
                    if (onGenerateController && !onGenerateController.isNullController) onRemoveAddController.onGenerateController = onGenerateController;                //Generate
                    if (onGenerateAddController && !onGenerateAddController.isNullController) onRemoveAddController.onGenerateAddController = onGenerateAddController;    //AddGenerate
                    if (onRemoveController && !onRemoveController.isNullController) onRemoveAddController.onRemoveController = onRemoveController;                        //Remove
                }
                onRemoveAddController.Reload(level);
            }
        }






        //�R���g���[���[�ǉ�
        public void AddController(MagicObjectController controller, SpellObjectControllerType controllerType)
        {
            //�R���g���[���[�ǉ�
            switch (controllerType)
            {
                case SpellObjectControllerType.GENERATE:
                    if(!onGenerateAddController) onGenerateAddController = controller;
                    break;
                case SpellObjectControllerType.REMOVE:
                    if (!onRemoveAddController) onRemoveAddController = controller;
                    break;
            }
        }




        //���Ί֐� - Init�̍Ō��generate���Ă�ł�p�^�[�������邽�ߐ�ɃC�x���g�o�^���ς܂���
        public void Trigger(MagicTriggerParameter param)
        {
            //param��Origin�̎Q�Ƃ��؂�Ă��Ȃ����m�F
            if (!param.isLivingOrigin()) return;

            //generator�Ő���
            List<BaseMagicObject> magicObjectList = generator.Generate(magicObject, param);

            //�C�x���g�o�^����
            foreach(BaseMagicObject magicObject in magicObjectList)
            {
                //�C�x���g�o�^
                magicObject.onGenerate.Subscribe(_ =>
                {
                    if (!magicObject.param.isLivingOrigin()) return;
                    if (onGenerateController) onGenerateController.Trigger(magicObject.param);
                    if (onGenerateAddController) onGenerateAddController.Trigger(magicObject.param);
                }).AddTo(this);
                magicObject.onRemove.Subscribe(_ =>
                {
                    if (!magicObject.param.isLivingOrigin()) return;
                    if (onRemoveController) onRemoveController.Trigger(magicObject.param);
                    if (onRemoveAddController) onRemoveAddController.Trigger(magicObject.param);
                }).AddTo(this);

                //������ - generator�Ŕ񓯊������ꍇ�͂����ł�origin�̐����m�F������ׂ�
                //(�����X���b�h�҂�����Ȃ�Generator�C���^�t�F�[�X���ς��̂łƂ肠�����͂��̂܂܂ɂ��Ă���)
                //�ꉞ�ŏ��ɐ����m�F���s���Ă��邽�߂��̎��_�ł�origin�͐����Ă���
                //magicObject.Init(param);
                //->param��ς������Ƃ��ł��Ȃ����߂����ł͂��Ȃ�
                //�J�n�ɕύX
                magicObject.Trigger();
            }
        }


    }




    //Generator�C���^�[�t�F�[�X
    public interface IMagicObjectGenerator
    {
        public List<BaseMagicObject> Generate(BaseMagicObject prefab, MagicTriggerParameter param);
    }
}
