package com.sasa.ticketreservationapp.request;

import java.io.Serializable;

public class ReservationRequest implements Serializable {
    private String id;
    private String referenceNumber;
    private int passengerClass;
    private String destinationStationId;
    private String trainId;
    private String arrivalStationId;
    private String dateTime;
    private int noOfPassengers;
    private double price;

    public ReservationRequest(String id, String referenceNumber, int passengerClass, String destinationStationId, String trainId, String arrivalStationId, String dateTime, int noOfPassengers, double price) {
        this.id = id;
        this.referenceNumber = referenceNumber;
        this.passengerClass = passengerClass;
        this.destinationStationId = destinationStationId;
        this.trainId = trainId;
        this.arrivalStationId = arrivalStationId;
        this.dateTime = dateTime;
        this.noOfPassengers = noOfPassengers;
        this.price = price;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getReferenceNumber() {
        return referenceNumber;
    }

    public void setReferenceNumber(String referenceNumber) {
        this.referenceNumber = referenceNumber;
    }

    public int getPassengerClass() {
        return passengerClass;
    }

    public void setPassengerClass(int passengerClass) {
        this.passengerClass = passengerClass;
    }

    public String getDestinationStationId() {
        return destinationStationId;
    }

    public void setDestinationStationId(String destinationStationId) {
        this.destinationStationId = destinationStationId;
    }

    public String getTrainId() {
        return trainId;
    }

    public void setTrainId(String trainId) {
        this.trainId = trainId;
    }

    public String getArrivalStationId() {
        return arrivalStationId;
    }

    public void setArrivalStationId(String arrivalStationId) {
        this.arrivalStationId = arrivalStationId;
    }

    public String getDateTime() {
        return dateTime;
    }

    public void setDateTime(String dateTime) {
        this.dateTime = dateTime;
    }

    public int getNoOfPassengers() {
        return noOfPassengers;
    }

    public void setNoOfPassengers(int noOfPassengers) {
        this.noOfPassengers = noOfPassengers;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }
}
