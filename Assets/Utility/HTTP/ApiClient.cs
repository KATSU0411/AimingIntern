using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Protocol;

namespace HTTP{
	public class ApiClient :MonoBehaviour {

		private static ApiClient instance;

		private ApiClient(){}

		public static ApiClient Instance {
			get {

				if( instance == null ) {

					GameObject go = new GameObject("ApiClient");
					instance = go.AddComponent<ApiClient>();
				}

				return instance;
			}
		}

		HTTPRequester requester;
		string ipAddr;

		public void SetAccessToken(string token){
			requester.SetAccessToken (token);
		}
		
		public void SetIpAddress(string ipAddress){
			ipAddr = ipAddress;
		}

		void Awake()
		{
			requester = new HTTPRequester(this);
			DontDestroyOnLoad(this);
		}

		/// <summary>
		/// RequestShowUser
		/// /api/users/:user_idへGETでリクエストを行なう
		/// </summary>
		public void RequestShowUser(RequestShowUser param){
			var url = ipAddr + string.Format ("/api/users/{0}", param.user_id);
			requester.Get<ResponseShowUser> (url, ResponseShowUser);
		}

		/// <summary>
		/// ResponseShowUser
		/// /api/users/:user_idへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseShowUser> ResponseShowUser;

		/// <summary>
		/// RequestCreateUser
		/// /api/usersへPOSTでリクエストを行なう
		/// </summary>
		public void RequestCreateUser(RequestCreateUser param){
			var url = ipAddr + "/api/users";
			requester.Post<RequestCreateUser,ResponseCreateUser> (url, param, ResponseCreateUser);
		}

		/// <summary>
		/// ResponseCreateUser
		/// /api/usersへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseCreateUser> ResponseCreateUser;

		/// <summary>
		/// RequestCreateUserSession
		/// /api/user_sessionsへPOSTでリクエストを行なう
		/// </summary>
		public void RequestCreateUserSession(RequestCreateUserSession param){
			var url = ipAddr + "/api/user_sessions";
			requester.Post<RequestCreateUserSession,ResponseCreateUserSession> (url, param, ResponseCreateUserSession);
		}

		/// <summary>
		/// ResponseCreateUserSession
		/// /api/user_sessionsへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseCreateUserSession> ResponseCreateUserSession;

		/// <summary>
		/// RequestDeleteUserSession
		/// /api/user_sessions/:user_session_idへDELETEでリクエストを行なう
		/// </summary>
		public void RequestDeleteUserSession(RequestDeleteUserSession param){
			var url = ipAddr + string.Format ("/api/user_sessions/{0}", param.user_session_id);
			requester.Delete<RequestDeleteUserSession,ResponseDeleteUserSession> (url, param, ResponseDeleteUserSession);
		}

		/// <summary>
		/// ResponseDeleteUserSession
		/// /api/user_sessions/:user_session_idへDELETEでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseDeleteUserSession> ResponseDeleteUserSession;

		/// <summary>
		/// RequestShowRoom
		/// /api/rooms/:room_idへGETでリクエストを行なう
		/// </summary>
		public void RequestShowRoom(RequestShowRoom param){
			var url = ipAddr + string.Format ("/api/rooms/{0}", param.room_id);
			requester.Get<ResponseShowRoom> (url, ResponseShowRoom);
		}

		/// <summary>
		/// ResponseShowRoom
		/// /api/rooms/:room_idへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseShowRoom> ResponseShowRoom;

		/// <summary>
		/// RequestCreateRoom
		/// /api/roomsへPOSTでリクエストを行なう
		/// </summary>
		public void RequestCreateRoom(RequestCreateRoom param){
			var url = ipAddr + "/api/rooms";
			requester.Post<RequestCreateRoom,ResponseCreateRoom> (url, param, ResponseCreateRoom);
		}

		/// <summary>
		/// ResponseCreateRoom
		/// /api/roomsへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseCreateRoom> ResponseCreateRoom;

		/// <summary>
		/// RequestListRooms
		/// /api/roomsへGETでリクエストを行なう
		/// </summary>
		public void RequestListRooms(RequestListRooms param){
			var url = ipAddr + "/api/rooms";
			requester.Get<ResponseListRooms> (url, ResponseListRooms);
		}

		/// <summary>
		/// ResponseListRooms
		/// /api/roomsへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseListRooms> ResponseListRooms;

		/// <summary>
		/// RequestCreatePlayerEntry
		/// /api/rooms/:room_id/player_entriesへPOSTでリクエストを行なう
		/// </summary>
		public void RequestCreatePlayerEntry(RequestCreatePlayerEntry param){
			var url = ipAddr + string.Format ("/api/rooms/{0}/player_entries", param.room_id);
			requester.Post<RequestCreatePlayerEntry,ResponseCreatePlayerEntry> (url, param, ResponseCreatePlayerEntry);
		}

		/// <summary>
		/// ResponseCreatePlayerEntry
		/// /api/rooms/:room_id/player_entriesへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseCreatePlayerEntry> ResponseCreatePlayerEntry;

		/// <summary>
		/// RequestDeletePlayerEntry
		/// /api/player_entries/:player_entry_idへDELETEでリクエストを行なう
		/// </summary>
		public void RequestDeletePlayerEntry(RequestDeletePlayerEntry param){
			var url = ipAddr + string.Format ("/api/player_entries/{0}", param.player_entry_id);
			requester.Delete<RequestDeletePlayerEntry,ResponseDeletePlayerEntry> (url, param, ResponseDeletePlayerEntry);
		}

		/// <summary>
		/// ResponseDeletePlayerEntry
		/// /api/player_entries/:player_entry_idへDELETEでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseDeletePlayerEntry> ResponseDeletePlayerEntry;

		/// <summary>
		/// RequestCreateSpectatorEntry
		/// /api/rooms/:room_id/spectator_entriesへPOSTでリクエストを行なう
		/// </summary>
		public void RequestCreateSpectatorEntry(RequestCreateSpectatorEntry param){
			var url = ipAddr + string.Format ("/api/rooms/{0}/spectator_entries", param.room_id);
			requester.Post<RequestCreateSpectatorEntry,ResponseCreateSpectatorEntry> (url, param, ResponseCreateSpectatorEntry);
		}

		/// <summary>
		/// ResponseCreateSpectatorEntry
		/// /api/rooms/:room_id/spectator_entriesへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseCreateSpectatorEntry> ResponseCreateSpectatorEntry;

		/// <summary>
		/// RequestDeleteSpectatorEntry
		/// /api/spectator_entries/:spectator_entry_idへDELETEでリクエストを行なう
		/// </summary>
		public void RequestDeleteSpectatorEntry(RequestDeleteSpectatorEntry param){
			var url = ipAddr + string.Format ("/api/spectator_entries/{0}", param.spectator_entry_id);
			requester.Delete<RequestDeleteSpectatorEntry,ResponseDeleteSpectatorEntry> (url, param, ResponseDeleteSpectatorEntry);
		}

		/// <summary>
		/// ResponseDeleteSpectatorEntry
		/// /api/spectator_entries/:spectator_entry_idへDELETEでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseDeleteSpectatorEntry> ResponseDeleteSpectatorEntry;

		/// <summary>
		/// RequestShowGame
		/// /api/games/:game_idへGETでリクエストを行なう
		/// </summary>
		public void RequestShowGame(RequestShowGame param){
			var url = ipAddr + string.Format ("/api/games/{0}", param.game_id);
			requester.Get<ResponseShowGame> (url, ResponseShowGame);
		}

		/// <summary>
		/// ResponseShowGame
		/// /api/games/:game_idへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseShowGame> ResponseShowGame;

		/// <summary>
		/// RequestPrepareGame
		/// /api/games/:game_id/preparationへPOSTでリクエストを行なう
		/// </summary>
		public void RequestPrepareGame(RequestPrepareGame param){
			var url = ipAddr + string.Format ("/api/games/{0}/preparation", param.game_id);
			requester.Post<RequestPrepareGame,ResponsePrepareGame> (url, param, ResponsePrepareGame);
		}

		/// <summary>
		/// ResponsePrepareGame
		/// /api/games/:game_id/preparationへPOSTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponsePrepareGame> ResponsePrepareGame;

		/// <summary>
		/// RequestListPieces
		/// /api/games/:game_id/piecesへGETでリクエストを行なう
		/// </summary>
		public void RequestListPieces(RequestListPieces param){
			var url = ipAddr + string.Format ("/api/games/{0}/pieces", param.game_id);
			requester.Get<ResponseListPieces> (url, ResponseListPieces);
		}

		/// <summary>
		/// ResponseListPieces
		/// /api/games/:game_id/piecesへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseListPieces> ResponseListPieces;

		/// <summary>
		/// RequestShowPiece
		/// /api/pieces/:piece_idへGETでリクエストを行なう
		/// </summary>
		public void RequestShowPiece(RequestShowPiece param){
			var url = ipAddr + string.Format ("/api/pieces/{0}", param.piece_id);
			requester.Get<ResponseShowPiece> (url, ResponseShowPiece);
		}

		/// <summary>
		/// ResponseShowPiece
		/// /api/pieces/:piece_idへGETでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseShowPiece> ResponseShowPiece;

		/// <summary>
		/// RequestUpdatePiece
		/// /api/pieces/:piece_idへPUTでリクエストを行なう
		/// </summary>
		public void RequestUpdatePiece(RequestUpdatePiece param){
			var url = ipAddr + string.Format ("/api/pieces/{0}", param.piece_id);
			requester.Put<RequestUpdatePiece,ResponseUpdatePiece> (url, param, ResponseUpdatePiece);
		}

		/// <summary>
		/// ResponseUpdatePiece
		/// /api/pieces/:piece_idへPUTでリクエストを行った時のコールバックを登録する
		/// </summary>
		public UnityAction<ResponseUpdatePiece> ResponseUpdatePiece;
	}
}
