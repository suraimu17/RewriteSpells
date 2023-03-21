using UnityEngine;

using UniRx;
using System.Collections.Generic;


using Inventories.Parameters;

using CM.Magic;
using CM.Magic.Temp;

namespace Inventories.Debugs
{
    public class DebugEquipment : MonoBehaviour
    {
        //�ΏۃC���x���g��
        [SerializeField] private Inventory inventory;

        //�ΏۃL���X�^�[
        [SerializeField] private Caster caster;
        [SerializeField] private GameObject spellsParent;

        //�j���p�Ɏ����Ă���
        List<Spell> spellList = new List<Spell>();
 
        //�o�^����
        private void Start()
        {
            inventory.observable.Subscribe(_ => OnEquipUpdate());
        }


        //������(�O������Ăׂ�悤�ɂ���)
        public void OnEquipUpdate()
        {
            //���R�[�h

            /*
            //�����ς݃C���X�^���X�����ׂĔj��
            foreach(Spell spell in spellList)
            {
                Destroy(spell.gameObject);
            }
            spellList.Clear();

            //�C���x���g����S�Ď擾
            for(int i = 0; i < inventory.maxSlot; i++)
            {
                ItemStack item = inventory.GetItemStack(i);
                if (item == null)
                {
                    caster.SetCastable(null, i);
                    continue;
                }
                if (item.type != ItemType.SPELL)
                {
                    caster.SetCastable(null, i);
                    continue;
                }
                ItemSpellParam param = (ItemSpellParam)item.parameter;


                //�X�y�����擾
                Spell spell = MagicLoader.loader.GetSpell(item.code, 0);

                //�T�|�[�g���擾���ē����
                foreach (ItemStack sup in param.supports)
                {
                    if (sup == null) continue;
                    Support support = MagicLoader.loader.GetSupport(sup.code);
                    spell.AddSupport(support);
                }
                spell.Reload();

                //caster�ɃZ�b�g
                caster.SetCastable(spell, i);

                //caster�̎q�ɂ���
                if (spellsParent != null) spell.gameObject.transform.parent = spellsParent.transform;
                else spell.gameObject.transform.parent = caster.transform;

                spellList.Add(spell);
            }
            */
        }
    }
}

