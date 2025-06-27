import { Component, EventEmitter, Output } from '@angular/core';
import { LoginService } from '../../../services/login.service';
import { NavItem } from '../../models/responses/Response';

@Component({
  selector: 'app-menu',
  standalone: false,
  templateUrl: './menu.html',
  styleUrl: './menu.sass'
})
export class Menu {
  constructor(
    private _login: LoginService
  ) {
    const isAdm = this._login.HasUserPermission('adm');
    const isSys = this._login.HasUserPermission('sys');

    this.routes = [
      {
        label: 'Confirmandos',
        expanded: false,
        show: true,
        route: '',
        children: [
          {
            label: 'Inscribir',
            expanded: false,
            show: true,
            route: '/person/new'
          }, {
            label: 'Actualizar',
            expanded: false,
            show: true,
            route: '/person/all'
          }, {
            label: 'Asistencia',
            expanded: false,
            show: true,
            route: '',
            children: [
              {
                label: 'Escáner',
                expanded: false,
                route: '/attendance',
                show: true
              },
              {
                label: 'Manual',
                expanded: false,
                route: '/attendance/manual',
                show: true
              }, {
                label: 'Todo el día',
                expanded: false,
                route: '/attendance/all',
                show: true
              }, {
                label: 'Eliminar asistencias',
                expanded: false,
                route: '/attendance/delete',
                show: true
              }
            ]
          }, {
            label: 'Padres/Padrinos',
            expanded: false,
            show: true,
            route: '/parent/all'
          }
        ]
      }, {
        label: 'Impresiones',
        expanded: false,
        show: true,
        route: '',
        children: [
          {
            label: 'Agenda',
            expanded: false,
            show: true,
            route: '/print/planner'
          }, {
            label: 'Asistencia',
            expanded: false,
            show: true,
            route: '/print/attendance'
          }, {
            label: 'Diplomas',
            expanded: false,
            show: isAdm,
            route: ''
          }, {
            label: 'Gafetes',
            expanded: false,
            show: isAdm,
            route: '/print/badge'
          }, {
            label: 'Listado',
            expanded: false,
            route: '/print/list',
            show: true
          }
        ]
      }, {
        label: 'Gestiones',
        expanded: false,
        route: '',
        show: true,
        children: [
          {
            label: 'Agenda',
            expanded: false,
            route: '/planner',
            show: true
          }, {
            label: 'Cobros',
            expanded: false,
            route: '/charge/all',
            show: true
          }, {
            label: 'Documentos',
            expanded: false,
            route: '/docs/all',
            show: true
          }, {
            label: 'Fases de actividad',
            expanded: false,
            route: '/planner/stage',
            show: true
          }, {
            label: 'Grados académicos',
            expanded: false,
            route: '/degree/all',
            show: isAdm
          }, {
            label: 'Usuarios',
            expanded: false,
            route: '/user/all',
            show: isAdm
          }, {
            label: 'Sacramentos',
            expanded: false,
            route: '/sacrament/all',
            show: isAdm
          }
        ]
      }, {
        label: 'Monitoreo',
        expanded: false,
        route: '',
        show: isAdm,
        children: [
          {
            label: 'Acciones de logs',
            expanded: false,
            route: '/action/all',
            show: true
          }, {
            label: 'Logs',
            expanded: false,
            route: '/logs',
            show: true
          }
        ]
      }, {
        label: 'System',
        expanded: false,
        route: '',
        show: isSys,
        children: [
          {
            label: 'Roles',
            expanded: false,
            route: '/role/all',
            show: true
          }
        ]
      }
    ];
  }

  routes: NavItem[];
  @Output() closeMenu = new EventEmitter<boolean>();

  Toggle(
    route: NavItem
  ) {
    route.expanded = !route.expanded;
  }

  CloseMenu() {
    this.closeMenu.emit(false);
  }
}
