package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.swiperefreshlayout.widget.SwipeRefreshLayout;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.DBHelper.LoginDatabaseHelper;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.adapters.ReservationsAdapter;
import com.sasa.ticketreservationapp.models.ReservationModel;

import java.util.ArrayList;

public class CurrentBookingsActivity extends AppCompatActivity {

    SwipeRefreshLayout swipeRefreshLayout;
    RecyclerView recyclerView;
    ReservationsAdapter adapter;
    boolean isLoading = false;
    String key = null;

    Button createReservationBtn;

    SharedPreferences prefs;
    private boolean isOverlayVisible = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_current_bookings);
        LoginDatabaseHelper loginDb = new LoginDatabaseHelper(CurrentBookingsActivity.this);
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null && loginDb.IsUserLoginDataAvailableInSqlLite()){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
        }else{
            Intent intent = new Intent(CurrentBookingsActivity.this, LoginActivity.class);
            startActivity(intent);
        }
        System.out.print("ON CREATE CALLED");
//      Sets up the recycler view
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
    createReservationBtn = findViewById(R.id.createReservationBtn);


        createReservationBtn.setOnClickListener(v -> {
            Intent intent = new Intent(CurrentBookingsActivity.this, CreateReservationActivity.class);
            startActivity(intent);
            finish();
//                if(prefs.getString("nic", "") != null){
//                String nic = prefs.getString("nic", "");
//                Log.d("TAG", nic);
//            }
        });

    }
    //Loads the data to the recycler view
    private void loadData() {
        swipeRefreshLayout.setRefreshing(true);

        // Simulated data retrieval (replace this with your actual data retrieval logic)
        ArrayList<ReservationModel> reqs = new ArrayList<>();
        reqs.add(new ReservationModel("John Doe", "New York", "2", "T001", "12-11-2023, 9:00 AM", "Station A", "$50", "RES001"));
        reqs.add(new ReservationModel("Jane Smith", "Chicago", "1", "T001", "12-11-2023, 10:30 AM", "Station B", "$30", "RES002"));
        reqs.add(new ReservationModel("Bob Johnson", "Los Angeles", "3", "T001", "12-11-2023, 11:45 AM", "Station C", "$70", "RES003"));

        adapter.setItems(reqs);
        adapter.notifyDataSetChanged();
        isLoading = false;
        swipeRefreshLayout.setRefreshing(false);
    }

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
}
//    private void loadData() {
//        swipeRefreshLayout.setRefreshing(true);
//        dao.get(key).addValueEventListener(new ValueEventListener() {
//            @Override
//            public void onDataChange(@NonNull DataSnapshot snapshot) {
//                ArrayList<ReqModel> reqs = new ArrayList<>();
//                for (DataSnapshot data : snapshot.getChildren()) {
//                    ReqModel req = data.getValue(ReqModel.class);
//                    req.setKey(data.getKey());
//                    reqs.add(req);
//                    key = data.getKey();
//                }
//                adapter.setItems(reqs);
//                adapter.notifyDataSetChanged();
//                isLoading = false;
//                swipeRefreshLayout.setRefreshing(false);
//            }
//
//            @Override
//            public void onCancelled(@NonNull DatabaseError error) {
//                swipeRefreshLayout.setRefreshing(false);
//            }
//        });
//    }
//}