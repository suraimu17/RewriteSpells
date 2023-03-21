using UnityEngine;

using UniRx;

using CM.Magic.MagicObject;


namespace CM.Magic.Util
{
    //要求コンポーネント
    [RequireComponent(typeof(BaseMagicObject))]
    public class ParticleManager : MonoBehaviour
    {
        //パーティクル
        [SerializeField] private ParticleSystem generateParticle;
        [SerializeField] private ParticleSystem removeParticle;

        //地面に接触させる
        [SerializeField] private bool isGround;
        [SerializeField] private float groundOffset;


        //初期化 StartだとOnGenerateが走った後に登録することになるので気を付ける
        private void Awake()
        {
            BaseMagicObject magicObject = GetComponent<BaseMagicObject>();

            //Particleを非表示
            if(generateParticle) generateParticle.gameObject.SetActive(false);
            if(removeParticle) removeParticle.gameObject.SetActive(false);

            
            if (generateParticle != null) magicObject.onGenerate.Subscribe(_ =>
             {
                 //有効化して親を無しにする
                 generateParticle.gameObject.SetActive(true);

                 //スケーリング
                 Vector3 scale = generateParticle.transform.lossyScale;
                 generateParticle.transform.parent = null;
                 generateParticle.transform.localScale = scale;

                 if (isGround) GroundOffset(generateParticle.transform);

             }).AddTo(this);


            if (removeParticle != null) magicObject.onRemove.Subscribe(_ =>
             {
                 //有効化して親を無しにする
                 removeParticle.gameObject.SetActive(true);

                 //スケーリング
                 Vector3 scale = removeParticle.transform.lossyScale;
                 removeParticle.transform.parent = null;
                 removeParticle.transform.localScale = scale;

                 if (isGround) GroundOffset(removeParticle.transform);
             }).AddTo(this);
        }



        //groundOffset
        private void GroundOffset(Transform transform)
        {
            //壁を貫通している場合は壁前に設定
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + groundOffset, transform.position.z);
            }
        }

    }
}
