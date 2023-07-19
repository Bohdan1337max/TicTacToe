using System;

namespace TicTacToe;

public class Test
{
    static void ChangeReferenceType(Student std2)
    {
        std2.StudentName = "Steve";
    } 
    public void Run() 
    {
        Student std1 = new Student();
        std1.StudentName = "Bill";
    
        ChangeReferenceType(std1);

        Console.WriteLine(std1.StudentName);
    }
}

internal struct Student
{
    public string StudentName { get; set; }
}