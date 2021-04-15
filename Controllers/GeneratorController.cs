using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PdfGenerator.Controllers
{
    public class Fontstyling : Font
    {
        public Base64FormattingOptions formattingOptions  { get; set; }
       
        
        public string Underlined { get; set; }
    }
    public class PdfModel
    {
        public string PdfTitle { get; set; }
        public string PdfContent { get; set; }
    }
    [ApiController]
    [Route("api/[controller]")]
    public  class Generator: ControllerBase
    {
        public ILogger<Generator> Logger { get; }

        public Generator(ILogger<Generator> logger)
        {
            Logger = logger;
        }
        [HttpGet]
        [Route("Printname")]
        public  IActionResult PrintName()
        {
            
            return Ok("hello world");
        }
        [HttpPost]
        [Route("GeneratePdf")]
        public IActionResult GeneratePdf(PdfModel model)
        {
            Document document = new Document(PageSize.A4, 10, 10, 10, 10);

            string filepath = "C:/Users/HI/Documents/";
            var save_path = Path.Combine(filepath);
            Guid guid = Guid.NewGuid();
            PdfModel pdfModel = new PdfModel
            {
                PdfContent = model.PdfContent,
                PdfTitle = model.PdfTitle
            };
            try
            {

               
                PdfWriter.GetInstance(document, new FileStream(save_path+"/"+guid+"A.pdf", FileMode.Create));
                document.Open();
                string font1 = FontFactory.HELVETICA;
                Font f1 = new Font();
                f1.Color = BaseColor.BLUE;
                f1.SetFamily(font1);

                Chunk title = new Chunk(model.PdfTitle, FontFactory.GetFont("dax-black"));
                Paragraph pdftitle = new Paragraph(title);
                title.SetUnderline(0.5f, -1.5f);
          
                document.Add(pdftitle);



                Fontstyling fst1 = new Fontstyling();
                string contentfontfamily = FontFactory.HELVETICA_BOLD;
                fst1.SetFamily(contentfontfamily);
                fst1.formattingOptions = Base64FormattingOptions.InsertLineBreaks;


                Chunk begining = new Chunk(model.PdfContent, FontFactory.GetFont("georgia", 10f));
                Phrase pdfcontent = new Phrase(begining);
                Paragraph pdfcontents = new Paragraph(10f);
                pdfcontents.Add(begining);
                pdfcontents.Add(pdfcontent);

                pdfcontents.SpacingAfter = 10;
                    


                // Paragraph pdfcontent = new Paragraph(3, pdfModel.PdfContent,fst1);
                //pdfcontent.KeepTogether = false;

                //pdfcontent.IndentationLeft = 10;
                //pdfcontent.SpacingBefore = 50;
                //pdfcontent.SpacingAfter = 50;
                document.Add(pdfcontent);
                //document.Close();

                    return Ok("Pdf has been successfully created");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest();
            }
            finally
            {
                document.Close();
            }


        }

    }
}
