import HttpServiceConfig from '../configs/http-service-config'

class ReservationsAPIService {
    getReservations = (req, headers = {}) => {
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

export default new ReservationsAPIService()
