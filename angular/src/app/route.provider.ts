import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/welcome',
        name: 'welcome',
        order: 2,
        layout: eLayoutType.empty,
        invisible : true
      },
      
      {
        path: '/MyPatient',
        name: '::Menu:MyPatient',
        iconClass: 'fas fa-user-alt',
        order: 3,
        requiredPolicy: 'DawaaNeo.Dashboard.Tenant',
        layout: eLayoutType.application,
      },

      {
        path: '/Services',
        name: '::Menu:Service',
        iconClass: 'fa fa-handshake-o',
        order: 4,
        requiredPolicy: 'DawaaNeo.Dashboard.Tenant',
        layout: eLayoutType.application,
      },
    ]);
  };
}
