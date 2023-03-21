using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using UniRx;
using UniRx.Triggers;

using CM.Magic.Parameter;

namespace CM.Magic.MagicObject
{
    //�K�v�R���|�[�l���g
    [RequireComponent(typeof(Collider))]
    public class AreaOfEffect : BaseMagicObject
    {
        //�p�����[�^
        [SerializeField] private float duration;     //���ʎ���

        
        //������
        public override void Trigger()
        {
            //�e��Init
            base.Trigger();

            //���t���[����ɍ폜
            new Action(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                if (this != null)
                {
                    Destroy(gameObject);
                    OnRemove();
                }
            })();

            //�Փˎ�
            this.OnTriggerEnterAsObservable()
                .Where(collider => !collider.isTrigger)                         //���肪�g���K�[����Ȃ�
                .Where(collider => this.param.isLivingOrigin())                 //Origin�����m�F
                .Where(collider => collider.tag != this.param.origin.tag)       //�^�O��origin�ƈႤ
                .Where(collider => !filterList.Contains(collider.gameObject))   //filter�Ɋ܂܂�ĂȂ�
                .Subscribe(collider =>
                {
                    this.param = new MagicTriggerParameter(this.param, param.end, transform.position);  //end�ς��Ȃ�

                    //�_���[�W��^������ꍇ�͗^����
                    IDamagable damagable = collider.GetComponent<IDamagable>();
                    if (damagable != null) damagable.Damage(damage);

                    //filter�ɒǉ�
                    filterList.Add(collider.gameObject);
                }).AddTo(this);

            //�S�ďI�������OnGenerate
            OnGenerate();
        }
    }
}
