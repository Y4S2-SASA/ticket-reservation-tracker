import React from 'react'
import { Route, Redirect } from 'react-router-dom'

export default function AuthenticateRoutesConfig({
  component: Component,
  ...rest
}) {
  const isAuthenticated = localStorage.getItem('auth')
  console.log(isAuthenticated)
  return (
    <Route
      {...rest}
      render={(props) =>
        isAuthenticated ? <Component {...props} /> : <Redirect to="/login" />
      }
    />
  )
}
