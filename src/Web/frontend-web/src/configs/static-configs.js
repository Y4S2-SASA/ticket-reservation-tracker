export const NAVBAR_ITEMS = [
  {
    id: 1,
    pathUrl: '/dashboard',
    icon: '/images/icons/dashboard.png',
    label: 'Dashboard',
    entitlementRoles: ['Back Office'],
  },
  {
    id: 2,
    pathUrl: '/users',
    icon: '/images/icons/users.png',
    label: 'Users',
    entitlementRoles: ['Back Office'],
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
