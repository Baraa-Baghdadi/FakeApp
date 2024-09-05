using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;


namespace DawaaNeo.QrPdf
{
    public class QrPdfService : ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly Lazy<IConfiguration> _configuration;
        private readonly IWebHostEnvironment _Environment;
        public QrPdfService(ICurrentUser currentUser, Lazy<IConfiguration> configuration, IWebHostEnvironment Environment)
        {
            _currentUser = currentUser;
            _configuration = configuration;
            _Environment = Environment;
        }

        public async Task<string> GenerateQrPdf(Guid providerId)
        {
            var path = Path.Combine(_Environment.WebRootPath, DawaaNeoConsts.FilesFolderName);
            string Dawaa24LogoPath = Path.Combine(_Environment.WebRootPath, DawaaNeoConsts.EmailingFolderName, _configuration.Value["EmailTemplateSetting:Logo"]);
            using (var ms = new MemoryStream(System.IO.File.ReadAllBytes(Path.Combine(path, "qr_code" + providerId + ".png"))))
            {
                Document.Create(contaier =>
                {
                    contaier.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));
                        page.Header().Column(x => { x.Spacing(10); x.Item().Image(Dawaa24LogoPath); });
                        page.Content().PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(10);
                            x.Item().Image(Image.FromStream(ms));
                        });
                        page.Footer().AlignCenter().Text(x => { x.Span("Page "); x.CurrentPageNumber(); });
                    });
                }).GeneratePdf(Path.Combine(path, "qr_code" + providerId + ".pdf"));
            }
            return Path.Combine(path, "qr_code" + providerId + ".pdf");
        }
    }
}
