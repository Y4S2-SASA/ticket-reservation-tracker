package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.sasa.ticketreservationapp.DBHelper.LoginDatabaseHelper;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.handlers.AuthHandler;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.request.AccountStatusRequest;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProfileActivity extends AppCompatActivity {

    private TextView displayName, emailField, mobileNoField, firstNameField, lastNameField, nicField;
    private ApiInterface apiInterface;
    private SharedPreferences prefs;
    private String id, token;
    private Button saveChangesBtn, deactivateAccountBtn, logoutButton;
    private UserModel userModel;
    private UserModel currentUser;
    private Integer accStatus;

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
        deactivateAccountBtn = findViewById(R.id.deactivateAccBtn);
        logoutButton = findViewById(R.id.logoutBtn);
        saveChangesBtn = findViewById(R.id.saveBtn);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
        String fullName = "WELCOME " + prefs.getString("displayName", "") + " !";
        displayName.setText(fullName);
        if(prefs.getString("nic", "") != null){
            id = prefs.getString("nic", "");
            token = prefs.getString("token", "");
            fetchUserData(token, id);
            Log.d("TAG", id);
        }

        saveChangesBtn.setOnClickListener(v -> {
            if(userModel != null){
                currentUser = userModel;
                currentUser.setEmail(emailField.getText().toString());
                currentUser.setMobileNumber(mobileNoField.getText().toString());
                currentUser.setFirstName(firstNameField.getText().toString());
                currentUser.setLastName(lastNameField.getText().toString());
                currentUser.setNic(userModel.getNic());
                currentUser.setRole(3);
                currentUser.setPassword("");
                currentUser.setUserName(userModel.getUserName());
                updateUserData(currentUser);
            }else{
                Log.d("TAG", "userModel.getEmail()");
            }
        });

        logoutButton.setOnClickListener(v -> {
                AuthHandler.clearLoginData(new LoginDatabaseHelper(ProfileActivity.this), getSharedPreferences("userCredentials", MODE_PRIVATE).edit());
                Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
                startActivity(intent);
                finish();
        });

        deactivateAccountBtn.setOnClickListener(v -> {
            updateAccountStatus(id, 3);
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
    private void fetchUserData(String token, String id){
        Call<UserModel> call = apiInterface.getUserById("Bearer " + token, id);
        call.enqueue(new Callback<UserModel>() {
            @Override
            public void onResponse(Call<UserModel> call, Response<UserModel> response) {
                if (response.isSuccessful() && response.body() != null) {
                    userModel = response.body();
                    Log.d("TAG", userModel.getUserName());
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
    }

    private void updateUserData(UserModel updatedUser) {
        apiInterface.updateUser("Bearer " + token, updatedUser).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    Toast.makeText(ProfileActivity.this, "Update Successful", Toast.LENGTH_SHORT).show();
                    fetchUserData(token, id);
                    Log.d("TAG", "Save Successful");
                } else {
                    Log.d("TAG", response.toString());
                }
            }
            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                t.printStackTrace();
                Log.d("TAG", t.toString());
            }
        });
    }

    private void updateAccountStatus(String nic, int status) {
        ApiInterface apiInterface = ApiClient.getApiClient().create(ApiInterface.class);

        AccountStatusRequest accountStatusRequest = new AccountStatusRequest(nic, status);

        Call<Void> call = apiInterface.updateUserStatus("Bearer " + token, accountStatusRequest);

        call.enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                if (response.isSuccessful()) {
                    Log.d("TAG", "Worked");
                    Toast.makeText(ProfileActivity.this, "Account Deactivated Successfully!", Toast.LENGTH_SHORT);
                }else{
                    Log.d("TAG", response.toString());
                }
            }
            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                t.printStackTrace();
                Log.d("TAG", t.toString());
            }
        });
    }

}