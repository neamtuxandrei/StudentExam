import { TestBed } from '@angular/core/testing';

import { StudentGradesService } from './student-grades.service';

describe('StudentGradesService', () => {
  let service: StudentGradesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StudentGradesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
