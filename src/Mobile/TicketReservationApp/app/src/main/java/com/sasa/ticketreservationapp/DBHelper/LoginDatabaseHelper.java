package com.sasa.ticketreservationapp.DBHelper;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

/*
 * File: LoginDatabaseHelper.java
 * Purpose: Handles the presist login details in SQLite database
 */
public class LoginDatabaseHelper extends SQLiteOpenHelper {
    private static final String DATABASE_NAME = "UserData.db";
    private static final int DATABASE_VERSION = 1;
    public static final String TABLE_USER = "user";
    public static final String COLUMN_ID = "id";
    public static final String COLUMN_TOKEN = "token";
    public static final String COLUMN_NIC = "nic";
    public static final String COLUMN_DISPLAY_NAME = "display_name";

    private static final String TABLE_CREATE =
           "CREATE TABLE IF NOT EXISTS " + TABLE_USER + " (" +
                    COLUMN_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    COLUMN_TOKEN + " TEXT, " +
                    COLUMN_NIC + " TEXT, " +
                    COLUMN_DISPLAY_NAME + " TEXT);";

    public LoginDatabaseHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    public long insertLoginData(String token, String nic, String displayName) {

        SQLiteDatabase db = this.getWritableDatabase();
        // Delete previous login credentials before inserting new one
        if (IsUserLoginDataAvailableInSqlLite()) {
            Log.d("LOGIN", "Deleting previous login data from sql lite");
            clearTable();
        }
        ContentValues values = new ContentValues();
        values.put(COLUMN_TOKEN, token);
        values.put(COLUMN_NIC, nic);
        values.put(COLUMN_DISPLAY_NAME, displayName);

        long newRowId = db.insert(TABLE_USER, null, values);
        // db.close();
        return newRowId;
    }

    public boolean clearTable() {
        SQLiteDatabase db = this.getWritableDatabase();
        int affectedRows = db.delete(TABLE_USER, null, null);
        if (affectedRows == -1) {
            return false;
        }
        return true;
    }
    public boolean IsUserLoginDataAvailableInSqlLite() {
        SQLiteDatabase db = this.getReadableDatabase();

        // Define the query to check if a user is logged in (you can adjust the condition as needed)
        String query = "SELECT COUNT(*) FROM " + TABLE_USER + " WHERE " + COLUMN_TOKEN + " IS NOT NULL";

        Cursor cursor = db.rawQuery(query, null);
        boolean isLoggedIn = false;

        if (cursor != null) {
            try {
                if (cursor.moveToFirst()) {
                    int count = cursor.getInt(0);
                    isLoggedIn = count > 0;
                    Log.d("LOGIN", String.valueOf(count));
                }
            } finally {
                cursor.close();
            }
        }
        return isLoggedIn;
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL(TABLE_CREATE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_USER);
        onCreate(db);
    }
}
