using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class SignIn : MonoBehaviour {

    private InputField server_field;
    private InputField username_field;
    private InputField passwd_field;

	// Use this for initialization
	void Start () {
        // 通信先アドレスのInputFieldを取得
        server_field = GameObject.Find("InputField_Server").GetComponent<InputField>();

        // 各フィールドを取得
        username_field = GameObject.Find("InputField_UserName").GetComponent<InputField>();
        passwd_field = GameObject.Find("InputField_Password").GetComponent<InputField>();

        server_field.text = "127.0.0.1:3000";

        this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClick()
    {
        CreateUserSession();
    }

    // ----------------------------------------------------
    // ログインセッションの生成
    // ----------------------------------------------------
    public void CreateUserSession()
    {
        // 通信先を指定
        ApiClient.Instance.SetIpAddress("http://" + server_field.text);

        // 各情報を格納
        var user = new RequestCreateUserSession();
        user.name = username_field.text;
        user.password = passwd_field.text;

        // ログイン
        ApiClient.Instance.ResponseCreateUserSession = ResponseCreateUserSession;
        ApiClient.Instance.RequestCreateUserSession(user);
    }

    // ----------------------------------------------------
    // ログインセッション生成コールバック関数
    // ----------------------------------------------------
    public void ResponseCreateUserSession(ResponseCreateUserSession response)
    {
        // トークンをセット
        ApiClient.Instance.SetAccessToken(response.access_token);

        // 情報保持
        UserInfo.user_session_id = response.user_session_id;
        UserInfo.token = response.access_token;
        UserInfo.user_id = response.user_id;

        // RoomListシーンに切り替え
        SceneManager.LoadScene("RoomListScene");
    }
}
