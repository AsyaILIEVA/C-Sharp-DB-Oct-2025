//using System;
//using System.Collections.Generic;

//namespace E01_EntityFramework_Introduction.Models;

//public class Address
//{
//    //Primary Key
//    public int AddressId { get; set; }

//    public string AddressText { get; set; } = null!;//not null

//    //This is property corresponding to FK TownID
//    public int? TownId { get; set; } // int? - nullable, can be null

//    //This is the navigation property corresponding to FK TownId;  a.Town.Name
//    public virtual Town? Town { get; set; } // can be overriden, lazy loading

//    //e.AddressId(FK-many) -> a.AddressId(PK-one)
//    public virtual ICollection<Employee> Employees { get; set; } 
//        = new List<Employee>();


//}

//using E01_EntityFramework_Introduction.Models;

namespace SoftUni.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string AddressText { get; set; } = null!;

        /* This is the property corresponding to FK TownID */
        public int? TownId { get; set; }

        /* This is the navigation property corresponding to FK TownID */
        /* virtual keyword is used to enable 'Lazy' loading of data -> memory reduction */
        /* This can lead to SQL network overhead */
        public virtual Town? Town { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
            = new List<Employee>();
    }
}