using UnityEngine;
using UnityEngine.UI;

using UniRx;

using Game.players;


namespace CM.UI.Presenter
{
    public class HpTextPresenter : MonoBehaviour
    {

        [SerializeField] private Text text;
        [SerializeField] private PlayerHP hp;


        private void Start()
        {
            text.text = hp.onUpdateHealth.Value.ToString();

            int value = 0;
            hp.onUpdateHealth.Subscribe(currentHealth =>
            {
                if (currentHealth < 0) value = 0;
                else value = currentHealth;

                text.text = value.ToString();
            }).AddTo(this);
        }

    }
}
