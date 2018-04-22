using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMiss : MonoBehaviour {

    public Texture hit;
    public Texture miss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeHit()
    {
        this.gameObject.GetComponent<RawImage>().texture = hit;
    }

    public void MakeMiss()
    {
        this.gameObject.GetComponent<RawImage>().texture = miss;
    }
}
