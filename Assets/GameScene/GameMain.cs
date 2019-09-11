using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class GameMain : MonoBehaviour
{
    private float m_time_elapsed = 0;
    private bool m_flgMatched = false;

    private GameObject GoodPiece;
    private GameObject EvilPiece;
    private GameObject UnknownPiece;
    private GameObject field;
    private RectTransform field_rect;

    private bool m_flgFirst = false;

    // Use this for initialization
    void Start()
    {
        //GetRoomInfo();
        GoodPiece = (GameObject)Resources.Load("GoodPrefab");
        EvilPiece = (GameObject)Resources.Load("EvilPrefab");
        UnknownPiece = (GameObject)Resources.Load("UnknownPrefab");
        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();


        // Debug
        InitPieces();
    }


    // Update is called once per frame
    void Update()
    {
        //m_time_elapsed += Time.deltaTime;
        //if(m_time_elapsed >= 2)
        //{
        //    GetGameInfo();
        //    GetRoomInfo();
        //}
    }

    // ------------------------------------------------
    // 初期コマ8つの設置
    // ------------------------------------------------
    void InitPieces()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_flgFirst)
            {
                SetPiece("good", i + 2, 1);
                SetPiece("evil", i + 2, 2);
            }
            else
            {
                SetPiece("good", i + 2, 5);
                SetPiece("evil", i + 2, 6);
            }
        }
    }

    // ------------------------------------------------
    // コマの設置
    // ------------------------------------------------
    private GameObject SetPiece(string kind, int x, int y)
    {
        GameObject piece;

        if (kind == "good") piece = Instantiate(GoodPiece);
        else if (kind == "evil") piece = Instantiate(EvilPiece);
        else if (kind == "unknown") piece = Instantiate(UnknownPiece);
        else return null;

        // 子要素として追加
        piece.transform.SetParent(field_rect, false);

        // 座標の指定
        Piece p1 = piece.GetComponent<Piece>();
        p1.position_x = x;
        p1.position_y = y;
        p1.flgFirst = m_flgFirst;
        p1.kind = kind;

        return piece;
    }

    // ------------------------------------------------
    // Game情報取得
    // ------------------------------------------------
    void GetGameInfo()
    {
        ApiClient.Instance.ResponseShowGame = ResponseShowGame;
        var param = new RequestShowGame();
        param.game_id = UserInfo.game_id;
        ApiClient.Instance.RequestShowGame(param);
    }
    public void ResponseShowGame(ResponseShowGame response)
    {
        if(response.first_mover_user_id == UserInfo.user_id)
        {
            m_flgFirst = true;
        }
    }


    // ------------------------------------------------
    // Room情報取得
    // ------------------------------------------------
    void GetRoomInfo()
    {
        ApiClient.Instance.ResponseShowRoom = ResponseShowRoom;
        var param = new RequestShowRoom();
        param.room_id = UserInfo.room_id;
        ApiClient.Instance.RequestShowRoom(param);
    }
    public void ResponseShowRoom(ResponseShowRoom response)
    {
        // 情報保持
        UserInfo.game_id = response.game_id;

        if (response.status == "playing" && !m_flgMatched)
        {
            m_flgMatched = true;
            InitPieces();
        }
    }
}
