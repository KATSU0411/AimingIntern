using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class RoomFilter : MonoBehaviour {

    GetRoomList list;

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Dropdown>().onValueChanged.AddListener(OnValueChanged);
        list = GameObject.Find("Canvas/Panel/Scroll View_RoomList/Image/Button_reload").GetComponent<GetRoomList>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnValueChanged(int value)
    {
        list.filter = value;
        list.OnClick();
    }
}
