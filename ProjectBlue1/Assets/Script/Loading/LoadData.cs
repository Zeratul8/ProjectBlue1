using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine_LoadReady());
        PlayGamesPlatform.Activate();
        GPGSSaveData.Instance.LoadData();
        
    }

    IEnumerator Coroutine_LoadReady()
    {
        yield return new WaitForSeconds(1f);
        while(true)
        {
            if(GPGSSaveData.Instance.SavedGame() == null)
            {
                Debug.Log("!!!!!!!!!!!!클라우드에러!!!!!!!!!!!");
                PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "Error!", "클라우드에러!!");
                PopUp.Instance.SetOneButtonPopup(() => SceneManager.LoadSceneAsync("Start"));
                PopUp.Instance.OpenPopUp(PopUp.PopUpType.oneBtn);
                break;
            }
            if (GPGSSaveData.Instance.IsReady)
            {
                yield return new WaitForSeconds(1f);
                SceneManager.LoadSceneAsync("GamePlay");
                break;
            }
            yield return null;
        }
    }
}
