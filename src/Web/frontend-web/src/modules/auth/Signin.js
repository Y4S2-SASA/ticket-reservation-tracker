import React, { Fragment, useEffect, useState } from 'react'
import { Container, Row, Col, Image } from 'react-bootstrap'
import TMTextField from '../../components/TMTextField'
import TMButton from '../../components/TMButton'
import OrDivider from '../../components/TMOrDivider'
import Loader from '../../components/TMLoader'
import { Link, useHistory } from 'react-router-dom'
import { Formik, Field, Form, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import AuthAPIService from '../../api-layer/auth'
import { ToastContainer, toast } from 'react-toastify'
// import { jwtDecode } from '../../utils/helpers'

const validationSchema = Yup.object().shape({
  username: Yup.string().required('Username is required'),
  password: Yup.string().required('Password is required'),
})

const defaultValues = {
  username: 'superadmin',
  password: '123',
}

export default function SignIn() {
  const history = useHistory()
  const [loader, setLoader] = useState(false)

  useEffect(() => {
    setLoader(true)
    setTimeout(() => {
      setLoader(false)
    }, 2000)
  }, [])

  const handleSubmit = async (data, setSubmitting) => {
    try {
      console.log('Form Data:', data)
      const payload = {
        userName: data.username,
        password: data.password,
      }
      const response = await AuthAPIService.authLogin(payload)
      console.log(response)
      if (response) {
        setSubmitting(false)
        if (response?.isLoginSuccess) {
          const authData = {
            token: response?.token,
            // decodedData: jwtDecode(response?.token),
            role: response?.role,
            authSuccess: response?.isLoginSuccess,
            userId: response?.userId,
            displayName: response?.displayName,
          }
          const strigifiedData = JSON.stringify(authData)
          localStorage.setItem('auth', strigifiedData)
          history.push('/dashboard')
          toast.success(response?.message)
        } else {
          toast.error(response?.message || 'Error occured. Try again!')
        }
      }
    } catch (e) {
      console.log('error', e)
      toast.error('Error occured. Try again!')
      setSubmitting(false)
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <Fragment>
      <ToastContainer />
      {loader ? (
        <Loader />
      ) : (
        <Container className="signin-container">
          <Row>
            <Col className="left-side">
              <div className="left-content">
                <div className="signin-form">
                  <Image
                    src="/images/logo/sasa_inveresed.png"
                    alt="Logo"
                    className="logo"
                  />
                  <Formik
                    initialValues={defaultValues}
                    validationSchema={validationSchema}
                    onSubmit={(values, { setSubmitting }) => {
                      handleSubmit(values, setSubmitting)
                    }}
                  >
                    {({ isValid, isSubmitting }) => (
                      <Form className="needs-validation user-add" noValidate="">
                        <div>
                          <Field
                            type="text"
                            name="username"
                            placeholder="Username"
                            as={TMTextField}
                          />
                          <ErrorMessage
                            name="username"
                            component="div"
                            className="error-message"
                          />
                        </div>
                        <div>
                          <Field
                            type="password"
                            name="password"
                            placeholder="Password"
                            as={TMTextField}
                          />
                          <ErrorMessage
                            name="password"
                            component="div"
                            className="error-message"
                          />
                        </div>
                        {/* </Form.Group> */}
                        <TMButton
                          variant="primary"
                          type="submit"
                          color="primary"
                          style={{ marginTop: '20px' }}
                          disabled={!isValid || isSubmitting}
                          loading={isSubmitting}
                        >
                          Sign In
                        </TMButton>
                      </Form>
                    )}
                  </Formik>
                  <OrDivider color="white" />
                  <div className="icon-row">
                    <Row>
                      <Col>
                        <Image
                          src="/images/icons/twitter.png"
                          alt="twitter"
                          width={50}
                        />
                      </Col>
                      <Col>
                        <Image
                          src="/images/icons/facebook.png"
                          alt="facebook"
                          width={35}
                        />
                      </Col>
                      <Col>
                        <Image
                          src="/images/icons/google.png"
                          alt="google"
                          width={35}
                        />
                      </Col>
                    </Row>
                    <div className="signup-text" style={{ marginTop: '20px' }}>
                      <span
                        style={{
                          color: 'white',
                          fontWeight: 300,
                          fontSize: '14px',
                          opacity: 0.7,
                        }}
                      >
                        Don't have an account?
                      </span>{' '}
                      <Link
                        to="/signup"
                        style={{ color: '#72C9F8', fontWeight: 800 }}
                      >
                        Sign up
                      </Link>
                    </div>
                  </div>
                </div>
              </div>
            </Col>
            <Col className="right-side">
              <div className="right-content">
                <h2 className="welcome-head1">Welcome To</h2>
                <h1 className="welcome-head2">SASA Train Booking</h1>
                <p className="welcome-msg">
                  Experience effortless travel planning with our comprehensive
                  ticket reservation system. Easily book your tickets, access
                  flexible options, make secure payments, and enjoy
                  round-the-clock customer support. Stay informed with real-time
                  updates and manage your reservations with ease. Your journey
                  starts here.
                </p>
              </div>
            </Col>
          </Row>
        </Container>
      )}
    </Fragment>
  )
}
