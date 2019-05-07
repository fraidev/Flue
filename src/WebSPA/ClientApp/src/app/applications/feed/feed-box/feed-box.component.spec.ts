import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedBoxComponent } from './feed-box.component';

describe('FeedBoxComponent', () => {
  let component: FeedBoxComponent;
  let fixture: ComponentFixture<FeedBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeedBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
