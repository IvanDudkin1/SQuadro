using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQuadro.Models;
using System.IO;
using System.IO.Compression;

namespace SQuadro.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        public DocumentsController(IListTemplate IListTemplate, IUsersHelper IUsersHelper)
        {
            this.IListTemplate = IListTemplate;
            this.IUsersHelper = IUsersHelper;
        }

        private IListTemplate IListTemplate;
        private IUsersHelper IUsersHelper;
        private EntityContext context = EntityContext.Current;

        public ActionResult Index()
        {
            ViewBag.ActiveInterfaceGroup = MainMenu.DocumentsGroup;
            ViewBag.ActiveInterfaceItem = MainMenu.Documents;

            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            return PartialView("ListTemplateView", IListTemplate);
        }

        public ActionResult IndexPartial()
        {
            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.InitParams = Tuple.Create<object, object, object>(true, null, null);  
            IListTemplate.Initialize();

            return PartialView("ListTemplateViewPartial", IListTemplate);
        }

        [HttpPost]
        public ActionResult DataSourceCallback(DataTablesParam param)
        {
            IListTemplate.ParentID = IUsersHelper.CurrentUser.OrganizationID;
            IListTemplate.Initialize();
            int totalRecords, filteredRecords;
            var result = IListTemplate.GetDataSource(param, Request, out totalRecords, out filteredRecords);

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = filteredRecords,
                aaData = result
            });
        }

        [HttpPost]
        public ActionResult GetDocumentDetails(Guid? id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = DocumentsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
                content = this.PartialViewToString("Details", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult SetDocumentDetails(DocumentModel model)
        {
            bool result = false;

            try
            {
                if (ModelState.IsValid)
                {
                    DocumentsService.SetDocument(model, IUsersHelper.CurrentUser, context);
                    context.SaveChanges();
                    result = true;
                }
                else
                {
                    ModelState.AddModelError("", "Please correct all errors.");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.GetMessage());
            }
            string content = String.Empty;
            if (!result) content = this.PartialViewToString("Details", model);
            return Json(new { Result = result, Content = content });
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            bool result = false;
            string description = String.Empty;
            try
            {
                DocumentsService.DeleteDocument(id, IUsersHelper.CurrentUser, context);
                context.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description });
        }

        [HttpPost]
        public ActionResult DeleteDocuments(Guid[] selectedDocuments)
        {
            bool result = false;
            string description = String.Empty;
            try
            {
                DocumentsService.DeleteDocuments(selectedDocuments, IUsersHelper.CurrentUser, context);
                context.SaveChanges();
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description });
        }

        public ActionResult DownloadAsZip(string selectedDocuments)
        {
            Guid tmpID = Guid.Empty; ;

            using (var outputStream = new PositionWrapperStream(Response.OutputStream))
            {
                using (ZipArchive archive = new ZipArchive(outputStream, ZipArchiveMode.Create, false))
                {
                    foreach (Guid id in selectedDocuments.Split(',').Where(s => Guid.TryParse(s, out tmpID)).Select(s => tmpID))
                    {
                        Document document = context.Documents.SingleOrDefault(d => d.ID == id);
                        if (document == null)
                            continue;

                        ZipArchiveEntry entry = archive.CreateEntry(document.FileName);

                        using (BinaryWriter writer = new BinaryWriter(entry.Open()))
                        {
                            writer.Write(document.File);
                        }
                    }
                }
            
                Response.AppendHeader("content-disposition", "attachment; filename=documents.zip");
                Response.ContentType = "application/x-compressed";
            }
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult GetDetails(Guid id)
        {
            bool result = false;
            string description = String.Empty;
            string content = String.Empty;

            try
            {
                var model = DocumentsService.GetViewModel(id, IUsersHelper.CurrentUser.OrganizationID, context);
                content = this.PartialViewToString("DetailsPartial", model);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }
            return Json(new { Result = result, Description = description, Content = content });
        }

        [HttpPost]
        public ActionResult GetDocumentsList(string term)
        {
            return Json(ListsHelper.Documents(IUsersHelper.CurrentUser.OrganizationID).Where(c => String.IsNullOrEmpty(term) || c.Name.ToLower().Contains(term.ToLower())).Select(
                c => new { id = c.ID, text = c.Name }));
        }

        [HttpPost]
        public ActionResult GetSelectedDocuments(string selection)
        {
            Guid tmpID = Guid.Empty;

            var selectionArr = selection.Split(',').Where(item => Guid.TryParse(item, out tmpID)).Select(item => tmpID);

            var result = EntityContext.Current.Documents.Where(d => selectionArr.Contains(d.ID)).OrderBy(d => d.Name).Select(
                d => new { id = d.ID, text = d.Name });

            return Json(result);
        }

        public ActionResult View(Guid id)
        {
            bool result = false;
            string description = String.Empty;
            Document document = null;
            try
            {
                document = DocumentsService.GetDocument(id, context);
                result = true;
            }
            catch (Exception e)
            {
                description = e.GetMessage();
            }

            if (result)
            {
                string contentType = GetContentType(Path.GetExtension(document.FileName));
                
                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = HttpUtility.UrlPathEncode(document.FileName),
                    Inline = true
                };
                Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
                return new FileStreamResult(new MemoryStream(document.File), contentType);
            }
            else
                return new ContentResult() { Content = description };
        }

        [AllowAnonymous]
        public ActionResult Download(Guid id, long expiration, string token)
        {
            Document document = DocumentsService.GetDocument(id, context);

            DateTime expirationDate = new DateTime(expiration);

            if (expirationDate.AddDays(1) <= DateTime.Now)
                throw new HttpException(403, "Access denied!");
            
            if (token != SecurityHelper.GetDateTimeHash(expirationDate))
                throw new HttpException(403, "Access denied!");

            string contentType = GetContentType(Path.GetExtension(document.FileName));

            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = HttpUtility.UrlPathEncode(document.FileName),
                Inline = true
            };
            Response.AppendHeader("Content-Disposition", contentDisposition.ToString());
            return new FileStreamResult(new MemoryStream(document.File), contentType);
        }

        private string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return System.Net.Mime.MediaTypeNames.Application.Pdf;
                case ".doc":
                case ".docx":
                case ".rtf":
                    return System.Net.Mime.MediaTypeNames.Application.Rtf;
                case ".txt":
                    return System.Net.Mime.MediaTypeNames.Text.Plain;
                case ".htm":
                case ".html":
                    return System.Net.Mime.MediaTypeNames.Text.Html;
                case ".xml":
                    return System.Net.Mime.MediaTypeNames.Text.Xml;
                case ".jpg":
                case ".jpeg":
                    return System.Net.Mime.MediaTypeNames.Image.Jpeg;
                case ".gif":
                    return System.Net.Mime.MediaTypeNames.Image.Gif;
                case ".tiff":
                    return System.Net.Mime.MediaTypeNames.Image.Tiff;
                default:
                    return System.Net.Mime.MediaTypeNames.Application.Octet;
            }
        }

        class PositionWrapperStream : Stream
        {
            private readonly Stream wrapped;

            private int pos = 0;

            public PositionWrapperStream(Stream wrapped)
            {
                this.wrapped = wrapped;
            }

            public override bool CanSeek { get { return false; } }

            public override bool CanWrite { get { return true; } }

            public override long Position
            {
                get { return pos; }
                set { throw new NotSupportedException(); }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                pos += count;
                wrapped.Write(buffer, offset, count);
            }

            public override void Flush()
            {
                wrapped.Flush();
            }

            protected override void Dispose(bool disposing)
            {
                wrapped.Dispose();
                base.Dispose(disposing);
            }

            // all the other required methods can throw NotSupportedException
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long length)
            {
                throw new NotImplementedException();
            }

            public override long Length { get { throw new NotImplementedException(); } }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead
            {
                get { throw new NotImplementedException(); }
            }
        }

        public string Convert()
        {
            foreach (Document document in IUsersHelper.CurrentUser.Organization.Documents.ToList())
            {
                string path = DocumentsService.GetFullPath(IUsersHelper.CurrentUser.OrganizationID, document.FileName);
                string dir = Path.GetDirectoryName(path);
                if (document.File == null)
                {
                    if (System.IO.File.Exists(path))
                    {
                        document.File = System.IO.File.ReadAllBytes(path);
                        System.IO.File.Delete(path);
                    }
                    else
                        document.Delete(context);
                    context.SaveChanges();
                }
                else 
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    } 
                }
                if (Directory.Exists(dir) && !Directory.GetFiles(dir).Any())
                    Directory.Delete(dir);
            }

            return "Documents have been converted.";
        }
    }
}
