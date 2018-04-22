using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour {

    private Vector3 position;
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public GameObject character;
    public GameObject missilePrefab;

	// Use this for initialization
	void Start () {
        position = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireMissile()
    {
        GameObject missile = Instantiate(missilePrefab, position, transform.rotation);
        missile.transform.SetParent(GameObject.Find("CombatBG").transform, false);
        missile.GetComponent<Missile>().DesignateOrigin(position);
        missile.GetComponent<Missile>().DesignateEnemy();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        GameManager.instance.instructions.SetActive(true);
    }

    public void Destroyed()
    {
        StartCoroutine(DestroyedNum());
    }

    IEnumerator DestroyedNum()
    {
        yield return new WaitForSeconds(2);
        if (character == Champion.instance.gameObject)
        {
            if (Champion.instance.Dead && Champion.instance.GetComponentInChildren<Body>().Dead)
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (character == Engineer.instance.gameObject)
        {
            if (Engineer.instance.Dead && Engineer.instance.GetComponentInChildren<Body>().Dead)
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(3);
        GameManager.instance.instructions.SetActive(true);
    }

    public void Repaired()
    {
        this.gameObject.SetActive(true);
    }
}
