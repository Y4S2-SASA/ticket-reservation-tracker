/*
 * File: TMTable.js
 * Author: Jayathilake S.M.D.A.R./IT20037338
 */

import React from 'react'
import { Table, Form, Button, Spinner, Row } from 'react-bootstrap'
import { FaEdit, FaTrash } from 'react-icons/fa'
import { BsArrowLeft, BsArrowRight } from 'react-icons/bs'
import { getStatusColor } from '../configs/static-configs'

const StyledTable = ({
  headers,
  data,
  searchParameters,
  selectedIds,
  selectAll,
  onCheckboxChange,
  onSelectAllChange,
  editEnabled,
  deleteEnabled,
  onEditClick,
  onDeleteClick,
  handlePageChange,
  currentPage,
  isLoadingEnabaled,
  isLoading,
}) => {
  console.log('data', data)
  const renderActions = (id) => {
    return (
      <td>
        {editEnabled && (
          <Button variant="outline-info" onClick={() => onEditClick(id)}>
            <FaEdit />
          </Button>
        )}
        {deleteEnabled && (
          <Button
            variant="outline-danger"
            onClick={() => onDeleteClick(id)}
            style={{ marginLeft: '10px' }}
          >
            <FaTrash />
          </Button>
        )}
      </td>
    )
  }

  const renderRows = () => {
    if (isLoadingEnabaled && isLoading) {
      return (
        <tr>
          <td colSpan={headers.length + 2}>
            <div
              style={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
              }}
            >
              <Spinner animation="border" />
            </div>
          </td>
        </tr>
      )
    } else if (data?.length > 0) {
      return data?.map((row, index) => (
        <tr key={index}>
          <td>
            <Form.Check
              type="checkbox"
              checked={
                selectedIds.includes(row.nic) || selectedIds.includes(row.id)
              }
              onChange={() => onCheckboxChange(row.nic ? row.nic : row.id)}
            />
          </td>
          {headers.map((header) => (
            <td key={header.key}>
              {header.key === 'status' ? (
                <div
                  style={{
                    backgroundColor: getStatusColor(row[header.key]),
                    textAlign: 'center',
                    width: '100px',
                    padding: '5px',
                    borderRadius: '50px',
                    fontSize: '12px',
                    color: 'white',
                    fontWeight: 'bold',
                  }}
                >
                  {row[header.key]}
                </div>
              ) : (
                <div style={{ maxWidth: '150px', wordWrap: 'break-word' }}>
                  {row[header.key]}
                </div>
              )}
            </td>
          ))}
          {renderActions(row.nic ? row.nic : row.id)}
        </tr>
      ))
    } else {
      return (
        <div style={{ fontSize: '20px', fontWeight: 900, color: '#7E5AE9' }}>
          No data found
        </div>
      )
    }
  }

  return (
    <div>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>
              <Form.Check
                type="switch"
                checked={selectAll}
                onChange={onSelectAllChange}
                color="success"
              />
            </th>
            {headers.map((header) => (
              <th key={header.key}>{header.label}</th>
            ))}
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>{renderRows()}</tbody>
      </Table>
      <div className="pagination">
        <Button
          variant="secondary"
          onClick={() => handlePageChange(currentPage - 1)}
          disabled={!searchParameters.hasPreviousPage}
        >
          <BsArrowLeft />
        </Button>
        <span className="page-number">
          {searchParameters.totalPages === 1
            ? 'Page 1'
            : `Page ${currentPage + 1} of ${searchParameters.totalPages}`}
        </span>
        <Button
          variant="secondary"
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={!searchParameters.hasNextPage}
        >
          <BsArrowRight />
        </Button>
      </div>
    </div>
  )
}

export default StyledTable
