import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InParentComponent } from './in-parent.component';

describe('InParentComponent', () => {
  let component: InParentComponent;
  let fixture: ComponentFixture<InParentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InParentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InParentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
