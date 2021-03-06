using System;
using System.Globalization;

namespace StudentGrades
{
    public class InMemoryStudent : StudentBase
    {
        private List<double> grades;

        public InMemoryStudent(string name, string surname) : base(name, surname)
        {
            grades = new List<double>();
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            if (grade >= 0 && grade <= 6)
            {
                AddGradeIfContent(grade);
            }
            else
            {
                throw new ArgumentException($"Invalid argument: {nameof(grade)}");
            }
        }

        public override void AddGrade(string grade)
        {
            var stringChecklist = new List<string>() { "1", "2", "3", "4", "5", "6", "1+", "2+", "3+", "4+", "5+", "6+", "1-", "2-", "3-", "4-", "5-", "6-" };
            var result = double.Parse(grade.Substring(0, 1));

            if (stringChecklist.Contains(grade))
            {
                if (grade.Contains("+"))
                {
                    result += 0.5;
                    AddGradeIfContent(result);
                }
                else if (grade.Contains("-"))
                {
                    result -= 0.25;
                    AddGradeIfContent(result);
                }
                else
                {
                    AddGradeIfContent(result);
                }
            }
            else
            {
                throw new ArgumentException($"Invalid argument: {nameof(grade)}");
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            result.Highest = double.MinValue;
            result.Lowest = double.MaxValue;

            var index = 0;

            while (index < grades.Count)
            {
                result.Lowest = Math.Min(grades[index], result.Lowest);
                result.Highest = Math.Max(grades[index], result.Highest);
                result.AverageInMemory += grades[index];
                index++;
            }

            result.AverageInMemory /= grades.Count;

            return result;
        }
        private void AddGradeIfContent(double result)
        {
            this.grades.Add(result);
            if (GradeAdded != null)
            {
                GradeAdded(this, new EventArgs());
            }
        }
    }
}