package com.tamatem.auth;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Headers;
import retrofit2.http.POST;

public interface LoginService {

    @Headers({
            "Host:tamatem.dev.be.starmena-streams.com",
            "Content-Type:application/json",
            "User-Agent:Android",
    })
    @POST("get-token/")
    Call<Object> attemptLogin(@Body LoginRequest loginRequest);
}
