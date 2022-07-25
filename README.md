# tamatem-auth

This repository has been created to ease the job of authentication and integration process with Tamatem platform just by implementing the following steps:

## Unity Integration:


## Android Setup:

Once your Unity project is ready and you want to setup your Android game/app, please follow the steps below:
1. Go to File -> Build Settings...
2. Click on Player Settings then make sure Android tab is selected.
3. Open Publisher Settings panel.
4. Check `Custom Gradle Properties Template` checkbox.
5. Now Open `gradleTemplate.gradle` file and paste the following line at the end of file.
```shell script
    android.useAndroidX=true
```
6. Close the Player Settings screens and make sure the "Export Project" checkbox is checked.
7. Press on the Export button and choose the folder that will contain the Android project.
8. Open the project using Android Studio and run your app.


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

### Runtime/Tamatem.cs

#### Game Configuraton Constants
- GAME_CLIENT_ID
- GAME_SCHEME
- GAME_REDIRECT_URI
- GAME_DEVELOPMENT_ENV

These values can be changed based on your game's configrations
When setting GAME_DEVELOPMENT_ENV to true, means that it will connect the game to the dev server which is made for development purposed. Please make sure to set it to false before releasing to Production.

#### authenticateUser()
this method is to open the in-app browser to let the user login

#### getPurchasedItems()
this method is to get the purchased items

#### getRedeemedItems()
this method is to get the redeemed items (you can set the is_redeemed paramter value from the `AuthenticationBehaviour.cs` script)

#### redeemInventories()
this method is to set all non-redeemed items to redeemed and get the whole list


### Runtime/AuthenticationBehaviour.cs
### Class TamatemSDK

#### bool IsloggedIn()
returns if user is logged in
#### void InitializeAuth()
triggers native authentication process
#### Item[] PurchasedInventory()
returns all items
#### Item[] FilterInventory(bool isRedeemed)
returns redeemed or non redeemed items based on isRedeemed
#### Item[] RedeemAllInventories()
returns redeemed items after setting them to redeemed
#### GetUser()
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
