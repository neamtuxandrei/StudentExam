export interface PresentationScheduleForStudent {
    studentPresentationDuration: number;
    presentationScheduleEntry: PresentationScheduleEntryForStudent;
}

export interface PresentationScheduleEntryForStudent {
    date: Date
}