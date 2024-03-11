using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplikacjaPogody3._0
{
    public class OWApiResponse
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public IList<List> list { get; set; }
        public City city { get; set; }

    }
}
