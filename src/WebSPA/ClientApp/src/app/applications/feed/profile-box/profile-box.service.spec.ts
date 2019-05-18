import { TestBed } from '@angular/core/testing';

import { ProfileBoxService } from './profile-box.service';

describe('ProfileBoxService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProfileBoxService = TestBed.get(ProfileBoxService);
    expect(service).toBeTruthy();
  });
});
