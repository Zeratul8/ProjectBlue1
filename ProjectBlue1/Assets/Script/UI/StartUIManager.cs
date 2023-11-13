using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    [SerializeField]
    Button loginBtn;
    [SerializeField]
    Button exitBtn;
    [SerializeField]
    GoogleLogin login;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        loginBtn.onClick.AddListener(login.LoginGPGS);
        exitBtn.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

}
