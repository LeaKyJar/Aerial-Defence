using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

    private Text timerText;
    private float timer = 10f;
    private bool deny = false;
    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!deny)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("f0") + "s";
        }
        else
        {
            timer -= Time.deltaTime;
            timerText.text = "The match has been declined. \n"+ timer.ToString("f0") + "s";
        }
    }

    public static void NextScreen()
    {
        SceneManager.LoadScene("Scenes/Game Scene1");
    }
    void OnDisable()
    {
        timer = 10f;
        deny = false;
    }
    public void reject() {
        deny = true;
    }
}
