using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class JoinRoom : MonoBehaviour
{

    public int room_id;

    // Use this for initialization
    void Start()
    {
        // 入場機能
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ----------------------------------------------------
    // ルームに入る
    // ----------------------------------------------------
    public void OnClick()
    {
        ApiClient.Instance.ResponseCreatePlayerEntry = ResponseCreatePlayerEntry;
        var param = new RequestCreatePlayerEntry();
        param.room_id = room_id;
        ApiClient.Instance.RequestCreatePlayerEntry(param);
    }

    public void ResponseCreatePlayerEntry(ResponseCreatePlayerEntry response)
    {
        // 情報保持
        UserInfo.room_id = response.room_id;
        UserInfo.player_entory_id = response.player_entry_id;
        UserInfo.flg_spectator = false;

        // Gameシーン切り替え
        SceneManager.LoadScene("GameScene");
    }
}
