import HttpServiceConfig from '../configs/http-service-config'

class MasterDataAPIService {
  getTrainMasterData = (headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(`/api/MasterData/getTrainDetailMasterData`, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getTrainList = (headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(`/api/MasterData/getTrainMasterData`, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getAllStationMasterData = (headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get('/api/MasterData/getAllStationMasterData', headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getAllStationMasterData = (headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get('/api/MasterData/getAllStationMasterData', headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }
}

export default new MasterDataAPIService()
