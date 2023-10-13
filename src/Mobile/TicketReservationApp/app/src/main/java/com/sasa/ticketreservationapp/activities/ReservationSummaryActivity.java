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

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ReservationSummaryActivity extends AppCompatActivity {
    private ApiInterface apiInterface;
    private SharedPreferences prefs;
    private String token, id;
    private EditText destinationField, subStationField, reservedDateField, passengerClassField, reservedTimeField, trainNameField, priceField, passengerCountField;
    private Button backBtn, confirmBtn;
    private String selectedDestinationId, selectedSubStationId, selectedTrainId, dateTime;
    private int pClass, passengerCount;
    private double ticketPrice;
    private CheckBox checkBox;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_reservation_summary);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
        }else{
            Intent intent = new Intent(ReservationSummaryActivity.this, LoginActivity.class);
            startActivity(intent);
        }
        if(prefs.getString("nic", "") != null){
            id = prefs.getString("nic", "");
            token = prefs.getString("token", "");
            Log.d("TAG", "Bearer" + token);
        }

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


        Intent intent = getIntent();
        ViewSummaryRequest viewSummaryRequest = (ViewSummaryRequest) intent.getSerializableExtra("viewSummaryRequest");

        if (viewSummaryRequest != null) {
            destinationField.setText(viewSummaryRequest.getDestinationStationName());
            subStationField.setText(viewSummaryRequest.getArrivalStationName());
            reservedDateField.setText(viewSummaryRequest.getReservedDate());
            passengerClassField.setText(String.valueOf(viewSummaryRequest.getPassengerClass()));
            reservedTimeField.setText(viewSummaryRequest.getReservedTime());
            trainNameField.setText(viewSummaryRequest.getTrainName());
            priceField.setText(String.valueOf(viewSummaryRequest.getPrice()));
            passengerCountField.setText(String.valueOf(viewSummaryRequest.getNoOfPassengers()));

//          Initialize Variables
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

        confirmBtn.setOnClickListener(v ->{
                ReservationRequest reservationRequest = new ReservationRequest("", "R001", pClass, selectedDestinationId, selectedTrainId, selectedSubStationId, dateTime, passengerCount, ticketPrice);
                saveReservationDetails(reservationRequest);
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
    private void saveReservationDetails(ReservationRequest reservationRequest) {
        Call<Void> call = apiInterface.saveReservation("Bearer " + token, reservationRequest);
        call.enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    // Handle successful response
                    Log.d("TAG", "Reservation saved successfully");
                    Toast.makeText(ReservationSummaryActivity.this, "Reservation Saved Successfully", Toast.LENGTH_SHORT).show();
                    // Assuming you have an instance of ReservationRequest named reservationRequest
                    Intent intent = new Intent(ReservationSummaryActivity.this, CurrentBookingsActivity.class);
                    startActivity(intent);
                } else {
                    // Handle unsuccessful response
                    Log.d("TAG", "Failed to save reservation: " + response.message());
                    Toast.makeText(ReservationSummaryActivity.this, "Failed to save reservation", Toast.LENGTH_SHORT).show();
                }
            }
            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                // Handle network errors or other failures
                Log.d("TAG", "Error: " + t.toString());
                Toast.makeText(ReservationSummaryActivity.this, "Network Error", Toast.LENGTH_SHORT).show();
            }
        });
    };
}