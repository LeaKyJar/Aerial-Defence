using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
 
    public void PlayGame() {
        //TODO: Run algorithm
        //Timer to change to setup screen before algorithm is up.
       
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); //Go to setup screen
           

    }

    public void QuitGame() { 
        Application.Quit();
    }
    
}
