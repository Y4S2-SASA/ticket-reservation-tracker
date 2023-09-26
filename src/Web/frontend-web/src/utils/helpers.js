import jwt_decode from 'jwt-decode'

export const jwtDecode = (token) => {
  var decoded = jwt_decode(token)
  return decoded
}
