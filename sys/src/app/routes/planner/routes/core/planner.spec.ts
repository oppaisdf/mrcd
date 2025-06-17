import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Planner } from './planner';

describe('Planner', () => {
  let component: Planner;
  let fixture: ComponentFixture<Planner>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Planner]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Planner);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
