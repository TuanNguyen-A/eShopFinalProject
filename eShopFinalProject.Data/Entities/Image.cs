using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Image
    {
        public string PublicId { get; set; }
        public string Url { get; set; }
        public int? ProductId { get; set; }

        //Vitual Object
        public virtual Product Product { get; set; }
    }
}
