/*
 * File: UserList.js
 * Author: Dunusinghe A.V./IT20025526
 */

import React, { Fragment, useEffect, useReducer, useState } from 'react'
import StyledTable from '../../components/TMTable'
import Loader from '../../components/TMLoader'
import MainLayout from '../../components/Layouts/MainLayout'
import LayoutHeader from '../../components/Layouts/LayoutHeader'
import DropdownStyledButton from '../../components/TMDropdownButton'
import { ROLES, STATUS_LIST, authDetails } from '../../configs/static-configs'
import { Button, Form, InputGroup } from 'react-bootstrap'
import { FaPlus, FaSearch } from 'react-icons/fa'
import { USERS_HEADERS } from '../../configs/dataConfig'
import UserAPIService from '../../api-layer/users'
import UserDialog from './UserDialog'
import ConfirmationDialog from '../../components/TMConfirmationDialog'
import { ToastContainer, toast } from 'react-toastify'

export default function UserList() {
  const auth = authDetails()
  const [dataLoading, setDataLoading] = useState(true)
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
  const [userSettings, setUserSettings] = useState({
    openDialog: false,
    action: '',
    parentData: null,
  })
  const [
    showStatuConfirmationDialog,
    setShowStatuConfirmationDialog,
  ] = useState(false)
  const [selectedFilterRole, setSelectedFilterRole] = useState(0)

  const getAllUsers = async () => {
    try {
      setDataLoading(true)
      const payload = {
        searchText: searchText,
        status: filterByStatus,
        role: selectedFilterRole,
        currentPage: currentPage,
        pageSize: 10,
      }

      const response = await UserAPIService.getUsers(payload)
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
  console.log(searchParameters)

  useEffect(() => {
    getAllUsers()
  }, [currentPage, searchText, filterByStatus, selectedFilterRole])

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
      const allIds = filteredData.map((row) => row.nic)
      setSelectedIds(allIds)
    }
    setSelectAll(!selectAll)
  }

  const handleEditClick = (id) => {
    console.log(`Edit clicked for ID: ${id}`)
    setUserSettings({
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
    selectedIds.forEach(async (nic) => {
      const payload = {
        nic: nic,
        status: selectedStatus,
      }
      const response = await UserAPIService.updateUserStatus(payload)
      if (response) {
        await toast.success(
          response?.successMessage || 'Successfully changed the status',
        )
        await getAllUsers()
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

  const handleRoleFilter = (itemId) => {
    setSelectedFilterRole(itemId)
  }

  // const handlePageChange = (newPage) => {
  //   setSearchParameers({ ...searchParameters, currentPage: newPage })
  // }
  const handlePageChange = (newPage) => {
    setCurrentPage(newPage)
  }

  const handleSearchInputChange = async (e) => {
    await setSearchText(e.target.value)
    await setCurrentPage(0)
  }

  const onCloseUserDialog = () => {
    setUserSettings({
      openDialog: false,
      action: '',
      parentData: null,
    })
  }

  const buttonComp = () => {
    return (
      <div className="user-head-btn-group">
        <InputGroup>
          <Form.Control
            type="text"
            placeholder="Search Users"
            value={searchText}
            onChange={(e) => handleSearchInputChange(e)}
            style={{
              height: '46px',
              border: 'none',
              borderRadius: '46px',
            }}
          />
        </InputGroup>
        {auth?.role === 'Travel Agent' && (
          <Button
            style={{
              height: '46px',
              backgroundColor: '#7E5AE9',
              border: 'none',
              borderRadius: '46px',
              width: '170px',
            }}
            onClick={() =>
              setUserSettings({
                openDialog: true,
                action: 'add',
                parentData: null,
              })
            }
          >
            <FaPlus /> Add
          </Button>
        )}
        {selectedIds?.length > 0 ? (
          <DropdownStyledButton
            dropdownTitle={'Change status'}
            items={STATUS_LIST.filter(
              (status) => status.showOnChange === true,
            ).map((status) => ({
              ...status,
              isDisabledToRole:
                auth?.role === 'Back Office'
                  ? status.id === 4
                  : status.id === 2 || status.id === 3,
            }))}
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
            <DropdownStyledButton
              dropdownTitle={'Filter by Role'}
              items={ROLES}
              handleChange={handleRoleFilter}
              selectedStatus={selectedFilterRole}
            />
          </>
        )}
      </div>
    )
  }
  console.log(userSettings.openDialog)
  return (
    <MainLayout loading={true} loadingTime={2000}>
      <ToastContainer />
      <div className="users-container">
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
          title="Users"
          subtitle="User Management"
          buttonComponent={buttonComp()}
        />
        <StyledTable
          headers={USERS_HEADERS}
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
          {userSettings.openDialog && (
            <UserDialog
              userSettings={userSettings}
              onClose={onCloseUserDialog}
              onSave={onCloseUserDialog}
              callBackUsers={getAllUsers}
            />
          )}
        </div>
      </div>
    </MainLayout>
  )
}
