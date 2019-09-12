using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class Piece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    private GameObject draggingObject;
    private Transform canvasTran;
    private List<GameObject> Highlights;

    public bool flgFirst { set; get; }
    private float PIECE_SIZE;

    [NonSerialized] public PieceInfo info;

    private GameObject field;
    private RectTransform field_rect;

    private float CANVAS_SCALE;


    void Awake()
    {
        info = new PieceInfo();
        //UserInfo.game_status = "preparing";
    }

    // Use this for initialization
    void Start()
    {
        Highlights = new List<GameObject>();

        canvasTran = GameObject.Find("Canvas/Panel").GetComponent<Transform>();

        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();
        // 1コマ当たりのサイズ
        PIECE_SIZE = field_rect.rect.width / 6.0f;
        //Debug.Log(field_rect.position);

        CANVAS_SCALE = GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x;

        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PIECE_SIZE, PIECE_SIZE);
        SetHighlight();
    }

    // Update is called once per frame
    void Update()
    {
        if (!info.captured)
        {
            this.gameObject.GetComponent<RectTransform>().localPosition = Field2UI(info.point_x, info.point_y);
        }
    }

    // ------------------------------------------------
    // 移動可能範囲を示すハイライトオブジェクトの追加
    // ------------------------------------------------
    private void SetHighlight()
    {
        GameObject Highlight = (GameObject)Resources.Load("HighlightPrefab");
        for(int i = 0; i < 4; i++)
        {
            GameObject hl = Instantiate(Highlight, this.gameObject.GetComponent<RectTransform>());
            hl.GetComponent<RectTransform>().sizeDelta = new Vector2(PIECE_SIZE, PIECE_SIZE);
            Vector2 pos = new Vector2();
            int sign = flgFirst ? 1 : -1;
            if (i == 0) pos.y = PIECE_SIZE * sign;
            else if (i == 1) pos.x = PIECE_SIZE * sign;
            else if (i == 2) pos.y = -PIECE_SIZE * sign;
            else if (i == 3) pos.x = -PIECE_SIZE * sign;

            hl.GetComponent<RectTransform>().localPosition = pos;

            Highlights.Add(hl);
        }
    }

    private void BeginHighLight()
    {
        List<bool> cm = new List<bool>();
        cm.Add(CanMove(new Vector2(info.point_x, info.point_y + 1)));
        cm.Add(CanMove(new Vector2(info.point_x + 1, info.point_y)));
        cm.Add(CanMove(new Vector2(info.point_x, info.point_y - 1)));
        cm.Add(CanMove(new Vector2(info.point_x - 1, info.point_y)));


        for(int i = 0; i < 4; i++)
        {
            if (!cm[i]) continue;
            Highlights[i].GetComponent<Image>().color = new Vector4(1, 1, 0, 0.2f);
        }
    }
    private void EndHighLight()
    {
        foreach(GameObject obj in Highlights)
        {
            obj.GetComponent<Image>().color = Vector4.zero;
        }
    }

    // フィールドの座標をUIの座標に変換
    private Vector2 Field2UI(int x, int y)
    {
        if (flgFirst) return new Vector2(PIECE_SIZE * (x - 1 + 0.5f), PIECE_SIZE * (y - 1 + 0.5f));
        else return new Vector2(PIECE_SIZE * (7 - x - 0.5f), PIECE_SIZE * (7 - y - 0.5f));
    }

    // マウス座標をフィールド座標に変換
    private Vector2 Mouse2Field(PointerEventData e)
    {
        float x = e.position.x;
        float y = e.position.y;
        x -= field_rect.position.x;
        y -= field_rect.position.y;

        x /= CANVAS_SCALE;
        y /= CANVAS_SCALE;


        Vector2 pos = new Vector2((int)(x / PIECE_SIZE) + (1), (int)(y / PIECE_SIZE) + (1));
        if (x <= 0) pos.x -= 1;
        if (y <= 0) pos.y -= 1;
        if (flgFirst) return pos;
        else return new Vector2(7 - pos.x, 7 - pos.y);
    }

    // コマのドラッグイベント
    public void OnBeginDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.flg_spectator) return;
        if (UserInfo.game_status == "finished" || UserInfo.game_status == "exited") return;
        CreateDragObject();
        draggingObject.transform.position = e.position;
        if (UserInfo.flg_turn) BeginHighLight();
    }
    public void OnDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.flg_spectator) return;
        if (UserInfo.game_status == "finished" || UserInfo.game_status == "exited") return;
        draggingObject.transform.position = e.position;
    }
    public void OnEndDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.flg_spectator) return;
        if (UserInfo.game_status == "finished" || UserInfo.game_status == "exited") return;
        gameObject.GetComponent<Image>().color = Vector4.one;
        Destroy(draggingObject);
        EndHighLight();

        var pos = Mouse2Field(e);

        if (UserInfo.game_status == "preparing")
        {
            SwapPiece(e);
        }
        else if (UserInfo.flg_turn)
        {
            Move(pos);
        }
    }

    // ------------------------------------------------
    // コマの移動
    // ------------------------------------------------
    public void Move(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        Move(x, y);
    }
    public void Move(int x, int y)
    {
        if (CanMove(x, y))
        {
            info.point_x = x;
            info.point_y = y;
            UpdatePieceInfo();
        }
    }

    // ------------------------------------------------
    // コマの移動の可否
    // ------------------------------------------------
    public bool CanMove(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        return CanMove(x, y);
    }
    public bool CanMove(int x, int y)
    {
        if (y < 1 || y > 6) return false;
        if (x < 0 || x > 7) return false;
        if (flgFirst) { if (((x == 0 || x == 7) && (y != 6 || info.kind != "good"))) return false; }
        else { if (((x == 0 || x == 7) && (y != 1 || info.kind != "good"))) return false; }

        PieceInfo dstinfo = field.GetComponent<GameMain>().isPiece(x, y);
        if (dstinfo != null && dstinfo.owner_user_id == UserInfo.user_id) return false;

        if (x == info.point_x)
        {
            if (Math.Abs(y - info.point_y) != 1) return false;
            return true;
        }
        else if (y == info.point_y)
        {
            if (Math.Abs(x - info.point_x) != 1) return false;
            return true;
        }

        return false;
    }


    // ------------------------------------------------
    // 初期配置フェーズのコマのスワップ
    // ------------------------------------------------
    private void SwapPiece(PointerEventData e)
    {

        var raycastResluts = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e, raycastResluts);

        foreach (var obj in raycastResluts)
        {
            if (obj.gameObject.tag == "Piece")
            {
                // ドラッグ先と位置を交換
                Piece swapper = obj.gameObject.GetComponent<Piece>();
                int tmp;
                tmp = swapper.info.point_x;
                swapper.info.point_x = info.point_x;
                info.point_x = tmp;

                tmp = swapper.info.point_y;
                swapper.info.point_y = info.point_y;
                info.point_y = tmp;
            }
        }
    }


    // ドラッグ中マウスオーバーでハイライト
    public void OnPointerEnter(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.game_status != "preparing") return;
        if (UserInfo.flg_spectator) return;
        if (e.pointerDrag == null) return;
        if (e.pointerDrag == this.gameObject) return;
        this.gameObject.transform.Find("Image").GetComponent<Image>()
                                        .color = new Vector4(1, 1, 0, 0.2f);
    }
    public void OnPointerExit(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.flg_spectator) return;
        if (UserInfo.game_status != "preparing") return;
        this.gameObject.transform.Find("Image").GetComponent<Image>()
                                        .color = Vector4.zero;
    }

    // ドラッグオブジェクト作成
    private void CreateDragObject()
    {
        draggingObject = new GameObject("Dragging Object");
        draggingObject.transform.SetParent(canvasTran);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;

        // レイキャストがブロックされないように
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image draggingImage = draggingObject.AddComponent<Image>();
        Image sourceImage = GetComponent<Image>();

        draggingImage.sprite = sourceImage.sprite;
        draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
        draggingImage.color = sourceImage.color;
        draggingImage.material = sourceImage.material;

        gameObject.GetComponent<Image>().color = Vector4.one * 0.6f;
    }

    // ------------------------------------------------
    // コマの移動をサーバに送信
    // ------------------------------------------------
    void UpdatePieceInfo()
    {
        ApiClient.Instance.ResponseUpdatePiece = ResponseUpdatePiece;
        var param = new RequestUpdatePiece();
        param.piece_id = info.piece_id;
        param.point_y = info.point_y;
        param.point_x = info.point_x;

        ApiClient.Instance.RequestUpdatePiece(param);
    }
    public void ResponseUpdatePiece(ResponseUpdatePiece response)
    {

    }
}
