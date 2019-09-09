using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class Top : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SignUpOnClick()
    {
        CreateNewUser();
        CreateUserSession();
    }
    public void SignInOnClick()
    {
        CreateUserSession();
    }

    // --------------------------------
    // 新規ユーザ登録
    // --------------------------------
    public void CreateNewUser()
    {

        // 通信先を指定
        if (server_field.text == "") server_field.text = "127.0.0.1:3000";
        ApiClient.Instance.SetIpAddress("http://" + server_field.text);

        // 各情報を格納
        var user = new RequestCreateUser();
        user.name = username_field.text;
        user.password = passwd_field.text;

        // 登録
        ApiClient.Instance.ResponseCreateUser = ResponseCreateUser;
        ApiClient.Instance.RequestCreateUser(user);

    }

    // --------------------------------
    // ユーザ登録コールバック関数
    // --------------------------------
    public void ResponseCreateUser(ResponseCreateUser response)
    {
        Debug.Log("Success");
    }

    // --------------------------------
    // 新規ユーザ登録
    // --------------------------------
    public void CreateUserSession()
    {
        // 通信先を指定
        if (server_field.text == "") server_field.text = "127.0.0.1:3000";
        ApiClient.Instance.SetIpAddress("http://" + server_field.text);

        // 各情報を格納
        var user = new RequestCreateUserSession();
        user.name = username_field.text;
        user.password = passwd_field.text;

        // ログイン
        ApiClient.Instance.ResponseCreateUserSession = ResponseCreateUserSession;
        ApiClient.Instance.RequestCreateUserSession(user);
    }

    // --------------------------------
    // ユーザ登録コールバック関数
    // --------------------------------
    public void ResponseCreateUserSession(ResponseCreateUserSession response)
    {
        Debug.Log("Success");
        ApiClient.Instance.SetAccessToken(response.access_token);

        SceneManager.LoadScene("RoomListScene");
    }

    
}
