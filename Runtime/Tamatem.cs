using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AuthenticationScope 
{
    public interface DataRequestsProcess {
        void loginSucceeded(JObject result);
        void loginFailed();
        void purchasedItemsResults(string result);
        void redeemedItemsResults(string result);
        void redeeemInventoriesResult(string result);
    }

    public class Tamatem : MonoBehaviour, DataRequestsProcess
    {
        //TODO: please change the below constants to match your game's configurations
        private const string GAME_CLIENT_ID = "pi4dEipJyFLDbO9DOYWFlolNpOgzjjYI2oq0qVJz";
        private const string GAME_SCHEME = "game1";
        private const string GAME_REDIRECT_URI = "game1://oauth-callback";
        private const bool GAME_DEVELOPMENT_ENV = true;

        public Text InfoText;

        private AuthenticationBehaviour getAuthenticationBehaviour() {
            return AuthenticationBehaviour.getInstance();
        }

        public void authenticateUser() {
            InfoText.text = "start login process";
            AuthenticationBehaviour instance = getAuthenticationBehaviour();
            if(instance == null) {
                InfoText.text = "no instance found";
               return;
            }
            instance.setParameters(this, GAME_CLIENT_ID, GAME_SCHEME, GAME_REDIRECT_URI, GAME_DEVELOPMENT_ENV);
            instance.InitializeAuth();
        }

        public void getPurchasedItems() {
            AuthenticationBehaviour instance = getAuthenticationBehaviour();
            if(instance == null || instance.GetAccessToken() == null) {
                InfoText.text = "You need to login first";
                if(instance == null) {
                InfoText.text = "no instance found";
            }
               return;
            }

            InfoText.text = "Loading purchased items ...";
            instance.getPurchasedItems();
        }

        public void getRedeemedItems() {
            AuthenticationBehaviour instance = getAuthenticationBehaviour();
            if(instance == null || instance.GetAccessToken() == null) {
                InfoText.text = "You need to login first";
                if(instance == null) {
                InfoText.text = "no instance found";
            }
               return;
            }

            InfoText.text = "Loading redeemed items ...";
            instance.getRedeemedItems();
        }

        public void redeemInventories() {
            AuthenticationBehaviour instance = getAuthenticationBehaviour();
            if(instance == null || instance.GetAccessToken() == null) {
                InfoText.text = "You need to login first";
                if(instance == null) {
                InfoText.text = "no instance found";
            }
               return;
            }

            InfoText.text = "Redeeming items ...";
            instance.redeemInventories();
        }

        public void loginSucceeded(JObject result) {
            //TODO: Handle your login data here
            InfoText.text = "User Logged In Successfully";
        }

        public void loginFailed() {
            //TODO: Handle being failed to login here
            InfoText.text = "Failed to login";
        }

        public void purchasedItemsResults(string result) {
            if(result == null) {
                InfoText.text = "Failed to retrieve purchased items";
            } else {
                InfoText.text = result;
            }
        }

        public void redeemedItemsResults(string result) {
            if(result == null) {
                InfoText.text = "Failed to retrieve redeemed items";
            } else {
                InfoText.text = result;
            }
        }

        public void redeeemInventoriesResult(string result) {
            if(result == null) {
                InfoText.text = "Failed to redeem items";
            } else {
                InfoText.text = result;
            }
        }
    }
}