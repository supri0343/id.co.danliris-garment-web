using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class UpdateGarmentSewingOutCommandHandler : ICommandHandler<UpdateGarmentSewingOutCommand, GarmentSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public UpdateGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentSewingOut> Handle(UpdateGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSewingOut(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                var item = request.Items.Where(o => o.Id == sewOutItem.Identity).Single();

                var diffSewInQuantity = sewOutItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity);

                if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                {
                    sewInItemToBeUpdated[sewOutItem.SewingInItemId] += diffSewInQuantity;
                }
                else
                {
                    sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, diffSewInQuantity);
                }

                if (!item.IsSave)
                {
                    item.Quantity = 0;

                    if (request.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            sewOutDetail.Remove();
                            await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                        });
                    }

                    sewOutItem.Remove();

                }
                else
                {
                    

                    if (request.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            if (sewOutDetail.Identity != Guid.Empty)
                            {
                                var detail = item.Details.Where(o => o.Id == sewOutDetail.Identity).SingleOrDefault();

                                if (detail != null)
                                {
                                    sewOutDetail.SetQuantity(detail.Quantity);
                                    sewOutDetail.SetSizeId(new SizeId(detail.Size.Id));
                                    sewOutDetail.SetSizeName(detail.Size.Size);

                                    sewOutDetail.Modify();
                                    
                                }
                                else
                                {
                                    sewOutDetail.Remove();
                                }
                                await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                            }
                            else
                            {
                                GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                Guid.NewGuid(),
                                sewOutItem.Identity,
                                sewOutDetail.SizeId,
                                sewOutDetail.SizeName,
                                sewOutDetail.Quantity,
                                sewOutDetail.UomId,
                                sewOutDetail.UomUnit
                            );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);
                            }
                            
                        });

                        foreach(var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty)
                            {
                                GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                    Guid.NewGuid(),
                                    sewOutItem.Identity,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit
                                );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);
                            }
                        }
                        sewOutItem.SetQuantity(item.TotalQuantity);
                    }
                    else
                    {
                        sewOutItem.SetQuantity(item.Quantity);
                    }

                    sewOutItem.SetRemainingQuantity(item.RemainingQuantity);
                    sewOutItem.Modify();
                }


                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.Modify();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
