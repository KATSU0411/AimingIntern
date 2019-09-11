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

    public bool flgFirst { set; get; }
    private float PIECE_SIZE;

    [NonSerialized] public PieceInfo info;

    private GameObject field;
    private RectTransform field_rect;


    void Awake()
    {
        info = new PieceInfo();
    }

    // Use this for initialization
    void Start()
    {
        canvasTran = GameObject.Find("Canvas/Panel").GetComponent<Transform>();

        field = GameObject.Find("Canvas/Panel/Image_field/Panel_piece");
        field_rect = field.GetComponent<RectTransform>();
        // 1コマ当たりのサイズ
        PIECE_SIZE = field_rect.rect.width / 6.0f;

        this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(PIECE_SIZE, PIECE_SIZE);

    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<RectTransform>().localPosition = Field2UI(info.point_x, info.point_y);
    }

    // フィールドの座標をUIの座標に変換
    private Vector2 Field2UI(int x, int y)
    {
        if (flgFirst) return new Vector2(PIECE_SIZE * (x - 1), PIECE_SIZE * (y - 1));
        else return new Vector2(PIECE_SIZE * (x - 1), PIECE_SIZE * (7 - y - 1));
    }
    
    // マウス座標をフィールド座標に変換
    private Vector2 Mouse2Field(PointerEventData e)
    {
        float x = e.position.x;
        float y = e.position.y;
        x -= field_rect.position.x;
        y -= field_rect.position.y;

        Vector2 pos = new Vector2((int)(x / PIECE_SIZE) + 1, (int)(y / PIECE_SIZE) + 1);
        if (flgFirst) return pos;
        else return new Vector2(pos.x, 7 - pos.y);
    }

    // コマのドラッグイベント
    public void OnBeginDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        CreateDragObject();
        draggingObject.transform.position = e.position;
    }
    public void OnDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        draggingObject.transform.position = e.position;
    }
    public void OnEndDrag(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        gameObject.GetComponent<Image>().color = Vector4.one;
        Destroy(draggingObject);

        var pos = Mouse2Field(e);
        Debug.Log(pos);

        if (UserInfo.game_status == "preparing")
        {
            SwapPiece(e);
        }
        else if(UserInfo.flg_turn)
        {
            MovePiece(Mouse2Field(e));
        }
    }


    // ------------------------------------------------
    // コマの移動
    // ------------------------------------------------
    private void MovePiece(Vector2 pos)
    {
        if(pos.x == info.point_x)
        {
            if(Math.Abs(pos.y - info.point_y) == 1)
            {

            }
        }
        else if(pos.y == info.point_y)
        {

        }
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
                PieceInfo tmp;
                tmp = swapper.info;
                swapper.info = info;
                info = tmp;
            }
        }
    }


    // ドラッグ中マウスオーバーでハイライト
    public void OnPointerEnter(PointerEventData e)
    {
        if (info.kind == "unknown") return;
        if (UserInfo.game_status != "preparing") return;
        if (e.pointerDrag == null) return;
        if (e.pointerDrag == this.gameObject) return;
        this.gameObject.transform.Find("Image").GetComponent<Image>()
                                        .color = new Vector4(1, 1, 0, 0.2f);
    }
    public void OnPointerExit(PointerEventData e)
    {
        if (info.kind == "unknown") return;
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

}
