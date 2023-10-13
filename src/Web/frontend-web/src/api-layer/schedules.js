import HttpServiceConfig from '../configs/http-service-config'

class ScheduleAPIService {
    getScheduleTrainsData = (req, headers = {}) => {
        const formattedReq = {
            destinationStationId: req.destinationStationId,
            startPointStationId: req.startPointStationId,
            dateTime: req.dateTime,
            passengerClass: req.passengerClass
          }

        return new Promise((resolve, reject) => {
          HttpServiceConfig.post('/api/Schedule/getScheduleTrainsData', formattedReq, headers)
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
            passengerClass: req.passengerClass
          }

        return new Promise((resolve, reject) => {
          HttpServiceConfig.post('/api/Schedule/getSchedulePrice', formattedReq, headers)
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