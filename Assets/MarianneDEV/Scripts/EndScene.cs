using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public GameObject mainMenu;

    public void MainMenu() 
    {
        SceneManager.LoadScene(0);
    }
    
}
