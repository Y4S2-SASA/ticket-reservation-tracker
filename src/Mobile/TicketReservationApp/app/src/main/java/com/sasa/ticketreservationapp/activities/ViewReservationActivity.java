package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.MenuItem;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;

public class ViewReservationActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_view_reservation);


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
}