using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        void onFail() 
        {
            Debug.Log("Failed to retreive token");
        }
    }
}
