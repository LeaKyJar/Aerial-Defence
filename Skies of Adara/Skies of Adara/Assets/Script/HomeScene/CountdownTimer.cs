using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

    private Text timerText;
    private float timer = 0f;
    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("f0") + "s";
    }

    public static void NextScreen()
    {
        SceneManager.LoadScene("Scenes/Game Scene1");
    }
    void OnDisable()
    {
        timer = 0f;
    }
}
