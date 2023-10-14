package com.sasa.ticketreservationapp.util;

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
        SimpleDateFormat inputFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", Locale.getDefault());
        SimpleDateFormat outputFormat = new SimpleDateFormat("HH:mm:ss", Locale.getDefault());

        try {
            Date date = inputFormat.parse(inputTime);
           return outputFormat.format(date);
        } catch (ParseException e) {
            e.printStackTrace();
            return null;
        }
    }

}
