package com.sasa.ticketreservationapp.config;

import com.sasa.ticketreservationapp.request.AvailableSeatRequest;
import com.sasa.ticketreservationapp.request.LoginRequest;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.request.AccountStatusRequest;
import com.sasa.ticketreservationapp.request.PriceRequest;
import com.sasa.ticketreservationapp.request.ReservationRequest;
import com.sasa.ticketreservationapp.request.ScheduleRequest;
import com.sasa.ticketreservationapp.request.StatusChangeRequest;
import com.sasa.ticketreservationapp.response.ReservationDetailsResponse;
import com.sasa.ticketreservationapp.response.ReservationResponse;
import com.sasa.ticketreservationapp.response.BasicReservationResponse;
import com.sasa.ticketreservationapp.response.ScheduleResponse;
import com.sasa.ticketreservationapp.response.StationResponse;
import com.sasa.ticketreservationapp.response.LoginResponse;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

/*
 * File: ApiInterface.java
 * Purpose: Handles all API calls in the mobile application
 */
public interface ApiInterface {
    @POST("api/User/saveUser")
    Call<Void> saveUser(@Body UserModel userModel);

    @POST("api/Authentication/authenticate")
    Call<LoginResponse> loginUser(@Body LoginRequest loginRequest);

    @GET("api/User/getUserById/{id}")
    Call<UserModel> getUserById(@Header("Authorization") String authorization, @Path("id") String id);

    @PUT("api/User/updateUser")
    Call<Void> updateUser(@Header("Authorization") String authorization, @Body UserModel userModel);

    @GET("api/MasterData/getAllStationMasterData")
    Call<List<StationResponse>> getStations(@Header("Authorization") String authorization);

    @PUT("api/User/deactiveTravelerAccount")
    Call<Void> updateUserStatus(@Header("Authorization") String authorization, @Body AccountStatusRequest accountStatusRequest);

    @POST("api/Schedule/getScheduleTrainsData")
    Call<List<ScheduleResponse>> getScheduleTrainsData(@Header("Authorization") String authorization, @Body ScheduleRequest request);

    @POST("api/Schedule/getAvailableTrainSeatCount")
    Call<Integer> getAvailableTrainSeatCount(@Header("Authorization") String authorization, @Body AvailableSeatRequest request);

    @POST("api/Schedule/getSchedulePrice")
    Call<Integer> getSchedulePrice(@Header("Authorization") String authorization, @Body PriceRequest request);

    @POST("api/Reservation/saveReservation")
    Call<BasicReservationResponse> saveReservation(@Header("Authorization") String authorization, @Body ReservationRequest request);

    @GET("api/Reservation/getTraverlerReservation/{status}")
    Call<List<ReservationResponse>> getTraverlerReservation(@Header("Authorization") String authorization, @Path("status") Integer status);

    @GET("api/Reservation/getReservationById/{id}")
    Call<ReservationDetailsResponse> getReservationById(@Header("Authorization") String authorization, @Path("id") String id);

    @PUT("api/Reservation/changeReservationStatus")
    Call<BasicReservationResponse> changeReservationStatus(@Header("Authorization") String authorization, @Body StatusChangeRequest statusChangeRequest);

}
