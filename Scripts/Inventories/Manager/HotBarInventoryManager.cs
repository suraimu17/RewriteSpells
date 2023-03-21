using System.Collections.Generic;

using UnityEngine;

using UniRx;

using CM.Magic;



namespace Inventories.Manager
{
    public class HotBarInventoryManager : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private Caster caster;

        private List<ICastable> casting = new List<ICastable>();

        private void Start()
        {
            //�N�[���^�C�����̃A�C�e���𓮂����Ȃ��悤��
            inventory.setCondition = (item, index) =>
            {
                //�z�b�g�o�[�ɃZ�b�g���悤�Ƃ����Ƃ�
                if (item != null)
                {
                    if (casting.Contains(item.castable)) return false;
                }

                //�z�b�g�o�[�ɕʂ̃z�b�g�o�[����Z�b�g���悤�Ƃ����Ƃ�
                if (inventory.GetItemStack(index) != null)
                {
                    if (casting.Contains(inventory.GetItemStack(index).castable)) return false;
                }

                return true;
            };


            //�C�x���g�o�^
            caster.onCastEvent.Subscribe(e =>
            {
                Spell spell = e.castable as Spell;
                if (spell.coolTime <= 0) return;
                casting.Add(e.castable);

                spell.onEndCoolTime.Subscribe(e =>
                    casting.Remove(spell));
            });
        }
    }
}
