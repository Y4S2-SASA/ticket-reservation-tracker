import React from 'react'
import { Route, Redirect } from 'react-router-dom'

export default function AuthenticateRoutesConfig({
  component: Component,
  ...rest
}) {
  const authentication = JSON.parse(localStorage.getItem('auth'))

  return (
    <Route
      {...rest}
      render={(props) =>
        authentication?.authSuccess ? (
          <Component {...props} />
        ) : (
          <Redirect to="/login" />
        )
      }
    />
  )
}
