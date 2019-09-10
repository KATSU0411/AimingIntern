using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class SignOut : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClick()
    {
        var session = new RequestDeleteUserSession();
        session.user_session_id = UserInfo.user_session_id;
        // 登録
        ApiClient.Instance.ResponseDeleteUserSession = ResponseDeleteUserSession;
        ApiClient.Instance.RequestDeleteUserSession(session);
    }

    public void ResponseDeleteUserSession(ResponseDeleteUserSession response)
    {
        Debug.Log("sign out");

        // Topシーンに切り替え
        SceneManager.LoadScene("TopScene");
    }
}
