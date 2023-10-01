import React, { Fragment, useEffect, useState } from 'react'
import StyledTable from '../../components/TMTable'
import Loader from '../../components/TMLoader'
import MainLayout from '../../components/Layouts/MainLayout'
import LayoutHeader from '../../components/Layouts/LayoutHeader'
import DropdownStyledButton from '../../components/TMDropdownButton'
import { STATUS_LIST } from '../../configs/static-configs'
import { Button, Form, InputGroup } from 'react-bootstrap'
import { FaPlus, FaSearch } from 'react-icons/fa'
import { USERS_HEADERS } from '../../configs/sample-data'
import UserAPIService from '../../api-layer/users'
import UserDialog from './UserDialog'

export default function UserList() {
  const [selectedIds, setSelectedIds] = useState([])
  const [selectAll, setSelectAll] = useState(false)
  const [loader, setLoader] = useState(false)
  const [selectedStatus, setSelectedStatus] = useState(null)
  const [searchParameters, setSearchParameers] = useState({
    searchText: '',
    status: 0,
    role: 0,
    currentPage: 1,
    pageSize: 10,
    hasNextPage: false,
    hasPreviousPage: false,
    totalRecordCount: 0,
    totalPages: 0,
  })
  const [currentPage, setCurrentPage] = useState(0)
  const [filteredData, setFilteredData] = useState([])
  const [searchText, setSearchText] = useState('')
  const [userSettings, setUserSettings] = useState({
    openDialog: false,
    action: '',
    parentData: null,
  })

  useEffect(() => {
    setLoader(true)
    setTimeout(() => {
      setLoader(false)
    }, 2000)
  }, [])

  const getAllUsers = async () => {
    try {
      const payload = {
        searchText: searchText,
        status: 0,
        role: 0,
        currentPage: currentPage,
        pageSize: 10,
      }

      const response = await UserAPIService.getUsers(payload)
      if (response) {
        const filteredItems = response.items.filter((item) => {
          const matchesSearchText = searchText
            ? item.fullName.toLowerCase().includes(searchText.toLowerCase())
            : true
          const matchesStatus = searchParameters.status
            ? item.status === searchParameters.status
            : true
          const matchesRole = searchParameters.role
            ? item.role === searchParameters.role
            : true

          return matchesSearchText && matchesStatus && matchesRole
        })

        const startIndex =
          (searchParameters.currentPage - 1) * searchParameters.pageSize
        const endIndex = startIndex + searchParameters.pageSize
        const newData = filteredItems.slice(startIndex, endIndex)

        setFilteredData(newData)
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
  console.log(searchParameters)

  useEffect(() => {
    getAllUsers()
  }, [currentPage, searchText])

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
    console.log(`Selected Item ID: ${itemId}`)
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
        {selectedIds?.length > 0 ? (
          <DropdownStyledButton
            dropdownTitle={'Change status'}
            items={STATUS_LIST}
            handleChange={handleStatusChange}
            selectedStatus={selectedStatus}
          />
        ) : (
          <>
            <DropdownStyledButton
              dropdownTitle={'Filter by Status'}
              items={STATUS_LIST}
              handleChange={handleStatusChange}
              selectedStatus={selectedStatus}
            />
            <DropdownStyledButton
              dropdownTitle={'Filter by Role'}
              items={STATUS_LIST}
              handleChange={handleStatusChange}
              selectedStatus={selectedStatus}
            />
          </>
        )}
      </div>
    )
  }
  console.log(userSettings.openDialog)
  return (
    <Fragment>
      {loader ? (
        <Loader />
      ) : (
        <MainLayout>
          <div className="users-container">
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
              deleteEnabled={true}
              onEditClick={handleEditClick}
              onDeleteClick={handleDeleteClick}
              handlePageChange={handlePageChange}
              currentPage={currentPage}
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
      )}
    </Fragment>
  )
}
