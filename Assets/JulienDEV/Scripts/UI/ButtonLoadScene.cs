using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonLoadScene : MonoBehaviour
{
    //[SerializeField] private string _gamePlayScene;

    public void StartButton()
    {
        //SceneManager.LoadScene(_gamePlayScene, LoadSceneMode.Single);

        Debug.Log("loading game");
        SceneManager.LoadScene(1);
    }
}
