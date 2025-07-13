using Barnamenevisan.Data.Context;
using Barnamenevisan.Domain.Interfaces;
using Barnamenevisan.Domain.Models.Ecommerce;

namespace Barnamenevisan.Data.Repositories;

public class SliderImageRepository(BarnamenevisanDbContext context):Repository<SliderImage>(context), ISliderImageRepository
{
    
}