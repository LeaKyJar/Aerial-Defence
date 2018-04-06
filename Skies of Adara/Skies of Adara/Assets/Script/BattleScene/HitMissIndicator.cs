using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMissIndicator : MonoBehaviour {

    public Texture hitIndicator;
    public Texture missIndicator;
    public Texture incomingIndicator;
    public Texture targetedIndicator;
    public int acceleration = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void IndicateHitCoroutine(Vector3 tilePos)
    {
        StartCoroutine(IndicateHitMiss(tilePos, hitIndicator));
    }

    public void IndicateMissCoroutine(Vector3 tilePos)
    {
        StartCoroutine(IndicateHitMiss(tilePos, missIndicator));
    }

    public void IndicateIncomingCoroutine(Vector3 tilePos)
    {
        this.gameObject.transform.localScale.Set(1.4f,1.4f,1);
        StartCoroutine(IndicateIncoming(tilePos));
        this.gameObject.transform.localScale.Set(1, 1, 1);
    }

    public void IndicateTargetedCoroutine(Vector3 tilePos)
    {
        this.gameObject.transform.localScale.Set(1.4f, 1.4f, 1);
        StartCoroutine(IndicateTargeted(tilePos));
        this.gameObject.transform.localScale.Set(1, 1, 1);
    }

    IEnumerator IndicateHitMiss(Vector3 tilePos, Texture texture)
    {
        this.gameObject.transform.position = tilePos;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 20, 0);
        this.GetComponent<RawImage>().texture = texture;
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }

    IEnumerator IndicateIncoming(Vector3 tilePos)
    {
        this.gameObject.transform.position = tilePos;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.GetComponent<Animation>().Play();
        this.GetComponent<RawImage>().texture = incomingIndicator;
        yield return new WaitForSeconds(2);
        this.GetComponent<Animation>().Stop();
        this.GetComponent<RawImage>().color = new Vector4 (255,255,255,255);
        this.gameObject.SetActive(false);
    }

    IEnumerator IndicateTargeted(Vector3 tilePos)
    {
        this.gameObject.transform.position = tilePos;
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        this.GetComponent<Animation>().Play("TargetSelected");
        this.GetComponent<RawImage>().texture = targetedIndicator;
        yield return new WaitForSeconds(3);
        this.GetComponent<Animation>().Stop("TargetSelected");
        this.GetComponent<RawImage>().color = new Vector4(255, 255, 255, 255);
        this.gameObject.SetActive(false);
    }





}
