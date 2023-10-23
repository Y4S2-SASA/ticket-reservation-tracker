/*
 * File: Dashboard.js
 * Author: Perera M.S.D/IT20020262
 */

import React, { Fragment, useEffect, useState } from 'react'
import MainLayout from '../../components/Layouts/MainLayout'
import Loader from '../../components/TMLoader'
import MasterDataAPIService from '../../api-layer/master-data'
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'

const DataBlock = ({ title, count, borderColor, labelBackgroundImage }) => {
  const dataBlockStyle = {
    backgroundImage: `linear-gradient(rgba(0, 0, 0, 0.1), rgba(0, 0, 0, 0.5)), url(${labelBackgroundImage})`,
    backgroundPosition: 'center',
    backgroundCover: 'cover',
  }

  const labelStyle = {
    // backgroundImage: `linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url(${labelBackgroundImage})`,
    fontWeight: 900,
  }

  return (
    <div className="data-block" style={dataBlockStyle}>
      <div className="data-count" style={{ color: 'white' }}>
        {count}
      </div>
      <div className="data-label" style={labelStyle}>
        {title}
      </div>
    </div>
  )
}

export default function Dashboard() {
  const [dashboardData, setDashboardData] = useState(null)

  useEffect(() => {
    MasterDataAPIService.getDashboardData().then((data) => {
      setDashboardData(data)
    })
    // const trainMdRes = MasterDataAPIService.getTrainMasterData()
    // const strigifiedMasterData = JSON.stringify(trainMdRes)
    // localStorage.setItem('train-masterdata', strigifiedMasterData)
  }, [])

  return (
    <Fragment>
      <MainLayout loading={true} loadingTime={2000}>
        <div>
          <div style={{ fontWeight: 600, fontSize: '28px', color: '#330065' }}>
            Dashboard Statistics
          </div>
          <hr />
          <div className="data-block-container">
            {dashboardData && (
              <Row>
                <Col sm={6} md={6}>
                  <DataBlock
                    title="Active Reservation Count"
                    count={dashboardData.activeReservationCount}
                    borderColor="#8428E2"
                    labelBackgroundImage="https://images.pexels.com/photos/18512162/pexels-photo-18512162/free-photo-of-freight-train-on-railway-track.jpeg?auto=compress&cs=tinysrgb&w=600"
                  />
                </Col>
                <Col sm={6} md={6}>
                  <DataBlock
                    title="Active Train Count"
                    count={dashboardData.activeTrainCount}
                    borderColor="#330065"
                    labelBackgroundImage="https://images.pexels.com/photos/11334742/pexels-photo-11334742.jpeg?auto=compress&cs=tinysrgb&w=600"
                  />
                </Col>
                <Col sm={6} md={6}>
                  <DataBlock
                    title="Active User Count"
                    count={dashboardData.activeUserCount}
                    borderColor="#7E5AE9"
                    labelBackgroundImage="https://images.pexels.com/photos/3831850/pexels-photo-3831850.jpeg?auto=compress&cs=tinysrgb&w=600"
                  />
                </Col>
                <Col sm={6} md={6}>
                  <DataBlock
                    title="Station Count"
                    count={dashboardData.stationCount}
                    borderColor="#72C9F8"
                    labelBackgroundImage="https://images.pexels.com/photos/1170187/pexels-photo-1170187.jpeg?auto=compress&cs=tinysrgb&w=600"
                  />
                </Col>
              </Row>
            )}
          </div>
        </div>
      </MainLayout>
    </Fragment>
  )
}
