using OfficeOpenXml;
using Persistence.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataExporter
{
    public class ParettoExporter : IExporter
    {
        private readonly string _destination;
        private readonly ParettoHandler _handler;
        private readonly IVenteRepository _venteRepository;

        public ParettoExporter(string destination, ParettoHandler handler, IVenteRepository venteRepository)
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
                
                var worksheet = package.Workbook.Worksheets.Add(nameof(ParettoExporter));

                int columnId = 1;
                foreach(var row in data)
                {
                    worksheet.Cells[1, columnId].Value = row.Count;
                    worksheet.Cells[2, columnId].Value = row.CountPercent;
                    worksheet.Cells[3, columnId++].Value = row.SumPercent;
                }

                
                await package.SaveAsync();
            }
        }
    }
}