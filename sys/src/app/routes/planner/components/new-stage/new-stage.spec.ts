import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewStage } from './new-stage';

describe('NewStage', () => {
  let component: NewStage;
  let fixture: ComponentFixture<NewStage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NewStage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewStage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
