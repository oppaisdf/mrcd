import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Activity } from './activity';

describe('Activity', () => {
  let component: Activity;
  let fixture: ComponentFixture<Activity>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Activity]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Activity);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
