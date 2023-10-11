package com.sasa.ticketreservationapp.request;


public class ScheduleRequest {
    private String destinationStationId;

    private String startPointStationId;

    private String dateTime;

    private int passengerClass;

    // Constructor
    public ScheduleRequest(String destinationStationId, String startPointStationId, String dateTime, int passengerClass) {
        this.destinationStationId = destinationStationId;
        this.startPointStationId = startPointStationId;
        this.dateTime = dateTime;
        this.passengerClass = passengerClass;
    }

    public String getDestinationStationId() {
        return destinationStationId;
    }

    public String getStartPointStationId() {
        return startPointStationId;
    }

    public String getDateTime() {
        return dateTime;
    }

    public int getPassengerClass() {
        return passengerClass;
    }
}
