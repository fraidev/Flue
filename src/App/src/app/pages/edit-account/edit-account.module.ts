import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { EditAccountPage } from './edit-account';
import { FormsModule } from '@angular/forms';

const routes: Routes = [
    {
        path: '',
        component: EditAccountPage
    }
];

@NgModule({
    declarations: [EditAccountPage],
    imports: [
        CommonModule,
        IonicModule,
        FormsModule,
        SharedModule,
        RouterModule.forChild(routes)
    ],
    exports: [RouterModule],
    providers: [],
})
export class EditAccountModule { }
