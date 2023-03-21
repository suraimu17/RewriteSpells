using System.Collections.Generic;
using UnityEngine;


namespace CM.Magic.Temp
{
    public class MagicLoader
    {
        //�p�X
        private const string SPELL_FOLDER_NAME = "Spells";
        private const string SUPPORT_FOLDER_NAME = "Supports";
        private const string PARENT_GAMEOBJECT = "SpellGameObjectStack";

        //�Ǘ��pDic
        private Dictionary<string, Spell> spellDic;
        private Dictionary<string, Support> supportDic;

        private GameObject parent;

        //�V���O���g��
        private static MagicLoader _loader;
        public static MagicLoader loader
        {
            get
            {
                return _loader != null ? _loader : _loader = new MagicLoader();
            }
        }

        //�R���X�g���N�^�ŃA�Z�b�g�ǂݍ���
        private MagicLoader()
        {
            //�X�y�����[�h
            spellDic = new Dictionary<string, Spell>();
            Object[] o_spell = Resources.LoadAll(SPELL_FOLDER_NAME, typeof(Spell));
            foreach (Spell spell in o_spell)
            {
                spellDic.Add(spell.name, spell);
                Debug.Log("<color=red>" + spell.name + " is Loaded</color>");
            }

            //�T�|�[�g���[�h
            supportDic = new Dictionary<string, Support>();
            Object[] o_support = Resources.LoadAll(SUPPORT_FOLDER_NAME, typeof(Support));
            foreach (Support support in o_support)
            {
                supportDic.Add(support.name, support);
                Debug.Log("<color=red>" + support.name + " is Loaded</color>");
            }
        }



        //ID�ɂ��X�y���A�T�|�[�g�̎擾
        public Spell GetSpell(string name, int level)
        {
            if (spellDic.ContainsKey(name))
            {
                Spell spell = spellDic[name];
                Spell spellInstance = spell.Instantiate(level);

                spellInstance.name = spell.name;    //(Clone)������


                //�e�I�u�W�F�N�g
                if(parent == null) parent = new GameObject(PARENT_GAMEOBJECT);
                spellInstance.transform.parent = parent.transform;

                return spellInstance;
            }
            else
                return null;
        }

        public Support GetSupport(string name)
        {
            if (supportDic.ContainsKey(name))
            {
                Support support = supportDic[name];
                Support supportInstance = GameObject.Instantiate(support) as Support;

                supportInstance.name = support.name;    //(Clone)������

                return supportInstance;
            }
            else
                return null;
        }
    }

}