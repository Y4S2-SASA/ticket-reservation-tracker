/*
 * File: ScheduleDialog.js
 * Author: Jayathilake S.M.D.A.R./IT20037338
 */

import React, { Fragment, useEffect, useState } from 'react'
import { Modal, Button, Form, Row, Col, Spinner, Table } from 'react-bootstrap'
import { Formik, Field, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import { getObjectById } from '../../../configs/static-configs'
import TrainsAPIService from '../../../api-layer/trains'
import { Typeahead } from 'react-bootstrap-typeahead'
import DatePicker from 'react-datepicker'
import 'react-datepicker/dist/react-datepicker.css'
import 'react-bootstrap-typeahead/css/Typeahead.css'
import 'react-bootstrap-typeahead/css/Typeahead.bs5.css'
import 'react-datepicker/dist/react-datepicker-cssmodules.css'
import MasterDataAPIService from '../../../api-layer/master-data'
import ScheduleAPIService from '../../../api-layer/schedules'
import { ToastContainer, toast } from 'react-toastify'

const ScheduleDialog = ({ settings, onClose, trainData, callBackData }) => {
  const { openDialog, action, parentData } = settings
  const [data, setData] = useState(null)
  const [dataLoading, setDataLoading] = useState(false)
  const [stations, setStations] = useState([])
  const [trains, setTrains] = useState([])
  const [selectedStations, setSelectedStations] = useState(null)
  const [departureTime, setDepartureTime] = useState(new Date())
  const [arrivalTime, setArrivalTime] = useState(new Date())
  const [filteredStations, setFilteredStations] = useState([])
  const [selectedTrain, setSelectedTrain] = useState(null)
  const [selectedDepartureStation, setSelectedDepartureStation] = useState(null)
  const [selectedArrivalStation, setSelectedArrivalStation] = useState(null)
  const [subStationArrivalTime, setSubStationArrivalTime] = useState(new Date())
  const [subStationDetails, setSubStationDetails] = useState([])
  const [isStationsLoading, setStationsLoading] = useState(true)

  useEffect(() => {
    const getStations = async () => {
      try {
        setStationsLoading(true)
        const res = await MasterDataAPIService.getAllStationMasterData()
        setStations(res)
        setStationsLoading(false)
      } catch (e) {
        console.log(e)
        setStationsLoading(false)
      }
    }
    const getTrains = async () => {
      try {
        const res = await MasterDataAPIService.getTrainList()
        setTrains(res)
      } catch (e) {
        console.log(e)
      }
    }
    getStations()
    getTrains()
  }, [])

  useEffect(() => {
    if (selectedArrivalStation && selectedDepartureStation) {
      const filtered = stations.filter(
        (station) =>
          station.id !== selectedArrivalStation[0]?.id &&
          station.id !== selectedDepartureStation[0]?.id,
      )
      setFilteredStations(filtered)
    } else {
      setFilteredStations(stations)
    }
  }, [selectedArrivalStation, selectedDepartureStation, stations])

  useEffect(() => {
    const getById = async () => {
      setDataLoading(true)
      const response = await ScheduleAPIService.getScheduleById({
        id: parentData?.id,
      })
      if (response) {
        await setData(response)

        setTimeout(() => {
          setDataLoading(false)
        }, 1000)
      }
    }
    if (action === 'edit') {
      getById()
    }
  }, [action])

  useEffect(() => {
    if (action === 'edit') {
      setEditData()
    }
  }, [action, stations, data])

  const setEditData = async () => {
    try {
      setDataLoading(true)
      await setSelectedDepartureStation(
        getObjectById(data?.departureStationId, stations),
      )
      await setSelectedArrivalStation(
        getObjectById(data?.arrivalStationId, stations),
      )
      await processSubStationDetails(data).then(async (updatedArray) => {
        await setSubStationDetails(updatedArray)
      })
      await setArrivalTime(new Date(data?.arrivalTime))
      await setDepartureTime(new Date(data?.departureTime))

      setTimeout(() => {
        setDataLoading(false)
      }, 1000)
    } catch (e) {
      console.log(e)
      setTimeout(() => {
        setDataLoading(false)
      }, 1000)
    }
  }

  async function processSubStationDetails(response) {
    const updatedArray = await Promise.all(
      response.subStationDetails.map(async (detail) => {
        const stationObject = await getObjectById(detail.stationId, stations)
        console.log('stationObject', detail.arrivalTime)
        if (stationObject) {
          return {
            arrivalTime: new Date(detail.arrivalTime),
            stations: stationObject,
          }
        }
        return null
      }),
    )

    return updatedArray.filter((item) => item !== null)
  }

  const userSchema = Yup.object().shape({
    trainId: Yup.string(),
    departureStationId: Yup.string(),
    arrivalStationId: Yup.string(),
    departureTime: Yup.date(),
    arrivalTime: Yup.date(),
    subStationDetails: Yup.array(),
  })

  const initialValues = {
    trainId: action === 'add' ? '' : data?.trainId,
    departureStationId: action === 'add' ? '' : data?.departureStationId,
    arrivalStationId: action === 'add' ? '' : data?.arrivalStationId,
    departureTime: action === 'add' ? '' : data?.departureTime,
    arrivalTime: action === 'add' ? '' : data?.arrivalTime,
    subStationDetails: action === 'add' ? [] : data?.subStationDetails || [],
  }

  const handleSubmit = async () => {
    const modifiedSubStationData = subStationDetails.map((item) => ({
      stationId: item.stations[0].id,
      arrivalTime: item.arrivalTime.toISOString(),
    }))
    const payload = {
      id: action === 'edit' ? parentData?.id : '',
      trainId: trainData.id,
      departureStationId: selectedDepartureStation[0].id,
      arrivalStationId: selectedArrivalStation[0].id,
      departureTime: departureTime.toISOString(),
      arrivalTime: arrivalTime.toISOString(),
      subStationDetails: modifiedSubStationData,
    }

    try {
      const response = await ScheduleAPIService.createSchedule(payload)
      if (response) {
        await toast.success(
          response?.successMessage || 'Schedule Created Successfully',
        )
        await callBackData()
        await onClose()
      } else {
        console.log(response)
        toast.error(response?.message || 'Error try again')
      }
    } catch (e) {
      console.log(e)
    }
  }

  const handleChangeDepartureStation = (selected) => {
    setSelectedDepartureStation(selected)
  }

  const handleChangeArrivalStation = (selected) => {
    setSelectedArrivalStation(selected)
  }

  const handleChangeStations = (selected) => {
    setSelectedStations(selected)
  }

  const handleAddButtonClick = () => {
    if (selectedStations.length > 0) {
      const newSubStation = {
        stations: selectedStations,
        arrivalTime: subStationArrivalTime,
      }
      setSubStationDetails([...subStationDetails, newSubStation])

      setSelectedStations([])
      setSubStationArrivalTime(new Date())
    }
  }

  const handleRemoveButtonClick = (index) => {
    const updatedSubStationDetails = [...subStationDetails]
    updatedSubStationDetails.splice(index, 1)
    setSubStationDetails(updatedSubStationDetails)
  }

  return (
    <Fragment>
      <ToastContainer />
      <Modal
        show={openDialog}
        onHide={onClose}
        backdrop="static"
        size="lg"
        centered
        keyboard={false}
      >
        <Modal.Header closeButton>
          <Modal.Title>
            {action === 'edit' ? 'Edit Schedule' : 'Add Schedule'}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {action === 'edit' && dataLoading ? (
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
              {({ handleSubmit }) => (
                <Form onSubmit={handleSubmit}>
                  {/* <Row>
                  <Col sm={12}>
                    <Form.Group>
                      <Form.Label>Train</Form.Label>
                      <Typeahead
                        id="trainId"
                        labelKey="name"
                        options={trains}
                        selected={selectedTrain}
                        onChange={handleChangeTrains}
                        placeholder="Select Train"
                      />
                    </Form.Group>
                  </Col>
                </Row> */}

                  <Row style={{ marginTop: '10px' }}>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Departure Station*</Form.Label>
                        <Typeahead
                          id="departureStationId"
                          labelKey="name"
                          options={stations}
                          selected={selectedDepartureStation}
                          onChange={handleChangeDepartureStation}
                          placeholder="Select options..."
                          isLoading={isStationsLoading}
                          disabled={isStationsLoading}
                        />
                        {!selectedDepartureStation && (
                          <div style={{ color: 'red', fontSize: '14px' }}>
                            Departure station is Required!
                          </div>
                        )}
                      </Form.Group>
                    </Col>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Departure Time*</Form.Label>
                        <br />
                        <DatePicker
                          showTimeSelect
                          selected={departureTime}
                          onChange={(date) => setDepartureTime(date)}
                          dateFormat="Pp"
                          minDate={new Date()}
                          maxDate={new Date().setDate(
                            new Date().getDate() + 30,
                          )}
                        />
                        {!departureTime && (
                          <div style={{ color: 'red', fontSize: '14px' }}>
                            Departure time is Required!
                          </div>
                        )}
                      </Form.Group>
                    </Col>
                  </Row>

                  <Row style={{ marginTop: '10px' }}>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Arrival Station*</Form.Label>
                        <Typeahead
                          id="arrivalStationId"
                          labelKey="name"
                          options={stations}
                          selected={selectedArrivalStation}
                          onChange={handleChangeArrivalStation}
                          placeholder="Select options..."
                          isLoading={isStationsLoading}
                          disabled={isStationsLoading}
                        />
                        {!selectedArrivalStation && (
                          <div style={{ color: 'red', fontSize: '14px' }}>
                            Arrival station is Required!
                          </div>
                        )}
                      </Form.Group>
                    </Col>
                    <Col sm={6}>
                      <Form.Group>
                        <Form.Label>Arrival Time*</Form.Label>
                        <br />
                        <DatePicker
                          showTimeSelect
                          selected={arrivalTime}
                          onChange={(date) => setArrivalTime(date)}
                          dateFormat="Pp"
                          minDate={new Date()}
                          maxDate={new Date().setDate(
                            new Date().getDate() + 30,
                          )}
                        />
                        {!arrivalTime && (
                          <div style={{ color: 'red', fontSize: '14px' }}>
                            Arrival time is Required!
                          </div>
                        )}
                      </Form.Group>
                    </Col>
                  </Row>

                  <div
                    style={{
                      border: '1px solid #8428E2',
                      marginTop: '20px',
                      padding: '10px',
                      borderRadius: '10px',
                    }}
                  >
                    <h6 style={{ fontWeight: 700 }}>Sub Station/s Details</h6>
                    <hr />
                    <Row style={{}}>
                      <Col sm={6}>
                        <Form.Group style={{}}>
                          <Form.Label>Sub Station*</Form.Label>
                          <Typeahead
                            id="subStationDetails"
                            labelKey="name"
                            // multiple
                            options={filteredStations}
                            selected={selectedStations}
                            onChange={handleChangeStations}
                            placeholder="Select options..."
                            isLoading={isStationsLoading}
                            disabled={isStationsLoading}
                          />
                          <ErrorMessage
                            name="passengerClasses"
                            component="div"
                            className="text-danger"
                          />
                        </Form.Group>
                      </Col>
                      <Col sm={6}>
                        <Form.Group>
                          <Form.Label>Arrival Time*</Form.Label>
                          <br />
                          <DatePicker
                            showTimeSelect
                            selected={subStationArrivalTime}
                            onChange={(date) => setSubStationArrivalTime(date)}
                            dateFormat="Pp"
                            minDate={new Date()}
                            maxDate={new Date().setDate(
                              new Date().getDate() + 30,
                            )}
                          />
                        </Form.Group>
                      </Col>
                      {subStationDetails?.length === 0 && (
                        <div style={{ color: 'red', fontSize: '14px' }}>
                          Atleast one Sub Station should be assigned to the
                          schedule!
                        </div>
                      )}
                      <Button
                        onClick={handleAddButtonClick}
                        style={{
                          width: '200px',
                          marginTop: '10px',
                          alignSelf: 'right',
                          marginLeft: '10px',
                          backgroundColor: '#7E5AE9',
                          border: 'none',
                        }}
                        disabled={selectedStations?.length === 0}
                      >
                        Add
                      </Button>
                    </Row>
                  </div>

                  {subStationDetails.length > 0 && (
                    <Table striped bordered hover style={{ marginTop: '20px' }}>
                      <thead>
                        <tr>
                          <th>Station Name</th>
                          <th>Arrival Time</th>
                          <th>Action</th>
                        </tr>
                      </thead>
                      <tbody>
                        {subStationDetails.map((subStation, index) => (
                          <tr key={index}>
                            <td>
                              {subStation.stations
                                .map((station) => station.name)
                                .join(', ')}
                            </td>
                            <td>{subStation.arrivalTime.toLocaleString()}</td>
                            <td>
                              <Button
                                variant="danger"
                                onClick={() => handleRemoveButtonClick(index)}
                              >
                                X
                              </Button>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </Table>
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
                          disabled={
                            !selectedDepartureStation ||
                            !selectedArrivalStation ||
                            subStationDetails?.length === 0
                          }
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
    </Fragment>
  )
}

export default ScheduleDialog
