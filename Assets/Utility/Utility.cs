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
    public static List<GameObject> GetPieceInfo()
    {
        List<GameObject> pieces = new List<GameObject>();
        GameObject field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        foreach (Transform piece in field.transform)
        {
            Piece p = piece.GetComponent<Piece>();
            if (p == null) continue;

            pieces.Add(piece.gameObject);
        }
        return pieces;
    }

}
