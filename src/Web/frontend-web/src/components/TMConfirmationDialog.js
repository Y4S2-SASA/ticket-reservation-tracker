import React from 'react'
import { Modal, Button } from 'react-bootstrap'

export default function ConfirmationDialog({
  title,
  message,
  show,
  onHide,
  onConfirm,
}) {
  return (
    <Modal
      show={show}
      onHide={onHide}
      centered
      style={{ backgroundColor: 'rgba(0, 0, 0, 0.4' }}
    >
      <Modal.Header
        // closeButton
        backdrop="static"
        style={{ backgroundColor: '#330065', color: 'white' }}
      >
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>{message}</Modal.Body>
      <Modal.Footer>
        <Button
          variant="secondary"
          onClick={onHide}
          style={{
            backgroundColor: '#e5e5e5',
            border: 'none',
            color: '#330065',
          }}
        >
          Cancel
        </Button>
        <Button
          variant="primary"
          onClick={onConfirm}
          style={{ backgroundColor: '#7E5AE9', border: 'none' }}
        >
          Logout
        </Button>
      </Modal.Footer>
    </Modal>
  )
}
