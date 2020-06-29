using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialText : MonoBehaviour
{
    [HideInInspector] public int ID;
    TutorialManager tm;
    TMP_Text textMesh;
    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        tm = TutorialManager.instance;
    }

    void Update()
    {

        textMesh.text = tm.currentText;

        if (UIManager.instance.paused)
        {
            GetComponent<MeshRenderer>().enabled = false;

        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;

        }
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, Camera.main.transform.position + Camera.main.transform.forward * 8 + new Vector3(0, 2.5f, 0), 0.08f);
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.Rotate(new Vector3(0, 180, 0));

    }

}
