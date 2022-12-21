using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentAvalProducts.Queries.GetForLoaderAval_BC
{
    public class GetForLoaderAval_BCQuery : IQuery<GetForLoaderAval_BCViewModel>
    {
        //public int page { get; private set; }
        public int unit { get; private set; }
        public string ro { get; private set; }
        //public bool filter { get; private set; }
        //public string keyword { get; private set; }
        public string token { get; private set; }


        public GetForLoaderAval_BCQuery(int unit, string ro, string token)
        {
            //this.page = page;
            //this.size = size;
            //this.order = order;
            this.unit = unit;
            this.ro = ro;
            this.token = token;
        }

    }
}
