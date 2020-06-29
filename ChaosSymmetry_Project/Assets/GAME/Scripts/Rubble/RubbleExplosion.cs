using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleExplosion : MonoBehaviour
{
    public float explosionForce;
    public LayerMask explosionMask;
    public Collider[] hitColliders;
    public float blastradius = 3;
    CubeManager cm;
    Vector3 randomRotate;
    float maxRotation;
    bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        cm = CubeManager.instance.GetComponent<CubeManager>();
        hitColliders = Physics.OverlapSphere(this.transform.position, blastradius, explosionMask);
        maxRotation = CubeManager.instance.orbitMaxRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<RotateRubble>().RandomizeRotation();
                child.GetComponent<RotateRubble>().exploding = true;
            }
            exploded = false;
        }
    }

    public void Explode()
    {
        if (exploded == false)
        {

            foreach (Collider hitCol in hitColliders)
            {
                if (hitCol.GetComponent<Rigidbody>() != null)
                {
                    hitCol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, this.transform.position, blastradius, 1, ForceMode.Impulse);
                }
            }
            //RandomizeRotation();
            exploded = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        //hitColliders = Physics.OverlapSphere(this.transform.position, blastradius, explosionMask);
        Gizmos.DrawWireSphere(transform.position, blastradius);
    }
}
