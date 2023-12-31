import jwt_decode from 'jwt-decode'

export const NAVBAR_ITEMS = [
  {
    id: 1,
    pathUrl: '/dashboard',
    icon: '/images/icons/dashboard.png',
    label: 'Dashboard',
    entitlementRoles: ['Back Office', 'Travel Agent'],
  },
  {
    id: 2,
    pathUrl: '/users',
    icon: '/images/icons/users.png',
    label: 'Users',
    entitlementRoles: ['Back Office', 'Travel Agent'],
  },
  {
    id: 3,
    pathUrl: '/trains',
    icon: '/images/icons/train.png',
    label: 'Trains',
    entitlementRoles: ['Back Office'],
  },
  {
    id: 3,
    pathUrl: '/bookings',
    icon: '/images/icons/schedule.png',
    label: 'Bookings',
    entitlementRoles: ['Travel Agent'],
  },
]

export const STATUS_LIST = [
  {
    id: 0,
    label: '-All-',
    dropLabel: '-All-',
    isDisabled: true,
    color: '#E8AD00',
    showOnChange: false,
  },
  {
    id: 1,
    label: 'Pending',
    dropLabel: 'Pending',
    isDisabled: true,
    color: '#E8AD00',
    showOnChange: true,
  },
  {
    id: 2,
    label: 'Activated',
    dropLabel: 'Active',
    isDisabled: false,
    color: '#93D94E',
    showOnChange: true,
  },
  {
    id: 3,
    label: 'Deactivated',
    dropLabel: 'Deactive',
    isDisabled: false,
    color: '#F28705',
    showOnChange: true,
  },
  {
    id: 4,
    label: 'Deleted',
    dropLabel: 'Delete',
    isDisabled: false,
    color: '#F23030',
    showOnChange: true,
  },
]

export const getStatusColor = (status) => {
  switch (status) {
    case 'Pending':
      return '#E8AD00'
    case 'Activated':
      return '#93D94E'
    case 'Deactivated':
      return '#F28705'
    case 'Deleted':
      return '#F23030'
    default:
      return '#E8AD00'
  }
}

export const ROLES = [
  {
    id: 0,
    dropLabel: '-All-',
    showInForm: false,
    isDisabled: false,
    showInFilter: true,
  },
  {
    id: 1,
    dropLabel: 'Back Office',
    showInForm: true,
    isDisabled: false,
    showInFilter: true,
  },
  {
    id: 2,
    dropLabel: 'Travel Agent',
    showInForm: true,
    isDisabled: false,
    showInFilter: true,
  },
  {
    id: 3,
    dropLabel: 'Traveler',
    showInForm: true,
    isDisabled: false,
    showInFilter: true,
  },
]

function getTrainMasterData(property) {
  const masterData = localStorage.getItem('train-masterdata')

  const parsedData = JSON.parse(masterData)

  if (parsedData && parsedData[property]) {
    return parsedData[property]
  } else {
    return null
  }
}

export const TRAIN_STATUSES = getTrainMasterData('status')
export const TRAIN_AVAILABLE_DAYS = getTrainMasterData('availableDays')
export const TRAIN_PASSENGER_CLASSES = getTrainMasterData('passengerClasses')

export function getObjectById(id, array) {
  const foundObject = array.find((item) => item.id === id)
  return foundObject ? [foundObject] : []
}

const authStorage = localStorage.getItem('auth')

export function authDetails() {
  const parsedAuth = JSON.parse(authStorage)
  const authObj = {
    details: jwt_decode(parsedAuth?.token),
    role: parsedAuth?.role,
    userId: parsedAuth?.userId,
    displayName: parsedAuth?.displayName,
  }

  return authObj
}
console.log('authDetails', authDetails())

export const strongPasswordRegex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$/
