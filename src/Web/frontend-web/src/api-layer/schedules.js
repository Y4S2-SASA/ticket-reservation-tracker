import HttpServiceConfig from '../configs/http-service-config'

class ScheduleAPIService {
  getScheduleTrainsData = (req, headers = {}) => {
    const formattedReq = {
      destinationStationId: req.destinationStationId,
      startPointStationId: req.startPointStationId,
      dateTime: req.dateTime,
      passengerClass: req.passengerClass,
    }

    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Schedule/getScheduleTrainsData',
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

  getSchedulePrice = (req, headers = {}) => {
    const formattedReq = {
      selectedTrainId: req.selectedTrainId,
      departureStationId: req.departureStationId,
      arrivalStationId: req.arrivalStationId,
      selectedScheduleId: req.selectedScheduleId,
      passengerCount: req.passengerCount,
      passengerClass: req.passengerClass,
    }

    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Schedule/getSchedulePrice',
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

  getSchedules = (req, headers = {}) => {
    const formattedReq = {
      trainId: req.trainId,
      departureStationId: req.departureStationId,
      arrivalStationId: req.arrivalStationId,
      status: req.status,
      currentPage: req.currentPage,
      pageSize: req.pageSize,
    }

    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Schedule/getSchedulesByFilter',
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

  createSchedule = (req, headers = {}) => {
    const formattedReq = {
      id: req.id,
      trainId: req.trainId,
      departureStationId: req.departureStationId,
      arrivalStationId: req.arrivalStationId,
      departureTime: req.departureTime,
      arrivalTime: req.arrivalTime,
      subStationDetails: req.subStationDetails,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Schedule/saveSchedule',
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

  getScheduleById = (req, headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(
        `/api/Schedule/getScheduleById?id=${req.id}`,
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

export default new ScheduleAPIService()
