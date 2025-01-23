using OfficeOpenXml;
using Persistence.Contracts;
using System;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExporter
{
    public class PercentExporter : IExporter
    {
        private readonly string _destination;
        private readonly PercentHandler _handler;
        private readonly IVenteRepository _venteRepository;

        public PercentExporter(string destination, PercentHandler handler, IVenteRepository venteRepository)
        {
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _venteRepository = venteRepository ?? throw new ArgumentNullException(nameof(venteRepository));
        }

        public async Task Export()
        {

            var ventes = await _venteRepository.GetAll();
            var data = _handler.Calcualte(ventes);

            FileInfo existingFile = new FileInfo(_destination);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                var worksheet = package.Workbook.Worksheets.Add(nameof(PercentExporter));

                int rowId = 1;
                foreach (var row in data)
                {
                    worksheet.Cells[rowId, 1].Value = row.ProductId;
                    worksheet.Cells[rowId, 2].Value = row.Percent;
                    worksheet.Cells[rowId++, 3].Value = row.Sum;
                }


                await package.SaveAsync();
            }
        }
    }
}
