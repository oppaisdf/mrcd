import { TestBed } from '@angular/core/testing';

import { SacramentService } from './sacrament.service';

describe('SacramentService', () => {
  let service: SacramentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SacramentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
