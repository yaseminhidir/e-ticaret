using ETicaret.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Core.Controllers
{
    public class AdminOrderController : Controller
    {
        private eticaret_coreContext db;
        public AdminOrderController(eticaret_coreContext db)
        {
            this.db = db;
        }
        [HttpPost]
        public IActionResult GetOrders()
        {
            var query = db.Orders.
            Include(x => x.Address).
            Include(x => x.OrderStatu).
            Include(x => x.OrderDetails).
                ThenInclude(y => y.Product).
                 ThenInclude(i => i.ProductImages).
            Include(x => x.Customer).ToList();

            return Json(query);
        }
        [HttpPost]
        public IActionResult GetOrderStatus()
        {
            var status = db.OrderStatuses.ToList();

            return Json(status);
        }
        [HttpPost]
        public IActionResult GetOrderById([FromBody] DetailsWithIdRequest req)
        {
            var query = db.Orders.Where(x => x.Id == req.Id).
                    Include(x => x.Address).
                    Include(x => x.OrderStatu).
                    Include(x => x.OrderDetails).
                         ThenInclude(y => y.Product).
                               ThenInclude(z => z.ProductImages).FirstOrDefault();
            return Json(query);
        }
        [HttpPost]
        public IActionResult UpdateStatu([FromBody] UpdateOrderStatuRequest req)
        {
            var query = db.Orders.Where(x => x.Id == req.Id).FirstOrDefault();
            query.OrderStatuId = req.StatuId;
            db.SaveChanges();
            return Json(new { message = "Başarıyla güncellendi" });
        }
        [HttpPost]
        public IActionResult EditAddress([FromBody] Address address)
        {
            db.Addresses.Attach(address);
            db.Entry(address).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { message = "Başarıyla güncellendi" });
        }
        [HttpPost]
        public IActionResult CancelProduct([FromBody] OrderDetail orderDetail)
        {
            var query = db.OrderDetails.Where(x => x.Id == orderDetail.Id).FirstOrDefault();
            query.IsCanceled = true;
            var product = db.Products.Where(x => x.Id == orderDetail.ProductId).FirstOrDefault();
            product.Quantity = product.Quantity + orderDetail.Quantity;
            var order = db.Orders.Where(x => x.Id == orderDetail.OrderId).FirstOrDefault();
            order.TotalPrice = order.TotalPrice - (orderDetail.ProductPrice * orderDetail.Quantity);

            db.SaveChanges();
            return Json(new { message = "Başarıyla silindi" });
        }

        [HttpGet]
        public IActionResult GetOrdersToCsv()
        {
            var orders = db.Orders
                .Include(x => x.Address)
                .Include(x => x.Customer)
                .Include(x => x.OrderStatu)
                .ToList();
            List<string> csvLines = new List<string>();
            foreach (var order in orders)
            {
                var line = $@"{order.Customer.FirstName};{order.Customer.LastName};{order.Address.Name};{order.OrderStatu.Name};{order.TotalPrice}";
                csvLines.Add(line);
            }
            // arraydeki stringleri tek bir string haline getirir, stringlerin aralarına bi şey koyar.
            // \n alt satır karekterini temsil eder.
            var output = "sep=;\n" + string.Join('\n', csvLines);
            var memstr = new MemoryStream();
            var writer = new StreamWriter(memstr, Encoding.UTF8);
            writer.Write(output);
            writer.Flush();
            return File(memstr, "text/csv", "orders.csv");
        }

        [HttpGet]
        public IActionResult GetOrdersExcel()
        {
            var orders = db.Orders
                .Include(x => x.Address)
                .Include(x => x.Customer)
                .Include(x => x.OrderStatu)
                .ToList();
            // MemoryStream -> Ramde dosya oluşturur
            var memoryStream = new MemoryStream();
            // --- Below code would create excel file with dummy data----  

            //Excel oluşturuyor
            XSSFWorkbook workbook = new XSSFWorkbook();
            //Excel 'te sheet oluşturur.
            ISheet excelSheet = workbook.CreateSheet("rapor");

            //satır index 'i tutma
            int rowIndex = 0;

            foreach (var order in orders)
            {
                // satır oluşturma
                IRow row = excelSheet.CreateRow(rowIndex);
                //sütun oluşturup sütuna değerleri atama.
                row.CreateCell(0).SetCellValue(order.Id);
                row.CreateCell(1).SetCellValue(order.Customer.FirstName);
                row.CreateCell(2).SetCellValue(order.Customer.LastName);
                row.CreateCell(3).SetCellValue(order.Address.Name);
                row.CreateCell(4).SetCellValue(order.OrderStatu.Name);
                row.CreateCell(5).SetCellValue(Convert.ToDouble(order.TotalPrice ?? 0));
                //bir sonraki satır içni index 'i arttırma
                rowIndex++;
            }
            //yukarıda oluşturulan excel şablonu Ram 'de oluşturulan dosyaya yazılır.
            workbook.Write(memoryStream, true);
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "orders.xlsx");
        }

        [HttpPost]
        public IActionResult UploadOrderStatuExcel(IFormFile file)
        {
            XSSFWorkbook workbook = new XSSFWorkbook(file.OpenReadStream());
            var sheet = workbook.GetSheetAt(0);
            List<string> errorList = new List<string>();
            var orderStatuses = db.OrderStatuses.ToList();

            for (int rowIndex = 0; rowIndex <= sheet.LastRowNum; rowIndex++)
            {
                try
                {
                    var row = sheet.GetRow(rowIndex);

                    var orderId = row.GetCell(0).NumericCellValue;
                    var status = row.GetCell(4).StringCellValue;

                    var order = db.Orders.Where(x => x.Id == orderId).Include(x => x.OrderStatu).FirstOrDefault();
                    if (order == null)
                    {
                        errorList.Add($@"{orderId} Id 'li ürün bulunamadı. Satır {rowIndex + 1}");
                        continue;
                    }
                    if (status != order.OrderStatu.Name)
                    {
                        var newStatus = orderStatuses.Where(x => x.Name == status).FirstOrDefault();
                        if (newStatus == null)
                        {
                            var allStatuses = string.Join(", ", orderStatuses.Select(x => x.Name));
                            errorList.Add($@"{rowIndex + 1} numaralı satırdaki Statü bulunmadı. Order statüleri {allStatuses} olmalıdır.");
                            continue;
                        }
                        order.OrderStatuId = newStatus.Id;
                    }
                }
                catch
                {
                    errorList.Add($@"{rowIndex + 1} numaralı satırda okuması hatası gerçekleşti, lütfen tüm sütunları kontrol ediniz.");
                }
            }
            if (errorList.Count == 0)
            {
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { errorList, success = false });
            }

        }
        public class UpdateOrderStatuRequest
        {
            public int StatuId { get; set; }
            public int Id { get; set; }
        }

        public class CancelProductRequest
        {
            public Product Product { get; set; }
            public int Id { get; set; }
        }
    }
}
