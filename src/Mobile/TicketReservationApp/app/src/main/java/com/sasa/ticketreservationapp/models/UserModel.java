package com.sasa.ticketreservationapp.models;

/*
 * File: UserModel.java
 * Purpose: Acts as a model class for the user objects
 */
public class UserModel {
    private String firstName;

    private String lastName;

    private String nic;

    private String email;

    private String mobileNumber;

    private String userName;

    private String password;

    private Integer role;

    public UserModel(String firstName, String lastName, String nic, String email, String mobileNumber, String userName, String password, Integer role) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.nic = nic;
        this.email = email;
        this.mobileNumber = mobileNumber;
        this.userName = userName;
        this.password = password;
        this.role = role;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getNic() { return nic; }

    public void setNic(String nic) {
        this.nic = nic;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getMobileNumber() {
        return mobileNumber;
    }

    public void setMobileNumber(String mobileNumber) {
        this.mobileNumber = mobileNumber;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public Integer getRole() {
        return role;
    }

    public void setRole(Integer role) {
        this.role = role;
    }
}

