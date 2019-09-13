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
    private RectTransform my_case;
    private RectTransform enemy_case;
    private GameObject waiting;

    private GameObject win_panel;
    private GameObject lose_panel;

    private GameObject[] Pieces;

    private AutoMode auto;

    private bool m_flgFirst;

    private const int PIECE_NUM = 16;
    private int m_turn = 0;

    // Use this for initialization
    void Start()
    {
        m_flgMatched = false;
        m_flgStart = false;
        m_flgFirst = false;
        m_time_elapsed = 0;
        m_turn = 0;
        GoodPiece = (GameObject)Resources.Load("GoodPrefab");
        EvilPiece = (GameObject)Resources.Load("EvilPrefab");
        UnknownPiece = (GameObject)Resources.Load("UnknownPrefab");
        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();
        Pieces = new GameObject[PIECE_NUM];
        waiting = GameObject.Find("Canvas/Panel/Image_wait");

        my_case = GameObject.Find("Canvas/Panel/Image_case_my/Panel").GetComponent<RectTransform>();
        enemy_case = GameObject.Find("Canvas/Panel/Image_case_en/Panel").GetComponent<RectTransform>();

        win_panel = GameObject.Find("Canvas/Panel/Image_Win");
        lose_panel = GameObject.Find("Canvas/Panel/Image_Lose");

        auto = GameObject.Find("Canvas/Panel/Toggle_Auto").GetComponent<AutoMode>();

        win_panel.SetActive(false);
        lose_panel.SetActive(false);

        UserInfo.game_user_id = UserInfo.user_id;

        if (UserInfo.flg_spectator)
        {
            waiting.SetActive(false);
        }

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
            if(UserInfo.game_status == "finished" || UserInfo.game_status == "exited")
            {
                // ゲームが終わって2秒経過でルーム一覧へ戻る
                GameObject.Find("Canvas/Panel/Button_exit").GetComponent<RoomExit>().OnClick();
            }
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
                SetPiece("good", i + 2, 6);
                SetPiece("evil", i + 2, 5);
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

            if(pieces[i].owner_user_id != UserInfo.game_user_id)
            {
                obj.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
            }
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

        return SetPiece(info, field_rect);
    }
    private GameObject SetPiece(PieceInfo info)
    {
        if (info.captured)
        {
            if(info.owner_user_id == UserInfo.game_user_id)
            {
                return SetPiece(info, enemy_case);
            }
            else
            {
                return SetPiece(info, my_case);
            }
        }
        return SetPiece(info, field_rect);
    }
    private GameObject SetPiece(PieceInfo info, RectTransform parent)
    {
        GameObject piece;

        if (info.kind == "good") piece = Instantiate(GoodPiece);
        else if (info.kind == "evil") piece = Instantiate(EvilPiece);
        else if (info.kind == "unknown") piece = Instantiate(UnknownPiece);
        else return null;

        // 子要素として追加
        piece.transform.SetParent(parent, false);

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
            if(Pieces[i].GetComponent<Piece>().info.captured != response.pieces[i].captured)
            {
                Destroy(Pieces[i]);
                if(response.pieces[i].owner_user_id == UserInfo.game_user_id)
                {
                    Pieces[i] = SetPiece(response.pieces[i], enemy_case);
                }
                else
                {
                    Pieces[i] = SetPiece(response.pieces[i], my_case);
                }
            }
            else
            {
                Pieces[i].GetComponent<Piece>().info = response.pieces[i];
            }
        }

        if(UserInfo.game_status == "playing") auto.Auto(new List<GameObject>(Pieces));
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
        UserInfo.game_status = response.status;
        if (UserInfo.flg_spectator) UserInfo.game_user_id = response.first_mover_user_id;

        // 終了処理
        if(response.status == "finished")
        {
            waiting.SetActive(false);
            if (UserInfo.flg_spectator) return;

            if(response.winner_user_id == UserInfo.game_user_id)
            {
                GetPiecesInfo();
                win_panel.SetActive(true);
            }
            else
            {
                GetPiecesInfo();
                lose_panel.SetActive(true);
            }
            return;
        }

        if(response.status == "exited")
        {
            waiting.SetActive(false);
            if(response.winner_user_id == UserInfo.game_user_id)
            {
                win_panel.SetActive(true);
            }
        }


        // 先手後手格納
        if (response.first_mover_user_id == UserInfo.game_user_id)
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
            if (!UserInfo.flg_spectator)
            {
                GameObject.Find("Canvas/Panel/Button").SetActive(true);
                GameObject.Find("Canvas/Panel/Button_Shuffle").SetActive(true);
            }
            InitPieces();
        }

        // ターン進行
        if (response.status == "playing")
        {
            if (!m_flgStart)
            {
                GetPiecesInfo();
                return;
            }

            if(m_turn != response.turn_count)
            {
                m_turn = response.turn_count;
                GetPiecesInfo();
                UserInfo.flg_turn = (response.turn_mover_user_id == UserInfo.game_user_id);
                if (UserInfo.flg_spectator) return;
                waiting.SetActive(!UserInfo.flg_turn);
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
            if (info.captured) continue;

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
