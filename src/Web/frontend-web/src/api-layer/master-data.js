/*
 * File: master-data.js
 * Author: Jayathilake S.M.D.A.R./IT20037338
 */

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
    const formatterReq = {
      searchText: '',
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        `/api/MasterData/getTrainMasterData`,
        formatterReq,
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

  getDashboardData = (headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get('/api/Dashboard/getDashboardMasterData', headers)
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
