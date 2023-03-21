using UnityEngine;

using CM.Magic.Legacy;

namespace CM.Magic
{
    public class Support : MonoBehaviour
    {
        //インスペクタパラメータ 静的
        [Header("Parameter")]
        [SerializeField] private float _addLevel;
        [SerializeField] private float _addDelayTime;
        [SerializeField] private float _addCoolTime;
        [SerializeField] private float _addManaCost;

        //追加コントローラー
        [Header("Controller")]
        [SerializeField] private MagicObjectController _addController;
        [SerializeField] private SpellObjectControllerType _addControllerType;

        //条件
        [Header("Condtion")]
        [SerializeField] private SpellObjectType _spellObjectType;



        //参照用
        public float addLevel => _addLevel;
        public float addDelayTime => _addDelayTime;
        public float addCoolTime => _addCoolTime;
        public float addManaCost => _addManaCost;
        public MagicObjectController addController => _addController;
        public SpellObjectControllerType addControllerType => _addControllerType;
        public SpellObjectType spellObjectType => _spellObjectType;
    }
}
