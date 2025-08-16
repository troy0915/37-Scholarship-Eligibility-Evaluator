using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Applicant
{
    public string Name { get; }
    public double GPA { get; }
    public double Income { get; }
    public List<string> Extracurriculars { get; }
    public int Awards { get; }

    public Applicant(string name, double gpa, double income, List<string> extracurriculars, int awards)
    {
        Name = name;
        GPA = gpa;
        Income = income;
        Extracurriculars = extracurriculars.Select(e => e.ToLower().Trim()).ToList();
        Awards = awards;
    }

    public bool HasExtracurricular(string keyword)
    {
        return Extracurriculars.Contains(keyword.ToLower());
    }
}

public class ScholarshipRule
{
    public string Name { get; }
    public Func<Applicant, (bool eligible, List<string> reasons)> Evaluate { get; }

    public ScholarshipRule(string name, Func<Applicant, (bool, List<string>)> evaluate)
    {
        Name = name;
        Evaluate = evaluate;
    }
}

namespace _37__Scholarship_Eligibility_Evaluator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scholarships = new List<ScholarshipRule>
        {
            new ScholarshipRule("Academic Excellence", a =>
            {
                var reasons = new List<string>();
                if (a.GPA >= 3.8)
                    reasons.Add("GPA meets academic excellence threshold (>= 3.😎");
                else
                    reasons.Add("GPA too low for academic excellence");

                return (a.GPA >= 3.8, reasons);
            }),

            new ScholarshipRule("Community Leader", a =>
            {
                var reasons = new List<string>();
                bool active = a.HasExtracurricular("volunteer") || a.HasExtracurricular("student council");

                if (active)
                    reasons.Add("Has leadership-related extracurriculars");
                else
                    reasons.Add("No leadership extracurriculars found");

                bool awardsOk = a.Awards >= 1;
                if (awardsOk)
                    reasons.Add("Has at least 1 award");
                else
                    reasons.Add("No awards");

                return (active && awardsOk, reasons);
            }),

            new ScholarshipRule("Financial Need", a =>
            {
                var reasons = new List<string>();
                if (a.Income < 20000)
                    reasons.Add("Income below $20,000 threshold");
                else
                    reasons.Add("Income exceeds $20,000 threshold");

                bool decentGPA = a.GPA >= 3.0;
                if (decentGPA)
                    reasons.Add("GPA meets minimum requirement (>= 3.0)");
                else
                    reasons.Add("GPA below minimum requirement");

                return (a.Income < 20000 && decentGPA, reasons);
            })
        };

            var applicants = new List<Applicant>
        {
            new Applicant("Alice", 3.9, 18000, new List<string>{"Volunteer", "Drama Club"}, 2),
            new Applicant("Bob", 3.5, 25000, new List<string>{"Basketball"}, 0),
            new Applicant("Charlie", 3.2, 15000, new List<string>{"Student Council"}, 1)
        };

            foreach (var applicant in applicants)
            {
                Console.WriteLine($"\nApplicant: {applicant.Name}");
                foreach (var scholarship in scholarships)
                {
                    var (eligible, reasons) = scholarship.Evaluate(applicant);
                    Console.WriteLine($"  {scholarship.Name}: {(eligible ? "ELIGIBLE" : "NOT eligible")}");
                    foreach (var reason in reasons)
                    {
                        Console.WriteLine($"    - {reason}");
                    }
                }
            }
        }
    }
}
