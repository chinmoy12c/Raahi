using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    public GameObject ob1, ob2, ob3;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = (ob2.transform.position - ob1.transform.position).normalized;
        Quaternion rot = new Quaternion();
        rot.SetFromToRotation(
            new Vector3(0,0,1),
            direction
        );
        GameObject arrow = Instantiate(ob3, new Vector3(0,0,0), rot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
