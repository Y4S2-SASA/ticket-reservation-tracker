import React, { Fragment, useEffect, useState } from 'react'
import { Nav, Image } from 'react-bootstrap'
// import { FaBars, FaBell } from 'react-icons/fa'
import { useLocation, useHistory } from 'react-router-dom'
import { NAVBAR_ITEMS } from '../../configs/static-configs'
import ConfirmationDialog from '../TMConfirmationDialog'
import Loader from '../TMLoader'

const MainLayout = ({ children, loading = false, loadingTime }) => {
  const auth = JSON.parse(localStorage.getItem('auth'))
  const userRole = auth?.role
  const location = useLocation()
  const history = useHistory()
  const [loader, setLoader] = useState(false)

  const [showSidebar, setShowSidebar] = useState(true)
  const [showConfirmation, setShowConfirmation] = useState(false)

  useEffect(() => {
    if (loading && loadingTime > 0) {
      setLoader(true)
      setTimeout(() => {
        setLoader(false)
      }, 2000)
    }
  }, [])

  const toggleSidebar = () => {
    setShowSidebar(!showSidebar)
  }

  const handleLogout = () => {
    setShowConfirmation(true)
  }

  const confirmLogout = () => {
    localStorage.clear()
    history.push('/login')
  }

  return (
    <Fragment>
      {loader ? (
        <Loader />
      ) : (
        <div className="main-layout">
          <ConfirmationDialog
            show={showConfirmation}
            onHide={() => setShowConfirmation(false)}
            onConfirm={confirmLogout}
            title={'Confirm Logout'}
            message={'Are you sure you want to logout?'}
            leftButton="Cancel"
            rightButton="Logout"
          />
          <div className="navbar-container">
            <div
              className="left-sidebar"
              style={
                showSidebar
                  ? { height: `${window.innerHeight}pt` }
                  : { display: 'none' }
              }
            >
              <div>
                <div className="top-layer">
                  <div className="logo">
                    <Image
                      src="/images/logo/sasa_inveresed.png"
                      alt="Logo"
                      className="logo"
                    />
                  </div>
                  <div className="menu-section" onClick={toggleSidebar}>
                    <Image
                      src="/images/icons/menu_icon.png"
                      alt="menu"
                      className="menu-icon"
                    />
                    {/* <FaBars onClick={toggleSidebar} className="menu-icon" /> */}
                  </div>
                </div>
                <div className="links">
                  <Nav className="flex-column">
                    {NAVBAR_ITEMS.filter((item) =>
                      item.entitlementRoles.includes(userRole),
                    ).map((item, index) => (
                      <Nav.Link
                        key={index}
                        href={item.pathUrl}
                        className={
                          location.pathname === item.pathUrl
                            ? 'nav-link-item-active'
                            : 'nav-link-item'
                        }
                      >
                        <Image
                          src={item.icon}
                          alt={item.label}
                          className="item-icon"
                        />
                        {item.label}
                      </Nav.Link>
                    ))}

                    <Nav.Link
                      //   href="/logout"
                      className="nav-link-item"
                      onClick={handleLogout}
                    >
                      <Image
                        src="/images/icons/logout.png"
                        alt="menu"
                        className="item-icon"
                      />
                      Logout
                    </Nav.Link>
                  </Nav>
                </div>
              </div>
            </div>
            <div
              style={{
                display: 'flex',
                flexDirection: 'column',
                width: '100%',
              }}
            >
              <div className="top-navbar">
                {!showSidebar && (
                  <>
                    <div className="menu-section" onClick={toggleSidebar}>
                      <Image
                        src="/images/icons/menu_icon.png"
                        alt="menu"
                        className="menu-icon"
                      />
                    </div>
                  </>
                )}
                <div
                  className={`user-info ${showSidebar ? 'show-sidebar' : ''}`}
                >
                  <div className="info-sec">
                    <span className="user-name">{auth?.displayName}</span>
                    <span className="user-role">{auth?.role}</span>
                  </div>

                  <Image
                    src="https://images.pexels.com/photos/2766408/pexels-photo-2766408.jpeg?auto=compress&cs=tinysrgb&w=600"
                    alt="User Avatar"
                    className="user-avatar"
                  />
                </div>
              </div>

              <div className="content-wrapper">
                <div className="content">{children}</div>
              </div>
            </div>
          </div>
        </div>
      )}
    </Fragment>
  )
}

export default MainLayout
