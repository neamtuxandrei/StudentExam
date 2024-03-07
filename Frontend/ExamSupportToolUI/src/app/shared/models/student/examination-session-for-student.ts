import { PresentationScheduleForStudent } from "./presentation-schedule-for-student";
import { StudentPresentationForStudent } from "./student-presentation-for-student";

export interface ExaminationSessionForStudent {
    id: string;
    name: string;
    startDate: string;
    endDate: string;
    status: number;
    studentPresentation?: StudentPresentationForStudent;
    presentationSchedule?: PresentationScheduleForStudent;
}