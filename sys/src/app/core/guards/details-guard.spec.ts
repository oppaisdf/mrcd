import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { detailsGuard } from './details-guard';

describe('detailsGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => detailsGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
