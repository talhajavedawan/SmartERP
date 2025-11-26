import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    iconComponent: { name: 'cil-speedometer' }
  },
  {
    name: 'Notifications',
    url: '/notifications',
    iconComponent: { name: 'cil-bell' }
  },
  {
    name: 'Reports',
    url: '/reports',
    iconComponent: { name: 'cil-chart-line' }
  },
  {
    name: 'Search Options',
    url: '/search-options',
    iconComponent: { name: 'cil-magnifying-glass' }
  },
  {
    name: 'Widgets',
    url: '/widgets',
    iconComponent: { name: 'cil-calculator' }
  },
  {
    name: 'Pages',
    url: '/login',
    iconComponent: { name: 'cil-folder' },
    children: [
      {
        name: 'Login',
        url: '/login',
        iconComponent: { name: 'cil-lock-locked' }
      },
      {
        name: 'Register',
        url: '/register',
        iconComponent: { name: 'cil-user' }
      },
      {
        name: 'Error 404',
        url: '/404',
        iconComponent: { name: 'cil-warning' }
      },
      {
        name: 'Error 500',
        url: '/500',
        iconComponent: { name: 'cil-warning' }
      }
    ]
  }
];
//_nav