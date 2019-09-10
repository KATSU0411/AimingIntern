using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class FieldHighlight : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    // コマのドラッグイベント
     public void OnBeginDrag(PointerEventData e)
    {
        Debug.Log(e.position.x);
    }
    public void OnDrag(PointerEventData ev)
    {
    }
     public void OnEndDrag(PointerEventData ev)
    {
    }


}
