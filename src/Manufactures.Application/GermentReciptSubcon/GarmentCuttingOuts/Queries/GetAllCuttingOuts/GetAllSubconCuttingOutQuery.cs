using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GermentReciptSubcon.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    public class GetAllSubconCuttingOutQuery : IQuery<SubconCuttingOutListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string keyword { get; private set; }
        public string filter { get; private set; }

        public GetAllSubconCuttingOutQuery(int page, int size, string order, string keyword, string filter)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.keyword = keyword;
            this.filter = filter;
        }
    }
}
