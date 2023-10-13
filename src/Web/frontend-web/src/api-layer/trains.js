/*
 * File: trains.js
 * Author: Perera M.S.D/IT20020262
 */

import HttpServiceConfig from '../configs/http-service-config'

class TrainsAPIService {
  getTrains = (req, headers = {}) => {
    const formattedReq = {
      searchText: req.searchText,
      status: req.status,
      availableDay: req.availableDay,
      passengerClass: req.passengerClass,
      currentPage: req.currentPage,
      pageSize: req.pageSize,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Train/getTrainsByFilter',
        formattedReq,
        headers,
      )
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  createTrain = (req, headers = {}) => {
    const formattedReq = {
      id: req.id,
      trainName: req.trainName,
      seatCapacity: req.seatCapacity,
      availableDays: req.availableDays,
      passengerClasses: req.passengerClasses,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post('/api/Train/saveTrain', formattedReq, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getTrainById = (req, headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(`/api/Train/getTrainById/${req.id}`, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  updateTrainStatus = (req, headers = {}) => {
    const formattedReq = {
      id: req.id,
      status: req.status,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.put(
        '/api/Train/changeTrainStatus',
        formattedReq,
        headers,
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

export default new TrainsAPIService()
