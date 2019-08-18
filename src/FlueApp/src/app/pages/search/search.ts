import { Component, ElementRef, ViewChild } from '@angular/core';
import { ConferenceData } from '../../providers/conference-data';
import { Platform } from '@ionic/angular';

@Component({
  selector: 'page-search',
  templateUrl: 'search.html',
  styleUrls: ['./search.scss']
})
export class SearchPage {
  @ViewChild('mapCanvas', { static: true }) mapElement: ElementRef;

  constructor(public confData: ConferenceData, public platform: Platform) { }

}
