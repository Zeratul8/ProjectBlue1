using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageRepeat : MonoBehaviour
{
    [SerializeField]
    Toggle repeatToggle;

    Color toggleColor;
    
    // Start is called before the first frame update
    void Start()
    {
        toggleColor = repeatToggle.GetComponent<Image>().color;
        repeatToggle.onValueChanged.AddListener(SetRepeatMode);
    }

    void SetRepeatMode(bool isOn)
    {
        if(isOn)
            toggleColor = Color.yellow;
        else
            toggleColor = Color.white;
    }

    public bool GetRepeatMode()
    {
        return repeatToggle.isOn;
    }
}
