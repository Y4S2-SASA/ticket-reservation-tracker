package com.sasa.ticketreservationapp.activities;

import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;
import android.widget.ArrayAdapter;
import android.widget.Spinner;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateReservationActivity extends AppCompatActivity {
    private Spinner destinationSpinner;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_reservation);

        destinationSpinner = findViewById(R.id.destinationSpinner);

        // Fetch destinations from API
//        fetchDestinations();

        //FETCH DESTINATIONS TEMPORARILY
        fetchDestinations(destinations -> {
            ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, destinations);
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            destinationSpinner.setAdapter(adapter);
        });
//-------------------------------------------------------Bottom App BAR FUNCTION---------------------------------------------
        //Initialize variables and assign them
        BottomNavigationView bottomNavigationView = findViewById(R.id.bottom_navigation);

        //Perform Item Selected Event Listener
        bottomNavigationView.setOnItemSelectedListener(new BottomNavigationView.OnItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                if (menuItem.getItemId() == R.id.profile) {
                    startActivity(new Intent(getApplicationContext(), ProfileActivity.class));
                    overridePendingTransition(0, 0);
                    return true;
                }else if (menuItem.getItemId() == R.id.home) {
                    startActivity(new Intent(getApplicationContext(), CurrentBookingsActivity.class));
                    overridePendingTransition(0, 0);
                    return true;
                }else if (menuItem.getItemId() == R.id.history)
                    startActivity(new Intent(getApplicationContext(), BookingHistoryActivity.class));
                overridePendingTransition(0, 0);
                return true;
            }
        });
//-------------------------------------------------------Bottom App BAR FUNCTION--------------------------------------------
    }

    private void fetchDestinations(DestinationCallback callback) {
        // Make your HTTP request to fetch the list of destinations
        // Once you have the list, pass it to the callback function
        List<String> destinations = new ArrayList<>();
        destinations.add("New York");
        destinations.add("Los Angeles");
        destinations.add("London");
        destinations.add("Tokyo");

        callback.onCallback(destinations);
    }

    interface DestinationCallback {
        void onCallback(List<String> destinations);
    }
//    private void fetchDestinations() {
//
//        ApiInterface apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
//
//        Call<List<String>> call = apiInterface.getDestinations("Your_Authorization_Header_Value");
//
//        call.enqueue(new Callback<List<String>>() {
//            @Override
//            public void onResponse(Call<List<String>> call, Response<List<String>> response) {
//                if (response.isSuccessful() && response.body() != null) {
//                    List<String> destinations = response.body();
//                    ArrayAdapter<String> adapter = new ArrayAdapter<>(CreateReservationActivity.this, android.R.layout.simple_spinner_item, destinations);
//                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
//                    destinationSpinner.setAdapter(adapter);
//                }
//            }
//
//            @Override
//            public void onFailure(Call<List<String>> call, Throwable t) {
//                t.printStackTrace();
//            }
//        });
//    }
}