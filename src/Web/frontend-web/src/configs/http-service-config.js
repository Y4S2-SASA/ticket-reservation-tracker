import axios from 'axios'

const token = JSON.parse(localStorage.getItem('auth'))?.token

class HttpServices {
  get = (path = '', config = {}) => {
    const configHeaders = {
      headers: {
        ...config.headers,
        // Authorization: token ? `Bearer ${token}` : '',
      },
    }
    const url = `${process.env.REACT_APP_SERVER_URL + path}`
    return new Promise((resolve, reject) => {
      axios
        .get(url, configHeaders)
        .then((response) => {
          if (response.data) {
            resolve(response.data)
          } else {
            const res = { error: '204 No Content...' }
            reject(res)
          }
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  post = (path = '', req, config = {}) => {
    const configHeaders = {
      headers: {
        ...config.headers,
        // Authorization: token ? `Bearer ${token}` : '',
      },
    }
    const url = `${process.env.REACT_APP_SERVER_URL + path}`
    return new Promise((resolve, reject) => {
      axios
        .post(url, req, configHeaders)
        .then((response) => {
          if (response.data) {
            resolve(response.data)
          } else {
            const res = { error: '204 No Content...' }
            reject(res)
          }
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  put = (path = '', req, config = {}) => {
    const configHeaders = {
      headers: {
        ...config.headers,
        Authorization: token ? `Bearer ${token}` : '',
      },
    }
    const url = `${process.env.REACT_APP_SERVER_URL + path}`

    return new Promise((resolve, reject) => {
      axios
        .put(url, req, configHeaders)
        .then((response) => {
          if (response.data) {
            resolve(response.data)
          } else {
            const res = { error: '204 No Content...' }
            reject(res)
          }
        })
        .catch((error) => {
          reject(error)
        })
    })
  }

  delete = (path = '', id, config = {}) => {
    const configHeaders = {
      headers: {
        ...config.headers,
        Authorization: token ? `Bearer ${token}` : '',
      },
    }
    const url = `${process.env.REACT_APP_SERVER_URL + path}/${id}`

    return new Promise((resolve, reject) => {
      axios
        .delete(url, configHeaders)
        .then((response) => {
          if (response.data) {
            resolve(response.data)
          } else {
            const res = { error: '204 No Content...' }
            reject(res)
          }
        })
        .catch((error) => {
          reject(error)
        })
    })
  }
}

export default new HttpServices()
