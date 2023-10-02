package com.sasa.ticketreservationapp.activities;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.models.LoginModel;
import com.sasa.ticketreservationapp.models.UserModel;
import com.sasa.ticketreservationapp.response.LoginResponse;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {

    private EditText usernameField, passwordField;
    private Button signInBtn;

    private TextView signUpTxt;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        usernameField = findViewById(R.id.usernameField);
        passwordField = findViewById(R.id.passwordField);
        signInBtn = findViewById(R.id.signInBtn);
        signUpTxt = findViewById(R.id.signUpTxt);

        signUpTxt.setOnClickListener(v->{
            Intent intent = new Intent(LoginActivity.this, RegisterActivity.class);
            startActivity(intent);
            finish();
        });

        signInBtn.setOnClickListener(v -> {

            String username = usernameField.getText().toString();
            String password = passwordField.getText().toString();

            LoginModel loginModel = new LoginModel(username, password);

            ApiInterface apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
            Call<LoginResponse> call = apiInterface.loginUser(loginModel);

            call.enqueue(new Callback<LoginResponse>() {
                @Override
                public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                    if (response.isSuccessful()) {
                        LoginResponse loginResponse = response.body();
                        if(loginResponse.isLoginSuccess() == true){
                            SharedPreferences.Editor editor = getSharedPreferences("userCredentials", MODE_PRIVATE).edit();
                            String token = loginResponse.getToken().toString();
                            String nic = loginResponse.getUserId().toString();
                            String displayName = loginResponse.getDisplayName();
                            editor.putString("token", token);
                            editor.putString("nic", nic);
                            editor.putString("displayName", displayName);
                            editor.apply();
                            Toast.makeText(LoginActivity.this, "Login Successful", Toast.LENGTH_SHORT).show();
                            Intent intent = new Intent(LoginActivity.this, CurrentBookingsActivity.class);
                            startActivity(intent);
                            finish();
                        }else{
                            String message = loginResponse.getMessage().toString();
                            Toast.makeText(LoginActivity.this, message, Toast.LENGTH_SHORT).show();
                        }
                    } else {
                        Toast.makeText(LoginActivity.this, "Error Logging in", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<LoginResponse> call, Throwable t) {
                    Log.d("TAG", t.toString());
                    Toast.makeText(LoginActivity.this, "Network error", Toast.LENGTH_SHORT).show();
                }
            });
        });
    }
}