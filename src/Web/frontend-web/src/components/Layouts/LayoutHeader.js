import React from 'react'

const LayoutHeader = ({ title, subtitle, buttonComponent }) => {
  return (
    <div className="layout-box">
      <div className="layout-content">
        <div className="layout-text">
          {title && <h2 className="layout-title">{title}</h2>}
          {subtitle && <p className="layout-subtitle">{subtitle}</p>}
        </div>
      </div>
      {buttonComponent && (
        <div className="layout-button">{buttonComponent}</div>
      )}
    </div>
  )
}

export default LayoutHeader
