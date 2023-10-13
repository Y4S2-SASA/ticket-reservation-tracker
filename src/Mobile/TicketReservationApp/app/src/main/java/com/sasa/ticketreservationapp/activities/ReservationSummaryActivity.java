package com.sasa.ticketreservationapp.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.request.ReservationRequest;
import com.sasa.ticketreservationapp.request.ViewSummaryRequest;
import com.sasa.ticketreservationapp.response.BasicReservationResponse;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/*
 * File: ReservationSummaryActivity.java
 * Purpose: Handle Viewing the reservationSummary details and providing the confirmation for the update or the create.
 * Author: Jayathilake S.M.D.A.R/IT20037338
 * Description: This activity is responsible of viewing the reservation summary details so that the user is able to review them and provide their confirmation before the creation or update of the reservation.
 */

public class ReservationSummaryActivity extends AppCompatActivity {
    private ApiInterface apiInterface;
    private SharedPreferences prefs;
    private String token, id, method;
    private EditText destinationField, subStationField, reservedDateField, passengerClassField, reservedTimeField, trainNameField, priceField, passengerCountField;
    private Button backBtn, confirmBtn;
    private String selectedDestinationId, selectedSubStationId, selectedTrainId, dateTime;
    private int pClass, passengerCount;
    private double ticketPrice;
    private CheckBox checkBox;
    private BasicReservationResponse basicReservationResponse;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_reservation_summary);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class); // Initialize apiClient
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
            if(prefs.getString("nic", "") != null){
                id = prefs.getString("nic", "");
                token = prefs.getString("token", "");
            }
        }else{
            Intent intent = new Intent(ReservationSummaryActivity.this, LoginActivity.class);
            startActivity(intent);
        }

        // Initialize variables
        destinationField = findViewById(R.id.destinationField);
        subStationField = findViewById(R.id.startingStationField);
        reservedDateField = findViewById(R.id.reservedDateField);
        passengerClassField = findViewById(R.id.passengerClassField);
        reservedTimeField = findViewById(R.id.reservedTimeField);
        trainNameField = findViewById(R.id.trainNameField);
        priceField = findViewById(R.id.priceField);
        passengerCountField = findViewById(R.id.passengerCountField);
        backBtn = findViewById(R.id.backBtn);
        confirmBtn = findViewById(R.id.confirmReservationBtn);
        checkBox = findViewById(R.id.reservationConfirmationCheckBox);
        confirmBtn.setEnabled(false);

        // Acquire the extras from the Intent
        Intent intent = getIntent();
        ViewSummaryRequest viewSummaryRequest = (ViewSummaryRequest) intent.getSerializableExtra("viewSummaryRequest");
        method = intent.getStringExtra("Method");

        // Assign the acquired values to the relevant variables
        if (viewSummaryRequest != null && method != null) {
            destinationField.setText(viewSummaryRequest.getDestinationStationName());
            subStationField.setText(viewSummaryRequest.getArrivalStationName());
            reservedDateField.setText(viewSummaryRequest.getReservedDate());
            passengerClassField.setText(String.valueOf(viewSummaryRequest.getPassengerClass()));
            reservedTimeField.setText(viewSummaryRequest.getReservedTime());
            trainNameField.setText(viewSummaryRequest.getTrainName());
            priceField.setText(String.valueOf(viewSummaryRequest.getPrice()));
            passengerCountField.setText(String.valueOf(viewSummaryRequest.getNoOfPassengers()));
            pClass = viewSummaryRequest.getPassengerClass();
            selectedDestinationId = viewSummaryRequest.getDestinationStationId();
            selectedTrainId = viewSummaryRequest.getTrainId();
            selectedSubStationId = viewSummaryRequest.getArrivalStationId();
            dateTime = viewSummaryRequest.getDateTime();
            passengerCount = viewSummaryRequest.getNoOfPassengers();
            ticketPrice = viewSummaryRequest.getPrice();
        }

        checkBox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                // Enable or disable the confirm button based on the checkbox state
                confirmBtn.setEnabled(isChecked);
            }
        });

        // Handle confirmation functionality
        confirmBtn.setOnClickListener(v ->{
            Log.d("TAG", method);
            if(method.equals("UPDATE")){
                ReservationRequest reservationRequest = new ReservationRequest(viewSummaryRequest.getId(), "", pClass, selectedDestinationId, selectedTrainId, selectedSubStationId, dateTime, passengerCount, ticketPrice);
                saveReservationDetails(reservationRequest);
            }else if (method.equals("CREATE") ){
                ReservationRequest reservationRequest = new ReservationRequest("", "", pClass, selectedDestinationId, selectedTrainId, selectedSubStationId, dateTime, passengerCount, ticketPrice);
                saveReservationDetails(reservationRequest);
            }else{
                Log.d("TAG", "Invalid Method");
            }
        });

        // Handle Back navigation
        backBtn.setOnClickListener(v ->{
            Intent intentU = new Intent(ReservationSummaryActivity.this, CurrentReservationsActivity.class);
            startActivity(intentU);
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
                    startActivity(new Intent(getApplicationContext(), CurrentReservationsActivity.class));
                    overridePendingTransition(0, 0);
                    return true;
                }else if (menuItem.getItemId() == R.id.history)
                    startActivity(new Intent(getApplicationContext(), ReservationHistoryActivity.class));
                overridePendingTransition(0, 0);
                return true;
            }
        });
//-------------------------------------------------------Bottom App BAR FUNCTION--------------------------------------------
    }

    /// <summary>
    /// Handles the Saving or the Updating of the confirmed details
    /// </summary>
    /// <param name="request">ReservationRequest Object containing the necessary data</param>
    /// <param name="authorization">Authorization token of the user</param>
    /// <returns>BasicReservationResponse Object</returns>
    private void saveReservationDetails(ReservationRequest reservationRequest) {
        Call<BasicReservationResponse> call = apiInterface.saveReservation("Bearer " + token, reservationRequest);
        call.enqueue(new Callback<BasicReservationResponse>() {
            @Override
            public void onResponse(Call<BasicReservationResponse> call, Response<BasicReservationResponse> response) {
                if (response.isSuccessful()) {
                    basicReservationResponse = response.body();
                    // Handle successful response
                    if(method.equals("UPDATE")){
                        if(basicReservationResponse.getErrors() != null){
                            Log.d("TAG", "Update Failed");
                            String []reservationError = basicReservationResponse.getErrors().toString().split(",");
                            String errorMessage = reservationError[reservationError.length - 1].trim();
                            Toast.makeText(ReservationSummaryActivity.this, errorMessage, Toast.LENGTH_SHORT).show();
                        }else{
                            Log.d("TAG", "Reservation Updated Successfully");
                            Toast.makeText(ReservationSummaryActivity.this, "Reservation Updated Successfully", Toast.LENGTH_SHORT).show();
                        }
                    }else if (method.equals("CREATE") ){
                        if(basicReservationResponse.getErrors() != null){
                            Log.d("TAG", "Create Failed");
                            Toast.makeText(ReservationSummaryActivity.this, basicReservationResponse.getErrors().toString(), Toast.LENGTH_SHORT).show();
                        }else{
                            Log.d("TAG", "Reservation Created Successfully");
                            Toast.makeText(ReservationSummaryActivity.this, "Reservation Created Successfully", Toast.LENGTH_SHORT).show();
                        }
                    }else{
                        Log.d("TAG", "Invalid Method");
                    }
                    Intent intent = new Intent(ReservationSummaryActivity.this, CurrentReservationsActivity.class);
                    startActivity(intent);
                } else {
                    // Handle unsuccessful response
                    Log.e("TAG", "Request Failed: " + response.toString());
                    Toast.makeText(ReservationSummaryActivity.this, "Request Failed!", Toast.LENGTH_SHORT).show();
                }
            }
            @Override
            public void onFailure(Call<BasicReservationResponse> call, Throwable t) {
                // Handle network errors or other failures
                Log.d("TAG", "Error: " + t.toString());
                Toast.makeText(ReservationSummaryActivity.this, "Network Error", Toast.LENGTH_SHORT).show();
            }
        });
    };
}