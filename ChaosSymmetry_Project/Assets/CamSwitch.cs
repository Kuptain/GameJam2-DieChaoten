using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    PlayerShoot playerShootScript;
    IEnumerator coroutine;

    private CinemachineFreeLook freeLook1;
    private CinemachineFreeLook freeLook2;

    // Start is called before the first frame update
    void Start()
    {
        cam1.SetActive(true);
        playerShootScript = GameObject.Find("Player").GetComponent<PlayerShoot>();

        freeLook1 = cam1.GetComponent<CinemachineFreeLook>();
        freeLook2 = cam2.GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShootScript.camAnim)
        {
            coroutine = ProcessShake(0.5f);
            StartCoroutine(coroutine);
        }

        cam2.transform.position = cam1.transform.position;

        freeLook2.m_XAxis.Value = freeLook1.m_XAxis.Value;
        freeLook2.m_YAxis.Value = freeLook1.m_YAxis.Value;
    }

    private IEnumerator ProcessShake(float t = 3.5f)
    {
        cam1.SetActive(false);
        cam2.SetActive(true);

        yield return new WaitForSeconds(t);

        cam2.SetActive(false);
        cam1.SetActive(true);
        
        playerShootScript.camAnim = false;
    }
}
