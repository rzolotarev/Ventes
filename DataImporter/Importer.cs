using OfficeOpenXml;
using Persistence.Contracts;
using Persistence.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataImporter
{
    public class Importer
    {
        private readonly string _source;
        private readonly IVenteRepository _venteRepository;

        public Importer(string source, IVenteRepository venteRepository)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _venteRepository = venteRepository ?? throw new ArgumentNullException(nameof(venteRepository));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task ToDb()
        {
            await _venteRepository.Truncate();

            var counter = 1000;
            var collection = new List<Vente>(1000);
            foreach (var row in ReadRows())
            {
                counter--;
                collection.Add(new Vente { ClientId = row.ClientId, Price = row.Price, ProductId = row.ProductId, SoldCount = row.SoldCount, SoldDate = row.SoldDate });

                if (counter == 0)
                {
                    await _venteRepository.SaveAsync(collection);
                    collection.Clear();
                    counter = 1000;
                }
            }
        }

        public IEnumerable<Vente> ReadRows()
        {
            FileInfo existingFile = new FileInfo(_source);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.End.Row;     //get row count
               
                for (int row = 2; row <= rowCount; row++)
                {
                    var vente = new Vente();
                    vente.ClientId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim());
                    vente.SoldDate = DateTime.FromOADate(double.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()));
                    vente.ProductId = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim());
                    vente.SoldCount = short.Parse(worksheet.Cells[row, 4].Value.ToString().Trim());
                    vente.Price = decimal.Parse(worksheet.Cells[row, 5].Value.ToString().Trim());

                    yield return vente;
                }
            }
        }
    }
}