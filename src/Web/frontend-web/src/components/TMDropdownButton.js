/*
 * File: BookingDialog.js
 * Author: Bartholomeusz S.V/IT20274702
 */

import React, { useEffect, useState } from 'react'
import { Dropdown } from 'react-bootstrap'

const DropdownStyledButton = ({
  dropdownTitle,
  items,
  handleChange,
  selectedStatus,
  changeMode,
  role,
}) => {
  const [itemLabel, setItemLabel] = useState(null)

  useEffect(() => {
    if (selectedStatus) {
      const getSelected = items.find((item) => item.id === selectedStatus)
      setItemLabel(getSelected?.dropLabel)
    }
  }, [selectedStatus])

  return (
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
        {selectedStatus > 0 ? itemLabel : dropdownTitle}
      </Dropdown.Toggle>

      <Dropdown.Menu>
        {items?.map((item) => (
          <Dropdown.Item
            eventKey={item?.id}
            onClick={() => handleChange(item?.id)}
            disabled={
              (changeMode && item?.isDisabled) || item?.isDisabledToRole
            }
          >
            {item?.dropLabel}
          </Dropdown.Item>
        ))}
      </Dropdown.Menu>
    </Dropdown>
  )
}

export default DropdownStyledButton
