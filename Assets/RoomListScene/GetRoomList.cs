﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class GetRoomList : MonoBehaviour
{

    private GameObject RoomPanel;

    // Use this for initialization
    void Start()
    {
        ApiClient.Instance.ResponseListRooms = ResponseListRooms;
        var param = new RequestListRooms();
        ApiClient.Instance.RequestListRooms(param);
        RoomPanel = (GameObject)Resources.Load("RoomPrefab");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ----------------------------------------------------
    // ログインセッション生成コールバック関数
    // ----------------------------------------------------
    public void ResponseListRooms(ResponseListRooms response)
    {
        Debug.Log(response.rooms);

        //Content取得(ボタンを並べる場所)
        RectTransform content = GameObject.Find("Canvas/Scroll View_RoomList/Viewport/Content")
                                            .GetComponent<RectTransform>();

        //Contentの高さ決定
        //(RoomPanelの高さ+間隔)*Room数
        float listSpace = content.GetComponent<VerticalLayoutGroup>().spacing;
        float listHeight = RoomPanel.GetComponent<LayoutElement>().preferredHeight;

        content.sizeDelta = new Vector2(0, (listHeight + listSpace) * response.rooms.Count);

        foreach (RoomInfo room in response.rooms)
        {
            //RoomPanel
            GameObject panel = (GameObject)Instantiate(RoomPanel);

            //ボタンをContentの子に設定
            panel.transform.SetParent(content, false);

            //各子要素の設定
            GameObject button_join = panel.transform.Find("Button_Join").gameObject;
            GameObject text_owner = panel.transform.Find("Text_Owner").gameObject;
            GameObject text_status = panel.transform.Find("Text_Status").gameObject;
        }
    }
}
