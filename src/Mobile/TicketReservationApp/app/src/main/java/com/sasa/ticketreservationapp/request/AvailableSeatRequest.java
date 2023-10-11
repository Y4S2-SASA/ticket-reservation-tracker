package com.sasa.ticketreservationapp.request;

public class AvailableSeatRequest {
    private String trainId;
    private String destinationStationId;
    private String arrivalStationId;
    private String reservationDate;

    public AvailableSeatRequest(String trainId, String destinationStationId, String arrivalStationId, String reservationDate) {
        this.trainId = trainId;
        this.destinationStationId = destinationStationId;
        this.arrivalStationId = arrivalStationId;
        this.reservationDate = reservationDate;
    }

    public String getTrainId() {
        return trainId;
    }

    public void setTrainId(String trainId) {
        this.trainId = trainId;
    }

    public String getDestinationStationId() {
        return destinationStationId;
    }

    public void setDestinationStationId(String destinationStationId) {
        this.destinationStationId = destinationStationId;
    }

    public String getArrivalStationId() {
        return arrivalStationId;
    }

    public void setArrivalStationId(String arrivalStationId) {
        this.arrivalStationId = arrivalStationId;
    }

    public String getReservationDate() {
        return reservationDate;
    }

    public void setReservationDate(String reservationDate) {
        this.reservationDate = reservationDate;
    }
}
