
package com.sasa.ticketreservationapp.models;



import java.io.Serializable;

public class ReservationModel implements Serializable {

    private String name;
    private String destination;
    private String noOfPassengers;
    private String trainNo;
    private String reservedTime;
    private String stationName;
    private String price;
    private String res_ID;

    //Constructors
    public ReservationModel() {
    }

    public ReservationModel(String name, String destination, String noOfPassengers, String trainNo, String reservedTime, String stationName, String price, String res_ID) {
        this.name = name;
        this.destination = destination;
        this.noOfPassengers = noOfPassengers;
        this.trainNo = trainNo;
        this.reservedTime = reservedTime;
        this.stationName = stationName;
        this.price = price;
        this.res_ID = res_ID;
    }

//    public ReservationModel(String destination, String reservedTime) {
//        this.destination = destination;
//        this.reservedTime = reservedTime;
//    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDestination() {
        return destination;
    }

    public void setDestination(String destination) {
        this.destination = destination;
    }

    public String getNoOfPassengers() {
        return noOfPassengers;
    }

    public void setNoOfPassengers(String noOfPassengers) {
        this.noOfPassengers = noOfPassengers;
    }

    public String getTrainNo() {
        return trainNo;
    }

    public void setTrainNo(String trainNo) {
        this.trainNo = trainNo;
    }

    public String getReservedTime() {
        return reservedTime;
    }

    public void setReservedTime(String reservedTime) {
        this.reservedTime = reservedTime;
    }

    public String getStationName() {
        return stationName;
    }

    public void setStationName(String stationName) {
        this.stationName = stationName;
    }

    public String getPrice() {
        return price;
    }

    public void setPrice(String price) {
        this.price = price;
    }

    public String getRes_ID() {
        return res_ID;
    }

    public void setRes_ID(String res_ID) {
        this.res_ID = res_ID;
    }

}