using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomber : Character, IPointerClickHandler
{
    public GameObject atkButton;
    public static Bomber instance = null;

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

    public override void TakeDamage()
    {
        Dead = true;
        GameObject.Find("Champion").SetActive(false);
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
