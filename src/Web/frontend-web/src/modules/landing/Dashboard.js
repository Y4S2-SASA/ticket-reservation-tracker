import React, { Fragment, useEffect, useState } from 'react'
import MainLayout from '../../components/Layouts/MainLayout'
import Loader from '../../components/TMLoader'

export default function Dashboard() {
  const [loader, setLoader] = useState(false)

  useEffect(() => {
    setLoader(true)
    setTimeout(() => {
      setLoader(false)
    }, 2000)
  }, [])

  return (
    <Fragment>
      {loader ? (
        <Loader />
      ) : (
        <MainLayout>
          <div>
            <div>Dashboard</div>
          </div>
        </MainLayout>
      )}
    </Fragment>
  )
}
