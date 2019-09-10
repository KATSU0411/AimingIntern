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

    private float PIECE_SIZE;

    private List<PieceInfo> Pieces;

    // Use this for initialization
    void Start()
    {
        //GetRoomInfo();
        GoodPiece = (GameObject)Resources.Load("GoodPrefab");
        EvilPiece = (GameObject)Resources.Load("EvilPrefab");
        UnknownPiece = (GameObject)Resources.Load("UnknownPrefab");
        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();
        // 1コマ当たりのサイズ
        PIECE_SIZE = field_rect.rect.width / 6.0f;

        Pieces = new List<PieceInfo>();

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

    // 初期コマ8つの設置
    void InitPieces()
    {
        for (int i = 0; i < 4; i++)
        {
            InitPiece(true, i + 2, 1);
            InitPiece(false, i + 2, 2);
        }
    }

    // フィールドの座標をUIの座標に変換
    private Vector2 FieldPos(int x, int y)
    {
        return new Vector2(PIECE_SIZE * (x - 1), PIECE_SIZE * (y-1));
    }

    // コマの設置
    private GameObject InitPiece(bool flgGood, int x, int y)
    {
        GameObject piece;

        if (flgGood) piece = Instantiate(GoodPiece);
        else piece = Instantiate(EvilPiece);

        // 子要素として追加
        piece.transform.SetParent(field_rect, false);

        // 座標の指定
        RectTransform piece_rect = piece.GetComponent<RectTransform>();
        piece_rect.sizeDelta = new Vector2(PIECE_SIZE, PIECE_SIZE);
        piece_rect.localPosition = FieldPos(x, y);

        var p = new PieceInfo();
        p.point_x = x;
        p.point_y = y;
        p.kind = (flgGood ? "good" : "evil");

        Pieces.Add(p);

        // =================================================================
        // ここ！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
        Piece p1 = piece.GetComponent<Piece>();
        p1.position_x = x;
        p1.position_y = y;
        // =================================================================

        return piece;
    }

    // Game情報取得
    void GetGameInfo()
    {

        ApiClient.Instance.ResponseShowGame = ResponseShowGame;
        var param = new RequestShowGame();
        param.game_id = UserInfo.game_id;
        ApiClient.Instance.RequestShowGame(param);
    }
    public void ResponseShowGame(ResponseShowGame response)
    {
    }


    // Room情報取得
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
            InitPieces();
        }
    }
}
