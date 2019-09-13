using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class RoomExit : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ルームから退出
    public void OnClick()
    {
        if (!UserInfo.flg_spectator)
        {
            ApiClient.Instance.ResponseDeletePlayerEntry = ResponseDeletePlayerEntry;
            var param = new RequestDeletePlayerEntry();
            param.player_entry_id = UserInfo.player_entory_id;
            ApiClient.Instance.RequestDeletePlayerEntry(param);
        }
        else
        {
            ApiClient.Instance.ResponseDeleteSpectatorEntry = ResponseDeleteSpectatorEntry;
            var param = new RequestDeleteSpectatorEntry();
            param.spectator_entry_id = UserInfo.player_entory_id;
            ApiClient.Instance.RequestDeleteSpectatorEntry(param);
        }
    }

    public void ResponseDeletePlayerEntry(ResponseDeletePlayerEntry response)
    {
        UserInfo.game_status = null;
        UserInfo.flg_turn = false;

        // シーン切り替え
        SceneManager.LoadScene("RoomListScene");
    }
    public void ResponseDeleteSpectatorEntry(ResponseDeleteSpectatorEntry response)
    {
        UserInfo.game_status = null;
        UserInfo.flg_turn = false;
        UserInfo.flg_spectator = false;

        // シーン切り替え
        SceneManager.LoadScene("RoomListScene");
    }


    private void OnApplicationQuit()
    {
        OnClick();
    }
}
