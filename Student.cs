using System;

class Student
{
    public int RollNo;
    public string Name;
    public DateTime DOB;
    public double Marks;

    public int GetAge()
    {
        int age = DateTime.Today.Year - DOB.Year;

        if (DOB.Date > DateTime.Today.AddYears(-age))
            age--;

        return age;
    }
}
