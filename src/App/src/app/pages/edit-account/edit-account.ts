import { Component, OnInit } from '@angular/core';
import { Person } from '../../shared/models';
import { PeopleService } from '../../services';
import { ToastController } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
    selector: 'edit-account',
    templateUrl: './edit-account.html',
    styleUrls: ['./edit-account.scss']
})
export class EditAccountPage implements OnInit {
    person = new Person;
    file: File;
    image: string | ArrayBuffer;

    constructor(public peopleApi: PeopleService,
        private toastCtrl: ToastController,
        private router: Router) { }

    get getAvatar() {
        return this.person.profilePicture ? this.person.profilePicture : `/assets/img/profile.png`;
    }

    ngOnInit(): void {
        this.getPerson();
    }

    getPerson() {
        this.peopleApi.getMe().subscribe((person) => {
            this.person = person;
            console.log(this.person);
        });
    }

    fileChangeListener($evt: { target: { files: File[]; }; }) {
        this.file = $evt.target.files[0];
        const myReader = new FileReader();

        myReader.onloadend = (e) => {
            this.person.profilePicture = myReader.result.toString();
        };
        myReader.readAsDataURL(this.file);
        console.log(this.file);
    }

    onUpdatePerson() {
        const cmd = {
            name: this.person.name,
            email: this.person.email,
            description: this.person.description,
            profilePicture: this.person.profilePicture,
        };

        this.peopleApi.updatePerson(cmd).subscribe(async () => {
            const toast = await this.toastCtrl.create({
                message: 'Sua conta foi atualizada com sucesso.',
                duration: 2000
            });
            await toast.present();

            this.router.navigateByUrl('/app/tabs/account');
        });
    }
}
