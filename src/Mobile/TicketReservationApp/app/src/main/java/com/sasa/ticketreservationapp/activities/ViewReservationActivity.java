package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.models.ReservationModel;
import com.sasa.ticketreservationapp.request.ScheduleRequest;
import com.sasa.ticketreservationapp.response.ReservationDetailsResponse;
import com.sasa.ticketreservationapp.response.ScheduleResponse;
import com.sasa.ticketreservationapp.response.StationResponse;
import com.sasa.ticketreservationapp.util.DateTimeConverter;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

public class ViewReservationActivity extends AppCompatActivity {
    private Spinner pClassSpinner;
    private Spinner reservedTimeSpinner;
    private ApiInterface apiInterface;
    private Integer pClass;
    private String id, token, selectedDestinationId, selectedSubStationId, reservedDate, convertedReservedDate, convertedResDate, convertedTime, selectedTrainName, selectedScheduleId, selectedTrainId, selectedArrivalTime, selectedDestinationName, selectedSubStationName, reservedTime, reservationId;
    private SharedPreferences prefs;
    private Integer availableSeatCount, newTicketPrice, passengerCount, currentValue = 0;
    private EditText reservedDateField, passengersField, trainNoField, priceField;
    private TextView incrementBtn, decrementBtn;
    private AutoCompleteTextView searchDestinationField;
    private AutoCompleteTextView searchSubStationField;
    private Calendar calendar;
    private List<StationResponse> stations;
    private List<ScheduleResponse> schedules;
    private Button viewSummaryBtn, getTrainsBtn, backBtn;
    private Integer[] pClassTypes = {1, 2, 3};
    private String[] arrivalTime;
    private List<String> stationList = new ArrayList<>();
    private List<String> scheduleList = new ArrayList<>();
    private ScheduleRequest scheduleRequest;

    private ReservationDetailsResponse reservationDetailsResponse;
    private boolean getTrainsStatus = false;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_view_reservation);

        Intent intent = getIntent();
        ReservationModel reservationModel = (ReservationModel) intent.getSerializableExtra("reservationModel");
        reservationId = intent.getStringExtra("reservationId");

        if (reservationModel != null && reservationId != null) {
//            fetchReservation(reservationId);
            String[] parts = reservationModel.getDateTime().split(" ");
            reservedDate = parts[0];
            reservedTime = parts[1] + " " + parts[2];
            Log.d("TAG", reservationModel.getDestinationStationName());
            searchDestinationField.setText(reservationModel.getDestinationStationName().toString());
//            searchSubStationField.setText(reservationModel.getArrivalStationName());
//            selectedDestinationName = reservationModel.getDestinationStationName();
//            selectedSubStationName = reservationModel.getArrivalStationName();
//            reservedDateField.setText(reservedDate);
//            convertedReservedDate = reservedDate;
//            convertedResDate = DateTimeConverter.convertDate(reservedDate);
//            selectedArrivalTime = reservedTime;
//            if(reservationModel.getPassengerClass().equals("First Class")){
//                pClass = 1;
//            }else if(reservationModel.getPassengerClass().equals("Second Class")){
//                pClass = 2;
//            }else if(reservationModel.getPassengerClass().equals("Third Class")){
//                pClass = 3;
//            }
//            arrivalTime = new String[]{reservedTime};
////            if(arrivalTime.length != 0){
////                ArrayAdapter<String> reservedTimeAdapter = new ArrayAdapter<>(EditReservationActivity.this, android.R.layout.simple_spinner_item, arrivalTime);
////                reservedTimeAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
////                reservedTimeSpinner.setAdapter(reservedTimeAdapter);
////            }else{
////                Log.e("TAG", "No Arrival Time");
////            }
//            trainNoField.setText(reservationModel.getTrainName());
//            selectedTrainName = reservationModel.getTrainName();
//            priceField.setText(String.valueOf(reservationModel.getPrice()));
//            double priceAsDouble = reservationModel.getPrice();
//            newTicketPrice = (int) priceAsDouble;
//            passengerCount = reservationModel.getNoOfPassengers();
//            passengersField.setText(String.valueOf(reservationModel.getNoOfPassengers()));

        }

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
}

// ((ReservationHistoryActivity) context).toggleOverlay(true);
//         AlertDialog.Builder builder = new AlertDialog.Builder(context);
//         View dialogView = LayoutInflater.from(context).inflate(R.layout.delete_reservation_dialog_layout, null);
//         builder.setView(dialogView);
//
//         Button positiveButton = dialogView.findViewById(R.id.deleteReservationBtn);
//         Button negativeButton = dialogView.findViewById(R.id.cancelBtn);
//
//         AlertDialog dialog = builder.create();
//
//         positiveButton.setOnClickListener(new View.OnClickListener() {
//@Override
//public void onClick(View v) {
//        // Handle positive button click
//        if(prefs.getString("nic", "") != null){
//        token = prefs.getString("token", "");
//        }
//        StatusChangeRequest request = new StatusChangeRequest(String.valueOf(req.getId()), 4);
//
//        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
//        Call<BasicReservationResponse> call = apiInterface.changeReservationStatus("Bearer " + token, request);
//        call.enqueue(new Callback<BasicReservationResponse>() {
//@Override
//public void onResponse(Call<BasicReservationResponse> call, Response<BasicReservationResponse> response) {
//        if (response.isSuccessful()) {
//        Log.d("DELETE", "Reservation Deleted Successfully!");
//        } else {
//        Log.d("ERROR", "Reservation Delete Failed!");
//        }
//        dialog.dismiss();
//        }
//@Override
//public void onFailure(Call<BasicReservationResponse> call, Throwable t) {
//        // Handle failure
//        Log.d("ERROR", t.toString());
//        dialog.dismiss();
//        }
//        });
//        ((ReservationHistoryActivity) context).toggleOverlay(false);
//
//        dialog.dismiss();
//        }
//        });
//
//        negativeButton.setOnClickListener(new View.OnClickListener() {
//@Override
//public void onClick(View v) {
//        // Handle negative button click
//        // For example, you can dismiss the dialog
//        ((ReservationHistoryActivity) context).toggleOverlay(false);
//        dialog.dismiss();
//        }
//        });
//        dialog.show();
//        }