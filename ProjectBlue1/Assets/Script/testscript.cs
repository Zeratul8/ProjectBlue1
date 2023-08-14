using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    public bool IsTest { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        IsTest = true;
        Changed(IsTest);
        Debug.Log(IsTest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Changed(bool isTrue)
    {
        isTrue = !isTrue;
    }
}
