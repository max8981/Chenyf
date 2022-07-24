using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainApp.Controllers
{
    [ApiController]
    [Route("/api/file")]
    public class FilesController : ControllerBase
    {
        private IWebHostEnvironment env;
        public FilesController(IWebHostEnvironment env)
        {
            this.env = env;
        }
        [HttpPost, Route("upload")]
        public ResultModel UploadFile([FromForm] IFormCollection formCollection)
        {
            ResultModel result = new ResultModel();
            result.Message = "success";
            result.Code = 0;
            result.Url = "/api/file/download";
            try
            {
                string uploadPath = System.IO.Path.Combine(env.ContentRootPath, "upload");
                if (!System.IO.Directory.Exists(uploadPath))
                {
                    System.IO.Directory.CreateDirectory(uploadPath);
                }
                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    string filePath = System.IO.Path.Combine(uploadPath, file.FileName);
                    using FileStream fs = new(filePath, FileMode.OpenOrCreate);
                    file.CopyTo(fs);
                    break;
                }
            }
            catch (Exception ex)
            {
                result.Code = 100;
                result.Message = $"文件上传失败：{ex.Message}!";
            }
            return result;
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        [HttpGet, Route("download")]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        public FileResult Download()
        {

            try
            {

                string filePath = "";
                Stream stream = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
                FileStreamResult actionresult = new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"));
                actionresult.FileDownloadName = "";
                return actionresult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public class ResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}

