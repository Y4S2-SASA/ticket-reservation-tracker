package com.sasa.ticketreservationapp.handlers;

import android.view.View;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.sasa.ticketreservationapp.R;

public class ReservationsHandler extends RecyclerView.ViewHolder{
    public TextView tv_reqPrefix, tv_destination, tv_reservedTime, txt_option;

    public ReservationsHandler(@NonNull View itemView) {
        super(itemView);

        //Initialize Variables
        tv_reqPrefix = itemView.findViewById(R.id.tv_reqPrefix);
        tv_destination = itemView.findViewById(R.id.tv_destination);
        tv_reservedTime = itemView.findViewById(R.id.tv_reservedTime);
        txt_option = itemView.findViewById(R.id.txt_option);
    }
}

