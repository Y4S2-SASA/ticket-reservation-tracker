package com.sasa.ticketreservationapp.request;

/*
 * File: PriceRequest.java
 * Purpose: Acts as a request class for creating priceRequest objects
 */
public class PriceRequest {
        private String selectedTrainId;
        private String departureStationId;
        private String arrivalStationId;
        private String selectedScheduleId;
        private int passengerCount;
        private int passengerClass;

    public PriceRequest(String selectedTrainId, String departureStationId, String arrivalStationId, String selectedScheduleId, int passengerCount, int passengerClass) {
        this.selectedTrainId = selectedTrainId;
        this.departureStationId = departureStationId;
        this.arrivalStationId = arrivalStationId;
        this.selectedScheduleId = selectedScheduleId;
        this.passengerCount = passengerCount;
        this.passengerClass = passengerClass;
    }

    public String getSelectedTrainId() {
        return selectedTrainId;
    }

    public void setSelectedTrainId(String selectedTrainId) {
        this.selectedTrainId = selectedTrainId;
    }

    public String getDepartureStationId() {
        return departureStationId;
    }

    public void setDepartureStationId(String departureStationId) {
        this.departureStationId = departureStationId;
    }

    public String getArrivalStationId() {
        return arrivalStationId;
    }

    public void setArrivalStationId(String arrivalStationId) {
        this.arrivalStationId = arrivalStationId;
    }

    public String getSelectedScheduleId() {
        return selectedScheduleId;
    }

    public void setSelectedScheduleId(String selectedScheduleId) {
        this.selectedScheduleId = selectedScheduleId;
    }

    public int getPassengerCount() {
        return passengerCount;
    }

    public void setPassengerCount(int passengerCount) {
        this.passengerCount = passengerCount;
    }

    public int getPassengerClass() {
        return passengerClass;
    }

    public void setPassengerClass(int passengerClass) {
        this.passengerClass = passengerClass;
    }
}
