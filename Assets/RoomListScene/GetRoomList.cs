using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class GetRoomList : MonoBehaviour
{

    private GameObject RoomPanel;
    public int filter; // 0:all 1:playing only 2:waiting only

    // Use this for initialization
    void Start()
    {
        filter = 0;
        ApiClient.Instance.ResponseListRooms = ResponseListRooms;
        var param = new RequestListRooms();
        ApiClient.Instance.RequestListRooms(param);
        RoomPanel = (GameObject)Resources.Load("RoomPrefab");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ルーム更新
    public void OnClick()
    {

        ApiClient.Instance.ResponseListRooms = ResponseListRooms;
        var param = new RequestListRooms();
        ApiClient.Instance.RequestListRooms(param);
    }

    // ----------------------------------------------------
    // ルーム一覧生成
    // ----------------------------------------------------
    public void ResponseListRooms(ResponseListRooms response)
    {
        //Content取得(ボタンを並べる場所)
        GameObject cont = GameObject.Find("Canvas/Panel/Scroll View_RoomList/Viewport/Content");
        RectTransform content = cont.GetComponent<RectTransform>();

        // 現在のルーム全削除
        foreach (Transform t in cont.transform)
        {
            Destroy(t.gameObject);
        }

        //Contentの高さ決定
        //(RoomPanelの高さ+間隔)*Room数
        float listSpace = content.GetComponent<VerticalLayoutGroup>().spacing;
        float listHeight = RoomPanel.GetComponent<LayoutElement>().preferredHeight;

        content.sizeDelta = new Vector2(0, (listHeight + listSpace) * response.rooms.Count);

        foreach (RoomInfo room in response.rooms)
        {
            if (filter == 1 && room.status != "playing") continue;
            if (filter == 2 && room.status != "waiting") continue;
            //RoomPanel
            GameObject panel = Instantiate(RoomPanel, content);


            //各子要素の設定
            GameObject button_join = panel.transform.Find("Button_Join").gameObject;
            GameObject button_spect = panel.transform.Find("Button_Spect").gameObject;
            GameObject text_owner = panel.transform.Find("Text_Owner").gameObject;
            GameObject text_status = panel.transform.Find("Text_Status").gameObject;

            text_owner.GetComponent<Text>().text = room.owner_name;
            text_status.GetComponent<Text>().text = (room.status == "waiting" ? "対戦待ち" : "対戦中");

            // 対戦中なら対戦ボタンが使用不可
            button_join.GetComponent<Button>().interactable = (room.status == "waiting" ? true : false);

            // 入場機能
            button_join.GetComponent<JoinRoom>().room_id = room.room_id;

            // 観戦機能
            button_spect.GetComponent<JoinSpectatorRoom>().room_id = room.room_id;
        }
    }
}
