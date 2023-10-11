package com.sasa.ticketreservationapp.activities;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.request.AvailableSeatRequest;
import com.sasa.ticketreservationapp.request.PriceRequest;
import com.sasa.ticketreservationapp.request.ReservationRequest;
import com.sasa.ticketreservationapp.request.ScheduleRequest;
import com.sasa.ticketreservationapp.response.ScheduleResponse;
import com.sasa.ticketreservationapp.response.StationResponse;
import com.sasa.ticketreservationapp.util.DateTimeConverter;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateReservationActivity extends AppCompatActivity {
    private Spinner pClassSpinner;
    private Spinner reservedTimeSpinner;
    private ApiInterface apiInterface;
    private Integer pClass;
    private String id,
            token,
            selectedDestinationId,
            selectedSubStationId,
            passengers,
            reservedTime,
            trainNo,
            price,
            reservedDate,
            convertedReservedDate,
            convertedTime,
            selectedTrainName,
            selectedScheduleId,
            selectedTrainId,
            selectedArrivalTime;
    private SharedPreferences prefs;
    private Integer availableSeatCount, newTicketPrice, passengerCount;
    private EditText reservedDateField, passengersField, reservedTimeField, trainNoField, priceField;
    private TextView incrementBtn, decrementBtn;
    private AutoCompleteTextView searchDestinationField;
    private AutoCompleteTextView searchSubStationField;
    private Calendar calendar;
    private List<StationResponse> stations;
    private List<ScheduleResponse> schedules;
    private Button viewSummaryBtn, getTrainsBtn;
    private Integer[] pClassTypes = {1, 2, 3};
    private String[] arrivalTime = {"No data available", "No data available"};
    private List<String> stationList = new ArrayList<>();
    private List<String> scheduleList = new ArrayList<>();
    private ScheduleRequest scheduleRequest;
    private Boolean scheduleStatus = false;
    SimpleDateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.getDefault());
    SimpleDateFormat outputFormat = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_reservation);

        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
        }else{
            Intent intent = new Intent(CreateReservationActivity.this, LoginActivity.class);
            startActivity(intent);
        }
        if(prefs.getString("nic", "") != null){
            id = prefs.getString("nic", "");
            token = prefs.getString("token", "");
            fetchStations();
            Log.d("TAG", "Bearer" + token);
        }

        priceField = findViewById(R.id.priceField);
        trainNoField = findViewById(R.id.trainNoField);
        passengersField = findViewById(R.id.passengerCount);
        pClassSpinner = findViewById(R.id.pClassSpinner);
        reservedTimeSpinner = findViewById(R.id.reservedTimeSpinner);
        searchDestinationField = findViewById(R.id.searchDestinationField);
        searchSubStationField = findViewById(R.id.searchSubStationField);
        reservedDateField = findViewById(R.id.reservedDateField);
        viewSummaryBtn = findViewById(R.id.viewSummaryBtn);
        getTrainsBtn = findViewById(R.id.fetchTrainsBtn);
        decrementBtn = findViewById(R.id.decrementBtn);
        incrementBtn = findViewById(R.id.incrementBtn);

        // Create an ArrayAdapter using the stationList ArrayList
        ArrayAdapter<String> destinationAdapter = new ArrayAdapter<>(this, android.R.layout.simple_dropdown_item_1line, stationList);
        // Set the adapter to the AutoCompleteTextView
        searchDestinationField.setAdapter(destinationAdapter);
        searchDestinationField.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String selectedDestinationName = (String) parent.getItemAtPosition(position);
                Log.d("TAG", "Selected Destination: " + selectedDestinationName);

                for (StationResponse destination : stations) {
                    if (destination.getName().equals(selectedDestinationName)) {
                        selectedDestinationId = destination.getId();
                        Log.d("TAG", "Selected Destination ID: " + selectedDestinationId);
                        break;
                    }
                }
            }
        });

        ArrayAdapter<String> subStationAdapter = new ArrayAdapter<>(this, android.R.layout.simple_dropdown_item_1line, stationList);
        searchSubStationField.setAdapter(subStationAdapter);
        searchSubStationField.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String selectedSubStationName = (String) parent.getItemAtPosition(position);
                Log.d("TAG", "Selected Start Point: " + selectedSubStationName);

                for (StationResponse subStation : stations) {
                    if (subStation.getName().equals(selectedSubStationName)) {
                        selectedSubStationId = subStation.getId();
                        Log.d("TAG", "Selected Destination ID: " + selectedSubStationId);
                        break;
                    }
                }
            }
        });

        incrementBtn.setOnClickListener(v -> {
            String currentValue = passengersField.getText().toString();
            passengerCount = Integer.parseInt(currentValue) + 1;

            // Check if newValue exceeds the maximum value
            if(availableSeatCount != null){
                if (passengerCount <= availableSeatCount) {
                    passengersField.setText(String.valueOf(passengerCount));
                    PriceRequest priceRequest = new PriceRequest(selectedTrainId, selectedSubStationId, selectedDestinationId, selectedScheduleId, passengerCount, pClass);
                    calculateTicketPrice(priceRequest);
                } else {
                    // Display a message or take any other action to indicate the limit has been reached
                    Toast.makeText(CreateReservationActivity.this, "Maximum passenger count reached", Toast.LENGTH_SHORT).show();
                }
            }
        });

        decrementBtn.setOnClickListener(v -> {
                String currentValue = passengersField.getText().toString();
                passengerCount = Math.max(0, Integer.parseInt(currentValue) - 1);
            if(availableSeatCount != null) {
                passengersField.setText(String.valueOf(passengerCount));
                PriceRequest priceRequest = new PriceRequest(selectedTrainId, selectedSubStationId, selectedDestinationId, selectedScheduleId, passengerCount, pClass);
                calculateTicketPrice(priceRequest);
            }else{
                Toast.makeText(CreateReservationActivity.this, "Maximum passenger count reached", Toast.LENGTH_SHORT).show();
            }
        });

        getTrainsBtn.setOnClickListener( v -> {
            reservedDate = reservedDateField.getText().toString();
            convertedReservedDate = DateTimeConverter.convertDate(reservedDate);
            pClass = (Integer) pClassSpinner.getSelectedItem();
            Log.d("TAG", selectedDestinationId );
            Log.d("TAG", selectedSubStationId);
            Log.d("TAG", reservedDate);

            scheduleRequest = new ScheduleRequest(selectedDestinationId, selectedSubStationId, convertedReservedDate, pClass);
            fetchTrains(scheduleRequest);
        });

        ArrayAdapter<Integer> pClassAdapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, pClassTypes);
        pClassAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        pClassSpinner.setAdapter(pClassAdapter);

        reservedTimeSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                selectedArrivalTime = (String) parent.getItemAtPosition(position);
                if(schedules.size() != 0){
                    // Iterate through the schedules to find the corresponding schedule
                    for (ScheduleResponse schedule : schedules) {
                        String convertedArrival = DateTimeConverter.convertTime(schedule.getArrivalTime());
                        if (convertedArrival.equals(selectedArrivalTime)) {
                            selectedScheduleId = schedule.getScheduleId();
                            selectedTrainName = schedule.getTrainName();
                            selectedTrainId = schedule.getTrainId();

                            trainNoField.setText(selectedTrainName.toString());
                            AvailableSeatRequest availableSeatRequest = new AvailableSeatRequest(selectedTrainId, selectedDestinationId, selectedSubStationId, convertedReservedDate);
                            fetchAvailableSeatCount(availableSeatRequest);
                            if(availableSeatCount != null){
                                Log.d("TAG", "Available Seats" + availableSeatCount);
                                Log.d("TAG", "Selected Schedule ID: " + selectedScheduleId);
                                Log.d("TAG", "Selected Train Name: " + selectedTrainName);
                                Log.d("TAG", "Selected Train ID: " + selectedTrainId);
                                break; // No need to continue searching
                            }else{
                                Log.d("TAG", "No count" );
                            }
                        }
                    }
                }else{
                    Log.d("TAG", "Empty");
                }
            }
            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                Log.d("TAG", "Nothing selected");
            }
        });

        calendar = Calendar.getInstance();
        reservedDateField.setOnClickListener(view -> showDatePickerDialog());

        viewSummaryBtn.setOnClickListener(v ->{
            double ticketPrice = (double) newTicketPrice;
            String dateTime = convertedReservedDate + "T" + selectedArrivalTime;
            Log.d("TAG", dateTime);
            ReservationRequest reservationRequest = new ReservationRequest("", "R001", pClass, selectedDestinationId, selectedTrainId, selectedSubStationId, dateTime, passengerCount, ticketPrice);
            setReservationDetails(reservationRequest);
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

    private void fetchStations() {

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        Call<List<StationResponse>> call = apiInterface.getStations("Bearer " + token);

        call.enqueue(new Callback<List<StationResponse>>() {
            @Override
            public void onResponse(Call<List<StationResponse>> call, Response<List<StationResponse>> response) {
                if (response.isSuccessful()) {
                    stations = response.body();
                    Log.d("TAG", "Number of stations: " + stations.size());
                    for (StationResponse station : stations) {
                        stationList.add(station.getName());
                    }
                } else {
                    Log.d("TAG", response.toString());
                    Log.d("TAG", token.toString());
                    Log.d("TAG", "Bearer" + token);
                }
            }

            @Override
            public void onFailure(Call<List<StationResponse>> call, Throwable t) {
                Log.d("TAG", t.toString());
            }
        });
    };

    private void fetchTrains(ScheduleRequest scheduleRequest) {

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        Call<List<ScheduleResponse>> call = apiInterface.getScheduleTrainsData("Bearer " + token, scheduleRequest);

        call.enqueue(new Callback<List<ScheduleResponse>>() {
            @Override
            public void onResponse(Call<List<ScheduleResponse>> call, Response<List<ScheduleResponse>> response) {

                if (response.isSuccessful()) {

                    schedules = response.body();
                    if(schedules.size() != 0){
                        // Populate the dropdown menu with arrival times.
                        // Assuming scheduleList is your list of arrival times
                        for (ScheduleResponse schedule : schedules) {
                           convertedTime = DateTimeConverter.convertTime(schedule.getArrivalTime());
                           scheduleList.add(convertedTime.toString());
                        }
                        scheduleStatus = true;
                        ArrayAdapter<String> reservedTimeAdapter = new ArrayAdapter<>(CreateReservationActivity.this, android.R.layout.simple_spinner_item, scheduleList);
                        reservedTimeAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                        reservedTimeSpinner.setAdapter(reservedTimeAdapter);
                    }else{
                        Log.d("TAG", "No Data");
                    }
                } else {
                    Log.d("TAG", response.toString());
                }
            }

            @Override
            public void onFailure(Call<List<ScheduleResponse>> call, Throwable t) {
                Log.d("TAG", t.toString());
            }
        });
    };

    private void fetchAvailableSeatCount(AvailableSeatRequest availableSeatRequest) {

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        Call<Integer> call = apiInterface.getAvailableTrainSeatCount("Bearer " + token, availableSeatRequest);

        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                if (response.isSuccessful()) {
                    availableSeatCount = response.body();
                    Log.d("TAG", "Seats " + availableSeatCount);
                } else {
                    Log.d("TAG", response.toString());
                    Log.d("TAG", token.toString());
                    Log.d("TAG", "Bearer" + token);
                }
            }
            @Override
            public void onFailure(Call<Integer> call, Throwable t) {
                Log.d("TAG", t.toString());
            }
        });
    };

    private void calculateTicketPrice(PriceRequest priceRequest) {

        Call<Integer> call = apiInterface.getSchedulePrice("Bearer " + token, priceRequest);

        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                if (response.isSuccessful() && response.body() != null) {
                    newTicketPrice = response.body();
                    if(newTicketPrice != null){
                        Log.d("TAG", newTicketPrice.toString());
                        priceField.setText(newTicketPrice.toString());
                    }else{
                        Log.d("TAG", "failed ");
                    }
                    Log.d("TAG", "Response successful: ");
                } else {
                    Log.d("TAG", "Response not successful: " + response.message());
                }
            }

            @Override
            public void onFailure(Call<Integer> call, Throwable t) {
                Log.d("TAG", "Error: " + t.toString());
            }
        });
    };

    private void setReservationDetails(ReservationRequest reservationRequest) {

        Call<Void> call = apiInterface.saveReservation("Bearer " + token, reservationRequest);

        call.enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    // Handle successful response
                    Log.d("TAG", "Reservation saved successfully");
                    Toast.makeText(CreateReservationActivity.this, "Reservation Saved Successfully", Toast.LENGTH_SHORT).show();
                    // Assuming you have an instance of ReservationRequest named reservationRequest
                    Intent intent = new Intent(CreateReservationActivity.this, ReservationSummaryActivity.class);
                    intent.putExtra("reservationRequest", reservationRequest);
                    startActivity(intent);
                } else {
                    // Handle unsuccessful response
                    Log.d("TAG", "Failed to save reservation: " + response.message());
                    Toast.makeText(CreateReservationActivity.this, "Failed to save reservation", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                // Handle network errors or other failures
                Log.d("TAG", "Error: " + t.toString());
                Toast.makeText(CreateReservationActivity.this, "Network Error", Toast.LENGTH_SHORT).show();
            }
        });

    };
};