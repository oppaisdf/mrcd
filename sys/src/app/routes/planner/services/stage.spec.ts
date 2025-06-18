import { TestBed } from '@angular/core/testing';

import { Stage } from './stage';

describe('Stage', () => {
  let service: Stage;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Stage);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
