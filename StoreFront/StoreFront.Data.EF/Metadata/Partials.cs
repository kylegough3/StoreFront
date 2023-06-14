using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StoreFront.Data.EF.Models //.Metadata
{
    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    [ModelMetadataType(typeof(ManufacturerMetadata))]
    public partial class Manufacturer { }

    [ModelMetadataType(typeof(OrderMetaData))]
    public partial class Order { }

    [ModelMetadataType(typeof(OrderProductsMetadata))]
    public partial class OrderProducts { }

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product { }

    [ModelMetadataType(typeof(StockStatusMetadata))]
    public partial class StockStatus { }

    [ModelMetadataType(typeof(UserMetadata))]
    public partial class User { }
}
