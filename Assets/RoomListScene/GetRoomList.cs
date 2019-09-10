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

    public void Onclick()
    {
        // 現在のルーム全削除
        GameObject cont = GameObject.Find("Canvas/Panel/Scroll View_RoomList/Viewport/Content");
        foreach(Transform t in cont.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        ApiClient.Instance.ResponseListRooms = ResponseListRooms;
        var param = new RequestListRooms();
        ApiClient.Instance.RequestListRooms(param);
    }

    // ----------------------------------------------------
    // ログインセッション生成コールバック関数
    // ----------------------------------------------------
    public void ResponseListRooms(ResponseListRooms response)
    {
        Debug.Log(response.rooms);

        //Content取得(ボタンを並べる場所)
        RectTransform content = GameObject.Find("Canvas/Panel/Scroll View_RoomList/Viewport/Content")
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

            text_owner.GetComponent<Text>().text = room.owner_name;
            text_status.GetComponent<Text>().text = (room.status == "waiting" ? "対戦待ち" : "対戦中");
        }
    }
}
