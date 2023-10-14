package com.sasa.ticketreservationapp.request;

/*
 * File: LoginRequest.java
 * Purpose: Acts as a request class for creating loginRequest objects
 */
public class LoginRequest {
    private String username;
    private String password;

    public LoginRequest(String username, String password) {
        this.username = username;
        this.password = password;
    }
}
