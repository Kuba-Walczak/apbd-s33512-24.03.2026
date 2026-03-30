using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        return UniversityData.Students
            .Where(s => s.City == "Warsaw")
            .Select(s => $"{s.IndexNumber}, {s.FirstName}, {s.LastName}, {s.City}")
            .ToList();
    }
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students
            .Select(s => s.Email)
            .ToList();
    }
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        return UniversityData.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => $"{s.IndexNumber}, {s.FirstName}, {s.LastName}")
            .ToList();
    }
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        return [UniversityData.Courses
            .Where(c => c.Category == "Analytics")
            .Select(c => $"{c.Title}, {c.StartDate}")
            .FirstOrDefault() ?? "No course found"];
    }
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        return
        [UniversityData.Enrollments
            .Exists(e => e.IsActive) ? "Yes" : "No"];
    }
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        return
        [UniversityData.Lecturers
            .Count(l => !string.IsNullOrEmpty(l.Department)) == UniversityData.Lecturers.Count ? "Yes" : "No"];
    }
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        return [UniversityData.Enrollments
            .Count(e => e.IsActive)
            .ToString()];
    }
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students
            .OrderBy(s => s.City)
            .Select(s => s.City)
            .Distinct()
            .ToList();
    }
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        return UniversityData.Enrollments
            .OrderByDescending(e => e.EnrollmentDate)
            .Select(e => $"{e.EnrollmentDate}, {e.StudentId}, {e.CourseId}")
            .Take(3)
            .ToList();
    }
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        return UniversityData.Courses
            .OrderBy(c => c.Title).Select(c => $"{c.Title}, {c.Category}").Skip(2).Take(2)
            .ToList();
    }
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        return UniversityData.Students
            .Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new {s, e})
            .Select(x => $"{x.s.FirstName}, {x.s.LastName}, {x.e.CourseId}")
            .ToList();
    }
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        return UniversityData.Enrollments
            .Join(UniversityData.Students, e => e.StudentId, s => s.Id, (e, s) => new {e, s})
            .Join(UniversityData.Courses, es => es.e.CourseId, c => c.Id, (es, c) => new {es.e, es.s, c})
            .Select(x => $"{x.s.FirstName}, {x.s.LastName}, {x.c.Title}")
            .ToList();
    }
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        return UniversityData.Enrollments
            .Join(UniversityData.Courses, e => e.CourseId, c => c.Id, (e, c) => c)
            .GroupBy(x => x.Title).Select(x => $"{x.Key}, {x.Count()}")
            .ToList();
    }
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        return UniversityData.Enrollments
            .Join(UniversityData.Courses, e => e.CourseId, c => c.Id, (e, c) => new { e, c })
            .Where(x => x.e.FinalGrade.HasValue)
            .GroupBy(x => x.c.Title)
            .Select(x => $"{x.Key}, {x.Average(x => x.e.FinalGrade.Value)}")
            .ToList();
    }
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        return UniversityData.Lecturers
            .GroupJoin(UniversityData.Courses, l => l.Id, c => c.LecturerId, (l, c) => new { l, c })
            .Select(x => $"{x.l.FirstName}, {x.l.LastName}, {x.c.Count()}")
            .ToList();
    }
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        return UniversityData.Students
            .Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new { s, e })
            .Where(x => x.e.FinalGrade.HasValue)
            .GroupBy(x => $"{x.s.FirstName}, {x.s.LastName}")
            .Select(x => $"{x.Key}, {x.Max(x => x.e.FinalGrade.Value)}")
            .ToList();
    }
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        return UniversityData.Students
            .Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new { s, e })
            .Where(x => x.e.IsActive)
            .GroupBy(x => $"{x.s.FirstName}, {x.s.LastName}")
            .Where(x => x.Count() > 1)
            .Select(x => $"{x.Key}, {x.Count()}")
            .ToList();
    }
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        return UniversityData.Courses
            .Join(UniversityData.Enrollments, c => c.Id, e => e.CourseId, (c, e) => new { c, e })
            .Where(x => x.c.StartDate.Month == 4 && x.c.StartDate.Year == 2026)
            .GroupBy(x => x.c.Title)
            .Where(x => x.Any(x => x.e.FinalGrade.HasValue))
            .Select(x => x.Key)
            .ToList();
    }
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        return UniversityData.Lecturers
            .GroupJoin(UniversityData.Courses, l => l.Id, c => c.LecturerId, (l, c) => new { l, c })
            .SelectMany(x => x.c.DefaultIfEmpty(), (lc, c) => new { lc, c })
            .GroupJoin(UniversityData.Enrollments, x => x.c.Id, e => e.CourseId, (lcc, e) => new { lcc, e })
            .SelectMany(x => x.e.DefaultIfEmpty(), (lcce, e) => new { lcce, e })
            .Where(x => x.e.FinalGrade.HasValue)
            .GroupBy(x => $"{x.lcce.lcc.lc.l.FirstName}, {x.lcce.lcc.lc.l.LastName}")
            .Select(x => $"{x.Key}, {x.Average(e => e.e.FinalGrade.Value)}")
            .ToList();
    }
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        return UniversityData.Students
            .Join(UniversityData.Enrollments, s => s.Id, e => e.StudentId, (s, e) => new { s, e})
            .Where(x => x.e.IsActive)
            .GroupBy(x => x.s.City)
            .OrderByDescending(g => g.Count())
            .Select(x => $"{x.Key}, {x.Count()}")
            .ToList();
    }
    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
