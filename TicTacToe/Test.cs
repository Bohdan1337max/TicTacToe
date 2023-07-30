using System;
using System.Threading.Tasks;

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

    public async Task<int> ATask(int x)
    {
        Console.WriteLine($"Start waiting {x}");
        await Task.Delay(x * 1000);
        Console.WriteLine($"Waiting {x} sex");

        
        return x;
    }
}


internal struct Student
{
    public string StudentName { get; set; }
}