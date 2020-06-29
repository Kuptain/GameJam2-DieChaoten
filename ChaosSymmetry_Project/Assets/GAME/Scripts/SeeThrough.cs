using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThrough : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<CapsuleCollider>().height = Vector3.Distance(Camera.main.transform.position, ObjectManager.instance.player.transform.position)*6;
        GetComponent<CapsuleCollider>().center = new Vector3(0, 0 , GetComponent<CapsuleCollider>().height / 2);

        transform.LookAt(ObjectManager.instance.player.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        bool check = false;

        if (other.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.clusterHasShader)
        {
            if (other.GetComponent<MeshRenderer>() != null)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            }
            if (other.transform.childCount > 3)
            {
                for (int i = 3; i < other.transform.childCount; i++)
                {
                    if (other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        other.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                    }
                    foreach (Transform child in other.transform.GetChild(i))
                    {
                        if (child.gameObject.GetComponent<MeshRenderer>() != null)
                        {
                            child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        }

                    }
                }
            }

            other.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            check = true;


        }


        if (check == false && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.childCount > 3 && other.transform.parent.parent.gameObject.GetComponent<CubeDestroy>() != null)
        {
            other.transform.parent.parent.gameObject.transform.GetChild(2).gameObject.SetActive(true);

            for (int i = 3; i < other.transform.parent.parent.childCount; i++)
            {
                if (other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
                }
                foreach (Transform child in other.transform.parent.parent.GetChild(i))
                {
                    if (child.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    }

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool check = false;


        if (other.gameObject.GetComponent<CubeDestroy>() != null && CubeManager.instance.clusterHasShader)
        {
            if (other.GetComponent<MeshRenderer>() != null)
            {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;

            }
            if (other.transform.childCount > 3)
            {
                for (int i = 3; i < other.transform.childCount; i++)
                {
                    if (other.transform.GetChild(i).GetComponent<MeshRenderer>() != null)
                    {
                        other.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                    }
                    foreach (Transform child in other.transform.GetChild(i))
                    {
                        if (child.gameObject.GetComponent<MeshRenderer>() != null)
                        {
                            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                        }

                    }
                }
            }
            other.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            check = true;

        }

        if (check == false && other.transform.parent != null && other.transform.parent.parent != null && other.transform.parent.parent.childCount > 3 && other.transform.parent.parent.gameObject.GetComponent<CubeDestroy>() != null)
        {
            other.transform.parent.parent.gameObject.transform.GetChild(2).gameObject.SetActive(false);

            for (int i = 3; i < other.transform.parent.parent.childCount; i++)
            {
                if (other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>() != null)
                {
                    other.transform.parent.parent.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                }
                foreach (Transform child in other.transform.parent.parent.GetChild(i))
                {
                    if (child.gameObject.GetComponent<MeshRenderer>() != null)
                    {
                        child.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }

                }
            }
        }
    }

    }
