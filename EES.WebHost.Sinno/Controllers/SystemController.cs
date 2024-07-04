using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using EES.Infrastructure.Commons;
using EES.Infrastructure.Enums;
using Microsoft.Extensions.Logging;
using EES.Infrastructure.Data;
using System.Linq;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using EES.Infrastructure.Service;
using static System.Net.Mime.MediaTypeNames;

namespace EES.WebHost.Sinno.Controllers
{
    /// <summary>
    /// 系统接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "system")]
    [Authorize]
    public class SystemController : ControllerBase
    {
        private readonly ILogger<SystemController> _logger;

        private const long IMG_MAX_LENGTH = 30 * 1024 * 1024; //图片最大为2M

        private readonly string[] imgAllowExtensions = { ".jpg", ".png", ".jpeg" };

        private const long FILE_MAX_LENGTH = 100 * 1024 * 1024; //文件最大为50M

        private readonly string[] fileAllowExtensions = { ".xlsx", ".xls", ".csv", ".pdf", ".txt", ".docx", ".doc", ".xml", ".json", ".video", ".mp4" };

        private const long TEMPLATE_MAX_LENGTH = 10 * 1024 * 1024; //最大10M

        private readonly string[] templateAllowExtensions = { ".txt", ".xml", ".json" };

        //private readonly string[] audioAllowExtensions = { ".wav", ".FLAC", ".APE", ".ALAC" };

        private static readonly ConcurrentDictionary<string, object> templateDict = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;

            BasicFolderCheck();
        }

        /// <summary>
        /// 上传图片(一次上传一个)
        /// </summary>
        /// <returns></returns>
        [Route("uploadImg")]
        [HttpPost]
        public ApiResponseBase<UploadResult> UploadImg()
        {
            var img = Request.Form.Files[0];

            _logger.LogInformation("Received image upload. Image file name：{fileName}", img.FileName);

            var imgName = Guid.NewGuid().ToString("N");

            string imgFullName;

            if (img == null)
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.文件缺失);
            }
            try
            {
                var webRootPath = GlobalConfiguration.ContentRootPath;
                var filePath = @"\Files\img\";

                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (img.Length > IMG_MAX_LENGTH)
                {
                    return ApiResponseBase<UploadResult>.Instance(BusinessError.图片不能大于30M);
                }

                var imgExtension = Path.GetExtension(img.FileName).ToLower().Trim();//获取文件格式，拓展名

                if (!imgAllowExtensions.Contains(imgExtension))
                {
                    return ApiResponseBase<UploadResult>.Instance(BusinessError.图片格式不正确);
                }

                imgFullName = imgName + imgExtension; //完整的图片名   123.jpg

                using var fs = System.IO.File.Create(webRootPath + filePath + imgFullName);

                img.CopyTo(fs);
                fs.Flush();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image upload exception. Image file name:{filename}", img.FileName);
                return ApiResponseBase<UploadResult>.Instance(BusinessError.服务器异常);
            }

            return ApiResponseBase<UploadResult>.Success(data: new UploadResult() { Name = imgFullName });
        }

        /// <summary>
        /// 上传文件(一次一个)
        /// </summary>
        /// <returns></returns>
        [Route("uploadFile")]
        [HttpPost]
        public ApiResponseBase<UploadResult> UploadFile()
        {
            var file = Request.Form.Files[0];

            _logger.LogInformation("Received file upload. File name：{fileName}", file.FileName);

            var fileName = Guid.NewGuid().ToString("N");

            string fileFullName;

            if (file == null)
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.文件缺失);
            }
            try
            {
                var webRootPath = GlobalConfiguration.ContentRootPath;
                var filePath = @"\Files\file\";

                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (file.Length > FILE_MAX_LENGTH)
                {
                    return ApiResponseBase<UploadResult>.Instance(BusinessError.文件不能大于100M);
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLower().Trim();//获取文件格式，拓展名

                if (!fileAllowExtensions.Contains(fileExtension))
                {
                    return ApiResponseBase<UploadResult>.Instance(BusinessError.文件格式不正确);
                }

                fileFullName = fileName + fileExtension; //完整的文件名

                using var fs = System.IO.File.Create(webRootPath + filePath + fileFullName);

                file.CopyTo(fs);
                fs.Flush();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File upload exception. File name:{filename}", file.FileName);
                return ApiResponseBase<UploadResult>.Instance(BusinessError.服务器异常);
            }

            return ApiResponseBase<UploadResult>.Success(data: new UploadResult() { Name = fileFullName });
        }


        /// <summary>
        /// 上传音频(一次一个)
        /// </summary>
        /// <returns></returns>
        [Route("UploadAudio")]
        [HttpGet]
        public ApiResponseBase<UploadResult> UploadAudio(string text)
        {

            var fileName = Guid.NewGuid().ToString("N");

            string audioFullName=null;
           
            try
            {
                //var webRootPath = GlobalConfiguration.ContentRootPath;
                //var filePath = @"\Files\audio\";

                //if (!Directory.Exists(webRootPath + filePath))
                //{
                //    Directory.CreateDirectory(webRootPath + filePath);
                //}

                //audioFullName = fileName + ".wav"; //完整的文件名

                //// 使用System.Speech.Synthesis中的SpeechSynthesizer来生成音频文件
                //using var synth = new SpeechSynthesizer();
                //synth.SetOutputToWaveFile(webRootPath + filePath + audioFullName, new System.Speech.AudioFormat.SpeechAudioFormatInfo(32000, System.Speech.AudioFormat.AudioBitsPerSample.Sixteen, System.Speech.AudioFormat.AudioChannel.Mono));
                //PromptBuilder builder = new();
                //builder.AppendText(text);
                //builder.AppendBreak();
                //synth.Speak(builder);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Audio upload exception. Audio name:{filename}", fileName);
                return ApiResponseBase<UploadResult>.Instance(BusinessError.服务器异常);
            }

            return ApiResponseBase<UploadResult>.Success(data: new UploadResult() { Name = audioFullName });
        }


        /// <summary>
        /// 模板信息
        /// </summary>
        public class Info
        {
            /// <summary>
            /// 模块类型 0:基础模板  1 称量工单模板  2 乳化工单模板  3 灌包工单模板 (后续模板再定义)
            /// </summary>
            public int Type { get; set; }

            /// <summary>
            /// 基础模板这个值可以为null，工单模板， 该值对应的是 工单号
            /// </summary>
            public string? Flag1 { get; set; }

            /// <summary>
            /// 基础模板这个值可以为null，工单模板， 该值对应的是 批次号
            /// </summary>
            public string? Flag2 {  get; set; }
        }

        /// <summary>
        /// 上传模板
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [Route("uploadTemplate")]
        [HttpPost]
        public async Task<ApiResponseBase<UploadResult>> UpLoadTemplateAsync([FromForm] Info info)
        {
            var file = Request.Form.Files[0];

            if (file == null)
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.文件缺失);
            }

            if (file.Length > TEMPLATE_MAX_LENGTH)
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.模板文件最大不能超过10M);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLower().Trim();//获取文件格式，拓展名

            if (!templateAllowExtensions.Contains(fileExtension))
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.文件格式不正确);
            }

            try
            {
                var webRootPath = GlobalConfiguration.ContentRootPath + @"\Files\template";

                TemplateFolderCheck(webRootPath!, info.Type, DateTime.Now.ToString("yyyyMM"), out string templateFilePath);

                GetTemplateFileFullName(info.Type, info.Flag1, info.Flag2, fileExtension, out string templateFileFullName);

                var filePath = templateFilePath + @"\" + templateFileFullName; // psm\202309\xxxx_xxx_1234.json

                await WriteFileAsync(file, webRootPath + @"\" + filePath, Encoding.UTF8);

                return ApiResponseBase<UploadResult>.Success(data: new UploadResult() { Name = filePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Template upload exception. Template name:{filename}", file.FileName);
                return ApiResponseBase<UploadResult>.Instance(BusinessError.服务器异常);
            }
        }

        /// <summary>
        /// 修改模板
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        [Route("modifyTemplate")]
        [HttpPost]
        public async Task<ApiResponseBase> ModifyTemplateAsync([FromForm] string filepath)
        {
            var file = Request.Form?.Files[0];

            if (file == null)
            {
                return ApiResponseBase.Instance(BusinessError.文件缺失);
            }

            var templateRootPath = GlobalConfiguration.ContentRootPath + @"\Files\template";


            var fileFullPath = templateRootPath + @"\" + filepath;

            if (!System.IO.File.Exists(fileFullPath))
            {
                return ApiResponseBase.Instance(BusinessError.模板文件不存在);
            }
            else
            {
                string backupFilePath = GetBackupFilePath(fileFullPath, HttpAccessor.Accessor.Id);
                System.IO.File.Copy(fileFullPath, backupFilePath, true); // 备份原始文件
            }

            var res = ApiResponseBase.Success();

            if (templateDict.ContainsKey(filepath))
            {
                return ApiResponseBase.Instance(BusinessError.模板文件正在被他人使用);
            }

            var obj = new object();

            try
            {
                var flag = templateDict.TryAdd(filepath, obj);

                if (!flag)
                {
                    return ApiResponseBase.Instance(BusinessError.其他人正在修改此模板);
                }
                await WriteFileAsync(file, fileFullPath, Encoding.UTF8);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Template ModifyFormTaskExecuteCmd exception. Template path:{filepath}", filepath);
                res = ApiResponseBase.Instance(BusinessError.服务器异常);
            }
            finally
            {
                templateDict.TryRemove(new KeyValuePair<string, object>(filepath, obj));
            }

            return res;
        }


        /// <summary>
        /// 获取服务器的系统时间
        /// </summary>
        /// <returns></returns>
        [Route("getTime")]
        [HttpGet]
        public ApiResponseBase<ServerTime> GetServerTime()
        {
            var serverTime = new ServerTime()
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
            };

            return ApiResponseBase<ServerTime>.Success(serverTime);
        }


        /// <summary>
        /// 检查存放模板的文件夹是否存在，不存在则创建
        /// </summary>
        /// <param name="templateRootPath"></param>
        /// <param name="type">模板类型</param>
        /// <param name="subfolder"></param>
        /// <param name="templateFilePath">pms\202309</param>
        private static void TemplateFolderCheck(string templateRootPath, int type, string subfolder, out string templateFilePath)
        {
            var moduleFolderPath = GetModule(type);

            templateFilePath = moduleFolderPath;

            if (!Directory.Exists(templateRootPath + @"\" + moduleFolderPath))
            {
                Directory.CreateDirectory(templateRootPath + @"\" + moduleFolderPath);
            }

            if (type == 0)
            {
                return;
            }

            templateFilePath = templateFilePath + @"\" + subfolder;

            if (!Directory.Exists(templateRootPath + @"\" + templateFilePath))
            {
                Directory.CreateDirectory(templateRootPath + @"\" + templateFilePath);
            }
        }

        private static void GetTemplateFileFullName(int type, string? flag1, string? flag2, string fileExtension, out string fileFullName)
        {
            if (type == 0)
            {
                fileFullName = Guid.NewGuid().ToString("N") + fileExtension;

                return;  //基础模板文件
            }

            fileFullName = "_";

            if (!string.IsNullOrWhiteSpace(flag1))
            {
                fileFullName += flag1;
                fileFullName += "_";
            }

            if (!string.IsNullOrWhiteSpace(flag2))
            {
                fileFullName += flag2;
                fileFullName += "_";
            }

            fileFullName += DateTime.Now.ToUnixTimestamp();

            fileFullName += fileExtension;

            fileFullName = fileFullName.TrimStart('_');
        }

        private static string GetModule(int type)
        {
            return type switch
            {
                0 => "base",
                1 => "weighing",
                2 => "bulk",
                3 => "fp",
                _ => "unknown"
            };
        }


        private static void BasicFolderCheck()
        {
            var webRootPath = GlobalConfiguration.ContentRootPath;

            if (!Directory.Exists(webRootPath + @"\Files")) //基础文件夹
            {
                Directory.CreateDirectory(webRootPath + @"\Files");
            }

            if (!Directory.Exists(webRootPath + @"\Files\img"))
            {
                Directory.CreateDirectory(webRootPath + @"\Files\img"); //存放图片的文件夹
            }

            if (!Directory.Exists(webRootPath + @"\Files\file"))
            {
                Directory.CreateDirectory(webRootPath + @"\Files\file"); //存放文件的文件夹
            }

            if (!Directory.Exists(webRootPath + @"\Files\template"))
            {
                Directory.CreateDirectory(webRootPath + @"\Files\template"); //存放模板的文件夹
            }

            if (!Directory.Exists(webRootPath + @"\Files\audio"))
            {
                Directory.CreateDirectory(webRootPath + @"\Files\audio"); //存放音频的文件夹
            }

            if (!Directory.Exists(webRootPath + @"\Files\excel"))
            {
                Directory.CreateDirectory(webRootPath + @"\Files\excel"); //存放excel模板的文件夹
            }
        }


        private static string GetBackupFilePath(string filePath, long userId)
        {
            string directory = Path.GetDirectoryName(filePath)!;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string backupFileName = $"bak_{userId}_{fileNameWithoutExtension}_{timeStamp}{fileExtension}";
            return Path.Combine(directory, backupFileName);
        }

        /// <summary>
        /// 将数据写入文件(文件不存在则会自动创建)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static async Task WriteFileAsync(IFormFile file, string fileFullPath, Encoding? encoding)
        {
            encoding ??= Encoding.UTF8;
            // 创建一个文件锁定对象
            FileStream? fileLock = null;
            try
            {
                fileLock = System.IO.File.Open(fileFullPath, FileMode.Create, FileAccess.Write, FileShare.None);

                // 使用指定编码创建StreamWriter
                using StreamWriter writer = new(fileLock, encoding);
                // 使用IFormFile的输入流创建StreamReader
                using StreamReader reader = new(file.OpenReadStream(), encoding);
                // 读取并写入文本数据
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    writer.WriteLine(line);
                }
            }
            finally
            {
                // 在finally块中释放文件锁
                fileLock?.Dispose();
            }
        }


        /// <summary>
        /// 上传文档(一次一个)
        /// </summary>
        /// <returns></returns>
        [Route("UploadDocument")]
        [HttpPost]
        public ApiResponseBase<UploadResult> UploadDocument()
        {
            var file = Request.Form.Files[0];

            var fileName = Guid.NewGuid().ToString("N");

            string fileFullName;

            if (file == null)
            {
                return ApiResponseBase<UploadResult>.Instance(BusinessError.文件缺失);
            }
            try
            {
                var webRootPath = GlobalConfiguration.ContentRootPath;
                var filePath = @"\Files\document\";

                if (!Directory.Exists(webRootPath + filePath))
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                if (file.Length > FILE_MAX_LENGTH)
                {
                    return ApiResponseBase<UploadResult>.Instance(BusinessError.文件不能大于100M);
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLower().Trim();

                fileFullName = fileName + fileExtension;

                using var fs = System.IO.File.Create(webRootPath + filePath + fileFullName);

                file.CopyTo(fs);
                fs.Flush();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File upload exception. File name:{filename}", file.FileName);
                return ApiResponseBase<UploadResult>.Instance(BusinessError.服务器异常);
            }

            return ApiResponseBase<UploadResult>.Success(data: new UploadResult() { Name = fileFullName });
        }
    }
}
