import React from 'react'
import { BrowserRouter, Route, Switch, Redirect } from 'react-router-dom'
import AuthenticateRoutesConfig from './authenticate-routes-config'
import SignIn from '../modules/auth/Signin'
import Dashboard from '../modules/landing/Dashboard'
import UserList from '../modules/users/UserList'
import TrainsSchedulesConfig from '../modules/trains-schedules/TrainsSchedulesConfig'
import TrainList from '../modules/trains-schedules/trains/TrainList'
import BookingsList from '../modules/bookings/BookingsList'

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/login" component={SignIn} />
        <AuthenticateRoutesConfig path="/dashboard" component={Dashboard} />
        <AuthenticateRoutesConfig path="/users" component={UserList} />
        <AuthenticateRoutesConfig path="/trains" component={TrainList} />
        <AuthenticateRoutesConfig
          path="/trains-with-schedules"
          component={TrainsSchedulesConfig}
        />
        <Redirect from="/" to="/dashboard" />
        <AuthenticateRoutesConfig path="/bookings" component={BookingsList} />
      </Switch>
    </BrowserRouter>
  )
}
