/*
 * File: ScheduleList.js
 * Author: Jayathilake S.M.D.A.R./IT20037338
 */

import React, { Fragment, useEffect, useState } from 'react'

import { Button, Col, Dropdown, Form, InputGroup, Row } from 'react-bootstrap'
import { FaPlus } from 'react-icons/fa'

import TrainsAPIService from '../../../api-layer/trains'
import TrainDialog from './ScheduleDialog'
import StyledTable from '../../../components/TMTable'
import MainLayout from '../../../components/Layouts/MainLayout'
import LayoutHeader from '../../../components/Layouts/LayoutHeader'
import DropdownStyledButton from '../../../components/TMDropdownButton'
import {
  STATUS_LIST,
  TRAIN_AVAILABLE_DAYS,
  TRAIN_PASSENGER_CLASSES,
} from '../../../configs/static-configs'
import { SCHEDULE_HEADERS } from '../../../configs/dataConfig'
import ConfirmationDialog from '../../../components/TMConfirmationDialog'
import { useHistory } from 'react-router-dom'
import ScheduleAPIService from '../../../api-layer/schedules'
import MasterDataAPIService from '../../../api-layer/master-data'
import { ToastContainer, toast } from 'react-toastify'

export default function ScheduleList({ handleStep, trainData }) {
  const history = useHistory()
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
  const [stations, setStations] = useState([])

  useEffect(() => {
    const getStations = async () => {
      try {
        const res = await MasterDataAPIService.getAllStationMasterData()
        const newStation = { id: '', name: 'All' }
        const updatedStations = [newStation, ...res]
        setStations(updatedStations)
      } catch (e) {
        console.log(e)
      }
    }
    getStations()
  }, [])

  const getAllSchedules = async () => {
    try {
      const payload = {
        trainId: trainData?.id,
        departureStationId: selectedAvailability ? selectedAvailability : '',
        status: filterByStatus,
        arrivalStationId: selectedPassenger ? selectedPassenger : '',
        currentPage: currentPage,
        pageSize: 10,
      }

      const response = await ScheduleAPIService.getSchedules(payload)
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
    getAllSchedules()
  }, [currentPage, filterByStatus, selectedAvailability, selectedPassenger])

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
      const response = await ScheduleAPIService.updateScheduleStatus(payload)
      if (response) {
        await toast.success(
          response?.successMessage || 'Successfully changed the status',
        )
        await getAllSchedules()
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
    toast.success('Successfully filtered by Status')
  }

  const handleDepartureFilter = (itemId) => {
    setSelectedAvailability(itemId)
    if (!selectedPassenger) {
      toast.info('Please choose an Arrival Destination. It is required!')
    }
  }

  const handleArrivalFilter = (itemId) => {
    setSelectedPassenger(itemId)
    if (!selectedAvailability) {
      toast.info('Please choose an Departure Destination. It is required!')
    }
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
                  ? stations
                      .filter((d) => d.id === selectedAvailability)
                      .map((selectedClass) => selectedClass.name)
                      .join(', ')
                  : 'Departure Station'}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {stations?.map((item) => (
                  <Dropdown.Item
                    eventKey={item?.id}
                    onClick={() => handleDepartureFilter(item?.id)}
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
                  ? stations
                      .filter((d) => d.id === selectedPassenger)
                      .map((selectedClass) => selectedClass.name)
                      .join(', ')
                  : 'Arrival Station'}
              </Dropdown.Toggle>

              <Dropdown.Menu>
                {stations?.map((item) => (
                  <Dropdown.Item
                    eventKey={item?.id}
                    onClick={() => handleArrivalFilter(item?.id)}
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
    <div className="trains-container">
      <ToastContainer />
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
        title="Schedules"
        subtitle="Schedule Management"
        buttonComponent={buttonComp()}
      />
      <StyledTable
        headers={SCHEDULE_HEADERS}
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
              onClick={() => handleStep('train')}
              style={{
                border: 'none',
                borderRadius: '46px',
                marginLeft: '10px',
                width: '100px',
              }}
            >
              Back
            </Button>
          </Col>
          <Col sm={6}>
            <Button
              variant="primary"
              type="button"
              onClick={() => (window.location.href = '/trains')}
              style={{
                backgroundColor: '#8428E2',
                border: 'none',
                borderRadius: '46px',
                width: '100px',
              }}
            >
              Done
            </Button>
          </Col>
        </Row>
      </div>

      <div>
        {settings.openDialog && (
          <TrainDialog
            settings={settings}
            onClose={onCloseDialog}
            onSave={onCloseDialog}
            callBackData={getAllSchedules}
            trainData={trainData}
          />
        )}
      </div>
    </div>
  )
}
