/*
 * File: TrainList.js
 * Author: Perera M.S.D/IT20020262
 */

import React, { Fragment, useEffect, useState } from 'react'

import { Button, Dropdown, Form, InputGroup } from 'react-bootstrap'
import { FaPlus } from 'react-icons/fa'

import TrainsAPIService from '../../../api-layer/trains'
import TrainDialog from './TrainDialog'
import StyledTable from '../../../components/TMTable'
import MainLayout from '../../../components/Layouts/MainLayout'
import LayoutHeader from '../../../components/Layouts/LayoutHeader'
import DropdownStyledButton from '../../../components/TMDropdownButton'
import {
  STATUS_LIST,
  TRAIN_AVAILABLE_DAYS,
  TRAIN_PASSENGER_CLASSES,
} from '../../../configs/static-configs'
import { TRAIN_HEADERS } from '../../../configs/dataConfig'
import ConfirmationDialog from '../../../components/TMConfirmationDialog'
import { useHistory } from 'react-router-dom'
import { ToastContainer, toast } from 'react-toastify'

export default function TrainList() {
  const [dataLoading, setDataLoading] = useState(true);
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

  const getAllTrains = async () => {
    try {
      setDataLoading(true)
      const payload = {
        searchText: searchText,
        status: filterByStatus,
        availableDay: selectedAvailability,
        passengerClass: selectedPassenger,
        currentPage: currentPage,
        pageSize: 10,
      }

      const response = await TrainsAPIService.getTrains(payload)
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
    getAllTrains()
  }, [
    currentPage,
    searchText,
    filterByStatus,
    selectedAvailability,
    selectedPassenger,
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
    // setSettings({
    //   openDialog: true,
    //   action: 'edit',
    //   parentData: { id },
    // })
    history.push(`/trains-with-schedules/${id}`)
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
      console.log(response?.succeeded)
      if (response?.succeeded) {
        await toast.success(
          response?.successMessage || 'Successfully changed the status',
        )
        await getAllTrains()
        await setSelectedIds([])
      } else {
        await toast.error(
          response?.errors[0] ||
            'Try again. Cannot cancel trains with existing reservation/s.',
        )
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
            // setSettings({
            //   openDialog: true,
            //   action: 'add',
            //   parentData: null,
            // })
            history.push('/trains-with-schedules/new')
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
      <ToastContainer />
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
          title="Trains"
          subtitle="Train Management"
          buttonComponent={buttonComp()}
        />
        <StyledTable
          headers={TRAIN_HEADERS}
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
          isLoadingEnabaled={true}
          isLoading={dataLoading}
        />
        <div>
          {settings.openDialog && (
            <TrainDialog
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
