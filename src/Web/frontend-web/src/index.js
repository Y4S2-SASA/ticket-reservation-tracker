import React from 'react'
import ReactDOM from 'react-dom/client'
import './styles/app.scss'
import App from './App'
import 'bootstrap/dist/css/bootstrap.min.css'
import '@fontsource/montserrat'
import 'react-toastify/dist/ReactToastify.css'

const root = ReactDOM.createRoot(document.getElementById('root'))
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
