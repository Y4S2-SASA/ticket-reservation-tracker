/*
 * File: TMLoader.js
 * Author: Jayathilake S.M.D.A.R/IT20037338
 */

import { Image } from 'react-bootstrap'

export default function Loader() {
  return (
    <div id="splash-screen">
      <div className="logo">
        <Image
          width="300"
          src="/images/logo/sasa_inveresed.png"
          alt="logo"
          style={{ minWidth: 300 }}
          className="loader-logo"
        />
      </div>
      <div id="spinner">
        <div className="bounce1"></div>
        <div className="bounce2"></div>
        <div className="bounce3"></div>
      </div>
    </div>
  )
}
