import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentGradesComponent } from './student-grades.component';

describe('StudentGradesComponent', () => {
  let component: StudentGradesComponent;
  let fixture: ComponentFixture<StudentGradesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StudentGradesComponent]
    });
    fixture = TestBed.createComponent(StudentGradesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
