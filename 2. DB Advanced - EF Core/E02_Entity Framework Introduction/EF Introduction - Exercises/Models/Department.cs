//using System;
//using System.Collections.Generic;

//namespace E01_EntityFramework_Introduction.Models;

//public class Department
//{
//    public int DepartmentId { get; set; }//PK

//    public string Name { get; set; } = null!;

//    public int ManagerId { get; set; }//FK - Many

//    public virtual Employee Manager { get; set; } = null!;//NAV PROP TO FK - One

//    public virtual ICollection<Employee> Employees { get; set; } 
//        = new List<Employee>();//MANY EMPLOYEES TO ONE DEPT
//}

//using E01_EntityFramework_Introduction.Models;

namespace SoftUni.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; } = null!;

        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; } = null!;

        public virtual ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}