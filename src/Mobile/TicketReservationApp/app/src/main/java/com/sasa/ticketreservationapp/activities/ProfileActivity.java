package com.sasa.ticketreservationapp.activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.MenuItem;
import android.widget.Button;
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

/*
 * File: ProfileActivity.java
 * Purpose: Handle profile activity management functionalities
 * Author: Jayathilake S.M.D.A.R/IT20037338
 * Description: This activity is responsible of handling the profile management functionalities of the traveller.
 */
public class ProfileActivity extends AppCompatActivity {

    private TextView displayName, emailField, mobileNoField, firstNameField, lastNameField, nicField;
    private ApiInterface apiInterface;
    private SharedPreferences prefs;
    private String id, token;
    private Button saveChangesBtn, deactivateAccountBtn, logoutButton;
    private UserModel userModel, currentUser;
    private Integer accStatus;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);

        apiInterface = ApiClient.getApiClient().create(ApiInterface.class); // Initialize apiClient
        if(getSharedPreferences("userCredentials", MODE_PRIVATE) != null){ // Verify the logged in user details
            prefs = getSharedPreferences("userCredentials", MODE_PRIVATE);
            if(prefs.getString("nic", "") != null){
                id = prefs.getString("nic", "");
                token = prefs.getString("token", "");
                fetchUserData(token, id);
            }
        }else{
            Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
            startActivity(intent);
        }

        // Initialize variables
        emailField = findViewById(R.id.emailField);
        mobileNoField = findViewById(R.id.mobileNoField);
        firstNameField = findViewById(R.id.firstNameField);
        lastNameField = findViewById(R.id.lastNameField);
        nicField = findViewById(R.id.nicField);
        displayName = findViewById(R.id.profileTitle);
        deactivateAccountBtn = findViewById(R.id.deactivateAccBtn);
        logoutButton = findViewById(R.id.logoutBtn);
        saveChangesBtn = findViewById(R.id.saveBtn);


        String fullName = "WELCOME " + prefs.getString("displayName", "") + " !";
        displayName.setText(fullName);

        // Handle save changes related functionality
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
                Log.d("TAG", "No Data");
            }
        });

        // Handle Logout
        logoutButton.setOnClickListener(v -> {
                AuthHandler.clearLoginData(new LoginDatabaseHelper(ProfileActivity.this), getSharedPreferences("userCredentials", MODE_PRIVATE).edit());
                Intent intent = new Intent(ProfileActivity.this, LoginActivity.class);
                startActivity(intent);
                finish();
        });

        // Handle deactivate user account
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

    /// <summary>
    /// Handles fetching the user data based on the parameters
    /// </summary>
    /// <param name="request">Passes the users id</param>
    /// <param name="authorization">Authorization token of the user</param>
    /// <returns>UserModel object</returns>
    private void fetchUserData(String token, String id){
        Call<UserModel> call = apiInterface.getUserById("Bearer " + token, id);
        call.enqueue(new Callback<UserModel>() {
            @Override
            public void onResponse(Call<UserModel> call, Response<UserModel> response) {
                if (response.isSuccessful() && response.body() != null) {
                    userModel = response.body();
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

    /// <summary>
    /// Handles updating the user details based on the parameters
    /// </summary>
    /// <param name="request">UserModel Object containing the necessary data</param>
    /// <param name="authorization">Authorization token of the user</param>
    /// <returns></returns>
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

    /// <summary>
    /// Handles updating of user account status from the travellers end
    /// </summary>
    /// <param name="request">Passes AccountStatusRequest to the parameters</param>
    /// <param name="authorization">Authorization token of the user</param>
    /// <returns></returns>
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
                Log.e("TAG", "Error: " + t.toString());
            }
        });
    }
}