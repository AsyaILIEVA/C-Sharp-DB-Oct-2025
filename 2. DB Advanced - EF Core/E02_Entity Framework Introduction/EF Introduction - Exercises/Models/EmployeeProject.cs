//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace E01_EntityFramework_Introduction.Models
//{
//    public class EmployeeProject
//    {
//        public int EmployeeId { get; set; }
//        public virtual Employee Employee { get; set; } = null!;
//        public int ProjectId { get; set; }
//        public virtual Project Project { get; set; } = null!;
//    }
//}

//using E01_EntityFramework_Introduction.Models;

namespace SoftUni.Models
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; } = null!;
    }
}