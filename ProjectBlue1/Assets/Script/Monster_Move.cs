using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Move : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Translate(Vector3.right * 0.1f);
    }
    
}
