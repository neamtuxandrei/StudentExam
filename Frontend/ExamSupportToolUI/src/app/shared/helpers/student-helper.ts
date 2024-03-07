import { PresentationScheduleEntry } from "../models/presentation-schedule";

/**
 * 
 * @param entries An array of PresentationScheduleEntry objects.
 * @param entryId The ID of the entry to look for.
 * @param direction The direction in which the student should be moved ('up' or 'down').
 * @returns An object of presentation schedule entries grouped by date (dd-MM-yyyy).
 */
export const moveEntry = (entries: PresentationScheduleEntry[], entryId: string, direction: string) => {
    const studentIndex = findEntryIndexById(entries, entryId);
    const newIndex = direction === 'up' ? studentIndex - 1 : studentIndex + 1;

    let presentationScheduleEntriesByDate;

    if ((direction === 'up' && studentIndex > 0) || (direction === 'down' && studentIndex < entries.length - 1)) {
        swapArrayElements(entries, studentIndex, newIndex);
        swapEntryDates(entries, studentIndex, newIndex);

        presentationScheduleEntriesByDate = groupPresentationScheduleByDate(entries);
    }
    return presentationScheduleEntriesByDate!;
}

/**
 * Groups entries from an array of PresentationScheduleEntry objects by date.
 * 
 * @param entries An array of PresentationScheduleEntry objects.
 * @returns An object of entries sorted by date.
 */
export const groupPresentationScheduleByDate = (entries: PresentationScheduleEntry[]) => {
    const groupedSchedule: Record<string, PresentationScheduleEntry[]> = {};

    // Iterate through the entries and group them by date
    entries.forEach(entry => {
        const dateParts = entry.date.split('T')[0].split('-'); // Extract date parts and split by '-'
        const dateKey = `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`; // 'dd/MM/yyyy' format
        if (!groupedSchedule[dateKey]) {
            groupedSchedule[dateKey] = [];
        }
        groupedSchedule[dateKey].push(entry);
    });

    // Sort the keys (dates)
    const sortedKeys = Object.keys(groupedSchedule).sort((a, b) => {
        const datePartsA = a.split('/');
        const datePartsB = b.split('/');
        const dateA = new Date(
            parseInt(datePartsA[2], 10), // Year
            parseInt(datePartsA[1], 10) - 1, // Month (0-based)
            parseInt(datePartsA[0], 10) // Day
        );
        const dateB = new Date(
            parseInt(datePartsB[2], 10), // Year
            parseInt(datePartsB[1], 10) - 1, // Month (0-based)
            parseInt(datePartsB[0], 10) // Day
        );
        return dateA.getTime() - dateB.getTime();
    });

    const sortedSchedule: any[] = [];

    sortedKeys.forEach(key => {
        let item = { key, value: groupedSchedule[key] };
        sortedSchedule.push(item);
    });

    return sortedSchedule;
}


/**
 * Finds the index of an entry with a specific ID in an array of PresentationScheduleEntry objects.
 * 
 * @param entries An array of PresentationScheduleEntry objects.
 * @param entryId The ID of the entry to look for.
 * @returns The index of the entry if found -1, otherwise -1.
 */
export const findEntryIndexById = (entries: PresentationScheduleEntry[], entryId: string) => {
    for (let i = 0; i < entries.length; i++) {
        if (entries[i].id === entryId) {
            return i;
        }
    }
    return -1;
}

/**
 * Swaps two elements in an array by their indices.
 * 
 * @param arr The array in which elements will be swapped.
 * @param indexA The index of the first element to swap.
 * @param indexB The index of the second element to swap.
 */
const swapArrayElements = (arr: any[], indexA: number, indexB: number) => {
    [arr[indexA], arr[indexB]] = [arr[indexB], arr[indexA]];
}


/**
 * Swaps two entries date in an arraay by their indices.
 * 
 * @param entries The entries array in which elements will be swapped.
 * @param indexA The index of the first entries to swap.
 * @param indexB The index of the second entries to swap.
 */
const swapEntryDates = (entries: PresentationScheduleEntry[], indexA: number, indexB: number) => {
    [entries[indexA].date, entries[indexB].date] =
        [entries[indexB].date, entries[indexA].date];
}
