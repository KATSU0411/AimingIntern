using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class CreateRoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnClick()
    {
        ApiClient.Instance.ResponseCreateRoom = ResponseCreateRoom;
        var param = new RequestCreateRoom();
        ApiClient.Instance.RequestCreateRoom(param);
    }

    public void ResponseCreateRoom(ResponseCreateRoom response)
    {
        // roomid保持
        UserInfo.room_id = response.room_id;

        // Gameシーン切り替え
        SceneManager.LoadScene("GameScene");
    }
}
