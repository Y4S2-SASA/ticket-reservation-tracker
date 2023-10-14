package com.sasa.ticketreservationapp.response;

/*
 * File: ScheduleResponse.java
 * Purpose: Acts as a response class for creating scheduleResponse objects
 */
public class ScheduleResponse {
    private String trainId;

    private String trainName;

    private String arrivalTime;

    private String scheduleId;

    public String getTrainId() {
        return trainId;
    }

    public String getTrainName() {
        return trainName;
    }

    public String getArrivalTime() {
        return arrivalTime;
    }

    public String getScheduleId() {
        return scheduleId;
    }
}
