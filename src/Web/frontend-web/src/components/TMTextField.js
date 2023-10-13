/*
 * File: TMTextField.js
 * Author: Bartholomeusz S.V/IT20274702
 */

import React from 'react'
import { Form } from 'react-bootstrap'

function TMTextField(props) {
  const { type, placeholder, style, ...otherProps } = props
  const commonStyles = {
    borderRadius: '46px',
    height: '46px',
    marginBottom: '10px',
    border: '2px solid #72C9F8',
  }

  const mergedStyles = { ...commonStyles, ...style }

  return (
    <Form.Control
      type={type}
      placeholder={placeholder}
      style={mergedStyles}
      {...otherProps}
    />
  )
}

TMTextField.defaultProps = {
  style: {},
}

export default TMTextField
