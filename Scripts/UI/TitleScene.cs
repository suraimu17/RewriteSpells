using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{

    public void OnStart()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
