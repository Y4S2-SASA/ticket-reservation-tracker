package com.sasa.ticketreservationapp.adapters;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.PopupMenu;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.sasa.ticketreservationapp.activities.CurrentReservationsActivity;
import com.sasa.ticketreservationapp.activities.EditReservationActivity;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.handlers.ReservationsHandler;
import com.sasa.ticketreservationapp.models.ReservationModel;
import com.sasa.ticketreservationapp.request.StatusChangeRequest;
import com.sasa.ticketreservationapp.response.BasicReservationResponse;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/*
 * File: ReservationsAdapter.java
 * Purpose: Handles the population functionality of the fetched ReservationObjects to the reservation Item layout
 * Author: Dunusinghe A.V/IT20025526
 * Description: This activity is responsible of handling the reservation object population in the listview
 */
public class ReservationsAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
    private Context context;
    private SharedPreferences prefs;
    private ApiInterface apiInterface;
    private ArrayList<ReservationModel> list = new ArrayList<>();
    private String token;

    public ReservationsAdapter(Context ctx, SharedPreferences prefs, CurrentReservationsActivity currentReservationsActivity) {
        this.context = ctx;
        this.prefs = prefs;
    }

    public void setItems(ArrayList<ReservationModel> req){
        list.addAll(req);
    }

    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(context).inflate(R.layout.reservation_item, parent, false);
        return new ReservationsHandler(view);
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerView.ViewHolder holder, int position) {
        ReservationsHandler vhc = (ReservationsHandler) holder;
        ReservationModel req = list.get(position);
        String reservedTime = req.getDateTime();
        String destination = req.getDestinationStationName();
        String departure = req.getArrivalStationName();
        String prefix = (req.getDestinationStationName().substring(0, 2)).toUpperCase();
        vhc.tv_reservedTime.setText(reservedTime);
        vhc.tv_destination.setText(destination);
        vhc.tv_reqPrefix.setText(prefix);
        vhc.tv_departure.setText(departure);
        vhc.txt_option.setOnClickListener(v -> {
            PopupMenu popupMenu = new PopupMenu(context, vhc.txt_option);
            popupMenu.inflate(R.menu.options_menu);

            /// <summary>
            /// Handles navigation to the EditReservationActivity or updating the reservation status to canceled.
            /// </summary>
            /// <param name="request">StatusChangeRequest Object containing the necessary data</param>
            /// <param name="authorization">Authorization token of the user</param>
            /// <returns>A BasicReservationResponse Object</returns>
            popupMenu.setOnMenuItemClickListener(item -> {
                if (item.getItemId() == R.id.menu_update) {
                    Intent intentU = new Intent(context, EditReservationActivity.class);
                    intentU.putExtra("reservationModel", req);
                    intentU.putExtra("reservationId", req.getId());
                    context.startActivity(intentU);// Return true to indicate that the click has been handled
                }
                else if (item.getItemId() == R.id.menu_cancel) {
                    ((CurrentReservationsActivity) context).toggleOverlay(true); // Enable Overlay
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);
                    View dialogView = LayoutInflater.from(context).inflate(R.layout.cancel_reservation_dialog_layout, null);
                    builder.setView(dialogView);

                    Button positiveButton = dialogView.findViewById(R.id.deleteReservationBtn);
                    Button negativeButton = dialogView.findViewById(R.id.cancelBtn);

                    AlertDialog dialog = builder.create();

                    positiveButton.setOnClickListener(new View.OnClickListener() {
                        public void onClick(View v) {
                            // Handle positive button click
                            if(prefs.getString("nic", "") != null){
                                token = prefs.getString("token", "");
                            }
                            StatusChangeRequest request = new StatusChangeRequest(req.getId(), 3);
                            Log.d("REQ", request.getId());
                            Log.d("REQ", String.valueOf(request.getStatus()));

                            apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
                            Call<BasicReservationResponse> call = apiInterface.changeReservationStatus("Bearer " + token, request);
                            call.enqueue(new Callback<BasicReservationResponse>() {
                                @Override
                                public void onResponse(Call<BasicReservationResponse> call, Response<BasicReservationResponse> response) {
                                    if (response.isSuccessful()) {
                                        Log.d("CANCEL", "Reservation Cancelled Successfully!");
                                        Intent intent = new Intent(context, CurrentReservationsActivity.class);
                                        context.startActivity(intent);
                                        Toast.makeText(context, "Reservation Cancelled Successfully!", Toast.LENGTH_SHORT).show(); // Display Toast
                                    } else {
                                        Log.d("ERROR", response.toString());
                                    }
                                    dialog.dismiss();
                                }
                                @Override
                                public void onFailure(Call<BasicReservationResponse> call, Throwable t) {
                                    // Handle failure
                                    Log.d("ERROR", t.toString());
                                    dialog.dismiss();
                                }
                            });
                            ((CurrentReservationsActivity) context).toggleOverlay(false); // Disable Overlay

                            dialog.dismiss();
                        }
                    });
                    negativeButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            // Handle negative button click
                            ((CurrentReservationsActivity) context).toggleOverlay(false);
                            dialog.dismiss();
                        }
                    });
                    dialog.show();
                }
                return false; // Return false for unhandled cases
            });
            popupMenu.show();
        });
    }

    @Override
    public int getItemCount() {
        return list.size();
    }
}
