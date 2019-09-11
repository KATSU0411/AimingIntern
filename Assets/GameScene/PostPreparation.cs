﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class PostPreparation : MonoBehaviour
{

    Button button;

    // Use this for initialization
    void Start()
    {

        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnPointerClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick()
    {
        PostInitPieces();
    }

    // ------------------------------------------------
    // 盤のコマ情報取得
    // ------------------------------------------------
    private List<PieceInfo> GetPieceInfo()
    {
        List<PieceInfo> pieces = new List<PieceInfo>();
        GameObject field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        foreach (Transform piece in field.transform)
        {
            PieceInfo pi = new PieceInfo();
            Piece p = piece.GetComponent<Piece>();
            if (p == null) continue;
            pi = p.info;

            pieces.Add(pi);
        }

        return pieces;
    }

    // ------------------------------------------------
    // コマの初期情報送信
    // ------------------------------------------------
    void PostInitPieces()
    {
        ApiClient.Instance.ResponsePrepareGame = ResponsePrepareGame;
        var param = new RequestPrepareGame();

        param.game_id = UserInfo.game_id;
        param.piece_preparations = new List<PiecePreparationInfo>();

        var pieces = GetPieceInfo();
        foreach(var piece in pieces)
        {
            PiecePreparationInfo info = new PiecePreparationInfo();
            info.point_y = piece.point_y;
            info.point_x = piece.point_x;
            info.kind = piece.kind;

            param.piece_preparations.Add(info);
        }

        ApiClient.Instance.RequestPrepareGame(param);
    }

    public void ResponsePrepareGame(ResponsePrepareGame response)
    {
        GameObject.Find("Canvas/Panel/Image_wait").SetActive(true);
        Destroy(this.gameObject);
    }
}
