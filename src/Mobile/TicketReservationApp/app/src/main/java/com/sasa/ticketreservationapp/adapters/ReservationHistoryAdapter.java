package com.sasa.ticketreservationapp.adapters;

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
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.activities.ReservationHistoryActivity;
import com.sasa.ticketreservationapp.config.ApiClient;
import com.sasa.ticketreservationapp.config.ApiInterface;
import com.sasa.ticketreservationapp.handlers.ReservationHistoryHandler;
import com.sasa.ticketreservationapp.models.ReservationModel;
import com.sasa.ticketreservationapp.request.StatusChangeRequest;
import com.sasa.ticketreservationapp.response.BasicReservationResponse;
import java.util.ArrayList;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/*
 * File: ReservationHistoryAdapter.java
 * Purpose: Handles the population functionality of the fetched ReservationHistoryObjects to the reservation Item layout
 * Author: Perera M. S. D/IT20020262
 * Description: This activity is responsible of handling the reservationHistory object population in the listview
 */
public class ReservationHistoryAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
    //Initializing variables
    private Context context;
    private ArrayList<ReservationModel> list = new ArrayList<>();
    private ApiInterface apiInterface;
    private SharedPreferences prefs;
    private String token;

    // Constructor accepting Context and SharedPreferences
    public ReservationHistoryAdapter(Context ctx, SharedPreferences prefs) {
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
        return new ReservationHistoryHandler(view);
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerView.ViewHolder holder, int position) {
        ReservationHistoryHandler vhc = (ReservationHistoryHandler) holder;
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
            popupMenu.inflate(R.menu.more_options_menu);

            /// <summary>
            /// Handles updating the status of the reservation to deleted
            /// </summary>
            /// <param name="request">StatusChangeRequest Object containing the necessary data</param>
            /// <param name="authorization">Authorization token of the user</param>
            /// <returns>A BasicReservationResponse Object</returns>
            popupMenu.setOnMenuItemClickListener(item -> {
                if (item.getItemId() == R.id.menu_delete) {
                    ((ReservationHistoryActivity) context).toggleOverlay(true); // Enable overlay
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);
                    View dialogView = LayoutInflater.from(context).inflate(R.layout.delete_reservation_dialog_layout, null);
                    builder.setView(dialogView);

                    Button positiveButton = dialogView.findViewById(R.id.deleteReservationBtn);
                    Button negativeButton = dialogView.findViewById(R.id.cancelBtn);

                    AlertDialog dialog = builder.create();

                    positiveButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            // Handle positive button click
                            if(prefs.getString("nic", "") != null){
                                token = prefs.getString("token", "");
                            }
                            StatusChangeRequest request = new StatusChangeRequest(String.valueOf(req.getId()), 4);

                            apiInterface = ApiClient.getApiClient().create(ApiInterface.class);
                            Call<BasicReservationResponse> call = apiInterface.changeReservationStatus("Bearer " + token, request);
                            call.enqueue(new Callback<BasicReservationResponse>() {
                                @Override
                                public void onResponse(Call<BasicReservationResponse> call, Response<BasicReservationResponse> response) {
                                    if (response.isSuccessful()) {
                                        Log.d("DELETE", "Reservation Deleted Successfully!");
                                        Intent intent = new Intent(context, ReservationHistoryActivity.class);
                                        context.startActivity(intent);
                                        Toast.makeText(context, "Reservation Deleted Successfully!", Toast.LENGTH_SHORT).show(); // Display Toast
                                    } else {
                                        Log.d("ERROR", "Reservation Delete Failed!");
                                    }
                                    dialog.dismiss();
                                }
                                @Override
                                public void onFailure(Call<BasicReservationResponse> call, Throwable t) {
                                    // Handle failure
                                    Log.e("TAG", "Error: " + t.toString());
                                    dialog.dismiss();
                                }
                            });
                            ((ReservationHistoryActivity) context).toggleOverlay(false); // Remove overlay

                            dialog.dismiss();
                        }
                    });

                    negativeButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            // Handle negative button click
                            ((ReservationHistoryActivity) context).toggleOverlay(false);
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
