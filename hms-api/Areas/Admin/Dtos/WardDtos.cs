using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Areas.Admin.Dtos
{
    public class WardDtoForCreate
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }

    public class WardDtoForUpdate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
    }

    public class WardDtoForDelete
    {
        public string Id { get; set; }
       
    }

    public class WardDtoForView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
    }



    //public class WardSubCategoryDtoForCreate
    //{
    //    public string Name { get; set; }
    //    public int Capacity { get; set; }
    //}

    //public class WardSubCategoryDtoForUpdate
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public int Capacity { get; set; }
    //}

    //public class WardSubCategoryDtoForDelete
    //{
    //    public string Id { get; set; }

    //}

    //public class WardSubCategoryDtoForView
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public int Capacity { get; set; }
    //}





    //public class WardCategoryDtoForCreate
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //}

    //public class WardCategoryDtoForUpdate
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //}

    //public class WardCategoryDtoForDelete
    //{
    //    public string Id { get; set; }

    //}

    //public class WardCategoryDtoForView
    //{
    //    public string Id { get; set; }
    //    public string Name { get; set; }
    //    public int Capacity { get; set; }
    //}
}
