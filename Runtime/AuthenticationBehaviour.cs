using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        private static AuthenticationBehaviour _instance;
        private static AuthenticationBehaviour mono;
        internal DataRequestsProcess dataRequestsInterface;
        private Queue<Action> jobs = new Queue<Action>();
        private String clientID;
        private String scheme;
        private String redirectURI;
        private bool isDevelopment;

        void Awake(){
            if (_instance == null){
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this);
            }
        }

        void Start() {
            mono = this;
        }

        internal static AuthenticationBehaviour getInstance() {
            return _instance;
        }

        internal void setParameters(DataRequestsProcess dataRequestsProcess, String gameClientID, String gameScheme, String gameRedirectURI, bool isDevelopment) {
            _instance.dataRequestsInterface = dataRequestsProcess;
            _instance.clientID = gameClientID;
            _instance.scheme = gameScheme;
            _instance.redirectURI = gameRedirectURI;
            _instance.isDevelopment = isDevelopment;
        }

        #if UNITY_IOS
            [DllImport("__Internal")]
            private static extern void framework_Authenticate(string clientID, string scheme, string redirectURI, bool isDevelopment);
            [DllImport("__Internal")]
            private static extern void framework_setDelegate(DelegateCallbackFunction callback);
        #endif

        public delegate void DelegateCallbackFunction(string tokenModel);

        [MonoPInvokeCallback(typeof(DelegateCallbackFunction))]
        public static void onSuccess(string tokenModel) {
            Debug.Log("User Logged in iOS");
            Debug.Log("Message received: " + tokenModel);

            var result = JObject.Parse(tokenModel);
            mono.dataRequestsInterface.loginSucceeded(result);
            mono.updateUserParameters(result);
        }

        void Update() {
            while (jobs.Count > 0) {
                jobs.Dequeue().Invoke();
            }
        }

        internal void AddJob(Action newJob) {
            jobs.Enqueue(newJob);
        }

        internal void InitializeAuth()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaClass tamatemClass = new AndroidJavaClass("com.tamatem.auth.TamatemAuth");
                        AndroidJavaObject authInstance = tamatemClass.CallStatic<AndroidJavaObject>("getInstance");
                        authInstance.Call("startLoginProcess", activityContext, _instance.clientID, _instance.redirectURI, _instance.isDevelopment, new AndroidPluginCallback(mono));
                    }));
                }
            #endif
            #if UNITY_IOS && !UNITY_EDITOR
                framework_setDelegate(onSuccess);
                framework_Authenticate(_instance.clientID, _instance.scheme, _instance.redirectURI, _instance.isDevelopment);
            #endif
        }

        internal void getPurchasedItems() {
            Debug.Log("getPurchasedItems");
            if(_accessToken == null) {
                return;
            }

            Debug.Log("add getPurchasedItems job");
            AddJob(() => {
                // Will run on main thread, hence issue is solved
                StartCoroutine(PurchasedInventory());
            });
        }

        internal void getRedeemedItems() {
            Debug.Log("getRedeemedItems");
            if(_accessToken == null) {
                return;
            }

            Debug.Log("add getRedeemedItems job");
            AddJob(() => {
                // Will run on main thread, hence issue is solved
                StartCoroutine(FilterInventory(true));
            });
        }

        internal void updateUserParameters(JObject result) {

            SetAccessToken(result["access_token"].ToObject<string>());
            SetRefreshToken(result["refresh_token"].ToObject<string>());
            SetExpiry(result["expires_in"].ToObject<long>());
            SetUser(result["user"]);
        }

        private DateTime _JanFirst1970 = new DateTime(1970, 1, 1);
        private string _accessToken {get; set;}
        private long _expiry{get; set;}
        private string _refreshToken {get; set; }
        private JToken _user {get; set; }

        internal string GetAccessToken()
        {
            return _accessToken;
        }

        internal void SetAccessToken(string accessToken)
        {
            _accessToken = accessToken;
            Debug.Log("Access Token " + _accessToken);
        }

        internal string GetRefreshToken()
        {
            return _refreshToken;
        }

        internal void SetRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
            Debug.Log("Refresh Token " + _refreshToken);
        }
        internal long GetExpiry()
        {
            return _expiry;
        }

        internal void SetExpiry(long expiry)
        {
            _expiry = expiry + _getTime();
            Debug.Log("Expiry " + _expiry);
        }

        internal JToken GetUser()
        {
            return _user;
        }

        internal void SetUser(JToken user)
        {
            _user = user;
            Debug.Log("User " + _user);
        }

        private long _getTime()
        {
            return (long)((DateTime.Now.ToUniversalTime() - _JanFirst1970).TotalMilliseconds + 0.5);
        }

        internal bool IsloggedIn()
        {
           if (_accessToken == null && _getTime() < _expiry)
           {
                return false;
           }
           else {
                return true;
           }
        }

        internal IEnumerator PurchasedInventory() {
             using (UnityWebRequest www = UnityWebRequest.Get("https://tamatem.dev.be.starmena-streams.com/api/inventory-item/")){
                www.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                yield return www.Send();

                if (www.result != UnityWebRequest.Result.Success) {
                    dataRequestsInterface.purchasedItemsResults(null);
                    Debug.Log(www.error);
                }
                else {
                    dataRequestsInterface.purchasedItemsResults(www.downloadHandler.text);
                    Debug.Log(www.downloadHandler.text);
                }
             }
        }

        internal IEnumerator FilterInventory(bool isRedeemed) {
             using (UnityWebRequest www = UnityWebRequest.Get("https://tamatem.dev.be.starmena-streams.com/api/inventory-item/?is_redeemed=" + isRedeemed)){
                www.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                yield return www.Send();

                if (www.result != UnityWebRequest.Result.Success) {
                    dataRequestsInterface.redeemedItemsResults(null);
                    Debug.Log(www.error);
                }
                else {
                    dataRequestsInterface.redeemedItemsResults(www.downloadHandler.text);
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
            Debug.Log("Results retreived successfully!!");
            Debug.Log("Token retreived from Unity: " + obj);

            var result = JObject.Parse(obj);
            mono.dataRequestsInterface.loginSucceeded(result);
            mono.updateUserParameters(result);
        }

        void onFail()
        {
            Debug.Log("Failed to retreive token");
        }
    }
}