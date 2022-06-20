using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


namespace Tamatem.Tools{
    public class SampleToolKit : MonoBehaviour {
        
        #if UNITY_ANDROID
                [DllImport ("__Internal")]
                private static extern void NativeAndroidCode_runNativeCode();
        #endif

        public void runNativeCode()
        {
        #if UNITY_ANDROID
            NativeAndroidCode_runNativeCode();
            Debug.Log("Android Device");
        #else
            Debug.Log("Not a Mobile Device");
        #endif

        }
       public void OnButtonPress(){
        runNativeCode();
        Debug.Log("Hello Android");
       }

}
}
