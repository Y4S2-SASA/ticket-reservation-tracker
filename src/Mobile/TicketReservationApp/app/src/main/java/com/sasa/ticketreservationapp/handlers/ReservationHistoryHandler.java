package com.sasa.ticketreservationapp.handlers;

import android.view.View;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.sasa.ticketreservationapp.R;

/*
 * File: ReservationHistoryHandler.java
 * Purpose: Handles the initialization of the reservationItem layout for the population of reservationHistoryObjects
 * Author: Perera M. S. D/IT20020262
 * Description: This activity is responsible of initializing the reservation item layout for populating the reservation history objects fetched
 */
public class ReservationHistoryHandler extends RecyclerView.ViewHolder{
    public TextView tv_reqPrefix, tv_destination, tv_reservedTime, tv_departure, txt_option;

    public ReservationHistoryHandler(@NonNull View itemView) {
        super(itemView);

        //Initialize Variables
        tv_reqPrefix = itemView.findViewById(R.id.tv_reqPrefix);
        tv_destination = itemView.findViewById(R.id.tv_destination);
        tv_reservedTime = itemView.findViewById(R.id.tv_reservedTime);
        tv_departure = itemView.findViewById(R.id.tv_departure);
        txt_option = itemView.findViewById(R.id.txt_option);
    }
}
