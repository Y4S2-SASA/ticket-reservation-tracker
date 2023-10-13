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
import android.widget.ProgressBar;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.DBHelper.LoginDatabaseHelper;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.adapters.ReservationsAdapter;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.handlers.BookingHistoryHandler;
import com.sasa.ticketreservationapp.models.ReservationModel;
import com.sasa.ticketreservationapp.response.ReservationResponse;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class BookingHistoryActivity extends AppCompatActivity {

    SwipeRefreshLayout swipeRefreshLayout;
    RecyclerView recyclerView;
    ReservationsAdapter adapter;
    boolean isLoading = false;
    private String id, token;
    private ApiInterface apiInterface;
    SharedPreferences prefs;
    private boolean isOverlayVisible = false;
    private List<ReservationResponse> reservations;
    private ProgressBar progressBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_booking_history);

        progressBar = findViewById(R.id.progressBar);
        progressBar.setVisibility(View.VISIBLE);
        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
            if(prefs.getString("nic", "") != null){
                id = prefs.getString("nic", "");
                token = prefs.getString("token", "");
            }
        }else{
            Intent intent = new Intent(BookingHistoryActivity.this, LoginActivity.class);
            startActivity(intent);
        }
        loadData();
        //Sets up the recycler view
        swipeRefreshLayout = findViewById(R.id.swipereservation);
        recyclerView = findViewById(R.id.recyclerviewreservation);
        recyclerView.setHasFixedSize(true);
        LinearLayoutManager manager = new LinearLayoutManager(this);
        recyclerView.setLayoutManager(manager);
        adapter = new ReservationsAdapter(this);
        recyclerView.setAdapter(adapter);
        loadData();

        recyclerView.addOnScrollListener((new RecyclerView.OnScrollListener() {
            @Override
            public void onScrolled(@NonNull RecyclerView recyclerView, int dx, int dy) {
                LinearLayoutManager linearLayoutManager = (LinearLayoutManager) recyclerView.getLayoutManager();
                int totalItem = linearLayoutManager.getItemCount();
                int lastVisible = linearLayoutManager.findLastCompletelyVisibleItemPosition();
                if (totalItem < lastVisible + 3) {
                    if (!isLoading) {
                        isLoading = true;
                        loadData();
                    }
                }
            }
        }));

//-------------------------------------------------------Bottom App BAR FUNCTION---------------------------------------------
        //Initialize variables and assign them
        BottomNavigationView bottomNavigationView = findViewById(R.id.bottom_navigation);

        //Perform Item Selected Event Listener
        bottomNavigationView.setOnItemSelectedListener(new BottomNavigationView.OnItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem menuItem) {
                if (menuItem.getItemId() == R.id.profile) {
                    startActivity(new Intent(getApplicationContext(), CreateReservationActivity.class));
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
//-------------------------------------------------------Bottom App BAR FUNCTION---------------------------------------------
    }

    //Loads the data to the recycler view

    void toggleOverlay(boolean show) {
        if (show && !isOverlayVisible) {
            View overlayView = LayoutInflater.from(this).inflate(R.layout.overlay_layout, null);
            ((ViewGroup) getWindow().getDecorView().getRootView()).addView(overlayView);
            isOverlayVisible = true;
        } else if (!show && isOverlayVisible) {
            View overlayView = getWindow().getDecorView().findViewById(R.id.overlay); // Assuming you have a view with id overlay in overlay_layout.xml
            if (overlayView != null) {
                ((ViewGroup) overlayView.getParent()).removeView(overlayView);
                isOverlayVisible = false;
            }
        }
    }
    private void loadData() {
        Call<List<ReservationResponse>> call = apiInterface.getTraverlerReservation("Bearer " + token);
        call.enqueue(new Callback<List<ReservationResponse>>() {
            @Override
            public void onResponse(Call<List<ReservationResponse>> call, Response<List<ReservationResponse>> response) {
                if (response.isSuccessful()) {
                    reservations = response.body();
                    // Process the reservations data and update the adapter
                    processReservations(reservations);
                    Log.d("TAG", response.body().toString());
                } else {
                    Log.e("Load Data", response.toString());
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
                Log.e("L", t.toString());
                progressBar.setVisibility(View.GONE);
            }
        });
    }

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
        Log.d("TAG", response.getArrivalStationName());

        return model;
    }

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