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
  const [selectedDestination, setSelectedDestination] = useState('')
  const [selectedArrivalStation, setSelectedArrivalStation] = useState('')
  const [selectedTrain, setSelectedTrain] = useState('')
  const [selectedDate, setSelectedDate] = useState('')
  const [selectedReservation, setSelectedReservation] = useState('')

  const getAllTrains = () => {}
  const getAllReservations = async () => {
    try {
      const payload = {
        // searchText: searchText,
        // status: filterByStatus,
        // availableDay: selectedAvailability,
        // passengerClass: selectedPassenger,
        // currentPage: currentPage,
        reservationNumber: selectedReservation,
        fromDate: '2023-10-01',
        toDate: '2023-10-12',
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
    setSettings({
      openDialog: true,
      action: 'edit',
      parentData: { id },
    })
  }

  const handleDeleteClick = (id) => {
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
        {selectedIds?.length > 0 ? (
          <DropdownStyledButton
            dropdownTitle={'Change status'}
            items={STATUS_LIST.filter((status) => status.showOnChange === true)}
            handleChange={handleStatusChange}
            selectedStatus={selectedStatus}
            changeMode={true}
          />
        ) : (
          <>
            <DropdownStyledButton
              dropdownTitle={'Filter by Status'}
              items={STATUS_LIST}
              handleChange={handleStatusFilter}
              selectedStatus={filterByStatus}
              changeMode={false}
            />
            <Dropdown>
              <Dropdown.Toggle
                id="dropdown-autoclose-true"
                style={{
                  backgroundColor: '#8428E2',
                  width: '200px',
                  height: '46px',
                  border: 'none',
                  borderRadius: '46px',
                }}
              >
                {selectedAvailability
                  ? TRAIN_AVAILABLE_DAYS.filter(
                      (d) => d.id === selectedAvailability,
                    )
                      .map((selectedClass) => selectedClass.name)
                      .join(', ')
                  : 'Availability'}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {TRAIN_AVAILABLE_DAYS?.map((item) => (
                  <Dropdown.Item
                    eventKey={item?.id}
                    onClick={() => handleAvailabilityFilter(item?.id)}
                  >
                    {item?.name}
                  </Dropdown.Item>
                ))}
              </Dropdown.Menu>
            </Dropdown>

            <Dropdown>
              <Dropdown.Toggle
                id="dropdown-autoclose-true"
                style={{
                  backgroundColor: '#8428E2',
                  width: '150px',
                  height: '46px',
                  border: 'none',
                  borderRadius: '46px',
                }}
              >
                {selectedPassenger
                  ? TRAIN_PASSENGER_CLASSES.filter(
                      (d) => d.id === selectedPassenger,
                    )
                      .map((selectedClass) => selectedClass.name)
                      .join(', ')
                  : 'Passenger'}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {TRAIN_PASSENGER_CLASSES?.map((item) => (
                  <Dropdown.Item
                    eventKey={item?.id}
                    onClick={() => handlePassengerFilter(item?.id)}
                  >
                    {item?.name}
                  </Dropdown.Item>
                ))}
              </Dropdown.Menu>
            </Dropdown>
          </>
        )}
      </div>
    )
  }

  return (
    <MainLayout loading={true} loadingTime={2000}>
      <div className="trains-container">
        <ConfirmationDialog
          title="Confirm Status Change"
          message={`Are you sure you want to change the status to ${
            STATUS_LIST.find((d) => d.id === selectedStatus)?.dropLabel ||
            'Unknown'
          }?`}
          show={showStatuConfirmationDialog}
          onHide={handleCloseConfirmationDialog}
          onConfirm={handleConfirmStatusChange}
          leftButton="Cancel"
          rightButton="Confirm"
        />
        <LayoutHeader
          title="Bookings"
          subtitle="Booking Management"
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
          deleteEnabled={false}
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
