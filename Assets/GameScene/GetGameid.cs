using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HTTP;
using Protocol;

public class GetGameid : MonoBehaviour
{

    // Use this for initialization
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
