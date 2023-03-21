using UnityEngine;

using UniRx;
using UniRx.Triggers;

using CM.Magic.Parameter;


namespace CM.Magic.MagicObject
{
    //�K�v�R���|�[�l���g
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Projectile : BaseMagicObject
    {
        //�p�����[�^
        [SerializeField] private float speed;
        [SerializeField] private float range;
        [SerializeField] private bool isInitialDirection;

        //����
        private Vector3 direction;


        //�ړ���������
        private float distance = 0;

        //rigidBody
        private new Rigidbody rigidbody;


        //������
        public override void Trigger()
        {
            //�e��Init
            base.Trigger();

            //endObject��filter�ɒǉ�����
            if (this.param.end) filterList.Add(this.param.end);

            //���������߂ĉ�] ��]�������㏑��
            if(isInitialDirection) direction = direction = (this.param.target - param.souce).normalized;
            else direction = (this.param.target - transform.position).normalized;
            gameObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            //rigidBody�̎擾
            rigidbody = GetComponent<Rigidbody>();

            //fixedUpdate�Ői�ނ悤�ɂ���
            this.FixedUpdateAsObservable()
                .Where(_ => distance < range)
                .Subscribe(_ =>
                {
                    rigidbody.MovePosition(transform.position + direction * speed);
                    distance += speed;
                }).AddTo(this);


            //�˒��O���Ŏ���remove���Ă�
            this.FixedUpdateAsObservable()
                .Where(_ => distance >= range)
                .Subscribe(_ =>
                {
                    this.param = new MagicTriggerParameter(this.param, null, transform.position);
                    OnRemove();
                    Destroy(gameObject);
                }).AddTo(this);


            //�Փˎ�
            this.OnTriggerEnterAsObservable()
                .Where(collider => !collider.isTrigger)                         //���肪�g���K�[����Ȃ�
                .Where(collider => param.isLivingOrigin())                      //Origin�����m�F
                .Where(collider => collider.tag != this.param.origin.tag)       //�^�O��origin�ƈႤ
                .Where(collider => !filterList.Contains(collider.gameObject))   //filter�Ɋ܂܂�ĂȂ�
                .Subscribe(collider =>
                {
                    //�\���̂��X�V
                    this.param = new MagicTriggerParameter(this.param, collider.gameObject, transform.position);

                    //�_���[�W��^������ꍇ�͗^����
                    IDamagable damagable = collider.GetComponent<IDamagable>();
                    if (damagable != null) damagable.Damage(damage);


                    OnRemove();
                    Destroy(gameObject);
                }).AddTo(this);

            //�S�ďI�������OnGenerate
            OnGenerate();
        }
    }
}
