//
//  UnityPluginBridge.m
//  KeyCSDK
//
//  Created by sanad barjawi on 15/05/2022.
//

#import <Foundation/Foundation.h>
#import <AuthenticationServices/ASWebAuthenticationSession.h>
#import <KeyCSDK/KeyCSDK-Swift.h>

// alloc char and copy new instence editable
char* cStringCopy(const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

//exposes Functions to the header file
extern "C" {
    
#pragma mark - Functions
    
    int _addTwoNumberInIOS(int a , int b) {
        int result = [[UnityPlugin shared] AddTwoNumbersWithA:a b:b];
        return result;
    }
    
    char* _GetName() {
        return cStringCopy([[[UnityPlugin shared] getName] UTF8String]);
    }

    TokenModel* _Authenticate(char* scheme, char* redirectURI) {

        return [[UnityPlugin shared ] authenticateWithScheme:[NSString stringWithUTF8String: scheme] redirectURI:[NSString stringWithUTF8String: redirectURI]];
    }

}
