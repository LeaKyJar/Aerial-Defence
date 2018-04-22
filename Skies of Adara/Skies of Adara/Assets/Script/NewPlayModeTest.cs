using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.EventSystems;
public class NewPlayModeTest
{
    PhaseIndicator pi;

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator Test1LoadScene1()
    {
        SceneManager.LoadScene("Scenes/BattleScene");
        yield return new WaitForSeconds(0.01f);
        pi = GameObject.Find("PhaseIndicator").GetComponent<PhaseIndicator>();
        pi.IndicatePhaseCoroutine("Load Scene Done");
        yield return new WaitForSeconds(1);
    }
    [UnityTest]
    public IEnumerator Test2_1SnaptoDefaultChampion()
    {
        float initialx = Champion.instance.transform.position.x;
        float initialy = Champion.instance.transform.position.y;
        float intervalx = (initialx + 100) / 100;
        float intervaly = 0;
        Champion.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Champion.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            //yield return new WaitForSeconds(0.00001f);
            yield return new WaitForEndOfFrame();
        }
        Champion.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Champion Test");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test2_2SnaptoGridChampion()
    {
        float initialx = Champion.instance.transform.position.x;
        float initialy = Champion.instance.transform.position.y;
        float finalx = GameObject.Find("20").GetComponent<Tile>().transform.position.x;
        float finaly = GameObject.Find("20").GetComponent<Tile>().transform.position.y;
        float intervalx = (finalx - initialx) / 100;
        float intervaly = (finaly - initialy) / 100;
        Champion.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Champion.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Champion.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);

        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Champion Deployed");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test2_3DeployChampionBody()
    {
        yield return new WaitForSeconds(1);
        Clickalltiles();
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Champion Body Deployed");
        yield return new WaitForSeconds(1);
    }



    [UnityTest]
    public IEnumerator Test3_1SnaptoDefaultDefender()
    {
        float initialx = Defender.instance.transform.position.x;
        float initialy = Defender.instance.transform.position.y;
        float intervalx = (initialx + 100) / 100;
        float intervaly = 0;
        Defender.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Defender.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Defender.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Defender Test");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test3_2SnaptoGridDefender()
    {
        float initialx = Defender.instance.transform.position.x;
        float initialy = Defender.instance.transform.position.y;
        float finalx = GameObject.Find("20").GetComponent<Tile>().transform.position.x;
        float finaly = GameObject.Find("20").GetComponent<Tile>().transform.position.y;
        float intervalx = (finalx - initialx) / 100;
        float intervaly = (finaly - initialy) / 100;
        Defender.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Defender.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Defender.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);

        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Defender Deployed");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test3_3DeployDefenderShield()
    {
        yield return new WaitForSeconds(1);
        Clickalltiles();
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Defender Shield Deployed");
        yield return new WaitForSeconds(1);
    }
    [UnityTest]
    public IEnumerator Test4_1SnaptoDefaultEngineer()
    {
        float initialx = Engineer.instance.transform.position.x;
        float initialy = Engineer.instance.transform.position.y;
        float intervalx = (initialx + 100) / 100;
        float intervaly = 0;
        Engineer.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Engineer.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Engineer.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Engineer Test");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test4_2SnaptoGridEngineer()
    {
        float initialx = Engineer.instance.transform.position.x;
        float initialy = Engineer.instance.transform.position.y;
        float finalx = GameObject.Find("20").GetComponent<Tile>().transform.position.x;
        float finaly = GameObject.Find("20").GetComponent<Tile>().transform.position.y;
        float intervalx = (finalx - initialx) / 100;
        float intervaly = (finaly - initialy) / 100;
        Engineer.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Engineer.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Engineer.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);

        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Engineer Deployed");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test4_3DeployEngineerBody()
    {
        yield return new WaitForSeconds(1);
        Clickalltiles();
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Engineer Body Deployed");
        yield return new WaitForSeconds(1);
    }
    [UnityTest]
    public IEnumerator Test5_1SnaptoDefaultBomber()
    {
        float initialx = Bomber.instance.transform.position.x;
        float initialy = Bomber.instance.transform.position.y;
        float intervalx = (initialx + 100) / 100;
        float intervaly = 0;
        Bomber.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Bomber.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Bomber.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);
        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Bomber Test");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test5_2SnaptoGridBomber()
    {
        float initialx = Bomber.instance.transform.position.x;
        float initialy = Bomber.instance.transform.position.y;
        float finalx = GameObject.Find("20").GetComponent<Tile>().transform.position.x;
        float finaly = GameObject.Find("20").GetComponent<Tile>().transform.position.y;
        float intervalx = (finalx - initialx - 30) / 100;
        float intervaly = (finaly - initialy - 30) / 100;
        Bomber.instance.LightAllTiles();
        for (int i = 0; i < 101; i++)
        {
            Bomber.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
            yield return new WaitForEndOfFrame();
        }
        Bomber.instance.OnEndDrag(new PointerEventData(EventSystem.current));
        Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);

        pi.gameObject.SetActive(true);
        pi.IndicatePhaseCoroutine("Bomber Deployed");
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator Test6_1StartGameasFirst()
    {
        GameManager.instance.StartFirst = true;
        GameManager.instance.ExitPreparation();
        yield return new WaitForSeconds(5);
    }

    [UnityTest]
    public IEnumerator Test6_2DefenderShieldHit()
    {
        GameManager.instance.TileHit("100");
        //pi.gameObject.SetActive(true);
        //pi.IndicatePhaseCoroutine("Attack on Defender's shield has completed.");
        yield return new WaitForSeconds(10);
    }

    //[UnityTest]
    //public IEnumerator Test6_3EnemyFeedbackTile0()
    //{
    //    Champion.instance.OnPointerClick(new PointerEventData(EventSystem.current));
    //    yield return new WaitForSeconds(5);
    //    GameObject.Find("100").GetComponent<EnemyTile>().OnPointerClick(new PointerEventData(EventSystem.current));
    //    GameManager.instance.HitAlert("0");
    //    pi.gameObject.SetActive(true);
    //    pi.IndicatePhaseCoroutine("Hit from enemy has completed.");
    //    yield return new WaitForSeconds(1);
    //}

    //[UnityTest]
    //public IEnumerator Test6_4EnemyFeedbackTile4()
    //{

    //    Bomber.instance.OnPointerClick(new PointerEventData(EventSystem.current));
    //    yield return new WaitForSeconds(5);
    //    GameObject.Find("104").GetComponent<EnemyTile>().OnPointerClick(new PointerEventData(EventSystem.current));
    //    yield return new WaitForSeconds(0.5f);
    //    GameManager.instance.EmptyEnemyTile();
    //    yield return new WaitForSeconds(2);
    //    pi.gameObject.SetActive(true);
    //    pi.IndicatePhaseCoroutine("Miss from enemy has completed.");
    //    GameManager.instance.GridToggle();
        
    //    yield return new WaitForSeconds(10);
    //}
    //[UnityTest]
    //public IEnumerator TestStartingTurn()
    //{
    //    GameManager.instance.SetInstructions("Start of Turn Test");
    //    yield return new WaitForSeconds(3);
    //    GameManager.instance.StartFirst = true;
    //    GameManager.instance.ExitPreparation();
    //    GameManager.instance.SetInstructions("End of Turn Test");
    //    yield return new WaitForSeconds(3);
    //}
    //[UnityTest]
    //public IEnumerator TestTest()
    //{
    //    Rigidbody rb = GameObject.Find("Champion").GetComponent<Rigidbody>();
    //    Vector2 force = new Vector2(100, 100);
    //    rb.AddForce(force);
    //    float initialx = Champion.instance.transform.position.x;
    //    float initialy = Champion.instance.transform.position.y;
    //    float finalx = GameObject.Find("24").GetComponent<Tile>().transform.position.x;
    //    float finaly = GameObject.Find("24").GetComponent<Tile>().transform.position.y;
    //    float intervalx = (finalx - initialx) / 1000;
    //    float intervaly = (finaly - initialy) / 1000;
    //    for (int i = 0; i < 1000; i++)
    //    {
    //        Champion.instance.transform.position = new Vector3(i * intervalx + initialx, i * intervaly + initialy, 0);
    //        yield return new WaitForSeconds(10f / 1000);
    //    }
    //    Debug.Log(GameObject.Find("24").GetComponent<Tile>().transform.position);

    //    GameManager.instance.SetInstructions("DONE");
    //    yield return new WaitForSeconds(5);
    //}


    public void Clickalltiles()
    {
        Tile current;
        for (int i = 0; i < 25; i++)
        {
            current = GameObject.Find("" + i).GetComponent<Tile>();
            current.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

}
