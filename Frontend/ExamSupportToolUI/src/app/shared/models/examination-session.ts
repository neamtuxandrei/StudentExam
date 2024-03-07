import { Student } from "./student";
import { CommitteeMember } from "./committee";
import { ExaminationTicket } from "./examination-ticket";
import { StudentPresentation } from "./student-presentation";
import { PresentationSchedule } from "./presentation-schedule";

export interface ExaminationSession {
    id: string;
    name: string;
    startDate: string;
    endDate: string;
    status: number;
    numberOfStudent?: number;
    numberOfCommitteeMembers?: number;
    numberOfTickets?: number;
    headOfCommitteeMemberId: string;
    students: Student[];
    committeeMembers: CommitteeMember[];
    examinationTickets: ExaminationTicket[];
    studentPresentations: StudentPresentation[];
    studentPresentation?: StudentPresentation;
    presentationSchedule: PresentationSchedule;
}
