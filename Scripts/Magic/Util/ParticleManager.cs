using UnityEngine;

using UniRx;

using CM.Magic.MagicObject;


namespace CM.Magic.Util
{
    //�v���R���|�[�l���g
    [RequireComponent(typeof(BaseMagicObject))]
    public class ParticleManager : MonoBehaviour
    {
        //�p�[�e�B�N��
        [SerializeField] private ParticleSystem generateParticle;
        [SerializeField] private ParticleSystem removeParticle;

        //�n�ʂɐڐG������
        [SerializeField] private bool isGround;
        [SerializeField] private float groundOffset;


        //������ Start����OnGenerate����������ɓo�^���邱�ƂɂȂ�̂ŋC��t����
        private void Awake()
        {
            BaseMagicObject magicObject = GetComponent<BaseMagicObject>();

            //Particle���\��
            if(generateParticle) generateParticle.gameObject.SetActive(false);
            if(removeParticle) removeParticle.gameObject.SetActive(false);

            
            if (generateParticle != null) magicObject.onGenerate.Subscribe(_ =>
             {
                 //�L�������Đe�𖳂��ɂ���
                 generateParticle.gameObject.SetActive(true);

                 //�X�P�[�����O
                 Vector3 scale = generateParticle.transform.lossyScale;
                 generateParticle.transform.parent = null;
                 generateParticle.transform.localScale = scale;

                 if (isGround) GroundOffset(generateParticle.transform);

             }).AddTo(this);


            if (removeParticle != null) magicObject.onRemove.Subscribe(_ =>
             {
                 //�L�������Đe�𖳂��ɂ���
                 removeParticle.gameObject.SetActive(true);

                 //�X�P�[�����O
                 Vector3 scale = removeParticle.transform.lossyScale;
                 removeParticle.transform.parent = null;
                 removeParticle.transform.localScale = scale;

                 if (isGround) GroundOffset(removeParticle.transform);
             }).AddTo(this);
        }



        //groundOffset
        private void GroundOffset(Transform transform)
        {
            //�ǂ��ђʂ��Ă���ꍇ�͕ǑO�ɐݒ�
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + groundOffset, transform.position.z);
            }
        }

    }
}
