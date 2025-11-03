//using System;
//using System.Collections.Generic;

//namespace E01_EntityFramework_Introduction.Models;

//public class Project
//{
//    public int ProjectId { get; set; }//PK

//    public string Name { get; set; } = null!;//NOT NULL

//    public string? Description { get; set; }// CAN BE NULL

//    public DateTime StartDate { get; set; }

//    public DateTime? EndDate { get; set; }//CAN BE NULL

//    public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; } 
//        = new List<EmployeeProject>();
//}

namespace SoftUni.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ICollection<EmployeeProject> EmployeesProjects { get; set; }
            = new List<EmployeeProject>();
    }
}