

using ProductModel;

namespace ProductWebAPI2025.DataLayer
{
    public class DbSeeder
    {
        private readonly ProductDBContext _ctx;
        private readonly IWebHostEnvironment _hosting;
        private bool disposedValue;

        public DbSeeder(ProductDBContext ctx, IWebHostEnvironment hosting)
        {
            _ctx = ctx;
            _hosting = hosting;
        }

        public void SeedSuppliers()
        {
            if(!_ctx.Suppliers.Any())
            {
                List<Supplier> cvs_suppliers = 
                    DBHelper.GetResource<Supplier, MapSupplier>("ProductModel.Supplier.csv");
                _ctx.AddRange(cvs_suppliers);
                _ctx.SaveChanges();
            }
            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DbSeeder()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
