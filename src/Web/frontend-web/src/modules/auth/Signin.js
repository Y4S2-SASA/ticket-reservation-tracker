import React from 'react'
import { Container, Row, Col, Form, Button, Image } from 'react-bootstrap'

export default function SignIn() {
  return (
    <Container className="signin-container">
      <Row>
        <Col className="left-side">
          <div className="left-content">
            <Image src="path_to_background_image.jpg" fluid />
            <div className="signin-form">
              <Image src="logo.png" alt="Logo" className="logo" />
              <Form>
                <Form.Group controlId="formBasicUsername">
                  <Form.Control type="text" placeholder="Username" />
                </Form.Group>

                <Form.Group controlId="formBasicPassword">
                  <Form.Control type="password" placeholder="Password" />
                </Form.Group>

                <Form.Group controlId="formBasicCheckbox">
                  <Form.Check type="checkbox" label="Remember me" />
                </Form.Group>

                <Button variant="primary" type="submit">
                  Sign In
                </Button>
              </Form>
            </div>
          </div>
        </Col>
        <Col className="right-side">
          <div className="right-content">
            <h1>Welcome Back!</h1>
            <p>Sign in to your account to access our services.</p>
          </div>
        </Col>
      </Row>
    </Container>
  )
}
