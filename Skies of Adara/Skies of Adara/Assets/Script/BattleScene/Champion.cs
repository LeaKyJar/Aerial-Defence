using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Champion : Character, IPointerClickHandler
{

    private bool charSelected = false;
    public static Champion instance = null;
    public GameObject atkButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (GameManager.instance.AtkPhase && !this.CharSelected && this.Active)
        {
            atkButton.SetActive(true);
            this.CharSelected = true;
        }
        else if (GameManager.instance.AtkPhase && this.CharSelected)
        {
            atkButton.SetActive(false);
            this.CharSelected = false;
        }
    }
}
