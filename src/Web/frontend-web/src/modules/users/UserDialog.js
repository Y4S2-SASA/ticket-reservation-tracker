/*
 * File: UserDialog.js
 * Author: Dunusinghe A.V./IT20025526
 */

import React, { useEffect, useState } from 'react'
import { Modal, Button, Form, Row, Col, Spinner } from 'react-bootstrap'
import { Formik, Field, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import TMButton from '../../components/TMButton'
import UserAPIService from '../../api-layer/users'
import { ROLES, strongPasswordRegex } from '../../configs/static-configs'

const UserDialog = ({ userSettings, onClose, onSave, callBackUsers }) => {
  const { openDialog, action, parentData } = userSettings
  const [userData, setUserData] = useState(null)
  const [userDataLoading, setUserDataLoading] = useState(false)

  useEffect(() => {
    const getById = async () => {
      setUserDataLoading(true)
      const response = await UserAPIService.getUserById({ id: parentData?.id })
      if (response) {
        await setUserData(response)
        await setUserDataLoading(false)
      }
      console.log(response)
    }
    if (action === 'edit') {
      getById()
    }
  }, [action])

  const userSchema = Yup.object().shape({
    nic: Yup.string().required('NIC is required'),
    firstName: Yup.string().required('First Name is required'),
    lastName: Yup.string().required('Last Name is required'),
    mobileNumber: Yup.string().required('Mobile Number is required'),
    userName: Yup.string().required('Username is required'),
    email: Yup.string().email('Invalid email').required('Email is required'),
    role: Yup.number().required('Role is required'),
    password:
      action === 'add' &&
      Yup.string()
        .matches(strongPasswordRegex, 'Password must be strong')
        .required('Password is required'),
    confirmPassword:
      action === 'add' &&
      Yup.string()
        .required('Confirm Password is required')
        .matches(strongPasswordRegex, 'Password must be strong')
        .oneOf([Yup.ref('password'), null], 'Passwords must match'),
  })

  const initialValues = {
    nic: action === 'add' ? '' : userData?.nic,
    firstName: action === 'add' ? '' : userData?.firstName,
    lastName: action === 'add' ? '' : userData?.lastName,
    mobileNumber: action === 'add' ? '' : userData?.mobileNumber,
    userName: action === 'add' ? '' : userData?.userName,
    email: action === 'add' ? '' : userData?.email,
    role: action === 'add' ? 1 : userData?.role,
    password: action === 'add' ? '' : '',
    confirmPassword: action === 'add' ? '' : '',
  }

  console.log(parentData)

  const handleSubmit = async (formData) => {
    try {
      const payload = {
        nic: formData.nic,
        firstName: formData.firstName,
        lastName: formData.lastName,
        mobileNumber: formData.mobileNumber,
        userName: formData.userName,
        email: formData.email,
        role: Number(formData.role),
        password: action === 'add' ? formData.password : '',
      }
      if (action === 'add') {
        const response = await UserAPIService.createUser(payload)
        if (response) {
          console.log(response)
          await callBackUsers()
          await onClose()
        } else {
          console.log(response)
        }
      } else if (action === 'edit') {
        const response = await UserAPIService.updateUser(payload)
        if (response) {
          console.log(response)
          await callBackUsers()
          await onClose()
        } else {
          console.log(response)
        }
      }
    } catch (e) {
      console.log(e)
    }
    // onSave(values)
    // onClose()
  }

  return (
    <Modal
      show={openDialog}
      onHide={onClose}
      backdrop="static"
      keyboard={false}
    >
      <Modal.Header closeButton>
        <Modal.Title>
          {action === 'edit' ? 'Edit User' : 'Add User'}
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {action === 'edit' && userDataLoading ? (
          <center>
            <Spinner animation="border" role="status">
              <span className="visually-hidden">Loading...</span>
            </Spinner>
          </center>
        ) : (
          <Formik
            initialValues={
              action === 'edit'
                ? { ...initialValues, ...parentData }
                : initialValues
            }
            validationSchema={userSchema}
            onSubmit={handleSubmit}
          >
            {({ isValid, handleSubmit }) => (
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col sm={6}>
                    <Form.Group>
                      <Form.Label>NIC*</Form.Label>
                      <Field name="nic">
                        {({ field }) => (
                          <Form.Control
                            {...field}
                            disabled={action === 'edit'}
                          />
                        )}
                      </Field>
                      <ErrorMessage
                        name="nic"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                  <Col sm={6}>
                    <Form.Group>
                      <Form.Label>First Name*</Form.Label>
                      <Field name="firstName">
                        {({ field }) => <Form.Control {...field} />}
                      </Field>
                      <ErrorMessage
                        name="firstName"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                </Row>
                <Row style={{ marginTop: '10px' }}>
                  <Col sm={6}>
                    <Form.Group>
                      <Form.Label>Last Name*</Form.Label>
                      <Field name="lastName">
                        {({ field }) => <Form.Control {...field} />}
                      </Field>
                      <ErrorMessage
                        name="lastName"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                  <Col sm={6}>
                    <Form.Group>
                      <Form.Label>Mobile Number*</Form.Label>
                      <Field name="mobileNumber">
                        {({ field }) => <Form.Control {...field} />}
                      </Field>
                      <ErrorMessage
                        name="mobileNumber"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                </Row>
                <Row style={{ marginTop: '10px' }}>
                  <Col sm={6}>
                    <Form.Group>
                      <Form.Label>Username*</Form.Label>
                      <Field name="userName">
                        {({ field }) => <Form.Control {...field} />}
                      </Field>
                      <ErrorMessage
                        name="userName"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>

                  <Col sm={6}>
                    <Form.Group style={{}}>
                      <Form.Label>Role*</Form.Label>
                      <Field
                        name="role"
                        as="select"
                        style={{
                          width: '221px',
                          height: '37px',
                          border: '1px solid #dee2e6',
                          borderRadius: '0.375rem',
                        }}
                      >
                        {ROLES.filter((d) => d.showInForm === true).map(
                          (role, index) => (
                            <option
                              key={index}
                              value={role.id}
                              //   disabled={role.isDisabled}
                            >
                              {role.dropLabel}
                            </option>
                          ),
                        )}
                      </Field>
                      <ErrorMessage
                        name="role"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                </Row>
                <Row style={{ marginTop: '10px' }}>
                  <Col sm={12}>
                    <Form.Group>
                      <Form.Label>Email*</Form.Label>
                      <Field name="email">
                        {({ field }) => <Form.Control {...field} />}
                      </Field>
                      <ErrorMessage
                        name="email"
                        component="div"
                        className="text-danger"
                      />
                    </Form.Group>
                  </Col>
                </Row>

                {action === 'add' && (
                  <Row style={{ marginTop: '10px' }}>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Password*</Form.Label>
                        <Field name="password" type="password">
                          {({ field }) => (
                            <Form.Control {...field} type="password" />
                          )}
                        </Field>
                        <ErrorMessage
                          name="password"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Confirm Password*</Form.Label>
                        <Field name="confirmPassword" type="password">
                          {({ field }) => (
                            <Form.Control {...field} type="password" />
                          )}
                        </Field>
                        <ErrorMessage
                          name="confirmPassword"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                  </Row>
                )}

                <div
                  style={{
                    display: 'flex',
                    justifyContent: 'flex-end',
                    marginTop: '20px',
                  }}
                >
                  <Row>
                    <Col sm={6}>
                      <Button
                        variant="secondary"
                        onClick={onClose}
                        style={{
                          border: 'none',
                          borderRadius: '46px',
                          marginLeft: '10px',
                        }}
                      >
                        Cancel
                      </Button>
                    </Col>
                    <Col sm={6}>
                      <Button
                        variant="primary"
                        type="submit"
                        disabled={!isValid}
                        style={{
                          backgroundColor: '#8428E2',
                          border: 'none',
                          borderRadius: '46px',
                        }}
                      >
                        Save
                      </Button>
                    </Col>
                  </Row>
                </div>
              </Form>
            )}
          </Formik>
        )}
      </Modal.Body>
    </Modal>
  )
}

export default UserDialog
