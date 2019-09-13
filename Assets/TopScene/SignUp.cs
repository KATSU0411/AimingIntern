using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class SignUp : MonoBehaviour {

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
        CreateNewUser();
    }

    // ----------------------------------------------------
    // 新規ユーザ登録
    // ----------------------------------------------------
    public void CreateNewUser()
    {

        // 通信先を指定
        ApiClient.Instance.SetIpAddress("http://" + server_field.text);

        // 各情報を格納
        var user = new RequestCreateUser();
        user.name = username_field.text;
        user.password = passwd_field.text;

        // 登録
        ApiClient.Instance.ResponseCreateUser = ResponseCreateUser;
        ApiClient.Instance.RequestCreateUser(user);

    }

    // ----------------------------------------------------
    // ユーザ登録コールバック関数
    // ----------------------------------------------------
    public void ResponseCreateUser(ResponseCreateUser response)
    {
        // 登録後サインイン
        GameObject.Find("Canvas/Panel/Button_Signin").GetComponent<SignIn>().OnClick();
    }


}
