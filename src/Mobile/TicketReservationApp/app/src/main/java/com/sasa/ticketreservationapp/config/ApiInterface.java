package com.sasa.ticketreservationapp.config;

import com.sasa.ticketreservationapp.models.LoginModel;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.response.LoginResponse;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface ApiInterface {
    @POST("api/User/saveUser")
    Call<Void> saveUser(@Body UserModel userModel);

    @POST("api/Authentication/authenticate")
    Call<LoginResponse> loginUser(@Body LoginModel loginModel);

    @GET("api/User/getUserById/{id}")
    Call<UserModel> getUserById(@Path("id") String id);

}
