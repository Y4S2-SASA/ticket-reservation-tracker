package com.sasa.ticketreservationapp.request;

/*
 * File: StatusChangeRequest.java
 * Purpose: Acts as a request class for creating statusChangeRequest objects
 */
public class StatusChangeRequest {
    private String id;
    private int status;

    public StatusChangeRequest(String id, int status) {
        this.id = id;
        this.status = status;
    }

    // Getters and setters
    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }
}
