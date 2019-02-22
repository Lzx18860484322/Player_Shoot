using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    // Start is called before the first frame update
    public float spend = 2.0f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "buttle")
        {
            Rigidbody rig = gameObject.GetComponent<Rigidbody>();
            rig.velocity = other.gameObject.transform.forward * spend;
            Destroy(other);
        }
    }

}
