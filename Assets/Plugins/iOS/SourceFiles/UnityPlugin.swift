//
//  UnityPlugin.swift
//  KeyCSDK
//
//  Created by sanad barjawi on 15/05/2022.
//

//exposing function to the .mm file
import Foundation
import KeyCSDK
@objc final public class UnityPlugin : NSObject {
    
    @objc public static var shared = UnityPlugin()
    private var authManager: AuthPOCManager = AuthPOCManager()
    
    @objc public func AddTwoNumbers(a:Int,b:Int ) -> Int {
        print("i did Something with \(a) and \(b)!")
        return a + b
    }
    
    @objc public func getName() -> String {
        return "Sanad"
    }
    
    @objc public func authenticate(scheme: String, redirectURI: String) -> TokenModel? {
        switch authManager.auth(scheme: scheme, redirectURI: redirectURI) {
            
        case .success(let tokenmodel):
            return tokenmodel
        case .failure(_):
            return nil

        }
    }
    
    func sendUnityMessage(String: String) {
        //Uncomment once placed into unity as a plugin
        //        UnitySendMessage("UnityObjectName", "sendMessageFromiOS", "HII")
    }
    
}
