import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScanAllComponent } from './scan-all.component';

describe('ScanAllComponent', () => {
  let component: ScanAllComponent;
  let fixture: ComponentFixture<ScanAllComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ScanAllComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScanAllComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
