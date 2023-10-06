package com.sasa.ticketreservationapp.request;

public class AccountStatusRequest {
    private String nic;
    private int status;

    public AccountStatusRequest(String nic, int status) {
        this.nic = nic;
        this.status = status;
    }

    public String getNic() {
        return nic;
    }

    public void setNic(String nic) {
        this.nic = nic;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }
}
