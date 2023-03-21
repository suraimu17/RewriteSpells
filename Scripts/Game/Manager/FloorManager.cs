using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UniRx;
using Game.players;
using Game.Stage;
using Game.Effects;
using UnityEngine.SceneManagement;

namespace Game.Manager
{
    public class FloorManager : MonoBehaviour
    {
        [SerializeField] private Text floorText;
        [SerializeField] private GameObject choicePanel;
        [SerializeField] private Transform MoveSpPos;


        //参照を壊さないために、プロパティを介してstatic変数を弄るように改変
        public static int _currentFloor { private set; get; }   //ActiveGameOverのみは別シーンなため、publicでここから参照できるようにしておく。
        public int currentFloor { private set { _currentFloor = value; } get { return _currentFloor; } }


        private PlayerStairsSearch playerStairsSearch;
        private SceneInitializer sceneInitializer;
        private Fade fade;

        public bool IsStairs { private set; get; } = false;

        private bool IsSpfloor = false;

        private float fadeOutSpeed = 0f;


        private void Awake()
        {
            currentFloor = 1;
        }
        private void Start()
        {
            playerStairsSearch = FindObjectOfType<PlayerStairsSearch>();
            sceneInitializer = FindObjectOfType<SceneInitializer>();
            fade = FindObjectOfType<Fade>();

            FloorObservable();
            FloorStart();

            //currentFloorの初期化
        }

        private void FloorObservable()
        {
            this.ObserveEveryValueChanged(x => x.playerStairsSearch.onStairs)
                 .Where(x => x == true)
                 .Subscribe(_ =>
                 {
                     IsStairs = true;
                     OpenChoice();
                 })
                 .AddTo(this);

        }

        private IEnumerator StepFloor() 
        {
            floorText.enabled = true;
            floorText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(95, 25, 0);
            floorText.text = "B"+currentFloor+"F";

            sceneInitializer.DestroyStage();//TODO Task使えば、とめた後に敵の攻撃をなくせるかも
            yield return new WaitForSeconds(2.0f);
            fade.FadeIn(2.0f);
            floorText.enabled = false;
            IsStairs = false;

            sceneInitializer.ReloadStage();
        }

        private void OpenChoice() 
        {
            choicePanel.SetActive(true);
            Time.timeScale = 0;
        }

        public void CloseChoice() 
        {
            choicePanel.SetActive(false);
            Time.timeScale = 1;
            IsStairs = false;
        }

        public void ChoiceDown() 
        {
            CloseChoice();
            IsStairs = true;

            fadeOutSpeed = 1.0f;
            if (currentFloor%3==0&&IsSpfloor==false) 
            {
                IsSpfloor = true;
                Action on_sp = () =>
                {
                    StartCoroutine(StepSP());
                };
                fade.FadeOut(1.0f, on_sp);
                return;
            }

            currentFloor++;
            Debug.Log("現在の階層 " + currentFloor + "F");
            FloorStart();
            IsSpfloor = false;
        }

        private IEnumerator StepSP()
        {
            floorText.enabled = true;
            floorText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-55, 25, 0);
            floorText.text = "祈祷の間";

            sceneInitializer.DestroyStage();
            yield return new WaitForSeconds(2.0f);
            fade.FadeIn(2.0f);
            floorText.enabled = false;
            IsStairs = false;

            sceneInitializer._player.transform.position = MoveSpPos.position;
        }

        private void FloorStart() 
        {
            Action on_completed = () =>
            {
                StartCoroutine(StepFloor());
            };
            fade.FadeOut(fadeOutSpeed, on_completed);
            fade.alphaZero();
        }
    }
}
