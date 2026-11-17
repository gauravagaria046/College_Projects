# Student Record Manager (Console)

A menu-driven C# console application to manage student records.

## Features
- Add student records (Roll No, Name, DOB, Marks)
- View all saved students
- Search students by partial name (case-insensitive)
- Delete a student using Roll No
- Calculate age from Date of Birth
- Save and load records using `students.txt`
- No database required

## Requirements
- .NET 8 SDK or later

## Run
Open a terminal in this folder and run:

```bash
dotnet run
```

The `students.txt` file is created automatically when records are saved.
Each record is stored as: `RollNo|Name|DOB|Marks`.

## Date format
Enter date of birth in `dd/MM/yyyy` format, for example `15/05/2005`.
