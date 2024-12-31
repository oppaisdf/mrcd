import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { admGuard } from './adm.guard';

describe('admGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => admGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
