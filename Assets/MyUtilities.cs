using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DLLTest {

    public class MyUtilities {
        #if UNITY_ANDROID
        [DllImport ("__Internal")]
        private static extern void NativeAndroidCode_runNativeCode();
        #endif

       public void runNativeCode(){
        #if UNITY_ANDROID
        NativeAndroidCode_runNativeCode();
        Debug.Log("Android Device");

        #else
        Debug.Log("No Mobile Device.");
        #endif
       }
    }
}