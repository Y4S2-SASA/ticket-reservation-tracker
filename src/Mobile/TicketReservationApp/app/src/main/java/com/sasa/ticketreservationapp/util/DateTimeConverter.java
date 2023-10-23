package com.sasa.ticketreservationapp.util;

import android.util.Log;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

/*
 * File: DateTimeConverter.java
 * Purpose: Acts as a util class for converting date time value formats
 */
public class DateTimeConverter {

    public static String convertDate(String inputDate) {
        SimpleDateFormat inputFormat = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
        SimpleDateFormat outputFormat = new SimpleDateFormat("yyyy-MM-dd", Locale.US);

        try {
            Date date = inputFormat.parse(inputDate);
            return outputFormat.format(date);
        } catch (ParseException e) {
            e.printStackTrace();
            return null; // Handle the exception according to your needs
        }
    }
    public static String convertTime(String inputTime) {
        SimpleDateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault());
        SimpleDateFormat outputFormat = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());

        try {
            Date date = inputFormat.parse(inputTime);
            if (date != null) {
                Log.d("TRY TIME", outputFormat.format(date));
                return outputFormat.format(date);
            } else {
                Log.d("DATE IS NULL", inputTime);
                return null;
            }
        } catch (ParseException e) {
            Log.d("ERROR", e.toString());
            Log.d("CATCH", inputTime);
            return null;
        }
    }

}
