package com.sasa.ticketreservationapp.config;

import com.sasa.ticketreservationapp.models.LoginModel;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.response.LoginResponse;

import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface ApiInterface {
    @POST("api/User/saveUser")
    Call<Void> saveUser(@Body UserModel userModel);

    @POST("api/Authentication/authenticate")
    Call<LoginResponse> loginUser(@Body LoginModel loginModel);

    @GET("api/User/getUserById/{id}")
    Call<UserModel> getUserById(@Header("Authorization") String authorization, @Path("id") String id);


    @PUT("api/User/updateUser")
    Call<Void> updateUser(@Header("Authorization") String authorization, @Body UserModel userModel);


}
