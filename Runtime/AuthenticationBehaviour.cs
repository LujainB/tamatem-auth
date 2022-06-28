using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AOT;

namespace AuthenticationScope
{


    public class AuthenticationBehaviour : MonoBehaviour
    {

        [DllImport("__Internal")]
        private static extern void framework_Authenticate(string clientID, string scheme, string redirectURI);

        [DllImport("__Internal")]
        private static extern void framework_setDelegate(DelegateCallbackFunction callback);

        public delegate void DelegateCallbackFunction(string tokenModel);

        [MonoPInvokeCallback(typeof(DelegateCallbackFunction))]
        public static void onSuccess(string tokenModel) {
            Debug.Log("User Logged in iOS");
            Debug.Log("Message received: " + tokenModel);

            var result = JObject.Parse(tokenModel);
            mono.updateUserParameters(result);
        }

        private static AuthenticationBehaviour mono;

        internal static AuthenticationBehaviour wkr;
        Queue<Action> jobs = new Queue<Action>();

        void Awake() {
            wkr = this;
        }

        void Update() {
            while (jobs.Count > 0)
                jobs.Dequeue().Invoke();
        }

        internal void AddJob(Action newJob) {
            jobs.Enqueue(newJob);
        }

        private string _accessToken {get; set;}
        private long _expiry{get; set;}
        private string _refreshToken {get; set; }
        private JToken _user {get; set; }

        void Start() {
            mono = this;
        }

        public void InitializeAuth()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaClass tamatemClass = new AndroidJavaClass("com.tamatem.auth.TamatemAuth");
                        AndroidJavaObject authInstance = tamatemClass.CallStatic<AndroidJavaObject>("getInstance");
                        authInstance.Call("startLoginProcess", activityContext, "pi4dEipJyFLDbO9DOYWFlolNpOgzjjYI2oq0qVJz", "game1://oauth-callback", new AndroidPluginCallback(mono));
                    }));
                }
            #endif
            #if UNITY_IOS && !UNITY_EDITOR
                framework_setDelegate(onSuccess);
                framework_Authenticate("pi4dEipJyFLDbO9DOYWFlolNpOgzjjYI2oq0qVJz", "game1", "game1://oauth-callback");
            #endif
        }

        public void updateUserParameters(JObject result) {

            SetAccessToken(result["access_token"].ToObject<string>());
            SetRefreshToken(result["refresh_token"].ToObject<string>());
            SetExpiry(result["expires_in"].ToObject<long>());
            SetUser(result["user"]);

            Debug.Log("Before Coroutine!");
            AuthenticationBehaviour.wkr.AddJob(() => {
                // Will run on main thread, hence issue is solved
                StartCoroutine(purchasedInventory());
            });
            Debug.Log("After Coroutine!");
        }

        private DateTime _JanFirst1970 = new DateTime(1970, 1, 1);

        public string GetAccessToken()
        {
            return _accessToken;
        }

        public void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            Debug.Log("Access Token " + _accessToken);
        }

        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        public void SetRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
            Debug.Log("Refresh Token " + _refreshToken);
        }
        public long GetExpiry()
        {
            return _expiry;
        }

        public void SetExpiry(long expiry)
        {
            _expiry = expiry + _getTime();
            Debug.Log("Expiry " + _expiry);
        }

        public JToken GetUser()
        {
            return _user;
        }

        public void SetUser(JToken user)
        {
            _user = user;
            Debug.Log("User " + _user);
        }

        private long _getTime()
        {
            return (long)((DateTime.Now.ToUniversalTime() - _JanFirst1970).TotalMilliseconds + 0.5);
        }

        public bool IsloggedIn()
        {
           if (_accessToken == null && _getTime() < _expiry)
           {
                return false;
           }
           else {
                return true;
           }
        }

        IEnumerator purchasedInventory() {

            Debug.Log("Inside Coroutine!");
             using (UnityWebRequest www = UnityWebRequest.Get("https://tamatem.dev.be.starmena-streams.com/api/inventory-item/")){
                www.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                yield return www.Send();

                Debug.Log("purchased API sent!");
                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.Log("purchased API Error!");
                    Debug.Log(www.error);
                }
                else {
                    Debug.Log("Form upload complete!");
                    Debug.Log(www.downloadHandler.text);
                }
             }
        }
    }

    class AndroidPluginCallback : AndroidJavaProxy
    {
        private AuthenticationBehaviour mono;

        public AndroidPluginCallback(AuthenticationBehaviour mon) : base ("com.tamatem.auth.TamatemAuth$AuthorizationCallback") {
            mono = mon;
        }

        void onSuccess(string obj)
        {
            Debug.Log("User Logged in Android!!");
            Debug.Log("Token retrieved from Unity: " + obj);

            var result = JObject.Parse(obj);
            mono.updateUserParameters(result);
        }

        void onFail()
        {
            Debug.Log("Failed to retreive token");
        }
    }
}
