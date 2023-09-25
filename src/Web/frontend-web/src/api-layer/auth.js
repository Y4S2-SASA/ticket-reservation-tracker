import HttpServiceConfig from '../configs/http-service-config'

class AuthAPIService {
  authLogin = (req, headers = {}) => {
    const formattedReq = {
      userName: req.userName,
      password: req.password,
    }
    return new Promise((resolve, reject) => {
      HttpServiceConfig.post(
        '/api/Authentication/authenticate',
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

export default new AuthAPIService()
