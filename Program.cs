using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Program
{
    private static readonly List<Student> students = new();
    private static readonly string file = "students.txt";

    static void Main()
    {
        LoadRecords();

        while (true)
        {
            Console.WriteLine("\n===== STUDENT RECORD MANAGER =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Search Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Exit");
            Console.Write("\nEnter Choice: ");

            switch (Console.ReadLine())
            {
                case "1": AddStudent(); break;
                case "2": ViewStudents(); break;
                case "3": SearchStudent(); break;
                case "4": DeleteStudent(); break;
                case "5":
                    SaveRecords();
                    Console.WriteLine("Records saved. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Console.Write("Enter Roll No: ");
        if (!int.TryParse(Console.ReadLine(), out int rollNo))
        {
            Console.WriteLine("Invalid roll number.");
            return;
        }

        if (students.Any(s => s.RollNo == rollNo))
        {
            Console.WriteLine("This roll number already exists.");
            return;
        }

        Console.Write("Enter Name: ");
        string name = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Enter DOB (dd/MM/yyyy): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob)
            || dob.Date > DateTime.Today)
        {
            Console.WriteLine("Invalid DOB. Use dd/MM/yyyy and enter a valid past date.");
            return;
        }

        Console.Write("Enter Marks: ");
        if (!double.TryParse(Console.ReadLine(), out double marks) || marks < 0 || marks > 100)
        {
            Console.WriteLine("Marks must be between 0 and 100.");
            return;
        }

        students.Add(new Student
        {
            RollNo = rollNo,
            Name = name,
            DOB = dob,
            Marks = marks
        });

        SaveRecords();
        Console.WriteLine("Student added successfully!");
    }

    static void ViewStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No records found.");
            return;
        }

        Console.WriteLine("\n--- STUDENT RECORDS ---");
        foreach (Student s in students.OrderBy(s => s.RollNo))
        {
            Console.WriteLine($"Roll No: {s.RollNo} | Name: {s.Name} | DOB: {s.DOB:dd/MM/yyyy} | Age: {s.GetAge()} | Marks: {s.Marks:0.##}");
        }
    }

    static void SearchStudent()
    {
        Console.Write("Enter student name to search: ");
        string name = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (name.Length == 0)
        {
            Console.WriteLine("Please enter a name.");
            return;
        }

        var results = students
            .Where(s => s.Name.ToUpper().Contains(name))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        foreach (Student s in results)
        {
            Console.WriteLine($"Roll No: {s.RollNo} | Name: {s.Name} | Age: {s.GetAge()} | Marks: {s.Marks:0.##}");
        }
    }

    static void DeleteStudent()
    {
        Console.Write("Enter Roll No to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int rollNo))
        {
            Console.WriteLine("Invalid roll number.");
            return;
        }

        Student student = students.FirstOrDefault(s => s.RollNo == rollNo);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        students.Remove(student);
        SaveRecords();
        Console.WriteLine("Record deleted successfully.");
    }

    static void SaveRecords()
    {
        using StreamWriter writer = new(file);
        foreach (Student s in students)
        {
            writer.WriteLine($"{s.RollNo}|{s.Name}|{s.DOB:dd/MM/yyyy}|{s.Marks.ToString(CultureInfo.InvariantCulture)}");
        }
    }

    static void LoadRecords()
    {
        if (!File.Exists(file))
            return;

        foreach (string line in File.ReadAllLines(file))
        {
            string[] data = line.Split('|');

            if (data.Length != 4 ||
                !int.TryParse(data[0], out int rollNo) ||
                !DateTime.TryParseExact(data[2], "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob) ||
                !double.TryParse(data[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double marks))
                continue;

            students.Add(new Student
            {
                RollNo = rollNo,
                Name = data[1],
                DOB = dob,
                Marks = marks
            });
        }
    }
}
