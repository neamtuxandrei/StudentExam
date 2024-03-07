import * as XLSX from 'xlsx';

export class XLSXReader {
    constructor() { }

    static readStudents(file: File) {
        return new Promise((resolve, reject) => {
            var reader = new FileReader();

            reader.onload = (e: any) => {
                // Read the uploaded file content
                const fileContent = e.target.result;

                // Process the file content using xlsx library
                const workbook = XLSX.read(fileContent, { type: 'binary' });
                const sheetName = workbook.SheetNames[0];
                const sheet = workbook.Sheets[sheetName];

                // Convert sheet data to JSON
                const jsonData = XLSX.utils.sheet_to_json(sheet, { header: 0 });

                resolve(jsonData);
            };

            reader.readAsBinaryString(file);
        });
    }
}