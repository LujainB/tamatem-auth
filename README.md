# tamatem-auth

This repository has been created to ease the job of authentication and integration process with Tamatem platform just by implementing the following steps:

## Unity Integration:

 // todo set gameID & redirectURI

## Android Setup:

Once your Unity project is ready and you want to setup your Android game/app, please follow the steps below:
1. Go to File -> Build Settings...
2. Make sure the "Export Project" checkbox is checked.
3. Press on the Export button and choose the folder that will contain the Android project.
4. Open the project using Android Studio.
5. Go to "gradle.properites" file and add "android.useAndroidX=true" line
6. Voila! you are ready to go and use your app.


## iOS Setup:

Once your Unity project is ready and you want to setup your iOS game/app, please follow the steps below:
1. Go to File -> Build Settings...
2. Press on "Build And Run" and choose the folder that will contain the iOS project.
3. Once its finished, it will open the project on xCode.
4. Go to "Unity-iPhone" Target and open the "Build Phase" tab.
5. Expand the "Embed Frameworks" section and press the "+" button.
6. Type "keyCSDK" to add "keyCSDK.framework".
7. Now Make sure to stop all previous running tasks and run the app again.
8. Congrats! your iOS app is ready to go.


## SDK API:

### Runtime/AuthenticationBehaviour.cs
###Class TamatemSDK

#### bool isLoggedIn()
returns if user is logged in
#### void authenticate()
triggers native authentication process
#### Item[] purchasedInventory()
returns all items
#### Item[] filterInventory(bool isRedeemed)
returns redeemed or non redeemed items based on isRedeemed
#### bool redeemedItem(int ID)
redeems items
returns true if successful
#### getUserInfo()
returns JToken containing the following
```json
{
"id": 16,
"first_name": "Moath Test",
"last_name": "Test",
"email": "tesasdt+1@teasdsa3dasdasdasdst.com",
"country": "american-samoa",
"phone_number": "+962797351170",
"date_of_birth": "2022-05-14",
"sign_up_through": "email",
"gender": "f",
"qr_code": "/media/qr-codes/PR22000006.png",
"avatar": null,
"tamatem_id": "PR22000006"
}
```
