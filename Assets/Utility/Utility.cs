using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class Utility { 
    // ------------------------------------------------
    // 盤のコマ情報取得
    // ------------------------------------------------
    public static List<PieceInfo> GetPieceInfo()
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

}
