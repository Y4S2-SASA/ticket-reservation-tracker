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
import com.sasa.ticketreservationapp.request.ScheduleRequest;
import com.sasa.ticketreservationapp.request.ViewSummaryRequest;
import com.sasa.ticketreservationapp.response.ScheduleResponse;
import com.sasa.ticketreservationapp.response.StationResponse;
import com.sasa.ticketreservationapp.util.DateTimeConverter;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
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
    private String id, token, selectedDestinationId, selectedSubStationId, reservedDate, convertedReservedDate, convertedTime, selectedTrainName, selectedScheduleId, selectedTrainId, selectedArrivalTime, selectedDestinationName, selectedSubStationName;
    private SharedPreferences prefs;
    private Integer availableSeatCount, newTicketPrice, passengerCount;
    private EditText reservedDateField, passengersField, trainNoField, priceField;
    private TextView incrementBtn, decrementBtn;
    private AutoCompleteTextView searchDestinationField;
    private AutoCompleteTextView searchSubStationField;
    private Calendar calendar;
    private List<StationResponse> stations;
    private List<ScheduleResponse> schedules;
    private Button viewSummaryBtn, getTrainsBtn;
    private Integer[] pClassTypes = {1, 2, 3};
    private String[] arrivalTime = {"No data available"};
    private List<String> stationList = new ArrayList<>();
    private List<String> scheduleList = new ArrayList<>();
    private ScheduleRequest scheduleRequest;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_reservation);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
            if(prefs.getString("nic", "") != null){
                id = prefs.getString("nic", "");
                token = prefs.getString("token", "");
                fetchStations();
            }
        }else{
            Intent intent = new Intent(CreateReservationActivity.this, LoginActivity.class);
            startActivity(intent);
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

        // ArrayAdapter using the stationList ArrayList
        ArrayAdapter<String> destinationAdapter = new ArrayAdapter<>(this, android.R.layout.simple_dropdown_item_1line, stationList);
        // Set the adapter to the AutoCompleteTextView
        searchDestinationField.setAdapter(destinationAdapter);
        searchDestinationField.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                selectedDestinationName = (String) parent.getItemAtPosition(position);
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
                selectedSubStationName = (String) parent.getItemAtPosition(position);
                for (StationResponse subStation : stations) {
                    if (subStation.getName().equals(selectedSubStationName)) {
                        selectedSubStationId = subStation.getId();
                        Log.d("TAG", "Selected Destination ID: " + selectedSubStationId);
                        break;
                    }
                }
            }
        });

        calendar = Calendar.getInstance();
        reservedDateField.setOnClickListener(view -> showDatePickerDialog());
        reservedDateField.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View view, boolean hasFocus) {
                if (hasFocus) {
                    showDatePickerDialog();
                }
            }
        });

        ArrayAdapter<Integer> pClassAdapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, pClassTypes);
        pClassAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        pClassSpinner.setAdapter(pClassAdapter);

        getTrainsBtn.setOnClickListener( v -> {
            reservedDate = reservedDateField.getText().toString();
            convertedReservedDate = DateTimeConverter.convertDate(reservedDate);
            pClass = (Integer) pClassSpinner.getSelectedItem();
            scheduleRequest = new ScheduleRequest(selectedDestinationId, selectedSubStationId, convertedReservedDate, pClass);
            fetchTrains(scheduleRequest);
        });

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
                                break;
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

        viewSummaryBtn.setOnClickListener(v ->{
            double ticketPrice = (double) newTicketPrice;
            String dateTime = convertedReservedDate + "T" + selectedArrivalTime;
            ViewSummaryRequest viewSummaryRequest = new ViewSummaryRequest("", "", pClass, selectedDestinationId, selectedDestinationName, selectedTrainId, selectedTrainName, selectedSubStationId, selectedSubStationName, dateTime, convertedReservedDate, convertedTime, passengerCount, ticketPrice);
            Intent intent = new Intent(CreateReservationActivity.this, ReservationSummaryActivity.class);
            intent.putExtra("viewSummaryRequest", viewSummaryRequest);
            startActivity(intent);
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
        String myFormat = "MM/dd/yyyy";
        SimpleDateFormat sdf = new SimpleDateFormat(myFormat, Locale.US);
        reservedDateField.setText(sdf.format(calendar.getTime()));
    }

    private void fetchStations() {
        Call<List<StationResponse>> call = apiInterface.getStations("Bearer " + token);
        call.enqueue(new Callback<List<StationResponse>>() {
            @Override
            public void onResponse(Call<List<StationResponse>> call, Response<List<StationResponse>> response) {
                if (response.isSuccessful()) {
                    stations = response.body();
                    for (StationResponse station : stations) {
                        stationList.add(station.getName());
                    }
                } else {
                    Log.d("TAG", response.toString());
                }
            }
            @Override
            public void onFailure(Call<List<StationResponse>> call, Throwable t) {
                Log.d("TAG", t.toString());
            }
        });
    };

    private void fetchTrains(ScheduleRequest scheduleRequest) {
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

        Call<Integer> call = apiInterface.getAvailableTrainSeatCount("Bearer " + token, availableSeatRequest);
        call.enqueue(new Callback<Integer>() {
            @Override
            public void onResponse(Call<Integer> call, Response<Integer> response) {
                if (response.isSuccessful()) {
                    availableSeatCount = response.body();
                } else {
                    Log.d("TAG", response.toString());
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
                        priceField.setText(newTicketPrice.toString());
                    }else{
                        Log.e("TAG", "Calculation Failed ");
                    }
                } else {
                    Log.d("TAG", "Response not successful: " + response.message());
                }
            }
            @Override
            public void onFailure(Call<Integer> call, Throwable t) {
                Log.e("TAG", "Error: " + t.toString());
            }
        });
    };
};