import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanExpandComponent } from './pan-expand.component';

describe('PanExpandComponent', () => {
  let component: PanExpandComponent;
  let fixture: ComponentFixture<PanExpandComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PanExpandComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanExpandComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
