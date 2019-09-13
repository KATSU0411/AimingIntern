using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class JoinSpectatorRoom : MonoBehaviour
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
    // ルームに観戦者として入る
    // ----------------------------------------------------
    public void OnClick()
    {
        ApiClient.Instance.ResponseCreateSpectatorEntry = ResponseCreateSpectatorEntry;
        var param = new RequestCreateSpectatorEntry();
        param.room_id = room_id;
        UserInfo.room_id = room_id;
        ApiClient.Instance.RequestCreateSpectatorEntry(param);
    }

    public void ResponseCreateSpectatorEntry(ResponseCreateSpectatorEntry response)
    {
        // 情報保持
        UserInfo.player_entory_id = response.spectator_entry_id;
        UserInfo.flg_spectator = true;

        // Gameシーン切り替え
        SceneManager.LoadScene("GameScene");
    }
}
