using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Utilities.ViewModel.Blogs
{
    public class CreateBlogRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
