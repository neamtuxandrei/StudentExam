import { ExaminationTicket } from "./examination-ticket";
import { Student } from "./student";
import { StudentPresentationStatus } from "./student-presentation-status";

export interface StudentPresentation{
    id: string;
    examinationSessionId: string;
    studentId?: string;
    student?: Student;
    isAbsent?: boolean; 
    status?: StudentPresentationStatus;
    startingTime?: Date;
    theoryGrade: number;
    projectGrade: number;
    examinationTicket: ExaminationTicket;
}