package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.EditText;
import android.widget.TextView;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.models.UserModel;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProfileActivity extends AppCompatActivity {

    private TextView displayName;
    private EditText emailField;
    private EditText mobileNoField;
    private EditText firstNameField;
    private EditText lastNameField;
    private EditText nicField;

    private ApiInterface apiInterface;
    SharedPreferences prefs;
    String id;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
        }else{
            Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
            startActivity(intent);
        }

        // Initialize your EditText fields
        emailField = findViewById(R.id.emailField);
        mobileNoField = findViewById(R.id.mobileNoField);
        firstNameField = findViewById(R.id.firstNameField);
        lastNameField = findViewById(R.id.lastNameField);
        nicField = findViewById(R.id.nicField);
        displayName = findViewById(R.id.profileTitle);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        String fullName = "WELCOME " + prefs.getString("displayName", "") + " !";
        displayName.setText(fullName);
        if(prefs.getString("nic", "") != null){
            id = prefs.getString("nic", "");
            Log.d("TAG", id);
        }

        // Make the API call to get user data
        Call<UserModel> call = apiInterface.getUserById(id);
        call.enqueue(new Callback<UserModel>() {
            @Override
            public void onResponse(Call<UserModel> call, Response<UserModel> response) {
                if (response.isSuccessful() && response.body() != null) {
                    UserModel userModel = response.body();
                    // Populate the EditText fields
                    emailField.setText(userModel.getEmail());
                    mobileNoField.setText(userModel.getMobileNumber());
                    firstNameField.setText(userModel.getFirstName());
                    lastNameField.setText(userModel.getLastName());
                    nicField.setText(userModel.getNic());
                }
            }

            @Override
            public void onFailure(Call<UserModel> call, Throwable t) {
                t.printStackTrace();
            }
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
}