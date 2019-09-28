import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostComponent } from './post.component';
import { SharedModule } from '../../shared.module';
import { IonicModule } from '@ionic/angular';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [PostComponent],
    imports: [
        CommonModule,
        FormsModule,
        IonicModule],
    exports: [PostComponent],
    providers: [],
})
export class PostModule { }
