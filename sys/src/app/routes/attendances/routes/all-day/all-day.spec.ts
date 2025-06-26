import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllDay } from './all-day';

describe('AllDay', () => {
  let component: AllDay;
  let fixture: ComponentFixture<AllDay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AllDay]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllDay);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
