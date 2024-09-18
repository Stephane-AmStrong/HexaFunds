import { Routes } from '@angular/router';
import { AddressFormComponent } from '../../features/banking/Components/address-form/address-form.component';
import { DashboardComponent } from '../../features/banking/Components/dashboard/dashboard.component';
import { PageNotFoundComponent } from '../../shared/Components/page-not-found/page-not-found.component';

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
        path:'**',
        component:PageNotFoundComponent
    }
];
