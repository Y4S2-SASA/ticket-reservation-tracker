package com.sasa.ticketreservationapp.config;

import com.sasa.ticketreservationapp.models.LoginModel;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.response.LoginResponse;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface ApiInterface {
    @POST("api/User/saveUser")
    Call<Void> saveUser(@Body UserModel userModel);

    @POST("api/Authentication/authenticate")
    Call<LoginResponse> loginUser(@Body LoginModel loginModel);
}
