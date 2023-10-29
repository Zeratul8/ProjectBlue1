using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{
    Coroutine reLogin;
    public void LoginGPGS()
    {
        
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            reLogin = null;
            StopAllCoroutines();
            SceneManager.LoadSceneAsync("Loading");
        }
        else
        {
            PopUp.Instance.SetText(PopUp.PopUpType.oneBtn, "로그인실패", "!!!!!!로그인실패!!!!!");
#if UNITY_EDITOR
            PopUp.Instance.SetOneButtonPopup(()=> SceneManager.LoadSceneAsync(1));
#else
            PopUp.Instance.SetOneButtonPopup(null);
#endif
            PopUp.Instance.OpenPopUp(PopUp.PopUpType.oneBtn);
            /*
            if(reLogin == null)
                reLogin = StartCoroutine(Coroutine_ReLogin());
            */
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    IEnumerator Coroutine_ReLogin()
    {
        float time = 0;
        while(time < 10)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
            time += Time.deltaTime;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("GPGS 로그인 실패!!!!!");
    }
}
