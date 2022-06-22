using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using UnityEngine.Networking;
using System;

namespace AuthenticationScope 
{

 
    public class AuthenticationBehaviour : MonoBehaviour
    {
        public void InitializeAuth() 
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaClass tamatemClass = new AndroidJavaClass("com.tamatem.auth.TamatemAuth");
                        AndroidJavaObject authInstance = tamatemClass.CallStatic<AndroidJavaObject>("getInstance");
                        authInstance.Call("startLoginProcess", activityContext, "pi4dEipJyFLDbO9DOYWFlolNpOgzjjYI2oq0qVJz", "game1://oauth-callback", new AndroidPluginCallback());
                    }));
                }
            #endif
        }
    }

    class AndroidPluginCallback : AndroidJavaProxy
    {
        public AndroidPluginCallback() : base ("com.tamatem.auth.TamatemAuth$AuthorizationCallback") {}
    
        void onSuccess(AndroidJavaObject obj)
        {
            Debug.Log("Results retreived successfully!!");
            Debug.Log("Token retreived from Unity: " + obj);

            // TamatemAuth.SetAccesToken(obj.accessToken);
        }

        void onFail() 
        {
            Debug.Log("Failed to retreive token");
        }
    }

       public class TamatemSDK : MonoBehaviour{
        private string _accessToken {get; set;}
        private int _expiry{get; set;}
        private string _refreshToken {get; set; }
        private string _strigifiedUser {get; set; }

        private static DateTime _JanFirst1970 = new DateTime(1970, 1, 1);

        public string GetAccesToken()
        {
            return _accessToken;
        }

        public void SetAccesToken(string accessToken)
        {
            _accessToken = accessToken;
        }

        public string GetRefreshToken()
        {
            return _refreshToken;
        }

        public void SetRefreshToken(string refreshToken)
        {
            _refreshToken = refreshToken;
        }        
        public int GetExpiry()
        {
            return _expiry;
        }

        public void SetExpiry(int expiry)
        {
            _expiry = expiry;
        }        
        
        public string GetStringifiedUser()
        {
            return _strigifiedUser;
        }

        public void SetStringifiedUser(string userObjectString)
        {
            _strigifiedUser = userObjectString;
        }

        public static long getTime()
        {
            return (long)((DateTime.Now.ToUniversalTime() - _JanFirst1970).TotalMilliseconds + 0.5);
        }


        public bool IsloggedIn()
        {
           if (_accessToken == null )
           {
                return false;
           }
           else {
                return true;
           }
        }


        // public bool RefreshToken()
        // {
        //     // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //     // formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //     // formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        //     // UnityWebRequest www = UnityWebRequest.Post("https://www.my-server.com/myform", formData);
        //     // // var json = "{\"username\":\""+username+"\", \"password\":\""+password+"\", \"email\":\""+email+"\"}";

        //     // www.SetRequestHeader("Content-Type", "application/json");
        //     // yield return www.SendWebRequest();

        //     // if (www.responseCode != 200)
        //     // {
        //     //     Debug.Log(www.error);
        //     // }
        //     // else
        //     // {
        //     //     Debug.Log("Form upload complete!");
        //     // }
        // }



        // public bool IsTokenReadyOrReferesh()
        // {
        //     double timestampNow = 1498122000;
        //     DateTime fecha = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(timestamp);
        //     if(TamatemAuth.expiry <= timestampNow)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         this.RefreshToken();
        //     }
        // }
    }


}
