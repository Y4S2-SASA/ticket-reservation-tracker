package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ProgressBar;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.DBHelper.LoginDatabaseHelper;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.adapters.ReservationsAdapter;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.models.ReservationModel;
import com.sasa.ticketreservationapp.response.ReservationResponse;
import java.util.ArrayList;
import java.util.List;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/*
 * File: CurrentReservationsActivity.java
 * Purpose: View Active Reservations of the User
 * Author: Dunusinghe A.V/IT20025526
 * Description: This activity is responsible listing out the active reservations relating to the logged in user.
 */
public class CurrentReservationsActivity extends AppCompatActivity {
    private ApiInterface apiInterface;
    SharedPreferences prefs;
    SwipeRefreshLayout swipeRefreshLayout;
    RecyclerView recyclerView;
    ReservationsAdapter adapter;
    boolean isLoading = false;
    Button createReservationBtn;
    private String id, token;
    private boolean isOverlayVisible = false;
    private List<ReservationResponse> reservations;
    private ProgressBar progressBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_current_reservations);

        //Call progressbar
        progressBar = findViewById(R.id.progressBar);
        progressBar.setVisibility(View.VISIBLE);
        LoginDatabaseHelper loginDb = new LoginDatabaseHelper(CurrentReservationsActivity.this);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class); // Initialze apiClient
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
            if(prefs.getString("nic", "") != null){
                id = prefs.getString("nic", "");
                token = prefs.getString("token", "");
            }
        }else{
            Intent intent = new Intent(CurrentReservationsActivity.this, LoginActivity.class);
            startActivity(intent);
        }
        loadData();

//      Sets up the recycler view
        swipeRefreshLayout = findViewById(R.id.swipereservation);
        recyclerView = findViewById(R.id.recyclerviewreservation);
        recyclerView.setHasFixedSize(true);
        LinearLayoutManager manager = new LinearLayoutManager(this);
        recyclerView.setLayoutManager(manager);
        adapter = new ReservationsAdapter(this, prefs, CurrentReservationsActivity.this);
        recyclerView.setAdapter(adapter);

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

        createReservationBtn = findViewById(R.id.createReservationBtn);
        //Handle Create Reservation Navigation
        createReservationBtn.setOnClickListener(v -> {
            Intent intent = new Intent(CurrentReservationsActivity.this, CreateReservationActivity.class);
            startActivity(intent);
            finish();
        });
    }

    // Toggles an overlay in the background
    public void toggleOverlay(boolean show) {
        if (show && !isOverlayVisible) {
            View overlayView = LayoutInflater.from(this).inflate(R.layout.overlay_layout, null);
            ((ViewGroup) getWindow().getDecorView().getRootView()).addView(overlayView);
            isOverlayVisible = true;
        } else if (!show && isOverlayVisible) {
            View overlayView = getWindow().getDecorView().findViewById(R.id.overlay);
            if (overlayView != null) {
                ((ViewGroup) overlayView.getParent()).removeView(overlayView);
                isOverlayVisible = false;
            }
        }
    }

    /// <summary>
    /// Handles fetching the all active reservations based relating to the user.
    /// </summary>
    /// <param name="request">2</param>
    /// <param name="authorization">Authorization token of the user</param>
    /// <returns>List of ReservationResponse Objects</returns>
    public void loadData() {
        Call<List<ReservationResponse>> call = apiInterface.getTraverlerReservation("Bearer " + token, 2);

        call.enqueue(new Callback<List<ReservationResponse>>() {
            @Override
            public void onResponse(Call<List<ReservationResponse>> call, Response<List<ReservationResponse>> response) {
                if (response.isSuccessful()) {
                    reservations = response.body();
                    // Process the reservations data and update the adapter
                    processReservations(reservations);
                    Log.d("TAG", response.body().toString());
                } else {
                    Log.e("TAG", response.toString());
                }
                isLoading = false;
                swipeRefreshLayout.setRefreshing(false);
                progressBar.setVisibility(View.GONE);
            }

            @Override
            public void onFailure(Call<List<ReservationResponse>> call, Throwable t) {
                // Handle failure
                isLoading = false;
                swipeRefreshLayout.setRefreshing(false);
                Log.e("TAG", t.toString());
                progressBar.setVisibility(View.GONE);
            }
        });
    }

    // Converts the fetched data to a Reservation Model object
    private ReservationModel convertToModel(ReservationResponse response) {
        ReservationModel model = new ReservationModel();
        // Set values from ReservationResponse to ReservationModel
        model.setId(response.getId());
        model.setReferenceNumber(response.getReferenceNumber());
        model.setPassengerClass(response.getPassengerClass());
        model.setDestinationStationName(response.getDestinationStationName());
        model.setTrainName(response.getTrainName());
        model.setArrivalStationName(response.getArrivalStationName());
        model.setDateTime(response.getDateTime());
        model.setNoOfPassengers(response.getNoOfPassengers());
        model.setPrice(response.getPrice());

        return model;
    }

    // Processes the Reservation Response data and adds the converted reservation model objects to a list
    private void processReservations(List<ReservationResponse> reservations) {
        ArrayList<ReservationModel> reservationModels = new ArrayList<>();
        for (ReservationResponse response : reservations) {
            // Convert ReservationResponse to ReservationModel and add to list
            ReservationModel model = convertToModel(response);
            reservationModels.add(model);
        }
        adapter.setItems(reservationModels);
        adapter.notifyDataSetChanged();
    }
}