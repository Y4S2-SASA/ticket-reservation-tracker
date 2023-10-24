/*
 * File: BookingList.js
 * Author: Bartholomeusz S.V/IT20274702
 */

import React, { useEffect, useState } from 'react'
import StyledTable from '../../components/TMTable'
import MainLayout from '../../components/Layouts/MainLayout'
import LayoutHeader from '../../components/Layouts/LayoutHeader'
import { TRAIN_AVAILABLE_DAYS } from '../../configs/static-configs'
import { Button, Form, InputGroup } from 'react-bootstrap'
import { FaPlus } from 'react-icons/fa'
import { RESERVATION_HEADERS } from '../../configs/dataConfig'
import BookingDialog from './BookingDialog'
import ReservationsAPIService from '../../api-layer/reservations'
import { ToastContainer, toast } from 'react-toastify'

export default function BookingsList() {
  const [dataLoading, setDataLoading] = useState([])
  const [selectedIds, setSelectedIds] = useState([])
  const [selectAll, setSelectAll] = useState(false)
  const [searchParameters, setSearchParameers] = useState({
    pageSize: 10,
    hasNextPage: false,
    hasPreviousPage: false,
    totalRecordCount: 0,
    totalPages: 0,
  })
  const [currentPage, setCurrentPage] = useState(0)
  const [filteredData, setFilteredData] = useState([])
  const [searchText, setSearchText] = useState('')
  const [settings, setSettings] = useState({
    openDialog: false,
    action: '',
    parentData: null,
  })
  const [fromDate, setFromDate] = useState(null)
  const [toDate, setToDate] = useState(null)

  const ableToDeleteEdit = (id) => {
    const reservation = filteredData.filter((data) => data.id === id)

    if (reservation) {
      const reservationDate = new Date(reservation[0]?.dateTime)
      const today = new Date()
      const timeDifference = reservationDate - today
      const daysDifference = timeDifference / (1000 * 60 * 60 * 24)
      console.log(daysDifference)
      if (daysDifference >= 5) {
        return true
      } else {
        return false
      }
    } else {
      toast.info('Error. Try Again')
      return false
    }
  }

  const deleteReservation = async (id) => {
    if (ableToDeleteEdit(id)) {
      const response = await ReservationsAPIService.deleteReservation(id)
      if (response) {
        await toast.success('Reservation deleted')
        getAllReservations()
      } else {
        toast.error('Something went wrong')
        alert('Something went wrong')
      }
    } else {
      toast.info(
        'Reservation date must be 5 days or more earlier to eligilble to edit or delete',
      )
      const response = await ReservationsAPIService.deleteReservation(id)
      // if (response) {
      //     alert("Reservation deleted");
      //     getAllReservations();
      // } else {
      //     alert("Something went wrong")
      // }
    }
  }

  const getAllReservations = async () => {
    setDataLoading(true)
    try {
      const payload = {
        reservationNumber: searchText || '',
        fromDate: fromDate,
        toDate: toDate,
        trainId: '',
        destinationStationId: '',
        arrivalStationId: '',
        status: 0,
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
      setDataLoading(false)
    }
  }

  useEffect(() => {
    getAllReservations()
  }, [currentPage, searchText, fromDate, toDate])

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
      toast.info(
        'Reservation date must be 5 days or more earlier to eligilble to edit or delete',
      )
    }
  }

  const handleDeleteClick = (id) => {
    deleteReservation(id)
    console.log(`Delete clicked for ID: ${id}`)
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

  const buttonComp = () => {
    return (
      <div className="train-head-btn-group">
        <InputGroup>
          <Form.Control
            type="text"
            placeholder="Search Reservations"
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

        <label
          htmlFor="dateInputFrom"
          style={{ color: 'black', padding: '10px 0px' }}
        >
          From:
        </label>

        <input
          type="date"
          id="dateInputFrom"
          onChange={(e) => setFromDate(e.target.value)}
          value={fromDate}
          style={{
            backgroundColor: '#8428E2',
            height: '46px',
            border: 'none',
            borderRadius: '46px',
            padding: '10px',
            color: 'white',
            width: '300px',
          }}
        />

        <label
          htmlFor="dateInputTo"
          style={{ color: 'black', padding: '10px 0px' }}
        >
          To:
        </label>

        <input
          type="date"
          id="dateInputTo"
          onChange={(e) => setToDate(e.target.value)}
          value={toDate}
          style={{
            backgroundColor: '#8428E2',
            height: '46px',
            border: 'none',
            borderRadius: '46px',
            padding: '10px',
            color: 'white',
            width: '300px',
          }}
        />
      </div>
    )
  }

  return (
    <MainLayout loading={true} loadingTime={2000}>
      <ToastContainer />
      <div className="trains-container">
        {/* <ConfirmationDialog
                    title="Confirm Status Change"
                    message={`Are you sure you want to change the status to ${STATUS_LIST.find((d) => d.id === selectedStatus)?.dropLabel ||
                        'Unknown'
                        }?`}
                    show={showStatuConfirmationDialog}
                    onHide={handleCloseConfirmationDialog}
                    onConfirm={handleConfirmStatusChange}
                    leftButton="Cancel"
                    rightButton="Confirm"
                /> */}
        <LayoutHeader
          title="Reservations"
          subtitle="Reservation Management"
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
          isLoadingEnabaled={true}
          isLoading={dataLoading}
        />
        <div>
          {settings.openDialog && (
            <BookingDialog
              settings={settings}
              onClose={onCloseDialog}
              onSave={onCloseDialog}
              callBackData={getAllReservations}
            />
          )}
        </div>
      </div>
    </MainLayout>
  )
}
