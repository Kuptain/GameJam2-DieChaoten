using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    [SerializeField] TMP_Text hintPrefab;
    [HideInInspector] public string currentText = "";

    TMP_Text hint;

    public bool isActive = true;
    public string currentHint =
        "left";
    int fadeMode = 1;

    Vector3 originScale;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);


    }
    void Start()
    {
        hint = Instantiate(hintPrefab, ObjectManager.instance.player.transform.position, ObjectManager.instance.player.transform.rotation) as TMP_Text;
        currentHint = "left";
        currentText = "Let a cluster explode with LEFT CLICK!";
        originScale = hint.transform.localScale;
        hint.transform.localScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.gameStarted)
        {
            if (PlayerPrefs.GetInt("tutorial", 0) == 1)
            {
                SetText();
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

            if (PlayerPrefs.GetInt("tutorial", 0) == 0)
            {
                FadeOut();

            }
        }    
    }

    void FadeOut()
    {
        hint.transform.localScale = Vector3.Lerp(hint.transform.localScale, Vector3.zero, 0.05f);

    }
    void FadeIn()
    {
        hint.transform.localScale = Vector3.Lerp(hint.transform.localScale, originScale, 0.05f);

    }
    void SetText()
    {
        if (currentHint == "right")
        {
            currentText
                     = "Release RIGHT CLICK to freeze a cluster!";
        }
        if (currentHint == "float")
        {
            currentText
                    = "Now go! " +
                    "Hold SPACE to float longer distances";
        }
        if (currentHint == "slow")
        {
            currentText
                    = "Hold RIGHT CLICK to trigger slowmotion...";
        }
        if (currentHint == "slowDisable")
        {
            currentText
                    = "Press SHIFT again to disable slowmotion";
        }
        if (currentHint == "power")
        {
            currentText
                    = "Collect powerfull PowerUps...";
        }
        if (currentHint == "")
        {
            currentText
                    = "";
        }
    }
    public IEnumerator ChangeHintType(string type)
    {
        yield return new WaitForSeconds(1.75f);
        fadeMode = 1;
        currentHint = type;
    }
    public IEnumerator DisableAfter(float time)
    {
        yield return new WaitForSeconds(time);
        currentHint = "";
    }

    public void ChangeType(string type)
    {
        fadeMode = 2;

        StartCoroutine(ChangeHintType(type));
    }
}
