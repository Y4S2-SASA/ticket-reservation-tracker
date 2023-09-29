package com.sasa.ticketreservationapp;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface ApiInterface {
    @POST("api/User/saveUser")
    Call<Void> saveUser(@Body UserModel userModel);
}
