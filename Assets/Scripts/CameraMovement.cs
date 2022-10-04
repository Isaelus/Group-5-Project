using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{   
    [SerializeField] public GameObject target;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z);
    }
}
