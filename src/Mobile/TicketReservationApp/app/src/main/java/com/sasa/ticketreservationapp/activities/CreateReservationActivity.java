package com.sasa.ticketreservationapp.activities;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.MenuItem;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.response.DestinationResponse;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateReservationActivity extends AppCompatActivity {
    private Spinner destinationSpinner;
    private ApiInterface apiInterface;
    private String id, token, destinationId;
    private SharedPreferences prefs;
    private EditText reservedDateField;
    private Calendar calendar;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_reservation);

        destinationSpinner = findViewById(R.id.destinationSpinner);
        reservedDateField = findViewById(R.id.reservedDateField);
        calendar = Calendar.getInstance();

        reservedDateField.setOnClickListener(view -> showDatePickerDialog());

        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
        }else{
            Intent intent = new Intent(CreateReservationActivity.this, LoginActivity.class);
            startActivity(intent);
        }

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

    private void showDatePickerDialog() {
        new DatePickerDialog(
                this,
                (view, year, month, dayOfMonth) -> {
                    calendar.set(Calendar.YEAR, year);
                    calendar.set(Calendar.MONTH, month);
                    calendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);

                    updateDateField();
                },
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
        ).show();
    }

    private void updateDateField() {
        String myFormat = "MM/dd/yyyy"; // Change this format as per your requirement
        SimpleDateFormat sdf = new SimpleDateFormat(myFormat, Locale.US);

        reservedDateField.setText(sdf.format(calendar.getTime()));
    }

    //    private void fetchDestinations() {
//
//        ApiInterface apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
//
//        Call<List<DestinationResponse>> call = apiInterface.getDestinations("Bearer" + token);
//
//        call.enqueue(new Callback<List<DestinationResponse>>() {
//            @Override
//            public void onResponse(Call<List<DestinationResponse>> call, Response<List<DestinationResponse>> response) {
//                if (response.isSuccessful() && response.body() != null) {
//                    List<DestinationResponse> destinations = response.body();
//                    List<String> destinationNames = new ArrayList<>();
//                    Map<String, String> idToNameMap = new HashMap<>();
//
//                    for (DestinationResponse destination : destinations) {
//                        destinationNames.add(destination.getName());
//                        idToNameMap.put(destination.getId(), destination.getName());
//                    }
//
//                    ArrayAdapter<String> adapter = new ArrayAdapter<>(CreateReservationActivity.this, android.R.layout.simple_spinner_item, destinationNames);
//                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
//                    destinationSpinner.setAdapter(adapter);
//
//                    // Save the idToNameMap for later use
//                    // You can access the name corresponding to an id like idToNameMap.get(selectedId)
//                }
//            }
//
//            @Override
//            public void onFailure(Call<List<DestinationResponse>> call, Throwable t) {
//                t.printStackTrace();
//            }
//        });
//    };



};