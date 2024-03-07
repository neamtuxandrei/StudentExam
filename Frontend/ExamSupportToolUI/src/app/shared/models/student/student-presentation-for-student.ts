import { ExaminationTicketForStudent } from "./examination-ticket-for-student";

export interface StudentPresentationForStudent {
    status: number;
    startingTime?: Date;
    theoryGrade: number;
    projectGrade: number;
    examinationTicket: ExaminationTicketForStudent;
}