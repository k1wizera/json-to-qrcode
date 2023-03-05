using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using System.Diagnostics;
using System.Text.Json.Serialization;
using WebQrCode.Models;
using System.Drawing;

namespace WebQrCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            QRCodeGenerator qrCodeGenerator= new QRCodeGenerator();

            var dados = new Dados
            {
                Id = 1,
                Nome = "Otávio Faria",
                CPF = "41049658809"
            };

            var json = JsonConvert.SerializeObject(dados);

            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            var bitMapBytes = BitmapToBytes(qrCodeImage);

            return File(bitMapBytes, "image/jpeg");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                return stream.ToArray();
            }
        }
    }
}