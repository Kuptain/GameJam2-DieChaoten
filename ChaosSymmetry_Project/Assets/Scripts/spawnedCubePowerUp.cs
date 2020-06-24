using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnedCubePowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator FadeOut()
    {
        transform.GetChild(2).gameObject.SetActive(true);
        GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(2f);

        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);


        Destroy(this.gameObject);
    }
}
