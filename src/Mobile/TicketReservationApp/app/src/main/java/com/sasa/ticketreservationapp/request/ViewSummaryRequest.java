package com.sasa.ticketreservationapp.request;

import java.io.Serializable;

/*
 * File: ViewSummaryRequest.java
 * Purpose: Acts as a request class for creating viewSummaryRequest objects
 */
public class ViewSummaryRequest implements Serializable {
    private String id;
    private String referenceNumber;
    private int passengerClass;
    private String destinationStationId;
    private String destinationStationName;
    private String trainId;
    private String trainName;
    private String arrivalStationId;
    private String arrivalStationName;
    private String dateTime;
    private String reservedDate;
    private String reservedTime;
    private int noOfPassengers;
    private double price;

    public ViewSummaryRequest(String id, String referenceNumber, int passengerClass, String destinationStationId, String destinationStationName, String trainId, String trainName, String arrivalStationId, String arrivalStationName, String dateTime, String reservedDate, String reservedTime, int noOfPassengers, double price) {
        this.id = id;
        this.referenceNumber = referenceNumber;
        this.passengerClass = passengerClass;
        this.destinationStationId = destinationStationId;
        this.destinationStationName = destinationStationName;
        this.trainId = trainId;
        this.trainName = trainName;
        this.arrivalStationId = arrivalStationId;
        this.arrivalStationName = arrivalStationName;
        this.dateTime = dateTime;
        this.reservedDate = reservedDate;
        this.reservedTime = reservedTime;
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

    public String getDestinationStationName() {
        return destinationStationName;
    }

    public void setDestinationStationName(String destinationStationName) {
        this.destinationStationName = destinationStationName;
    }

    public String getTrainId() {
        return trainId;
    }

    public void setTrainId(String trainId) {
        this.trainId = trainId;
    }

    public String getTrainName() {
        return trainName;
    }

    public void setTrainName(String trainName) {
        this.trainName = trainName;
    }

    public String getArrivalStationId() {
        return arrivalStationId;
    }

    public void setArrivalStationId(String arrivalStationId) {
        this.arrivalStationId = arrivalStationId;
    }

    public String getArrivalStationName() {
        return arrivalStationName;
    }

    public void setArrivalStationName(String arrivalStationName) {
        this.arrivalStationName = arrivalStationName;
    }

    public String getDateTime() {
        return dateTime;
    }

    public void setDateTime(String dateTime) {
        this.dateTime = dateTime;
    }

    public String getReservedDate() {
        return reservedDate;
    }

    public void setReservedDate(String reservedDate) {
        this.reservedDate = reservedDate;
    }

    public String getReservedTime() {
        return reservedTime;
    }

    public void setReservedTime(String reservedTime) {
        this.reservedTime = reservedTime;
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
