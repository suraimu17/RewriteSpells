using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using UniRx;

using CM.Magic.Parameter;


namespace CM.Magic
{
    [RequireComponent(typeof(IMana))]
    [RequireComponent(typeof(ITargetProvider))]
    public class Caster : MonoBehaviour
    {
        [SerializeField] private Transform root;            //�����n�_
        [SerializeField] private float castTimeMultiply;    //�d�����Ԕ{��
        [SerializeField] private bool freezeY;              //���@��Y�����Œ肷�邩

        //�R���|�[�l���g
        private IMana mana;
        private ITargetProvider provider;


        //�g�p���̖��@��ۑ�
        private ICastable casting;
        public bool isCasting => casting != null;


        //�C�x���g
        private Subject<CastEvent> onCastSubject = new Subject<CastEvent>();
        private Subject<CastEvent> onEndCastSubject = new Subject<CastEvent>();
        public IObservable<CastEvent> onCastEvent => onCastSubject;
        public IObservable<CastEvent> onEndCastEvent => onEndCastSubject;




        //������
        private void Awake()
        {
            //�R���|�[�l���g�̎擾 �擾�Ɏ��s������G���[
            mana = GetComponent<IMana>();
            provider = GetComponent<ITargetProvider>();
            if(mana == null || provider == null)
            {
                Debug.LogError("Not Found IMana or ITargetProvider Component");
                enabled = false;
                return;
            }

            //root���ݒ肳��Ă��Ȃ�������Caster�ɂ���
            if (!root) root = transform;
        }

        


        //���@�̎g�p �L���X�g�������d������(castTime)��Ԃ�
        public float Cast(ICastable castable)
        {
            if(!isCasting && castable != null)         //�r�����łȂ���,castable��null�łȂ���
            {
                //�^�[�Q�b�g�̐ݒ�
                Vector3 target = provider.GetTarget(root);
                if (freezeY) target.y = root.position.y;

                //�r��
                float time = castable.Cast(gameObject, root, target, mana);
                if (time > 0)                                       //�������r���ł������𔻒�
                {
                    //�r���������@��ێ�
                    casting = castable;

                    //�d������(�r������)���Z�o
                    float freezeTime = time * castTimeMultiply;

                    //�C�x���g���s
                    CastEvent castEvent = new CastEvent(castable, freezeTime);
                    onCastSubject.OnNext(castEvent);

                    //casting�̍X�V�ƃC�x���g�̔��s
                    new Action(async () =>
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(freezeTime));
                        casting = null;
                        onEndCastSubject.OnNext(castEvent);
                    })();

                    return castEvent.freezeTime;
                }
            }
            return 0;
        }



        //���@�̃L�����Z��
        public void CancelCast()
        {
            if (isCasting)
            {
                casting.CancelCast();
                casting = null;
            }
        }
    }




    //�r���C���^�[�t�F�[�X
    public interface ICastable
    {
        //�r�� ���������ꍇ�ҋ@���Ԃ�Ԃ� ���s��null��Ԃ�
        public float Cast(GameObject origin, Transform root, Vector3 target, IMana mana);

        //�L�����Z��
        public void CancelCast();
    }


    //�}�i�C���^�[�t�F�[�X
    public interface IMana
    {
        public bool Reduce(float value);
    }


    //�^�[�Q�b�g�񋟃C���^�[�t�F�[�X
    public interface ITargetProvider
    {
        //�^�[�Q�b�g�擾
        public Vector3 GetTarget(Transform root); //target����Transform������
    }



    /*
     * freezeTime�Ɋւ��Ẵ��� �A�j���[�V�����̒�����1�b����Ȃ��Ȃ�Ō�ɏ�Z
     * 
     * player => 17F�ڍU���A30F�ڏI�� => 30/17
     * 
     * ���@�g�� => player�Ɠ���
     * 
     * �w� => 35/24/1.458�@
     * 
     * �X�P���g�� => 30/10
     * 
     * �S�[���� => 60/30
     * 
     */
}




