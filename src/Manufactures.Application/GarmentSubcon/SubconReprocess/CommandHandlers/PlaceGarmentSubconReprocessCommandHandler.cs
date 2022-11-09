using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Commands;
using Manufactures.Domain.GarmentSubcon.SubconReprocess.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.SubconReprocess.CommandHandlers
{
    public class PlaceGarmentSubconReprocessCommandHandler : ICommandHandler<PlaceGarmentSubconReprocessCommand, GarmentSubconReprocess>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconReprocessRepository _garmentSubconReprocessRepository;
        private readonly IGarmentSubconReprocessItemRepository _garmentSubconReprocessItemRepository;
        private readonly IGarmentSubconReprocessDetailRepository _garmentSubconReprocessDetailRepository;

        public PlaceGarmentSubconReprocessCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconReprocessRepository = storage.GetRepository<IGarmentSubconReprocessRepository>();
            _garmentSubconReprocessItemRepository = storage.GetRepository<IGarmentSubconReprocessItemRepository>();
            _garmentSubconReprocessDetailRepository = storage.GetRepository<IGarmentSubconReprocessDetailRepository>();
        }

        public async Task<GarmentSubconReprocess> Handle(PlaceGarmentSubconReprocessCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();
            
            GarmentSubconReprocess garmentSubconReprocess = new GarmentSubconReprocess(
                Guid.NewGuid(),
                GenerateSubconReprocessNo(request),
                request.ReprocessType,
                request.Date
            );

            await _garmentSubconReprocessRepository.Update(garmentSubconReprocess);

            foreach (var item in request.Items)
            {
                GarmentSubconReprocessItem garmentSubconReprocessItem = new GarmentSubconReprocessItem(
                    Guid.NewGuid(),
                    garmentSubconReprocess.Identity,
                    item.ServiceSubconSewingId,
                    item.ServiceSubconSewingNo,
                    item.ServiceSubconSewingItemId,
                    item.ServiceSubconCuttingId,
                    item.ServiceSubconCuttingNo,
                    item.ServiceSubconCuttingItemId,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name,
                    new BuyerId(item.Buyer.Id),
                    item.Buyer.Code,
                    item.Buyer.Name
                );

                await _garmentSubconReprocessItemRepository.Update(garmentSubconReprocessItem);

                foreach (var detail in item.Details)
                {   
                    GarmentSubconReprocessDetail garmentSubconReprocessDetail = new GarmentSubconReprocessDetail(
                        Guid.NewGuid(),
                        garmentSubconReprocessItem.Identity,
                        garmentSubconReprocess.ReprocessType == "SUBCON JASA KOMPONEN" ? new SizeId(detail.Size.Id) : null,
                        garmentSubconReprocess.ReprocessType== "SUBCON JASA KOMPONEN" ?detail.Size.Size : null,
                        detail.Quantity,
                        detail.ReprocessQuantity,
                        new UomId(detail.Uom.Id),
                        detail.Uom.Unit,
                        detail.Color,
                        detail.ServiceSubconCuttingDetailId,
                        detail.ServiceSubconCuttingSizeId,
                        detail.ServiceSubconSewingDetailId,
                        detail.DesignColor,
                        garmentSubconReprocess.ReprocessType != "SUBCON JASA KOMPONEN" ? new UnitDepartmentId(detail.Unit.Id) : null,
                        garmentSubconReprocess.ReprocessType != "SUBCON JASA KOMPONEN" ? detail.Unit.Code : null,
                        garmentSubconReprocess.ReprocessType != "SUBCON JASA KOMPONEN" ? detail.Unit.Name : null,
                        detail.Remark
                    );
                    await _garmentSubconReprocessDetailRepository.Update(garmentSubconReprocessDetail);
                }
            }


            _storage.Save();

            return garmentSubconReprocess;
        }

        private string GenerateSubconReprocessNo(PlaceGarmentSubconReprocessCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"RPS{year}{month}";

            var lastSubconReprocessNo = _garmentSubconReprocessRepository.Query.Where(w => w.ReprocessNo.StartsWith(prefix))
                .OrderByDescending(o => o.ReprocessNo)
                .Select(s => int.Parse(s.ReprocessNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SubconReprocessNo = $"{prefix}{(lastSubconReprocessNo + 1).ToString("D3")}";

            return SubconReprocessNo;
        }
    }
}