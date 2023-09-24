import React from 'react'
import { BrowserRouter, Route, Switch } from 'react-router-dom'
import AuthenticateRoutesConfig from './AuthenticateRoutesConfig'
import SignIn from '../modules/auth/Signin'
import Dashboard from '../modules/landing/Dashboard'

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/login" component={SignIn} />
        <AuthenticateRoutesConfig path="/dashboard" component={Dashboard} />
        <Route path="/" exact component={Dashboard} />
      </Switch>
    </BrowserRouter>
  )
}
