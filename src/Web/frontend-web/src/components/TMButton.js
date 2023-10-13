/*
 * File: TMButton.js
 * Author: Bartholomeusz S.V/IT20274702
 */

import React from 'react'
import { Button, Spinner } from 'react-bootstrap'

function TMButton(props) {
  const {
    type,
    variant,
    color,
    style,
    loading,
    children,
    ...otherProps
  } = props

  const commonStyles = {
    width: '100%',
    height: '46px',
    backgroundColor: color === 'primary' ? '#7E5AE9' : color,
    border: 'none',
    borderRadius: '46px',
  }

  const mergedStyles = { ...commonStyles, ...style }

  return (
    <Button
      className="tm-button"
      type={type}
      variant={variant}
      style={mergedStyles}
      // disabled={loading}
      {...otherProps}
    >
      {loading ? (
        <Spinner animation="border" role="status" size="sm" />
      ) : (
        children
      )}
    </Button>
  )
}

TMButton.defaultProps = {
  style: {},
  loading: false,
}

export default TMButton
