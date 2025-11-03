//using System;
//using System.Collections.Generic;

//namespace E01_EntityFramework_Introduction.Models;

//public class Town
//{
//    public int TownId { get; set; }//PK-ONE TOWN

//    public string Name { get; set; } = null!;//NOT NULL

//    public virtual ICollection<Address> Addresses { get; set; } 
//        = new List<Address>();//TOWNID-FK -MANY ADDRESSES
//}

namespace SoftUni.Models
{
    public class Town
    {
        public int TownId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Address> Addresses { get; set; }
            = new List<Address>();
    }
}