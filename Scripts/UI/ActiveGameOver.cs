using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.players;
using Game.Manager;

namespace Game.Manager
{
    public class ActiveGameOver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        //操作を加えたい子オブジェクトの親オブジェクトのTransformコンポーネントをインスペクターから選択する
        [SerializeField] private Transform _parentTransform;
        //private FloorManager floorManager;


        /*void Start()
        {
            floorManager = FindObjectOfType<FloorManager>();
        }*/
 
        void Update()
        {
            if (PlayerHP.instance.Alive==false)
            {
                //ゲームオブジェクト非表示→表示
                gameOverPanel.SetActive(true);
                FadeController.instance.isFadeOut = true;
                //フロアマネージャーから現在のフロアに応じてスコアを表示
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(FloorManager._currentFloor);   //static変数で参照するようにする
                PlayerHP.instance.Alive = true;

            }
        }
    }
}
    

