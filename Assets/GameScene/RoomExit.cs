using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class RoomExit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // ルームから退出
    public void OnClick()
    {
        ApiClient.Instance.ResponseDeletePlayerEntry = ResponseDeletePlayerEntry;
        var param = new RequestDeletePlayerEntry();
        param.player_entry_id = UserInfo.player_entory_id;
        ApiClient.Instance.RequestDeletePlayerEntry(param);
    }

    public void ResponseDeletePlayerEntry(ResponseDeletePlayerEntry response)
    {
        // シーン切り替え
        SceneManager.LoadScene("RoomListScene");
    }
}
