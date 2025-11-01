using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Educonnect
{
    class Program
    {
        static List<Student> students = new List<Student>();
        static string studentsFile = "students.json";

        static void Main()
        {
           
            LoadStudents();// class to load data from Json 

            Console.WriteLine("Welcome to EduConnect!");
            Console.WriteLine("Please select your role below:");
            Console.WriteLine("1: Student");
            Console.WriteLine("2: Personal Supervisor");
            Console.WriteLine("3: Senior Tutor");
            Console.Write("Enter your choice: ");
            string roleOption = Console.ReadLine();

            switch (roleOption)
            {
                case "1":
                    UserLogin("Student");
                    StudentMenu();
                    break;
                case "2":
                    UserLogin("Personal Supervisor");
                    PersonalSupervisorMenu();
                    break;
                case "3":
                    UserLogin("Senior Tutor");
                    SeniorTutorMenu();
                    break;
                default:
                    Console.WriteLine("Invalid selection. Exiting program.");
                    break;
            }

           
            SaveStudents();// SAVE THE DATA 
        }

        static void LoadStudents()
        {
            if (File.Exists(studentsFile))
            {
                string json = File.ReadAllText(studentsFile);
                students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
            }
        }

        static void SaveStudents()
        {
            string json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(studentsFile, json);
        }

        static void UserLogin(string role)
        {
            Console.WriteLine($"\nLogging in as a {role}.");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your ID: ");
            string id = Console.ReadLine();

            if (role == "Student")
            {
                Console.Write("Enter your course: ");
                string course = Console.ReadLine();

                Console.Write("Enter your year: ");
                string year = Console.ReadLine();

                
                var student = students.Find(s => s.ID == id);// ADD NEW STUDENT
                if (student == null)
                {
                    students.Add(new Student
                    {
                        Name = name,
                        ID = id,
                        CourseName = course,
                        Year = year
                    });
                }
            }

            Console.WriteLine($"Welcome, {name}! You are logged in as {role}.\n");
        }

        static void StudentMenu()// Menu for student user
        {
            Console.WriteLine("\nStudent Menu:");
            Console.WriteLine("1: Submit your mood");
            Console.WriteLine("2: Book a meeting with Personal Supervisor");
            Console.WriteLine("3: Submit a report");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    SubmitMood();
                    break;
                case "2":
                    BookMeeting("Personal Supervisor");
                    break;
                case "3":
                    SubmitReport();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        static void PersonalSupervisorMenu()//Menu for Ps
        {
            Console.WriteLine("\nPersonal Supervisor Menu:");
            Console.WriteLine("1: Book a meeting with a student");
            Console.WriteLine("2: Review a student");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    BookMeetingWithStudent();
                    break;
                case "2":
                    ReviewStudent();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        static void SeniorTutorMenu()//Menu for Senior tutor
        {
            Console.WriteLine("\nSenior Tutor Menu:");
            Console.WriteLine("1: View student and personal supervisors engagements");
            Console.WriteLine("2: View student statuses");
            Console.Write("Select an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewEngagements();
                    break;
                case "2":
                    ViewStudentStatuses();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        static void SubmitMood()//List of mood for sttudent users
        {
            Console.WriteLine("\nSelect a mood:");
            Console.WriteLine("1: Happy");
            Console.WriteLine("2: Sad");
            Console.WriteLine("3: Anxious");
            Console.Write("Enter your choice: ");
            string moodOption = Console.ReadLine();
            Console.Write("Please provide more details: ");
            string details = Console.ReadLine();
            Console.WriteLine($"Mood submitted: {moodOption} - Details: {details}");
        }

        static void SubmitReport()
        {
            Console.Write("Enter the title of the report: ");
            string reportTitle = Console.ReadLine();
            Console.Write("Enter your report details: ");
            string reportDetails = Console.ReadLine();
            Console.WriteLine($"Report '{reportTitle}' submitted successfully.");
        }

        static void BookMeeting(string participantName)
        {
            Console.Write($"\nEnter the meeting date with {participantName} (YYYY-MM-DD): ");
            string dateInput = Console.ReadLine();

            if (!DateTime.TryParse(dateInput, out DateTime meetingDate))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                return;
            }

            Console.Write("Enter the time of the meeting (HH:mm): ");
            string timeInput = Console.ReadLine();

            if (!DateTime.TryParse(timeInput, out DateTime meetingTime))// to avoid invalid time format
            {
                Console.WriteLine("Invalid time format. Please try again.");
                return;
            }

            Console.WriteLine($"Meeting scheduled with {participantName} on {meetingDate:yyyy-MM-dd} at {meetingTime:HH:mm}.");
        }

        static void BookMeetingWithStudent()
        {
            Console.Write("Enter the student's ID: ");
            string studentId = Console.ReadLine();
            BookMeeting("Student");
        }

        static void ReviewStudent()
        {
            Console.Write("Enter the student's ID: ");
            string studentId = Console.ReadLine();
            Console.Write("Enter your review: ");
            string review = Console.ReadLine();
            Console.WriteLine($"Review for student ID {studentId} submitted.");
        }

        static void ViewEngagements()
        {
            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}, ID: {student.ID}");
                foreach (var engagement in student.Engagements)
                {
                    Console.WriteLine($"  {engagement.Role} on {engagement.DateTime:yyyy-MM-dd}: {engagement.Details}");
                }
            }
        }

        static void ViewStudentStatuses()
        {
            foreach (var student in students)
            {
                Console.WriteLine($"Student: {student.Name}, ID: {student.ID}, Course: {student.CourseName}, Year: {student.Year}");
            }
        }

        public class Student
        {
            public string Name { get; set; }
            public string ID { get; set; }
            public string CourseName { get; set; }
            public string Year { get; set; }
            public List<Engagement> Engagements { get; set; } = new List<Engagement>();
        }

        public class Engagement
        {
            public string Role { get; set; }
            public DateTime DateTime { get; set; }
            public string Details { get; set; }
        }
        

    }
}
