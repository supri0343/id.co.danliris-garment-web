using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class PlaceGarmentSampleRequestCommandHandler : ICommandHandler<PlaceGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository _GarmentSampleRequestProductRepository;
        private readonly IGarmentSampleRequestSpecificationRepository _garmentSampleRequestSpecificationRepository;

        public PlaceGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            _garmentSampleRequestSpecificationRepository = storage.GetRepository<IGarmentSampleRequestSpecificationRepository>();
        }

        public async Task<GarmentSampleRequest> Handle(PlaceGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            request.SampleProducts = request.SampleProducts.ToList();
            request.SampleSpecifications = request.SampleSpecifications.ToList();

            GarmentSampleRequest GarmentSampleRequest = new GarmentSampleRequest(
                Guid.NewGuid(),
                request.SampleCategory,
                GenerateSampleRequestNo(request),
                GenerateROSample(request),
                request.RONoCC,
                request.Date,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SampleType,
                request.Packing,
                request.SentDate,
                request.POBuyer,
                request.Attached,
                request.Remark,
                request.IsPosted,
                request.IsReceived,
                request.ReceivedDate,
                request.ReceivedBy,
                request.IsRejected,
                request.RejectedDate,
                request.RejectedBy,
                request.IsRevised,
                request.RevisedBy,
                request.RevisedReason,
                request.ImagesPath,
                request.DocumentsPath,
                request.ImagesName,
                request.DocumentsFileName,
                new SectionId(request.Section.Id),
                request.Section.Code
            );

            foreach (var product in request.SampleProducts)
            {
                GarmentSampleRequestProduct GarmentSampleRequestProduct = new GarmentSampleRequestProduct(
                    Guid.NewGuid(),
                    GarmentSampleRequest.Identity,
                    product.Style,
                    product.Color,
                    new SizeId(product.Size.Id),
                    product.Size.Size,
                    product.SizeDescription,
                    product.Quantity,
                    product.Index
                );

                await _GarmentSampleRequestProductRepository.Update(GarmentSampleRequestProduct);
            }



            foreach (var specification in request.SampleSpecifications)
            {

                GarmentSampleRequestSpecification GarmentSampleRequestSpecification = new GarmentSampleRequestSpecification(
                    Guid.NewGuid(),
                    GarmentSampleRequest.Identity,
                    specification.Inventory,
                    specification.SpecificationDetail,
                    specification.Quantity,
                    specification.Remark,
                    new UomId(specification.Uom.Id),
                    specification.Uom.Unit,
                    specification.Index
                );

                await _garmentSampleRequestSpecificationRepository.Update(GarmentSampleRequestSpecification);
            }

            await _GarmentSampleRequestRepository.Update(GarmentSampleRequest);

            _storage.Save();

            return GarmentSampleRequest;
        }

        private string GenerateSampleRequestNo(PlaceGarmentSampleRequestCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yyyy");
            var month = now.ToString("MM");
            var code = request.Buyer.Code;

            var prefix = $"{code}/{month}{year}/";

            var lastSampleRequestNo = _GarmentSampleRequestRepository.Query.Where(w => w.SampleRequestNo.StartsWith(prefix))
                .OrderByDescending(o => o.SampleRequestNo)
                .Select(s => int.Parse(s.SampleRequestNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SampleRequestNo = $"{prefix}{(lastSampleRequestNo + 1).ToString("D3")}";

            return SampleRequestNo;
        }

        private string GenerateROSample(PlaceGarmentSampleRequestCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SampleCategory=="Commercial Sample" ? "CS" : "NCS";

            var prefix = $"{year}";

            var lastRONo = _GarmentSampleRequestRepository.Query.Where(w => w.RONoSample.StartsWith(prefix) && w.RONoSample.EndsWith(code))
                .OrderByDescending(o => o.RONoSample)
                .Select(s => int.Parse(s.RONoSample.Substring(2,5)))
                .FirstOrDefault();
            var RONo = $"{prefix}{(lastRONo + 1).ToString("D5")}{code}";

            return RONo;
        }
    }
}

