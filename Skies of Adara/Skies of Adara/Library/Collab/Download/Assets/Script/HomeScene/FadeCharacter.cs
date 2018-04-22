
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCharacter : MonoBehaviour {

    public Image imageDef;
    public Image imageChamp;
    public Image imageBomb;
    public Image imageEngine;

    public Image textDef;
    public Image textChamp;
    public Image textBomb;
    public Image textEngine;

    IEnumerator coroutine;

    //public List<string> coroutineList = new List<string>();
    
    // Use this for initialization
	void Start () {

        //make image transparent
        imageDef.canvasRenderer.SetAlpha(0f);
        imageChamp.canvasRenderer.SetAlpha(0f);
        imageBomb.canvasRenderer.SetAlpha(0f);
        imageEngine.canvasRenderer.SetAlpha(0f);

        //make text transparent
        textDef.canvasRenderer.SetAlpha(0f);
        textChamp.canvasRenderer.SetAlpha(0f);
        textBomb.canvasRenderer.SetAlpha(0f);
        textEngine.canvasRenderer.SetAlpha(0f);

        //make Defender opaque
        imageDef.CrossFadeAlpha(1f, 1.0f, false);
        textDef.CrossFadeAlpha(1f, 1.0f, false);
        StartCoroutine(WaitTime());
        
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(7.0f);
        imageDef.CrossFadeAlpha(0f, 1.0f, false);
        textDef.CrossFadeAlpha(0f, 1.0f, false);
        yield return new WaitForSeconds(2.0f);

        imageChamp.CrossFadeAlpha(1f, 1.0f, false);
        textChamp.CrossFadeAlpha(1f, 1.0f, false);
        yield return new WaitForSeconds(7.0f);
        imageChamp.CrossFadeAlpha(0f, 1.0f, false);
        textChamp.CrossFadeAlpha(0f, 1.0f, false);
        yield return new WaitForSeconds(2.0f);

        imageBomb.CrossFadeAlpha(1f, 1.0f, false);
        textBomb.CrossFadeAlpha(1f, 1.0f, false);
        yield return new WaitForSeconds(7.0f);
        imageBomb.CrossFadeAlpha(0f, 1.0f, false);
        textBomb.CrossFadeAlpha(0f, 1.0f, false);
        yield return new WaitForSeconds(2.0f);

        imageEngine.CrossFadeAlpha(1f, 1.0f, false);
        textEngine.CrossFadeAlpha(1f, 1.0f, false);
        yield return new WaitForSeconds(7.0f);
        imageEngine.CrossFadeAlpha(0f, 1.0f, false);
        textEngine.CrossFadeAlpha(0f, 1.0f, false);
        yield return new WaitForSeconds(2.0f);

        Start();
    }


}
