/*
 * File: TMConfirmationDialog.js
 * Author: Dunusinghe A.V./IT20025526
 */

export default function OrDivider(props) {
  return (
    <div
      style={{
        display: 'flex',
        justifyContent: 'space-between',
        color: props.color || 'white',
        marginTop: '20px',
        marginBottom: '20px',
      }}
    >
      <hr
        style={{
          flex: 1,
          borderTop: `1px solid ${props.color ? props.color : 'white'}`,
        }}
      />
      <span style={{ padding: '0 10px' }}>or</span>
      <hr
        style={{
          flex: 1,
          borderTop: `1px solid ${props.color ? props.color : 'white'}`,
        }}
      />
    </div>
  )
}
