import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Del } from './del';

describe('Del', () => {
  let component: Del;
  let fixture: ComponentFixture<Del>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [Del]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Del);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
