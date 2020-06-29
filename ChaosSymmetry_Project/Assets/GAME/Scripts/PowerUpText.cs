using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PowerUpText : MonoBehaviour
{
    [HideInInspector] public int ID;
    [HideInInspector] public GameObject powerUp;
    TutorialManager tm;
    TMP_Text textMesh;
   
    public string type = "";
    public bool consumable;

    public int fadeMode = 2;
    Vector3 originScale;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        originScale = transform.localScale;
    }

    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, powerUp.transform.position + new Vector3(0, 3f, 0), 0.05f);
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.Rotate(new Vector3(0, 180, 0));

        //textMesh.text = tm.currentText;

        //FadeAway
        if (fadeMode == 1)
        {
            FadeIn();
        }
        if (fadeMode == 2)
        {
            FadeOut();
        }
    }

    public void FadeOut()
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.05f);

    }
    public void FadeIn()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originScale, 0.05f);

    }

    public IEnumerator TimerFadeIn()
    {
        yield return new WaitForSeconds(1.75f);
        fadeMode = 1;
    }
 
    public void TimerFadeOut()
    {
        fadeMode = 2;
        StartCoroutine(TimerFadeIn());
    }
}
