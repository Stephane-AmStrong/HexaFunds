import { Routes } from '@angular/router';
import { AddressFormComponent } from '../../address-form/address-form.component';
import { DashboardComponent } from '../../features/banking/Components/dashboard/dashboard.component';
import { DragDropComponent } from '../../features/banking/Components/drag-drop/drag-drop.component';
import { PageNotFoundComponent } from '../../shared/Components/page-not-found/page-not-found.component';
import { TreeComponent } from '../../features/tree/tree.component';

export const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo:'/dashboard'
    },
    {
        path: 'dashboard',
        component: DashboardComponent,
    },
    {
        path: 'address-form',
        component: AddressFormComponent,
    },
    {
        path: 'tree',
        component: TreeComponent,
    },
    {
        path: 'drag-drop',
        component: DragDropComponent,
    },
    {
        path:'**',
        component:PageNotFoundComponent
    }
];
