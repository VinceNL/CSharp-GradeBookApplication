﻿using System;

using GradeBook.Enums;
using GradeBook.GradeBooks;

namespace GradeBook
{
    class Program
    {
        //#Todo refactor this into several stand alone and testable methods
        static void Main(string[] args)
        {
            Console.WriteLine("#=======================#");
            Console.WriteLine("# Welcome to GradeBook! #");
            Console.WriteLine("#=======================#");
            Console.WriteLine();

            var quit = false;
            while (!quit)
            {
                Console.WriteLine("What would you like to do?");
                var command = Console.ReadLine().ToLower();

                if (command.StartsWith("create"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Command not valid, Create requires a name.");
                        continue;
                    }
                    var name = parts[1];
                    var gradeBook = new BaseGradeBook(name, true);
                    Console.WriteLine("Created gradebook {0}.", name);
                    GradeBookInteraction(gradeBook);
                }
                else if(command.StartsWith("load"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Command not valid, Load requires a name.");
                        continue;
                    }
                    var name = parts[1];
                    var gradeBook = BaseGradeBook.Load(name);

                    if (gradeBook == null)
                        continue;

                    GradeBookInteraction(gradeBook);
                }
                else if(command == "help")
                { 
                    Console.WriteLine("GradeBook accepts the following commands:");
                    Console.WriteLine();
                    Console.WriteLine("Create 'Name' - Creates a new gradebook where 'Name' is the name of the gradebook.");
                    Console.WriteLine();
                    Console.WriteLine("Load 'Name' - Loads the gradebook with the provided 'Name'.");
                    Console.WriteLine();
                    Console.WriteLine("Help - Displays all accepted commands.");
                    Console.WriteLine();
                    Console.WriteLine("Quit - Exits the application");
                }
                else if(command == "quit")
                    quit = true;
                else
                    Console.WriteLine("{0} was not recognized, please try again.", command);
            }
            Console.WriteLine("Thank you for using GradeBook!");
            Console.WriteLine("Have a nice day!");
            Console.Read();
        }

        static void GradeBookInteraction(BaseGradeBook gradeBook)
        {
            Console.WriteLine("#=======================#");
            Console.WriteLine(gradeBook.Name);
            Console.WriteLine("#=======================#");
            Console.WriteLine(string.Empty);

            var close = false;
            while(!close)
            {
                Console.WriteLine("What would you like to do?");
                var command = Console.ReadLine().ToLower();
                if (command == "save")
                {
                    gradeBook.Save();
                    Console.WriteLine("{0} has been saved.", gradeBook.Name);
                }
                else if (command == "close")
                    close = true;
                else if (command.StartsWith("addgrade"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 3)
                    {
                        Console.WriteLine("Command not valid, AddGrade requires a name and score.");
                        continue;
                    }
                    var name = parts[1];
                    var score = Double.Parse(parts[2]);
                    gradeBook.AddGrade(name, score);
                    Console.WriteLine("Added a score of {0} to {1}'s grades", score, name);
                }
                else if (command.StartsWith("removegrade"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 3)
                    {
                        Console.WriteLine("Command not valid, RemoveGrade requires a name and score.");
                        continue;
                    }
                    var name = parts[1];
                    var score = Double.Parse(parts[2]);
                    gradeBook.RemoveGrade(name, score);
                    Console.WriteLine("Removed a score of {0} from {1}'s grades", score, name);
                }
                else if (command.StartsWith("add"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 4)
                    {
                        Console.WriteLine("Command not valid, Add requires a name, student type, enrollment type.");
                        continue;
                    }
                    var name = parts[1];

                    StudentType studentType;
                    if(!Enum.TryParse(parts[2], true, out studentType))
                    {
                        Console.WriteLine("{0} is not a valid student type, try again.", parts[2]);
                        continue;
                    }

                    EnrollmentType enrollmentType;
                    if(!Enum.TryParse(parts[3], true, out enrollmentType))
                    {
                        Console.WriteLine("{0} is not a volid enrollment type, try again.", parts[3]);
                        continue;
                    }

                    var student = new Student(name, studentType, enrollmentType);
                    gradeBook.AddStudent(student);
                    Console.WriteLine("Added {0} to the gradebook.", name);
                }
                else if (command.StartsWith("remove"))
                {
                    var parts = command.Split(' ');
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Command not valid, Remove requires a name.");
                        continue;
                    }
                    var name = parts[1];
                    gradeBook.RemoveStudent(name);
                    Console.WriteLine("Removed {0} from the gradebook.",name);
                }
                else if (command == "list")
                {
                    gradeBook.ListStudents();
                }
                else if (command == "statistics all")
                {
                    gradeBook.CalculateStatistics();
                }
                else if (command.StartsWith("statistics"))
                {
                    var parts = command.Split(' ');
                    if(parts.Length != 2)
                    {
                        Console.WriteLine("Command not valid, Requires Name or All.");
                        continue;
                    }
                    var name = parts[1];
                    gradeBook.CalculateStudentStatistics(name);
                }
                else if(command == "help")
                {
                    Console.WriteLine("While a gradebook is open you can use the following commands:");
                    Console.WriteLine();
                    Console.WriteLine("Add 'Name' 'Student Type' 'Enrollment Type' - Adds a new student to the gradebook with the provided name, type of student, and type of enrollment.");
                    Console.WriteLine();
                    Console.WriteLine("Accepted Student Types:");
                    Console.WriteLine("Standard - Student not enrolled in Honors classes or Duel Enrolled.");
                    Console.WriteLine("Honors - Students enrolled in Honors classes and not Duel Enrolled.");
                    Console.WriteLine("DuelEnrolled - Students who are Duel Enrolled.");
                    Console.WriteLine();
                    Console.WriteLine("Accepted Enrollement Types:");
                    Console.WriteLine("Campus - Students who are in the same disctrict as the school.");
                    Console.WriteLine("State - Students who's legal residence is outside the school's district, but is in the same state as the school.");
                    Console.WriteLine("National - Students who's legal residence is not in the same state as the school, but is in the same country as the school.");
                    Console.WriteLine("International - Students who's legal residence is not in the same country as the school.");
                    Console.WriteLine();
                    Console.WriteLine("List - Lists all students.");
                    Console.WriteLine();
                    Console.WriteLine("AddGrade 'Name' 'Score' - Adds a new grade to a student with the matching name of the provided score.");
                    Console.WriteLine();
                    Console.WriteLine("RemoveGrade 'Name' 'Score' - Removes a grade to a student with the matching name and score.");
                    Console.WriteLine();
                    Console.WriteLine("Remove 'Name' - Removes the student with the provided name.");
                    Console.WriteLine();
                    Console.WriteLine("Statistics 'Name' - Gets statistics for the specified student.");
                    Console.WriteLine();
                    Console.WriteLine("Statistics All - Gets general statistics for the entire gradebook.");
                    Console.WriteLine();
                    Console.WriteLine("Close - closes the gradebook and takes you back to the starting command options.");
                    Console.WriteLine();
                    Console.WriteLine("Save - saves the gradebook to the hard drive for later use.");
                }
                else
                    Console.WriteLine("{0} was not recognized, please try again.", command);
                Console.WriteLine("=====================================");
            }
            Console.WriteLine(gradeBook.Name + " has been closed.");
        }
    }
}