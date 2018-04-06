using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseIndicator : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        //this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void IndicatePhaseCoroutine(string phase)
    {
        StartCoroutine(IndicatePhase(phase));
    }
    

    IEnumerator IndicatePhase(string phase)
    {
        this.GetComponent<Text>().text = phase;
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
    


}
