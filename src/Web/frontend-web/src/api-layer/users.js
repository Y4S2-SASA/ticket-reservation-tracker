import HttpServiceConfig from '../configs/http-service-config'

class UserAPIService {
  getUsers = (req, headers = {}) => {
    const formattedReq = {
      searchText: req.searchText,
      status: req.status,
      role: req.role,
      currentPage: req.currentPage,
      pageSize: req.pageSize,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/User/getAllUsersByFilter',
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

  createUser = (req, headers = {}) => {
    const formattedReq = {
      nic: req.nic,
      firstName: req.firstName,
      lastName: req.lastName,
      mobileNumber: req.mobileNumber,
      userName: req.userName,
      email: req.email,
      role: req.role,
      password: req.password,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post('/api/User/saveUser', formattedReq, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  updateUser = (req, headers = {}) => {
    const formattedReq = {
      nic: req.nic,
      firstName: req.firstName,
      lastName: req.lastName,
      mobileNumber: req.mobileNumber,
      userName: req.userName,
      email: req.email,
      role: req.role,
      password: req.password,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.put('/api/User/updateUser', formattedReq, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  getUserById = (req, headers = {}) => {
    return new Promise((resolve, reject) => {
      HttpServiceConfig.get(`/api/User/getUserById/${req.id}`, headers)
        .then((data) => {
          resolve(data)
        })
        .catch((error) => {
          reject(error)
        })
    })
  }
}

export default new UserAPIService()
