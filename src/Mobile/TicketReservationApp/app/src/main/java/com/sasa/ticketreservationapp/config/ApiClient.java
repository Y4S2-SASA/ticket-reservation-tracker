package com.sasa.ticketreservationapp.config;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

/*
 * File: ApiClient.java
 * Purpose: Acts as the base of the API calls
 */
public class ApiClient {
    private static final String BASE_URL = "http://10.0.2.2:2023/";

    private static Retrofit retrofit = null;

    public static Retrofit getApiClient() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(BASE_URL)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }
}
