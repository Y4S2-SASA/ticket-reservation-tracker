import React, { Fragment, useEffect, useState } from 'react'
import StyledTable from '../../components/TMTable'
import Loader from '../../components/TMLoader'
import MainLayout from '../../components/Layouts/MainLayout'
import LayoutHeader from '../../components/Layouts/LayoutHeader'
import DropdownStyledButton from '../../components/TMDropdownButton'
import {
    STATUS_LIST,
    TRAIN_AVAILABLE_DAYS,
    TRAIN_PASSENGER_CLASSES,
} from '../../configs/static-configs'
import { Button, Dropdown, Form, InputGroup } from 'react-bootstrap'
import { FaPlus } from 'react-icons/fa'
import { RESERVATION_HEADERS } from '../../configs/dataConfig'
import BookingDialog from './BookingDialog'
import ConfirmationDialog from '../../components/TMConfirmationDialog'
import ReservationsAPIService from '../../api-layer/reservations'
import TrainsAPIService from '../../api-layer/trains'

export default function BookingsList() {
    const [selectedIds, setSelectedIds] = useState([])
    const [selectAll, setSelectAll] = useState(false)
    const [selectedStatus, setSelectedStatus] = useState(null)
    const [searchParameters, setSearchParameers] = useState({
        pageSize: 10,
        hasNextPage: false,
        hasPreviousPage: false,
        totalRecordCount: 0,
        totalPages: 0,
    })
    const [currentPage, setCurrentPage] = useState(0)
    const [filterByStatus, setFilterByStatus] = useState(0)
    const [filteredData, setFilteredData] = useState([])
    const [searchText, setSearchText] = useState('')
    const [settings, setSettings] = useState({
        openDialog: false,
        action: '',
        parentData: null,
    })
    const [
        showStatuConfirmationDialog,
        setShowStatuConfirmationDialog,
    ] = useState(false)
    const [selectedAvailability, setSelectedAvailability] = useState(0)
    const [selectedPassenger, setSelectedPassenger] = useState(0)
    const [selectedDestination, setSelectedDestination] = useState("")
    const [selectedArrivalStation, setSelectedArrivalStation] = useState("")
    const [selectedTrain, setSelectedTrain] = useState("")
    const [selectedDate, setSelectedDate] = useState("");
    const [selectedReservation, setSelectedReservation] = useState("");
    const [fromDate, setFromDate] = useState(null);
    const [toDate, setToDate] = useState(null);

    const getAllTrains = () => { }

    const ableToDeleteEdit = (id) => {
        const reservation = filteredData.filter(data => data.id === id)

        const reservationDate = new Date(reservation.dateTime);
        const today = new Date();  
        const timeDifference = today - reservationDate; 
        const daysDifference = timeDifference / (1000 * 60 * 60 * 24);

        if (daysDifference >= 5) {
            return true
        } else {
            return false
        }
    }
    const deleteReservation = async (id) => {
      if (ableToDeleteEdit(id)) {
        const response = await ReservationsAPIService.deleteReservation(id);
        if (response) {
            alert("Reservation deleted");
            getAllReservations();
        } else {
            alert("Something went wrong")
        }
      } else {
        alert("Reservation date must be 5 days or more earlier to eligilble to edit or delete")
        // const response = await ReservationsAPIService.deleteReservation(id);
        // if (response) {
        //     alert("Reservation deleted");
        //     getAllReservations();
        // } else {
        //     alert("Something went wrong")
        // }
      }
    }

    const getAllReservations = async () => {
        try {
            const payload = {
                reservationNumber: selectedReservation,
                fromDate: fromDate,
                toDate: toDate,
                trainId: selectedTrain,
                destinationStationId: selectedDestination,
                arrivalStationId: selectedArrivalStation,
                status: filterByStatus,
                currentPage: currentPage,
                pageSize: 10,
            }

            const response = await ReservationsAPIService.getReservations(payload)
            if (response) {
                setFilteredData(response.items)
                setSearchParameers({
                    ...searchParameters,
                    hasNextPage: response.hasNextPage,
                    hasPreviousPage: response.hasPreviousPage,
                    totalRecordCount: response.totalRecordCount,
                    totalPages: response.totalPages,
                })
            } else {
                setFilteredData([])
            }
        } catch (e) {
        } finally {
        }
    }

    useEffect(() => {
        //getAllTrains()
        getAllReservations()
    }, [
        currentPage,
        searchText,
        filterByStatus,
        selectedDestination,
        selectedArrivalStation,
        selectedTrain,
        selectedDate,
        selectedReservation,
        fromDate,
        toDate
    ])

    const handleCheckboxChange = (id) => {
        if (selectedIds.includes(id)) {
            setSelectedIds(selectedIds.filter((selectedId) => selectedId !== id))
        } else {
            setSelectedIds([...selectedIds, id])
        }
    }

    const handleSelectAllChange = () => {
        if (selectAll) {
            setSelectedIds([])
        } else {
            const allIds = filteredData.map((row) => row.id)
            setSelectedIds(allIds)
        }
        setSelectAll(!selectAll)
    }

    const handleEditClick = (id) => {
        console.log(`Edit clicked for ID: ${id}`)
        if (ableToDeleteEdit(id)) {
            setSettings({
                openDialog: true,
                action: 'edit',
                parentData: { id },
            })
        } else {
          alert("Reservation date must be 5 days or more earlier to eligilble to edit or delete")
        }
    }

    const handleDeleteClick = (id) => {
        deleteReservation(id)
        console.log(`Delete clicked for ID: ${id}`)
    }

    const handleStatusChange = (itemId) => {
        setSelectedStatus(itemId)
        setShowStatuConfirmationDialog(true)
    }

    const handleConfirmStatusChange = async () => {
        selectedIds.forEach(async (tId) => {
            const payload = {
                id: tId,
                status: selectedStatus,
            }
            const response = await TrainsAPIService.updateTrainStatus(payload)
            if (response) {
                await getAllTrains()
                await setSelectedIds([])
            }
            console.log(response)
        })

        setShowStatuConfirmationDialog(false)
    }

    const handleCloseConfirmationDialog = () => {
        setShowStatuConfirmationDialog(false)
    }

    const handleStatusFilter = (itemId) => {
        setFilterByStatus(itemId)
    }

    const handleAvailabilityFilter = (itemId) => {
        setSelectedAvailability(itemId)
    }

    const handlePassengerFilter = (itemId) => {
        setSelectedPassenger(itemId)
    }

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage)
    }

    const handleSearchInputChange = async (e) => {
        await setSearchText(e.target.value)
        await setCurrentPage(0)
    }

    const onCloseDialog = () => {
        setSettings({
            openDialog: false,
            action: '',
            parentData: null,
        })
    }

    console.log('TRAIN_AVAILABLE_DAYS', TRAIN_AVAILABLE_DAYS)

    const buttonComp = () => {
        return (
            <div className="train-head-btn-group">
                <InputGroup>

                    <Form.Control
                        type="text"
                        placeholder="Search Trains"
                        value={searchText}
                        onChange={(e) => handleSearchInputChange(e)}
                        style={{
                            height: '46px',
                            border: 'none',
                            borderRadius: '46px',
                        }}
                    />
                </InputGroup>
                <Button
                    style={{
                        height: '46px',
                        backgroundColor: '#7E5AE9',
                        border: 'none',
                        borderRadius: '46px',
                        width: '200px',
                    }}
                    onClick={() =>
                        setSettings({
                            openDialog: true,
                            action: 'add',
                            parentData: null,
                        })
                    }
                >
                    <FaPlus /> Add
                </Button>




                <label htmlFor="dateInputFrom" style={{ color: 'black', padding: '10px 0px' }}>From:</label>


                <input
                    type="date"
                    id="dateInputFrom"
                    onChange={e => setFromDate(e.target.value)}
                    value={fromDate}
                    style={{
                        backgroundColor: '#8428E2',
                        height: '46px',
                        border: 'none',
                        borderRadius: '46px',
                        padding: '10px',
                        color: 'white',
                        width: '300px'
                    }}
                />

                <label htmlFor="dateInputTo" style={{ color: 'black', padding: '10px 0px' }}>To:</label>


                <input
                    type="date"
                    id="dateInputTo"
                    onChange={e => setToDate(e.target.value)}
                    value={toDate}
                    style={{
                        backgroundColor: '#8428E2',
                        height: '46px',
                        border: 'none',
                        borderRadius: '46px',
                        padding: '10px',
                        color: 'white',
                        width: '300px'
                    }}
                />

            </div>
        )
    }

    return (
        <MainLayout loading={true} loadingTime={2000}>
            <div className="trains-container">
                <ConfirmationDialog
                    title="Confirm Status Change"
                    message={`Are you sure you want to change the status to ${STATUS_LIST.find((d) => d.id === selectedStatus)?.dropLabel ||
                        'Unknown'
                        }?`}
                    show={showStatuConfirmationDialog}
                    onHide={handleCloseConfirmationDialog}
                    onConfirm={handleConfirmStatusChange}
                    leftButton="Cancel"
                    rightButton="Confirm"
                />
                <LayoutHeader
                    title="Trains"
                    subtitle="Train Management"
                    buttonComponent={buttonComp()}
                />
                <StyledTable
                    headers={RESERVATION_HEADERS}
                    data={filteredData}
                    searchParameters={searchParameters}
                    selectedIds={selectedIds}
                    selectAll={selectAll}
                    onCheckboxChange={handleCheckboxChange}
                    onSelectAllChange={handleSelectAllChange}
                    editEnabled={true}
                    deleteEnabled={true}
                    onEditClick={handleEditClick}
                    onDeleteClick={handleDeleteClick}
                    handlePageChange={handlePageChange}
                    currentPage={currentPage}
                />
                <div>
                    {settings.openDialog && (
                        <BookingDialog
                            settings={settings}
                            onClose={onCloseDialog}
                            onSave={onCloseDialog}
                            callBackData={getAllTrains}
                        />
                    )}
                </div>
            </div>
        </MainLayout>
    )
}
