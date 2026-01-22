import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell.component/shell.component';

export const routes: Routes = [
    {
        path: '',
        component: ShellComponent
    },
    { path: '**', redirectTo: '' }
];
