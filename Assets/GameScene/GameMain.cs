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
    private bool m_flgStart = false;

    private GameObject GoodPiece;
    private GameObject EvilPiece;
    private GameObject UnknownPiece;
    private GameObject field;
    private RectTransform field_rect;
    private GameObject waiting;

    private GameObject[] Pieces;

    private bool m_flgFirst;

    private const int PIECE_NUM = 16;

    // Use this for initialization
    void Start()
    {

        m_flgMatched = false;
        m_flgStart = false;
        m_flgFirst = false;
        m_time_elapsed = 0;
        GoodPiece = (GameObject)Resources.Load("GoodPrefab");
        EvilPiece = (GameObject)Resources.Load("EvilPrefab");
        UnknownPiece = (GameObject)Resources.Load("UnknownPrefab");
        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();
        Pieces = new GameObject[PIECE_NUM];
        waiting = GameObject.Find("Canvas/Panel/Image_wait");
        GameObject.Find("Canvas/Panel/Button").SetActive(false);


        GetRoomInfo();
        // Debug
        //InitPieces();
    }


    // Update is called once per frame
    void Update()
    {
        m_time_elapsed += Time.deltaTime;
        if (m_time_elapsed >= 2)
        {
            if (!m_flgMatched) GetRoomInfo();
            else GetGameInfo();
            m_time_elapsed = 0;
        }
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
    // 初期配置フェーズが二人とも終わった後のコマ情報取得
    // ------------------------------------------------
    void SetAllPieces(List<PieceInfo> pieces)
    {
        foreach(Transform obj in field.transform)
        {
            Destroy(obj.gameObject);
        }

        for(int i=0; i<PIECE_NUM; i++)
        {
            var obj = SetPiece(pieces[i]);
            Pieces[i] = obj;
        }

    }

    // ------------------------------------------------
    // コマの設置
    // ------------------------------------------------
    private GameObject SetPiece(string kind, int x, int y)
    {
        PieceInfo info = new PieceInfo();
        info.point_y = y;
        info.point_x = x;
        info.kind = kind;

        return SetPiece(info);
    }
    private GameObject SetPiece(PieceInfo info)
    {
        GameObject piece;

        if (info.kind == "good") piece = Instantiate(GoodPiece);
        else if (info.kind == "evil") piece = Instantiate(EvilPiece);
        else if (info.kind == "unknown") piece = Instantiate(UnknownPiece);
        else return null;

        // 子要素として追加
        piece.transform.SetParent(field_rect, false);

        // 座標の指定
        Piece p = piece.GetComponent<Piece>();
        p.info = info;
        p.flgFirst = m_flgFirst;

        return piece;
    }


    // ------------------------------------------------
    // 全コマ情報取得
    // ------------------------------------------------
    void GetPiecesInfo()
    {
        ApiClient.Instance.ResponseListPieces = ResponseListPieces;
        var param = new RequestListPieces();
        param.game_id = UserInfo.game_id;
        ApiClient.Instance.RequestListPieces(param);
    }
    public void ResponseListPieces(ResponseListPieces response)
    {
        if (!m_flgStart)
        {
            m_flgStart = true;
            SetAllPieces(response.pieces);
        }

        for(int i=0;i<PIECE_NUM;i++)
        {
            Pieces[i].GetComponent<Piece>().info = response.pieces[i];
        }
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
        // 先手後手格納
        if (response.first_mover_user_id == UserInfo.user_id)
        {
            m_flgFirst = true;
        }
        else
        {
            m_flgFirst = false;
        }
        if (!m_flgMatched)
        {
            m_flgMatched = true;
            waiting.SetActive(false);
            GameObject.Find("Canvas/Panel/Button").SetActive(true);
            InitPieces();
            return;
        }

        UserInfo.game_status = response.status;
        if (response.status == "playing")
        {
            if (!m_flgStart)
            {
                GetPiecesInfo();
                return;
            }
            // 自ターンか判定
            if (response.turn_mover_user_id == UserInfo.user_id)
            {
                if (!UserInfo.flg_turn)
                {
                    UserInfo.flg_turn = true;
                    waiting.SetActive(false);
                    GetPiecesInfo();
                }
            }
            else
            {
                if (UserInfo.flg_turn)
                {
                    UserInfo.flg_turn = false;
                    waiting.SetActive(true);
                    GetPiecesInfo();
                }
            }
        }
    }

    // ------------------------------------------------
    // 与えられたフィールド座標の情報を返す
    // ------------------------------------------------
    public PieceInfo isPiece(int x, int y)
    {
        for(int i=0; i<PIECE_NUM; i++)
        {
            PieceInfo info = Pieces[i].GetComponent<Piece>().info;
            if(info.point_x == x && info.point_y == y)
            {
                return info; 
            }
        }

        return null;
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
            GetGameInfo();
        }
    }

}
