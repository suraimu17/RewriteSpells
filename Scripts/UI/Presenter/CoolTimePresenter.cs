using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

using CM.Magic;
using CM.Magic.Parameter;

using Inventories;

namespace CM.UI.Presenter
{
    public class CoolTimePresenter : MonoBehaviour
    {
        //Model
        [Header("Model")]
        [SerializeField] private Inventories.Inventory hotBar;
        [SerializeField] private Caster caster;

        //View
        [Header("View")]
        [SerializeField] private GameObject grid;  //�Ώۂ̃O���b�h ������(�����ɑΉ����邽�߂Ƃ����܂�slot�̐e�ł���Ƃ����_���d�v
        private Slot[] slots;                      //View�̃X���b�g�z��
        [SerializeField] private Text text;        //text��Prefab


        //�񓯊��Ȃ��ߑO��text���c���Ă�����������߂̎���
        private Dictionary<ICastable, Text> coolTimeDictionary = new Dictionary<ICastable, Text>();



        public void Awake()
        {
            //�C�x���g�o�^
            IDisposable disposable = null;
            disposable = grid.UpdateAsObservable().Subscribe(_ =>
            {
                //������
                Init();

                //�C�x���g��j��(2F�ȍ~�Ăяo���Ȃ��悤��)
                disposable.Dispose();
            });
        }




        //������
        private void Init()
        {
            //���f���̓o�^
            caster.onCastEvent.Subscribe(e => OnCast(e)).AddTo(this);

            //View�̎擾
            slots = grid.GetComponentsInChildren<Slot>();
            for (int i = 0; i < slots.Length; i++)
            {
                //�X���b�g�擾
                Slot slot = slots[i];
            }
        }


        //�L���X�g��
        private void OnCast(CastEvent castEvent)
        {
            Spell spell = castEvent.castable as Spell;
            if (spell != null)
            {
                //�L���X�g���ꂽ���@���Y���̃C���x���g���ɂ��邩����
                int? index = null;
                for (int i = 0; i < hotBar.maxSlot; i++)
                {
                    ItemStack item = hotBar.GetItemStack(i);
                    if (item != null)
                    {
                        if (castEvent.castable == item.castable)
                        {
                            index = i;
                            break;
                        }
                    }
                }
                if (index == null) return;�@//���������璆�f


                //�e�L�X�g���܂܂�Ă�����폜
                if (coolTimeDictionary.ContainsKey(spell))
                {
                    Text preInstance = coolTimeDictionary[spell];
                    if (preInstance != null) Destroy(preInstance);
                    coolTimeDictionary.Remove(spell);
                }

                //�e�L�X�g�𐶐����ĕ\��
                Text textInstance = Instantiate(text);
                textInstance.gameObject.SetActive(true);


                //text��index�ɉ����ďC��
                Slot slot = slots[index.Value];
                textInstance.transform.parent = text.transform.parent;
                textInstance.transform.position = slot.transform.position;


                //�ǉ�����
                coolTimeDictionary.Add(spell, textInstance);


                //���Ԃ�������
                float coolTime = spell.coolTime;
                float timeSpan = 0.1f;


                //�񓯊��ŃJ�E���g�_�E��(0.1�b�P��)
                new Action(async () =>
                {

                    while (coolTime > 0)
                    {
                        //�e�L�X�g�̍X�V
                        if(coolTime > 9.9 && textInstance) textInstance.text = ((int)coolTime).ToString();
                        else if (textInstance) textInstance.text = coolTime.ToString("f1");

                        //�f�B���C�̌�b������
                        await UniTask.Delay(TimeSpan.FromSeconds(timeSpan));
                        coolTime -= timeSpan;
                    }
                    if(textInstance != null) Destroy(textInstance.gameObject);  //�񓯊��Ȃ̂ŃV�[���J�ڑ΍�ƁA��ō폜���Ȃ������ꍇ��
                })();
            }
        }
    }

}