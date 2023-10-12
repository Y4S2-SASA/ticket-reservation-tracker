import React, { useEffect, useState } from 'react'
import StepWizard from '../../components/StepWizard'
import MainLayout from '../../components/Layouts/MainLayout'
import { Form, Row, Col, Spinner, Button } from 'react-bootstrap'
import { Formik, Field, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import TrainsAPIService from '../../api-layer/trains'
import {
  TRAIN_AVAILABLE_DAYS,
  TRAIN_PASSENGER_CLASSES,
} from '../../configs/static-configs'
import { useParams } from 'react-router-dom'
import ScheduleList from './schedules/ScheduleList'

export default function TrainsSchedulesConfig({
  settings,
  onSave,
  callBackData,
  onClose,
}) {
  // const { action, parentData } = settings
  const { id } = useParams()

  const [data, setData] = useState(null)
  const [dataLoading, setDataLoading] = useState(false)
  const [action] = useState(id === 'new' ? 'add' : 'edit')
  const parentData = { id: 1 }
  const [trainData, setTrainData] = useState(false)
  const [step, setStep] = useState('train')
  const [initialValues, setInitialValues] = useState({
    trainName: '',
    seatCapacity: '',
    availableDays: '',
    passengerClasses: [],
  })
  const [showForm, setShowForm] = useState(false)

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      setShowForm(true)
    }, 2000)
    return () => clearTimeout(timeoutId)
  }, [])

  useEffect(() => {
    const getById = async () => {
      setDataLoading(true)
      if (id !== 'new') {
        const response = await TrainsAPIService.getTrainById({ id })
        if (response) {
          await setData(response)
          await setDataLoading(false)
        }
        console.log(response)
      }
    }
    if (action === 'edit') {
      getById()
    }
  }, [action, id])

  const userSchema = Yup.object().shape({
    trainName: Yup.string().required('Name is required'),
    seatCapacity: Yup.number().required('Capacity is required'),
    availableDays: Yup.number().required('Availability is required'),
    passengerClasses: Yup.array()
      .min(1, 'Select at least one class')
      .required('Classes are required'),
  })

  useEffect(() => {
    if (action === 'edit' && data) {
      setInitialValues({
        trainName: data.trainName || '',
        seatCapacity: data.seatCapacity || '',
        availableDays: data.availableDays || '',
        passengerClasses: data.passengerClasses
          ? data.passengerClasses.map((id) => id.toString())
          : [],
      })
    }
  }, [action, data])

  const handleSubmit = async (formData) => {
    try {
      const passengers = formData.passengerClasses.map((str) =>
        parseInt(str, 10),
      )

      const payload = {
        id: action === 'edit' ? id : '',
        trainName: formData.trainName,
        seatCapacity: Number(formData.seatCapacity),
        availableDays: Number(formData.availableDays),
        passengerClasses: passengers,
      }

      const response = await TrainsAPIService.createTrain(payload)
      if (response) {
        console.log(response)
        await setTrainData(response)
        // await callBackData()
        await setStep('schedule')
      } else {
        console.log(response)
      }
    } catch (e) {
      console.log(e)
    }
  }

  return (
    <MainLayout>
      <center>
        <StepWizard
          currentActive={step === 'train' ? 1 : 2}
          leftStepperTitle={'Train Creation'}
          rightStepperTitle={'Assign Schedule/s'}
        />
      </center>
      {step === 'train' ? (
        <div
          className="train-box"
          style={{
            backgroundColor: 'rgba(126, 90, 233, 0.2)',
            padding: '20px',
            borderRadius: '20px',
          }}
        >
          <div className="train-header">
            <h2 style={{ fontWeight: 900 }}>
              {action === 'edit' ? 'Edit Train' : 'Add Train'}
            </h2>
          </div>
          <hr />
          {showForm ? (
            <Formik
              initialValues={initialValues}
              validationSchema={userSchema}
              onSubmit={handleSubmit}
            >
              {({ isValid, handleSubmit, values, setFieldValue }) => (
                <Form onSubmit={handleSubmit}>
                  <Row>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Train Name*</Form.Label>
                        <Field name="trainName">
                          {({ field }) => <Form.Control {...field} />}
                        </Field>
                        <ErrorMessage
                          name="trainName"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Seating Capacity*</Form.Label>
                        <Field name="seatCapacity">
                          {({ field }) => <Form.Control {...field} />}
                        </Field>
                        <ErrorMessage
                          name="seatCapacity"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                  </Row>
                  <Row style={{ marginTop: '10px' }}>
                    <Col sm={12}>
                      <Form.Group style={{}}>
                        <Form.Label>Available Day/s*</Form.Label>
                        <Field
                          name="availableDays"
                          as="select"
                          style={{
                            width: '100%',
                            height: '37px',
                            border: '1px solid #dee2e6',
                            borderRadius: '0.375rem',
                          }}
                        >
                          {TRAIN_AVAILABLE_DAYS.map((item, index) => (
                            <option key={index} value={item.id}>
                              {item.name}
                            </option>
                          ))}
                        </Field>
                        <ErrorMessage
                          name="availableDays"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                  </Row>

                  <Row style={{ marginTop: '20px' }}>
                    <Col sm={12}>
                      <Form.Group style={{}}>
                        <Form.Label>Passenger Classes*</Form.Label>
                        {TRAIN_PASSENGER_CLASSES.filter((d) => d.id !== 0).map(
                          (item, index) => (
                            <div key={index}>
                              <label>
                                <input
                                  type="checkbox"
                                  name="passengerClasses"
                                  value={item.id}
                                  checked={values.passengerClasses.includes(
                                    item.id.toString(),
                                  )}
                                  onChange={(e) => {
                                    const isChecked = e.target.checked
                                    const newValue = isChecked
                                      ? [
                                          ...values.passengerClasses,
                                          item.id.toString(),
                                        ]
                                      : values.passengerClasses.filter(
                                          (id) => id !== item.id.toString(),
                                        )
                                    setFieldValue('passengerClasses', newValue)
                                  }}
                                />
                                {item.name}
                              </label>
                            </div>
                          ),
                        )}
                        <ErrorMessage
                          name="passengerClasses"
                          component="div"
                          className="text-danger"
                        />
                      </Form.Group>
                    </Col>
                  </Row>
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
                          Next
                        </Button>
                      </Col>
                    </Row>
                  </div>
                </Form>
              )}
            </Formik>
          ) : (
            <div>
              <Spinner animation="grow" />
            </div>
          )}
        </div>
      ) : (
        <>
          <ScheduleList handleStep={setStep} trainData={trainData} />
        </>
      )}
    </MainLayout>
  )
}
