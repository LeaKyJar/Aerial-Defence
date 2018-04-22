using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePopUp : MonoBehaviour {

    public Texture victory;
    public Texture defeat;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void VictoryPopUp()
    {

        this.gameObject.GetComponent<RawImage>().texture = victory;
    }

    public void DefeatPopUp()
    {

        this.gameObject.GetComponent<RawImage>().texture = defeat;
    }
}
