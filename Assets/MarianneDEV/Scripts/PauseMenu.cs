using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject PauseMenuUI;

    private void Awake()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {

            
            if(GamePaused == true)
            {
                ResumeGame();
            }
            else             
            {
                PauseGame();
                
                
            }
        }

        if(GamePaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
 
    }

    public void ResumeGame()
    {
        //Disable PauseMenu
        PauseMenuUI.SetActive(false);

        //UnFreezeTime
        Time.timeScale = 1f;

        //GamePaused to false
        GamePaused = false;

        

        
    }


    void PauseGame()
    {
        
        //Show PauseMenu
        PauseMenuUI.SetActive(true);

        //FreezeTime
        Time.timeScale = 0f;

        //GamePaused to true
        GamePaused = true;

        //cursor visible
        Cursor.visible = true;

        

    }

    public void LoadMenu()
    {
        
        SceneManager.LoadScene("_MainMenu");
        
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("_MainTest");
    }
}
