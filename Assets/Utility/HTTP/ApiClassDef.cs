using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Protocol{
	[Serializable]
	public class RequestShowUser{
		[NonSerialized]
		public int user_id;
	}

	[Serializable]
	public class ResponseShowUser{
		public int user_id;
		public string name;
		public string created_at;
		public string updated_at;
	}


	[Serializable]
	public class RequestCreateUser{
		public string name;
		public string password;
	}

	[Serializable]
	public class ResponseCreateUser{
		public int user_id;
		public string name;
		public string created_at;
		public string updated_at;
	}


	[Serializable]
	public class RequestCreateUserSession{
		public string name;
		public string password;
	}

	[Serializable]
	public class ResponseCreateUserSession{
		public int user_session_id;
		public string access_token;
		public int user_id;
	}


	[Serializable]
	public class RequestDeleteUserSession{
		[NonSerialized]
		public int user_session_id;
	}

	[Serializable]
	public class ResponseDeleteUserSession{
		public int user_id;
		public string name;
		public string created_at;
		public string updated_at;
	}


	[Serializable]
	public class RequestShowRoom{
		[NonSerialized]
		public int room_id;
	}

	[Serializable]
	public class ResponseShowRoom{
		public int room_id;
		public string status;
		public int game_id;
		public string owner_name;
		public string created_at;
		public string updated_at;
	}


	[Serializable]
	public class RequestCreateRoom{
	}

	[Serializable]
	public class ResponseCreateRoom{
		public int player_entry_id;
		public int room_id;
		public int user_id;
	}


	[Serializable]
	public class RequestListRooms{
	}

	[Serializable]
	public class ResponseListRooms{
		public List<RoomInfo> rooms;
		public ResponseListRooms(List<RoomInfo> rooms)
		{
			this.rooms = rooms;
		}
	}

	[Serializable]
	public class RoomInfo{
		public int room_id;
		public string status;
		public int game_id;
		public string owner_name;
		public string created_at;
		public string updated_at;
	}

	[Serializable]
	public class RequestCreatePlayerEntry{
		[NonSerialized]
		public int room_id;
	}

	[Serializable]
	public class ResponseCreatePlayerEntry{
		public int player_entry_id;
		public int room_id;
		public int user_id;
	}


	[Serializable]
	public class RequestDeletePlayerEntry{
		[NonSerialized]
		public int player_entry_id;
	}

	[Serializable]
	public class ResponseDeletePlayerEntry{
		public int player_entry_id;
		public int room_id;
		public int user_id;
	}


	[Serializable]
	public class RequestCreateSpectatorEntry{
		[NonSerialized]
		public int room_id;
	}

	[Serializable]
	public class ResponseCreateSpectatorEntry{
		public int spectator_entry_id;
	}


	[Serializable]
	public class RequestDeleteSpectatorEntry{
		[NonSerialized]
		public int spectator_entry_id;
	}

	[Serializable]
	public class ResponseDeleteSpectatorEntry{
		public int spectator_entry_id;
	}


	[Serializable]
	public class RequestShowGame{
		[NonSerialized]
		public int game_id;
	}

	[Serializable]
	public class ResponseShowGame{
		public int game_id;
		public int turn_mover_user_id;
		public int turn_count;
		public int winner_user_id;
		public int first_mover_user_id;
		public int last_mover_user_id;
		public string status;
	}


	[Serializable]
	public class RequestPrepareGame{
		[NonSerialized]
		public int game_id;
		public List<PiecePreparationInfo> piece_preparations;
	}

	[Serializable]
	public class ResponsePrepareGame{
		public List<PiecePreparationInfo> piece_preparations;
		public ResponsePrepareGame(List<PiecePreparationInfo> piece_preparations)
		{
			this.piece_preparations = piece_preparations;
		}
	}

	[Serializable]
	public class PiecePreparationInfo{
		public int point_x;
		public int point_y;
		public string kind;
	}

	[Serializable]
	public class RequestListPieces{
		[NonSerialized]
		public int game_id;
	}

	[Serializable]
	public class ResponseListPieces{
		public List<PieceInfo> pieces;
		public ResponseListPieces(List<PieceInfo> pieces)
		{
			this.pieces = pieces;
		}
	}

	[Serializable]
	public class PieceInfo{
		public int piece_id;
		public int point_x;
		public int point_y;
		public int owner_user_id;
		public bool captured;
		public string kind;
	}

	[Serializable]
	public class RequestShowPiece{
		[NonSerialized]
		public int piece_id;
	}

	[Serializable]
	public class ResponseShowPiece{
		public int piece_id;
		public int point_x;
		public int point_y;
		public int owner_user_id;
		public bool captured;
		public string kind;
	}


	[Serializable]
	public class RequestUpdatePiece{
		[NonSerialized]
		public int piece_id;
		public int point_x;
		public int point_y;
	}

	[Serializable]
	public class ResponseUpdatePiece{
		public int piece_id;
		public int point_x;
		public int point_y;
		public int owner_user_id;
		public bool captured;
		public string kind;
	}

}
