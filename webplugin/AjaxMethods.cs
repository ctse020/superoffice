using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using SuperOffice.Diagnostics;
using SuperOffice.Data;
using SuperOffice.Globalization;

using SuperOffice.CRM.Services;
using SuperOffice.CRM.Web.Data;

using SuperOffice.DCF.Web;
using SuperOffice.DCF.Web.Factory;

namespace Ctse.SO.AjaxTest
{
    [SoWebObject("AjaxMethods")]
    public class AjaxMethods : IWebObject
    {
        public Response GenerateEmailWithAttachments(string mailTemplateFilename, int contactId, int personId, int projectId, int saleId, string attachmentFileNames)
        {
            try
            {
                // validate template file if it is a mime file (.eml, .som or .somail)
                var allowedExt = new string[] { ".eml", ".som", ".somail" };
                var ext = System.IO.Path.GetExtension(mailTemplateFilename).ToLower();
                if (allowedExt.Contains(ext))
                    throw new Exception(mailTemplateFilename + " is propably not a valid mime file");

                string cultName = ResourceManager.GetCulture(); // "nl-NL" or "en-US" or ...

                using (new CultureSettingHelper(new System.Globalization.CultureInfo(cultName, false)))
                {
                    using (DocumentAgent docAgent = new DocumentAgent())
                    {         
                        // get content of mail template
                        var templateStream = docAgent.GetTemplateStream(mailTemplateFilename, true, cultName);
                        if (templateStream == null)
                            throw new Exception("Template " + mailTemplateFilename + " not found.");

                        string emlContent = string.Empty;
                        using (StreamReader streamReader = new StreamReader(templateStream, Encoding.Default, true))
                            emlContent = streamReader.ReadToEnd();

                        if (!string.IsNullOrWhiteSpace(emlContent))
                        {
                            // parse mail template with variables
                            //var tvh = new TemplateVariablesHandler(contactId, personId, 0, 0, saleId, 0, projectId, 0, 0, "");
                            //emlContent = tvh.SubstituteTemplateVariables(emlContent, GeneratorEncoding.Mime);
                            emlContent = docAgent.SubstituteTemplateVariables(emlContent, GeneratorEncoding.Mime, contactId, personId, 0, 0, saleId, 0, projectId, cultName);

                            // get attachment(s) to add (from template folder)
                            FileNameAndContent[] attachments = null;
                            if (!string.IsNullOrWhiteSpace(attachmentFileNames))
                            {
                                attachments = attachmentFileNames.Split(',').Select(attachmentFilename =>
                                {

                                    var attachmentStream = docAgent.GetTemplateStream(attachmentFilename, true, cultName);
                                    if (attachmentStream == null)
                                        throw new Exception("Attachment " + attachmentFilename + " not found.");

                                    return new FileNameAndContent
                                    {
                                        FileName = attachmentFilename,
                                        Content = attachmentStream.ToByteArray()
                                    };

                                }).ToArray();
                            }

                            // generate valid eml-filename (otherwise the client will not recognize the file type if it is .som or .somail)
                            var emlFilename = System.IO.Path.ChangeExtension(Utils.GetValidFileName(mailTemplateFilename), ".eml");

                            // generate eml-file with attachment(s)
                            var emlFile = Utils.GenerateEmlFileWithAttachment(
                                new FileNameAndContent { FileName = emlFilename, Content = System.Text.Encoding.UTF8.GetBytes(emlContent) }, 
                                attachments
                            );

                            // return the response to the client
                            return new Response { FileName = emlFile.FileName, Content = System.Convert.ToBase64String(emlFile.Content) };

                        }
                        else
                        {
                            throw new Exception("Mail template content is empty");
                        }
                        
                    }
                }
                
            }
            catch (Exception ex)
            {
                var ex1 = ex;
                string err = ex1.Message;
                while (ex1.InnerException != null)
                {
                    ex1 = ex1.InnerException;
                    err = err + ";" + ex1.Message;
                }
                SoLogger.Logger.LogException(EventLogEntryType.Warning, new Exception(err));
                return new Response { Message = ApplicationUtility.FormatExceptionMessage(err) };
            }

        }
    }

    public class Response
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        
    }

    public class FileNameAndContent
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }

    public class Utils
    {
        public static string GetValidFileName(string fileName, string replChar = null)
        {
            if (replChar == null)
                replChar = string.Empty;
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), replChar));
        }

        public static FileNameAndContent GenerateEmlFileWithAttachment(FileNameAndContent emlFile, FileNameAndContent[] attachments)
        {
            // load eml content
            LumiSoft.Net.Mime.Mime m = LumiSoft.Net.Mime.Mime.Parse(emlFile.Content);

            // change contenttype
            var mainEntity = m.MainEntity;
            mainEntity.ContentType = LumiSoft.Net.Mime.MediaType_enum.Multipart_mixed;

            // add attachment(s)
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    MemoryStream stream = new MemoryStream(attachment.Content);
                    LumiSoft.Net.Mime.MimeEntity attachmentEntity = mainEntity.ChildEntities.Add();
                    attachmentEntity.ContentType = LumiSoft.Net.Mime.MediaType_enum.Application_octet_stream;
                    attachmentEntity.ContentDisposition = LumiSoft.Net.Mime.ContentDisposition_enum.Attachment;
                    attachmentEntity.ContentTransferEncoding = LumiSoft.Net.Mime.ContentTransferEncoding_enum.Base64;
                    attachmentEntity.ContentDisposition_FileName = attachment.FileName;
                    attachmentEntity.DataFromStream(stream);
                }
            }

            // set defaults
            mainEntity.Date = DateTime.Now;
            if (!mainEntity.Header.Contains("X-Unsent"))
            {
                mainEntity.Header.Add(new LumiSoft.Net.Mime.HeaderField("X-Unsent", "1"));
            }
            else
            {
                var unsent = mainEntity.Header.GetFirst("X-Unsent");
                unsent.Value = "1";
            }

            return new FileNameAndContent { FileName = emlFile.FileName, Content = m.ToByteData() };
        }

    }


}
