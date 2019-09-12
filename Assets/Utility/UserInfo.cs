using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static int user_session_id { set; get; }
    public static int user_id { set; get; }
    public static int game_user_id { set; get; }
    public static string token { set; get; }
    public static int room_id { set; get; }
    public static int player_entory_id { set; get; }
    public static int game_id { set; get; }
    public static string game_status { set; get; }
    public static bool flg_turn { set; get; }
    public static bool flg_spectator { set; get; }
}
