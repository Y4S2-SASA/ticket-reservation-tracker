import React, { useEffect, useState } from 'react'
import { Modal, Button, Form, Row, Col, Spinner } from 'react-bootstrap'
import Tab from 'react-bootstrap/Tab';
import Tabs from 'react-bootstrap/Tabs';
import { Formik, Field, ErrorMessage } from 'formik'
import * as Yup from 'yup'
import {
    ROLES,
    TRAIN_AVAILABLE_DAYS,
    TRAIN_PASSENGER_CLASSES,
} from '../../configs/static-configs'
import TrainsAPIService from '../../api-layer/trains'
import Select, { components } from 'react-select'
import { Typeahead } from 'react-bootstrap-typeahead';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import 'react-bootstrap-typeahead/css/Typeahead.css';
import 'react-bootstrap-typeahead/css/Typeahead.bs5.css';
import 'react-datepicker/dist/react-datepicker-cssmodules.css';
import MasterDataAPIService from '../../api-layer/master-data'
import ScheduleAPIService from '../../api-layer/schedules'
import ReservationsAPIService from '../../api-layer/reservations'


const BookingDialog = ({ settings, onClose, onSave, callBackData }) => {
    const { openDialog, action, parentData } = settings
    const [data, setData] = useState(null)
    const [dataLoading, setDataLoading] = useState(false)
    const [selectedOption, setSelectedOption] = useState(null)
    const [formDataDestination, setFormDataDestination] = useState({});
    const [destinationStationId, setDestinationStationId] = useState("");
    const [originStationId, setOriginStationId] = useState("");
    const [formDataOrigin, setFormDataOrigin] = useState({});
    const [formDataStartDate, setFormDataStartDate] = useState(new Date());
    const [stations, setStations] = useState([])
    const [isStationsLoading, setStationsLoading] = useState(true);
    const [isScheduleAvailable, setShedulesAvailability] = useState(false);
    const [isSchedulesLoading, setSchedulesLoading] = useState(false);
    const [isPriceLoading, setPriceLoading] = useState(false);
    const [schedules, setSchedules] = useState([]);
    const [formDataselectedPClass, setFormDataSelectedPclass] = useState(1);
    const [selectedTrain, setSelectedTrain] = useState("{}");
    const [passengerCount, setPassengerCount] = useState(0);
    const [price, setPrice] = useState(0);
    const [readyToSubmit, setReadyToSubmit] = useState(false);


    const handleSelectTime = (e) => {
        setSelectedTrain(e.target.value)
        console.log(JSON.parse(e.target.value))
        console.log(selectedTrain)

    }

    const getTrainObj = (attri) => {
        try {
            const trainObj = JSON.parse(selectedTrain);
            return trainObj[attri]
        } catch (error) {
            return ""
        }
    }

    const getAllStations = async () => {
        try {
            setStationsLoading(true)
            const stationsRes = await MasterDataAPIService.getAllStationMasterData()
            const _stations = stationsRes.map(station => ({ label: station.name, id: station.id }));
            setStations(_stations);
        } catch (error) {
            console.log(error)
            setStations([]);
        } finally {
            setStationsLoading(false);
        }
        return stations;
    }

    const checkPrice = async () => {
        try {
            setPriceLoading(true)
            const reqObj = {
                selectedTrainId: getTrainObj("trainId"),
                departureStationId: formDataDestination[0].id,
                arrivalStationId: formDataOrigin[0].id,
                selectedScheduleId: getTrainObj("scheduleId"),
                passengerCount: parseInt(passengerCount),
                passengerClass: parseInt(formDataselectedPClass)
            }
            // setSchedulesLoading(true)
            const price = await ScheduleAPIService.getSchedulePrice(reqObj)
            setPrice(price)
            setReadyToSubmit(true)
        } catch (error) {
            console.log(error)
            setReadyToSubmit(true)
        } finally {
            setPriceLoading(false)
        }
    }

    const checkTrainAvailability = async () => {
        try {
            const reqObj = {
                destinationStationId: formDataDestination[0].id,
                startPointStationId: formDataOrigin[0].id,
                dateTime: formDataStartDate.toLocaleDateString('en-CA'),
                passengerClass: parseInt(formDataselectedPClass),
            }
            setSchedulesLoading(true)
            const _schedules = await ScheduleAPIService.getScheduleTrainsData(reqObj)
            if (Array.isArray(_schedules) && _schedules.length > 0) {
                setShedulesAvailability(true)
            } else {
                setShedulesAvailability(false)
            }
            console.log(_schedules)
            setSchedules(_schedules)
        } catch (error) {
            console.log(error)
            setSchedules([])
        } finally {
            setSchedulesLoading(false)
        }
    }

    useEffect(() => {
        
        const getById = async () => {
            setDataLoading(true)
            const response = await ReservationsAPIService.getReservation(parentData.id)
            if (response) {
                await setData(response)
                await getAllStations()
                await setPriceLoading(false)
                await setShedulesAvailability(true)
                await setSchedulesLoading(false)
                await setReadyToSubmit(true)
                await setDestinationStationId(response.destinationStationId)
                await setOriginStationId(response.arrivalStationId)
                
              
            }
            await setDataLoading(false)
            console.log(response)
        }

        const setStationValues = (destinationStationId, arrivalStationId) => {
            console.log(destinationStationId)
            setFormDataOrigin(stations.map(station => arrivalStationId == station.id))
            setFormDataDestination(stations.map(station => destinationStationId == station.id))
        }
        if (action === 'edit') {
            getById()
        } else {
            getAllStations()
        }
    }, [action])

    const userSchema = Yup.object().shape({
        trainName: Yup.string().required('Name is required'),
        seatCapacity: Yup.number().required('Capacity is required'),
        availableDays: Yup.number().required('Availability is required'),
        passengerClasses: Yup.array()
            .min(1, 'Select at least one class')
            .required('Classes are required'),
    })

    const initialValues = {

        id: action === 'edit' ? data?.id : '',
        referenceNumber: action === 'edit' ? data?.referenceNumber : '',
        trainName: action === 'edit' ? data?.trainName : '',
        passengerClass: action === 'edit' ? data?.passengerClass : '',
        destinationStationName: action === 'edit' ? data?.destinationStationName : '',
        arrivalStationName: action === 'edit' ? data?.arrivalStationName : '',
        dateTime: action === 'edit' ? data?.dateTime : '',
        seatCapacity: action === 'edit' ? data?.noOfPassengers : '',
        price: action === 'edit' ? data?.price : '',
        // dateTime: action === 'edit' ? new Date(data?.dateTime) : new Date(),
    }

    const handleSubmit = async (formData) => {
        console.log("submitting")
        try {
            const payload = {
                id: action === 'edit' ? parentData.id : '',
                referenceNumber: action === 'edit' ? parentData.referenceNumber : '',
                trainId: getTrainObj("trainId"),
                passengerClass: formDataselectedPClass,
                destinationStationId: formDataDestination[0].id,
                arrivalStationId: formDataOrigin[0].id,
                dateTime: formDataStartDate,
                noOfPassengers: parseInt(passengerCount),
                price: price
            }
            const response = await ReservationsAPIService.saveReservation(payload)
            if (response) {
                console.log(response)
                // await callBackData()
                // await onClose()
            } else {
                console.log(response)
            }
        } catch (e) {
            console.log(e)
        }
    }

    const _handleSubmit = async (formData) => {
        console.log("submitting")
        try {
            const payload = {
                id: action === 'edit' ? parentData.id : '',
                referenceNumber: action === 'edit' ? parentData.referenceNumber : '',
                trainId: getTrainObj("trainId"),
                passengerClass: parseInt(formDataselectedPClass),
                destinationStationId: formDataDestination[0].id,
                arrivalStationId: formDataOrigin[0].id,
                dateTime: formDataStartDate,
                noOfPassengers: passengerCount,
                price: price
            }
            console.log(JSON.stringify(payload))
            const response = await ReservationsAPIService.saveReservation(payload)
            if (response) {
                console.log(response)
                // await callBackData()
                // await onClose()
            } else {
                console.log(response)
            }
        } catch (e) {
            console.log(e)
        }
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
                    {action === 'edit' ? 'Edit Reservation' : 'Add Reservation'}
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
                        {({ isValid, handleSubmit, values, setFieldValue }) => (
                            <>
                                <Form onSubmit={handleSubmit}>

                                    <Row>
                                        <Col sm={6}>
                                            <Form.Group>
                                                <Form.Label>Destination station*</Form.Label>
                                                <Typeahead
                                                    clearButton
                                                    id="des"
                                                    defaultSelected={stations.filter(station => station.id === destinationStationId)}
                                                    name="destinationStationName"
                                                    onChange={destination => setFormDataDestination(destination)}
                                                    isLoading={isStationsLoading}
                                                    disabled={isStationsLoading}
                                                    options={stations}
                                                    placeholder="Choose destination"
                                                />
                                            </Form.Group>
                                        </Col>
                                        <Col sm={6}>
                                            <Form.Group>
                                                <Form.Label>Origin station*</Form.Label>
                                                <Typeahead
                                                    clearButton
                                                    id="ori"
                                                    defaultSelected={stations.filter(station => station.id === originStationId)}
                                                    name="arrivalStationName"
                                                    onChange={_origin => setFormDataOrigin(_origin)}
                                                    isLoading={isStationsLoading}
                                                    disabled={isStationsLoading}
                                                    options={stations}
                                                    placeholder="Choose origin station"
                                                />
                                            </Form.Group>
                                        </Col>
                                    </Row>
                                    <Row style={{ marginTop: '10px' }}>
                                        <Col sm={6}>
                                            <Form.Group style={{}}>
                                                <Form.Label>Passenger class*</Form.Label>
                                                <Field
                                                    onChange={e => setFormDataSelectedPclass(e.target.value)}
                                                    value={formDataselectedPClass}
                                                    name="passengerClass"
                                                    as="select"
                                                    style={{
                                                        width: '100%',
                                                        height: '37px',
                                                        border: '1px solid #dee2e6',
                                                        borderRadius: '0.375rem',
                                                    }}
                                                >
                                                    {TRAIN_PASSENGER_CLASSES.map((item, index) => (
                                                        <option key={index} value={item.id}>
                                                            {item.name}
                                                        </option>
                                                    ))}

                                                </Field>
                                            </Form.Group>
                                        </Col>
                                        <Col sm={6}>
                                            <Form.Group style={{}}>
                                                <Form.Label>Booking date*</Form.Label>
                                                <DatePicker
                                                    name="dateTime"
                                                    selected={formDataStartDate}
                                                    onChange={(date) => setFormDataStartDate(date)}
                                                    minDate={new Date()}
                                                    maxDate={new Date().setDate(new Date().getDate() + 30)}
                                                />
                                            </Form.Group>
                                        </Col>
                                    </Row>
                                    <Col>
                                        <div
                                            style={{
                                                display: 'flex',
                                                justifyContent: 'flex-end',
                                                marginTop: '20px',
                                            }}
                                        >
                                            <Row>
                                                <Col sm={12}>
                                                    <Button
                                                        variant="secondary"
                                                        onClick={checkTrainAvailability}
                                                        style={{
                                                            border: 'none',
                                                            borderRadius: '46px',
                                                            marginLeft: '10px',
                                                        }}
                                                    >
                                                        {isSchedulesLoading ? <Spinner /> : "Check Availability"}
                                                    </Button>

                                                </Col>

                                            </Row>
                                        </div>
                                    </Col>
                                    {isScheduleAvailable && !isSchedulesLoading &&
                                        <>
                                            <Row style={{ marginTop: '10px' }}>
                                                <Col sm={6}>
                                                    <Form.Group style={{}}>
                                                        <Form.Label>Time*</Form.Label>
                                                        <Field
                                                            onChange={e => handleSelectTime(e)}
                                                            value={selectedTrain.arrivalTime}
                                                            name="time"
                                                            as="select"
                                                            style={{
                                                                width: '100%',
                                                                height: '37px',
                                                                border: '1px solid #dee2e6',
                                                                borderRadius: '0.375rem',
                                                            }}
                                                        >
                                                            {schedules.map((train, index) => (
                                                                <option key={index} value={JSON.stringify(train)}>
                                                                    {new Date(train.arrivalTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' })}
                                                                </option>
                                                            ))}

                                                        </Field>
                                                    </Form.Group>
                                                </Col>
                                                <Col sm={6}>
                                                    <Form.Group style={{}}>
                                                        <Form.Label>Train</Form.Label>

                                                        <Form.Control name="trainName" value={getTrainObj("trainName")} />



                                                    </Form.Group>

                                                </Col>
                                            </Row>
                                            <Row style={{ marginTop: '10px' }}>
                                                <Col sm={12}>
                                                    <Form.Group>
                                                        <Form.Label>Passenger count*</Form.Label>
                                                        <Field name="seatCapacity">
                                                            {({ field }) => <Form.Control {...field} onChange={(e) => {
                                                                setPassengerCount(e.target.value);
                                                                field.onChange(e);
                                                            }} />}
                                                        </Field>
                                                        <ErrorMessage
                                                            name="seatCapacity"
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
                                                    <Col sm={12}>
                                                        <Button
                                                            variant="secondary"
                                                            onClick={checkPrice}
                                                            style={{
                                                                border: 'none',
                                                                borderRadius: '46px',
                                                                marginLeft: '10px',
                                                            }}
                                                        >
                                                            {isPriceLoading ? <Spinner /> : "Check Price"}
                                                        </Button>
                                                    </Col>
                                                </Row>
                                            </div>
                                            {readyToSubmit &&
                                                <Row>
                                                    <Col sm={12}>
                                                        <Form.Group style={{}}>
                                                            <Form.Label>Price</Form.Label>

                                                            <Form.Control name="price" value={price} />



                                                        </Form.Group>

                                                    </Col>
                                                </Row>
                                            }


                                        </>
                                    }
                                    {readyToSubmit &&
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
                                                        variant="secondary"
                                                        //type="submit"
                                                        onClick={() => _handleSubmit()}
                                                        //disabled={!isValid}
                                                        style={{
                                                            backgroundColor: '#8428E2',
                                                            border: 'none',
                                                            borderRadius: '46px',
                                                        }}
                                                    >
                                                        Saveo
                                                    </Button>
                                                </Col>
                                            </Row>
                                        </div>}
                                </Form>
                            </>
                        )}
                    </Formik>
                )}
            </Modal.Body>
        </Modal>
    )
}

export default BookingDialog
