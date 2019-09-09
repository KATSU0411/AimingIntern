using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Network{
	
	public class UnityWebRequestClient{

		MonoBehaviour monoBehaviour;

		static readonly string contentType = "application/json";
		string accessToken = string.Empty;

		public UnityAction<DownloadHandler> onDone = null;
		public UnityAction<UnityWebRequest> onFail = null;

		public UnityWebRequestClient(MonoBehaviour mb){
			monoBehaviour = mb;
		}

		public void SetAccessToken(string token){
			accessToken = token;
		}

		// 送受信のためのUnityWebRequest Classを生成する
		// UnityWebRequestに用意されているREST Methodは使わない（送信時にURLエンコードされる）
		UnityWebRequest CreateRequester(string url,string method,string sendData = default(string)){
			var requester = new UnityWebRequest (url);
			if (!string.IsNullOrEmpty (sendData)) {
				requester.uploadHandler = new UploadHandlerRaw (System.Text.Encoding.UTF8.GetBytes (sendData));
			}
			requester.downloadHandler = new DownloadHandlerBuffer();
			requester.method = method;
			requester.SetRequestHeader ( "Content-Type", contentType );
			requester.SetRequestHeader ( "AUTHORIZATION", string.Format("Token token=\"{0}\"",accessToken));
			requester.chunkedTransfer = false;
			return requester;
		}

		// データ受信後の処理
		bool ResponseProcess(UnityWebRequest requester){
			if (requester.isNetworkError) {
				if (onFail != null) {
					onFail (requester);
				}
				Debug.LogError (requester.error);
				return false;
			}

			if (HasRequestError(requester.responseCode)) {
				try{
					var error = JsonUtility.FromJson<Protocol.Error> (requester.downloadHandler.text);
					Debug.Log("通信のエラー処理をしたいときはここに追加してね！");
					Debug.LogError( error.message );
				}catch(System.ArgumentException e) {
					Debug.LogError ("IPアドレスが設定されていないかも？ ApiClient.Instance.SetIpAddress(string ipAddress)にIPアドレスを設定してね！\n" + e.Message);
				}
				return false;
			}

			if (requester.downloadHandler.isDone) {
				if (onDone != null) {
					onDone (requester.downloadHandler);
				}
				return true;
			} else {
				Debug.LogError (requester.error);
			}
			return false;
		}

		// NOTE:400番台以外のエラー処理は考えない
		bool HasRequestError(long responseCode){
			return responseCode / 100 == 4;
		}

		#region GET

		public void Get(string url){
			monoBehaviour.StartCoroutine (GetRequest(url));
		}

		IEnumerator GetRequest(string url){
			var requester = CreateRequester(url, UnityWebRequest.kHttpVerbGET);

			yield return requester.Send();

			ResponseProcess (requester);
		}

		#endregion GET

		#region POST

		public void Post(string url,string sendData){
			monoBehaviour.StartCoroutine (PostRequest(url,sendData));
		}

		IEnumerator PostRequest(string url,string sendData){
			var requester = CreateRequester(url,UnityWebRequest.kHttpVerbPOST,sendData);

			yield return requester.Send ();

			ResponseProcess (requester);
		}

		#endregion POST

		#region DELETE

		public void Delete(string url,string sendData){
			monoBehaviour.StartCoroutine (DeleteRequest(url,sendData));
		}

		IEnumerator DeleteRequest(string url,string sendData){
			var requester = CreateRequester(url,UnityWebRequest.kHttpVerbDELETE,sendData);

			yield return requester.Send ();

			ResponseProcess (requester);
		}

		#endregion DELETE

		#region PUT

		public void Put(string url,string sendData){
			monoBehaviour.StartCoroutine (PutRequest(url,sendData));
		}

		IEnumerator PutRequest(string url,string sendData){
			var requester = CreateRequester(url,UnityWebRequest.kHttpVerbPUT,sendData);

			yield return requester.Send ();

			ResponseProcess (requester);
		}

		#endregion PUT
	}
}
