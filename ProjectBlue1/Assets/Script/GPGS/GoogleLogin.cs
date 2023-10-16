using GooglePlayGames.BasicApi;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{

    public void LoginGPGS()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            StopAllCoroutines();
        }
        else
        {
            StartCoroutine(Coroutine_ReLogin());
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
            yield return null;
        }

        Debug.Log("GPGS 로그인 실패!!!!!");
    }
}
