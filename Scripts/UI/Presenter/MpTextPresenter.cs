using UnityEngine;
using UnityEngine.UI;

using UniRx;

using Game.players;


namespace CM.UI.Presenter
{
    public class MpTextPresenter : MonoBehaviour
    {

        [SerializeField] private Text text;
        [SerializeField] private PlayerMP mp;


        private void Start()
        {
            text.text = mp.onUpdateMp.Value.ToString();

            int value = 0;
            mp.onUpdateMp.Subscribe(currentMp =>
            {
                if (currentMp < 0) value = 0;
                else value = (int)currentMp;

                text.text = value.ToString();
            }).AddTo(this);
        }

    }
}