package com.sasa.ticketreservationapp.adapters;

import android.app.AlertDialog;
import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.PopupMenu;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.sasa.ticketreservationapp.activities.EditReservationActivity;
import com.sasa.ticketreservationapp.R;
import com.sasa.ticketreservationapp.handlers.ReservationsHandler;
import com.sasa.ticketreservationapp.models.ReservationModel;

import java.util.ArrayList;

public class BookingHistoryAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {
    //Initializing variables
    private Context context;
    ArrayList<ReservationModel> list = new ArrayList<>();

    //    public int resMenu = R.id.menu_update;
    public BookingHistoryAdapter(Context ctx){
        this.context = ctx;
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
        String reservedTime = req.getReservedTime();
        String destination = req.getDestination();
        String prefix = req.getTrainNo();
        vhc.tv_reservedTime.setText(reservedTime);
        vhc.tv_destination.setText(destination);
        vhc.tv_reqPrefix.setText(prefix);
        vhc.txt_option.setOnClickListener(v -> {
            PopupMenu popupMenu = new PopupMenu(context, vhc.txt_option);
            popupMenu.inflate(R.menu.options_menu);

            popupMenu.setOnMenuItemClickListener(item -> {
                if (item.getItemId() == R.id.menu_update) {
                    Intent intentU = new Intent(context, EditReservationActivity.class);
                    intentU.putExtra("UPDATE", req);
                    context.startActivity(intentU);// Return true to indicate that the click has been handled
                }
                else if (item.getItemId() == R.id.menu_delete) {
//                    ((BookingHistoryActivity) context).toggleOverlay(true);
                    AlertDialog.Builder builder = new AlertDialog.Builder(context);
                    View dialogView = LayoutInflater.from(context).inflate(R.layout.dialog_layout, null);
                    builder.setView(dialogView);

                    Button positiveButton = dialogView.findViewById(R.id.deleteReservationBtn);
                    Button negativeButton = dialogView.findViewById(R.id.cancelBtn);

                    AlertDialog dialog = builder.create();

                    positiveButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            // Handle positive button click
                            // For example, you can dismiss the dialog
//                            ((BookingHistoryActivity) context).toggleOverlay(false);
                            dialog.dismiss();
                        }
                    });

                    negativeButton.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            // Handle negative button click
                            // For example, you can dismiss the dialog
//                            ((BookingHistoryActivity) context).toggleOverlay(false);
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
