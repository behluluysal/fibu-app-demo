using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Pagination
{
    public class QueryParams
    {
        public int Page { get; set; } = 0;
        public int PostsPerPage { get; set; } = 999999999;

        private string _related = "";
        public string[] Related
        {
            get
            {
                return _related.Split(",");
            }
            set
            {
                if (value.Count() != 0)
                    _related = String.Join(",",value);
                else
                    _related = "";
            }
        }


        private string _filter = "(x=>x.Id != null)";
        public string Filter 
        {
            get
            {
                return _filter;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    _filter = "(x=>x.Id != null)";
                else
                    _filter = value;
            }
        }

        private string _sort = "x=>x.Id Asc";
        public string Sort 
        {
            get
            {
                return _sort;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    _sort = "x=>x.Id Asc";
                else
                    _sort = value;
            }
        }

        
    }
}
