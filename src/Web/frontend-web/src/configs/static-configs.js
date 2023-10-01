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
    label: 'Pending',
    dropLabel: 'Pending',
    isDisabled: true,
    color: '#E8AD00',
  },
  {
    id: 1,
    label: 'Activated',
    dropLabel: 'Active',
    isDisabled: false,
    color: '#93D94E',
  },
  {
    id: 2,
    label: 'Deactivated',
    dropLabel: 'Deactive',
    isDisabled: false,
    color: '#F28705',
  },
  {
    id: 3,
    label: 'Deleted',
    dropLabel: 'Delete',
    isDisabled: false,
    color: '#F23030',
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
    name: '-All-',
    showInForm: false,
    isDisabled: false,
  },
  {
    id: 1,
    name: 'Back Office',
    showInForm: true,
    isDisabled: false,
  },
  {
    id: 2,
    name: 'Travel Agent',
    showInForm: true,
    isDisabled: false,
  },
  {
    id: 3,
    name: 'Traveler',
    showInForm: true,
    isDisabled: false,
  },
]
