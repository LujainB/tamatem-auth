package com.tamatem.auth;

import android.content.Context;
import android.content.Intent;

import androidx.annotation.RestrictTo;

import java.util.Random;

public class TamatemAuth {

    @RestrictTo(RestrictTo.Scope.LIBRARY)
    protected String codeVerifier;
    @RestrictTo(RestrictTo.Scope.LIBRARY)
    protected AuthorizationCallback authorizationCallback;
    @RestrictTo(RestrictTo.Scope.LIBRARY)
    protected String clientId;
    @RestrictTo(RestrictTo.Scope.LIBRARY)
    protected String redirectUri;

    private static TamatemAuth instance;

    public static TamatemAuth getInstance() {
        synchronized (TamatemAuth.class) {
            if (instance == null) {
                instance = new TamatemAuth();
            }
            return instance;
        }
    }

    private TamatemAuth() {
    }

    public void startLoginProcess(Context context, String clientId, String redirectUri,
                                  AuthorizationCallback authorizationCallback) {

        this.authorizationCallback = authorizationCallback;
        this.clientId = clientId;
        this.redirectUri = redirectUri;
        this.codeVerifier = generateRandomString();

        Intent intent = new Intent(context, LoadingActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(intent);
    }


    private String generateRandomString() {
        int stringLength = 45;
        int leftLimit = 48; // numeral '0'
        int rightLimit = 122; // letter 'z'
        Random random = new Random();
        return random.ints(leftLimit, rightLimit + 1)
                .filter(i -> (i <= 57 || i >= 65) && (i <= 90 || i >= 97))
                .limit(stringLength).collect(StringBuilder::new, StringBuilder::appendCodePoint, StringBuilder::append)
                .toString();
    }


    public interface AuthorizationCallback {
        void onSuccess(Object results);

        void onFail();
    }
}
