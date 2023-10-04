using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIManager : SingletonMonoBehaviour<BaseUIManager>
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //ÆË¾÷¶ç¿ì±â
            PopUp.Instance.SetTwoButtonPopup(CloseGame, null);
            PopUp.Instance.OpenPopUp(PopUp.PopUpType.oneBtn);
        }
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
