import HttpServiceConfig from '../configs/http-service-config'

class ScheduleAPIService {
    getScheduleTrainsData = (req, headers = {}) => {
        const formattedReq = {
            destinationStationId: req.trainName,
            startPointStationId: req.seatCapacity,
            dateTime: req.availableDays,
            passengerClass: req.passengerClasses,
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
}

export default new ScheduleAPIService()
