using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

    public float spend = 10.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.enemy))
        {
            other.gameObject.SetActive(false);
        }
        Rigidbody rig = other.gameObject.GetComponent<Rigidbody>();
        Debug.Log(rig);
        if (rig != null)
        {
            Debug.Log("进入");
            rig.velocity = other.gameObject.transform.forward * spend;
        }
        Destroy(gameObject);
    }
}
