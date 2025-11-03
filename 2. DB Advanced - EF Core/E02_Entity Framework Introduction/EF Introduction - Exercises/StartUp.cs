namespace SoftUni
{
    using System.Text;

    using Data;
    using Models;

    public class StartUp
    {
        static void Main(string[] args)
        {
            try
            {
                using SoftUniContext context = new SoftUniContext();

                string result = AddNewAddressToEmployee(context);

                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("## Unhandled exception occurred! ##");
                Console.WriteLine(e.Message);
            }
        }

        /* Problem 03 */
        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            /*
             *  SQL Server Query...
             *  .ToArray()/.ToList() -> Materialization Point
             *  Client In-Memory Processing...
             */
            var employees = context
                .Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary,
                })
                .ToArray();
            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 04
        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithSalary = context
                .Employees
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary,
                })
                .ToArray();

            foreach (var e in employeesWithSalary)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        /* Problem 05 */
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesRnD = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary,
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employeesRnD)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary.ToString("f2")}");
            }

            return sb.ToString().TrimEnd();
        }

        /* Problem 06 */
        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Employee nakovEmployee = context
                .Employees
                .First(e => e.LastName == "Nakov");

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4, 
            };
            nakovEmployee.Address = newAddress;

           context.SaveChanges();

            string[] employeesAddresses = context
                .Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => e.Address.AddressText)
                .Take(10)
                .ToArray();

            return string.Join(Environment.NewLine, employeesAddresses);
        }

        // Problem 07
        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithProjects = context
                .Employees
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    ManagerFirstName = e.Manager == null ?
                        null : e.Manager.FirstName,
                    ManagerLastName = e.Manager == null ?
                        null : e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Select(ep => ep.Project)
                        .Where(p => p.StartDate.Year >= 2001 &&
                                    p.StartDate.Year <= 2003)
                        .Select(p => new
                        {
                            ProjectName = p.Name,
                            p.StartDate,
                            p.EndDate
                        })
                        .ToArray()
                })
                .Take(10)
                .ToArray();

            foreach (var e in employeesWithProjects)
            {
                sb
                    .AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    string startDateFormatted = p.StartDate
                        .ToString("M/d/yyyy h:mm:ss tt");
                    string endDateFormatted = p.EndDate.HasValue ?
                        p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    sb
                        .AppendLine($"--{p.ProjectName} - {startDateFormatted} - {endDateFormatted}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        // Problem 14
        public static string DeleteProjectById(SoftUniContext context)
        {
            const int deleteProjectId = 2;

            IEnumerable<EmployeeProject> employeeProjectsDelete = context
                .EmployeesProjects
                .Where(ep => ep.ProjectId == deleteProjectId)
                .ToArray();
            context.EmployeesProjects.RemoveRange(employeeProjectsDelete);

            Project? deleteProject = context
                .Projects
                .Find(deleteProjectId);
            if (deleteProject != null)
            {
                context.Projects.Remove(deleteProject);
            }

            context.SaveChanges();

            string[] projectNames = context
                .Projects
                .Select(p => p.Name)
                .Take(10)
                .ToArray();

            return String.Join(Environment.NewLine, projectNames);
        }

        private static void RestoreDatabase(SoftUniContext context)
        {            
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
    

