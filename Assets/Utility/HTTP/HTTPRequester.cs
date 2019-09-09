using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using Network;
using UnityEngine.Networking;

namespace HTTP {
	public sealed class HTTPRequester{

		MonoBehaviour monoBehaviour;
		string token;

		public HTTPRequester(MonoBehaviour monoBehaviour){
			this.monoBehaviour = monoBehaviour;
		}

		public void SetAccessToken(string token){
			this.token = token;
		}

		UnityWebRequestClient CreateWebClient<T>(UnityAction<T> callback) {
			var webClient = new UnityWebRequestClient (monoBehaviour);
			webClient.SetAccessToken (token);

			webClient.onDone = (DownloadHandler handler) => {

				T json = default(T);
				try {
					json = JsonUtility.FromJson<T> (handler.text);
				} catch (System.ArgumentException e) {
					Debug.LogError (string.Format ("{1}で不正な値がサーバーから返されました。\n{0}\n{2}", handler.text,typeof(T),e.StackTrace));
				}
				callback(json);
			};
			webClient.onFail = (UnityWebRequest requester) => {
				Debug.LogError (requester.error);
			};

			return webClient;
		}

		//-------------------------------------------------------------
		// POSTリクエスト
		// @param
		// @リクエストURL
		// @送信データ
		// @callback
		//-------------------------------------------------------------
		public void Post<SEND,RESPONSE>(string url, SEND post, UnityAction<RESPONSE> callback) {
			var webClient = CreateWebClient<RESPONSE> (callback);
			var json = JsonUtility.ToJson(post);
			webClient.Post(url, json);
		}

		//-------------------------------------------------------------
		// GETリクエスト
		// @param
		// @リクエストURL
		// @callback
		//-------------------------------------------------------------
		public void Get<RESPONSE>(string url, UnityAction<RESPONSE> callback) {
			var webClient = CreateWebClient<RESPONSE> (callback);
			webClient.Get(url);
		}

		//-------------------------------------------------------------
		// DELETEリクエスト
		// @param
		// @リクエストURL
		// @送信データ
		// @callback
		//-------------------------------------------------------------
		public void Delete<SEND,RESPONSE>(string url, SEND data, UnityAction<RESPONSE> callback) {
			var webClient = CreateWebClient<RESPONSE> (callback);
			var json = JsonUtility.ToJson(data);
			webClient.Delete(url, json);
		}

		//-------------------------------------------------------------
		// PUTリクエスト
		// @param
		// @リクエストURL
		// @送信データ
		// @callback
		//-------------------------------------------------------------
		public void Put<SEND,RESPONSE>(string url, SEND data, UnityAction<RESPONSE> callback) {
			var webClient = CreateWebClient<RESPONSE> (callback);
			var json = JsonUtility.ToJson(data);
			webClient.Put(url, json);
		}
	}
}
