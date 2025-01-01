import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { sysGuard } from './sys.guard';

describe('sysGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => sysGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
