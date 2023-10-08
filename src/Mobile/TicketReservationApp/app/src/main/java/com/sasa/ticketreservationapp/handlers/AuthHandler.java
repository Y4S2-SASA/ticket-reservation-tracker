package com.sasa.ticketreservationapp.handlers;

import static android.content.Context.MODE_PRIVATE;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

import com.sasa.ticketreservationapp.DBHelper.LoginDatabaseHelper;
import com.sasa.ticketreservationapp.activities.LoginActivity;

public class AuthHandler {

    public static void persistLoginData(LoginDatabaseHelper db, SharedPreferences.Editor editor, String nic, String token, String displayName) {

        //Store login data in shared preferences
        editor.putString("token", token);
        editor.putString("nic", nic);
        editor.putString("displayName", displayName);
        editor.apply();
        Log.d("LOGIN", "Stored login data in shared preference");

        //Store login data in SQL lite
        long rowId = db.insertLoginData(token, nic, displayName);
        Log.d("LOGIN", "Stored login data in sql lite");
    }

    public static void clearLoginData(LoginDatabaseHelper db, SharedPreferences.Editor editor) {

        // Remove data from SharedPreferences
        editor.remove("token");
        editor.remove("nic");
        editor.remove("displayName");
        editor.apply();
        Log.d("LOGIN", "Deleted login data from shared preference");

        db.clearTable();
        Log.d("LOGIN", "Deleted login data from sql lite");
    }
}
