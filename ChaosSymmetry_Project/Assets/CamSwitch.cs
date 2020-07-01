using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;

    PlayerShoot playerShootScript;
    IEnumerator coroutine;

    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]

    // Start is called before the first frame update
    void Start()
    {
        playerShootScript = GameObject.Find("Player").GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShootScript.camAnim)
        {               
            coroutine = ProcessShake(0.5f);
            StartCoroutine(coroutine);
        }
        else
        {
            cam1.GetComponent<CinemachineFreeLook>().m_Lens.Dutch = 0;
        }

    }

    private IEnumerator ProcessShake(float t = 0.5f)
    {
        cam1.GetComponent<CinemachineFreeLook>().m_Lens.Dutch = Mathf.Clamp(cam1.GetComponent<CinemachineFreeLook>().m_Lens.Dutch, 1, -1);

        yield return new WaitForSeconds(t);
        
        playerShootScript.camAnim = false;
    }
}
