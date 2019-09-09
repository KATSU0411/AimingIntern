﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HTTP;
using Protocol;

public class SignIn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClick()
    {
        CreateNewUser();
    }

    // --------------------------------
    // 新規ユーザ登録
    // --------------------------------
    public void CreateNewUser()
    {
        // 通信先アドレスのInputFieldを取得
        InputField server_field = GameObject.Find("InputField_Server").GetComponent<InputField>();

        // 各フィールドを取得
        InputField username_field = GameObject.Find("InputField_UserName").GetComponent<InputField>();
        InputField passwd_field = GameObject.Find("InputField_Password").GetComponent<InputField>();

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
    }

}
