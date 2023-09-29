package com.sasa.ticketreservationapp;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import android.util.Log;

public class RegisterActivity extends AppCompatActivity {
    private EditText firstNameField, lastNameField, nicField, emailField, mobileNumberField, regUsernameField, regPasswordField;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        firstNameField = findViewById(R.id.firstNameField);
        lastNameField = findViewById(R.id.lastNameField);
        nicField = findViewById(R.id.nicField);
        emailField = findViewById(R.id.emailField);
        mobileNumberField = findViewById(R.id.mobileNumberField);
        regUsernameField = findViewById(R.id.regUsernameField);
        regPasswordField = findViewById(R.id.regPasswordField);

        Button signUpBtn = findViewById(R.id.signUpBtn);

        signUpBtn.setOnClickListener(v -> {
            String name = firstNameField.getText().toString();
            String lastName = lastNameField.getText().toString();
            String nic = nicField.getText().toString();
            String email = emailField.getText().toString();
            String mobileNumber = mobileNumberField.getText().toString();
            String username = regUsernameField.getText().toString();
            String password = regPasswordField.getText().toString();
            Integer role = 3;

            UserModel userModel = new UserModel(name, lastName, nic, email, mobileNumber, username, password, role);

            ApiInterface api = ApiClient.getApiClient().create(ApiInterface.class);

            Call<Void> call = api.saveUser(userModel);
            call.enqueue(new Callback<Void>() {
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        Log.d("TAG", response.toString());
                        Toast.makeText(RegisterActivity.this, "Data saved successfully", Toast.LENGTH_SHORT).show();
                    } else {
                        System.out.println(userModel);
                        Log.d("TAG", response.toString());
                        Toast.makeText(RegisterActivity.this, "Error saving data", Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    Log.d("TAG", t.toString());
                    Toast.makeText(RegisterActivity.this, "Network error", Toast.LENGTH_SHORT).show();
                }
            });
        });
    }
}