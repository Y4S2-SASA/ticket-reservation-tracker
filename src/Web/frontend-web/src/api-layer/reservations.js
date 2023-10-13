/*
 * File: reservations.js
 * Author: Bartholomeusz S.V/IT20274702
 */

import HttpServiceConfig from '../configs/http-service-config'

class ReservationsAPIService {
  saveReservation = (req, headers = {}) => {
    // Corrected the variable name from "header" to "headers"
    const formattedReq = {
      id: req.id,
      referenceNumber: '',
      passengerClass: req.passengerClass,
      trainId: req.trainId,
      destinationStationId: req.destinationStationId,
      arrivalStationId: req.arrivalStationId,
      dateTime: req.dateTime,
      noOfPassengers: req.noOfPassengers,
      price: req.price,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Reservation/saveReservation',
        formattedReq,
        headers, // Corrected the variable name from "headers" to "headers"
      )
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getReservation = (id) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(`/api/Reservation/getReservationById/${id}`)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  deleteReservation = (id) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.delete(`/api/Reservation/deleteReservation`, id)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getReservations = (req, headers = {}) => {
    // Corrected the variable name from "header" to "headers"
    const formattedReq = {
      reservationNumber: req.reservationNumber,
      fromDate: req.fromDate,
      toDate: req.toDate,
      trainId: req.trainId,
      destinationStationId: req.destinationStationId,
      arrivalStationId: req.arrivalStationId,
      status: req.status,
      currentPage: req.currentPage,
      pageSize: req.pageSize,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Reservation/getReservationsByFilter',
        formattedReq,
        headers, // Corrected the variable name from "headers" to "headers"
      )
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }
}

export default new ReservationsAPIService()
