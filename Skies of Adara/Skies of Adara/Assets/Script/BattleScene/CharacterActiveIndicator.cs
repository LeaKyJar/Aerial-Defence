using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterActiveIndicator : MonoBehaviour, IPointerClickHandler {

    private static volatile bool disable = false;
    private GameObject activeCharacter;
    public GameObject ActiveCharacter
    {
        get { return activeCharacter; }
        set { activeCharacter = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (disable)
        {
            this.gameObject.GetComponent<RawImage>().enabled = false;
            //DisableRawImageCoroutine();
        }
        else
        {

            //this.gameObject.GetComponent<RawImage>().enabled = true;
            //EnableRawImageCoroutine();
            
        }
	}

    void DisableRawImageCoroutine()
    {
        StartCoroutine(DisableRawImage());
    }

    IEnumerator DisableRawImage()
    {
        Debug.Log("DisableRawImage");
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<RawImage>().enabled = false;
    }

    void EnableRawImageCoroutine()
    {
        StartCoroutine(EnableRawImage());
    }

    IEnumerator EnableRawImage()
    {
        Debug.Log("EnableRawImage");
        yield return new WaitForSeconds(4);
        this.gameObject.GetComponent<RawImage>().enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        activeCharacter.GetComponent<Character>().OnPointerClick(eventData);
    }

    public void DisableIndicators()
    {
        disable = true;
        //this.gameObject.GetComponent<RawImage>().enabled = false;
    }

    public void EnableIndicators()
    {
        disable = false;
        //this.gameObject.GetComponent<RawImage>().enabled = true;
    }
}
