using DevEvent.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DevEvent.Web.Controllers
{
    /// <summary>
    /// 웹 에디터용 이미지 업로드 컨트롤러 
    /// 웹 에디터 편집중에 이미지를 추가하면 이미지가 업로드 되고 데이터를 되돌려준다. 
    /// </summary>
    [Authorize]
    public class ImageUploadController : Controller
    {
        private IStorageService StorageService;
        private string blobBaseUrl;

        public ImageUploadController(IStorageService storageService)
        {
            this.StorageService = storageService;

            // /storage/image 컨테이너 없으면 만들기 
            this.StorageService.CreateContainerAsync("images", true);

            blobBaseUrl = ConfigurationManager.AppSettings["AzureStorageBaseUrl"].ToString();
        }

        [HttpPost]
        public async Task<ActionResult> Image()
        {
            try
            {
                var stream = this.Request.InputStream;

                if (stream != null && stream.Length > 0)
                {
                    var filename = Server.UrlDecode(this.Request.Headers["file-name"].ToString());
                    var ext = filename.Substring(filename.LastIndexOf('.'));
                    var guid = Guid.NewGuid().ToString().ToLower();

                    await StorageService.UploadBlobAsync(stream, guid + ext, "images");
                    var url = blobBaseUrl + "/images/" + guid + ext;

                    // 저장 
                    await StorageService.UploadBlobAsync(stream, guid + ext, "images");
                    var result = "&bNewLine=true&sFileName=" + filename + "&sFileURL=" + Server.UrlEncode(url);

                    return Content(result);
                }
                else
                {
                    throw new ArgumentNullException("파일크기가 0이거나 파일이 전송되지 않았습니다.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ImageFallback(HttpPostedFileBase Filedata, string callback)
        {
            try
            {
                if (Filedata != null && Filedata.ContentLength > 0)
                {
                    var filename = Filedata.FileName;
                    var ext = filename.Substring(filename.LastIndexOf('.'));
                    var guid = Guid.NewGuid().ToString().ToLower();

                    // 저장 
                    var url = blobBaseUrl + "/images/" + guid + ext;
                    StorageService.UploadBlobAsync(Filedata.InputStream, guid + ext, "images");

                    return Redirect("/Content/plugins/smarteditor/sample/photo_uploader/callback.html?callback_func=" 
                        + callback + "&bNewLine=true&sFileURL=" 
                        + Server.UrlEncode(url) + "&sFileName=" + filename);
                }
                else
                {
                    throw new ArgumentNullException("파일크기가 0이거나 파일이 전송되지 않았습니다.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}