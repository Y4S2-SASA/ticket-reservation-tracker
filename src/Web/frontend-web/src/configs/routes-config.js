import React from 'react'
import { BrowserRouter, Route, Switch } from 'react-router-dom'
import AuthenticateRoutesConfig from './authenticate-routes-config'
import SignIn from '../modules/auth/Signin'
import Dashboard from '../modules/landing/Dashboard'
import UserList from '../modules/users/UserList'

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/login" component={SignIn} />
        <AuthenticateRoutesConfig path="/dashboard" component={Dashboard} />
        <AuthenticateRoutesConfig path="/users" component={UserList} />
      </Switch>
    </BrowserRouter>
  )
}
