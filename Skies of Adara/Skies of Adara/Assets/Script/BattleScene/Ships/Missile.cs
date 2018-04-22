using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missile : MonoBehaviour {

    public float speed;
    private Vector3 target = new Vector3(10000, 10000, 0);
    private Vector3 origin = new Vector3(-10000, -10000, 0);
    public Vector3 missTarget = new Vector3 (1500, 1535, 0 );
    public Vector3 enemyTarget = new Vector3 (-400, 1700, 0);

    public Texture leftFacingMissile;
    public Texture rightFacingMissile;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //Vector3 directionVector = target - this.gameObject.transform.position;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Distance(transform.position, target)<1)
        {
            GameObject.Destroy(this.gameObject);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        try
        {
            GameObject.Destroy(this.gameObject);
        }catch(Exception ex)
        {
            Debug.Log(ex);
        }
        GameManager.instance.instructions.SetActive(true);
    }

    public void DesignateTarget(Vector3 target)
    {
        this.target = target;
        this.gameObject.GetComponent<RawImage>().texture = rightFacingMissile;
    }

    public void DesignateOrigin(Vector3 origin)
    {
        this.origin = origin;
        this.gameObject.transform.position = origin;
    }

    public void DesignateMiss()
    {
        this.target = missTarget;
        this.gameObject.GetComponent<RawImage>().texture = rightFacingMissile;

    }

    public void DesignateEnemy()
    {
        this.target = enemyTarget;
        this.gameObject.GetComponent<RawImage>().texture = leftFacingMissile;
    }
    
}
