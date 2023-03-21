using UnityEngine;

using CM.Magic.Legacy;

namespace CM.Magic
{
    public class Support : MonoBehaviour
    {
        //�C���X�y�N�^�p�����[�^ �ÓI
        [Header("Parameter")]
        [SerializeField] private float _addLevel;
        [SerializeField] private float _addDelayTime;
        [SerializeField] private float _addCoolTime;
        [SerializeField] private float _addManaCost;

        //�ǉ��R���g���[���[
        [Header("Controller")]
        [SerializeField] private MagicObjectController _addController;
        [SerializeField] private SpellObjectControllerType _addControllerType;

        //����
        [Header("Condtion")]
        [SerializeField] private SpellObjectType _spellObjectType;



        //�Q�Ɨp
        public float addLevel => _addLevel;
        public float addDelayTime => _addDelayTime;
        public float addCoolTime => _addCoolTime;
        public float addManaCost => _addManaCost;
        public MagicObjectController addController => _addController;
        public SpellObjectControllerType addControllerType => _addControllerType;
        public SpellObjectType spellObjectType => _spellObjectType;
    }
}
