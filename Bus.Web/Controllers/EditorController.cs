using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Bus.Web.Controllers
{
    public class EditorController : Controller
    {
        #region 图片上传
        private string UploadImageDir = "";

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        public JsonResult UpLoadFile(string fileUrl)
        {
            this.UploadImageDir = "/Upload/" + DateTime.Now.ToString("yyyyMM") + "/";
            Hashtable ht = doUploadImagePost();
            return Json(ht, "text/html; charset=UTF-8");
        }
        public JsonResult UpLoadImagesAndThumb(string fileUrl, int PhotoID = 0, int ID = 0)
        {
            this.UploadImageDir = "/Upload/" + DateTime.Now.ToString("yyyyMM") + "/";
            Hashtable ht = doUploadImagePost(true, PhotoID, ID);
            return Json(ht, "text/html; charset=UTF-8");
        }
        private Hashtable doUploadImagePost(bool isthumb = false, int PhotoID = 0, int ID = 0)
        {
            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            Hashtable hash = new Hashtable();
            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
            //最大文件大小(KB)
            int maxSize = 1024 * 5;

            if (imgFile == null)
            {
                hash = showError("请选择文件。");
                return hash;
            }

            String saveUrl = Server.MapPath(this.UploadImageDir);
            if (!Directory.Exists(saveUrl))
            {
                Directory.CreateDirectory(saveUrl);
                //hash = showError("上传目录不存在。");
                //return hash;
            }

            String dirName = Request.QueryString["dir"];
            if (!extTable.ContainsKey(dirName))
            {
                hash = showError("目录名不正确。");
                return hash;
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = showError("上传文件扩展名是不允许的扩展名。\n只允许真实的" + ((String)extTable[dirName]) + "格式。");
                return hash;
            }
            //if (!Directory.Exists(saveUrl))
            //{
            //    Directory.CreateDirectory(saveUrl);
            //}

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;
            String FileSaveDir = saveUrl + newFileName;//最终文件名

            if (!String.IsNullOrEmpty(dirName) && dirName == "image")
            {
                if (imgFile.InputStream == null || imgFile.InputStream.Length > (maxSize * 1024))
                {
                    hash = showError("上传文件大小超过限制。");
                    return hash;
                }
                try
                {
                    string[] fileType = {
                        "7173",    //gif
                        "255216",  //jpg
                        "13780",   //png
                        "6677"     //bmp
                    };
                    string fileclass = "";//返回文件类型
                    if (!Bus.Core.Common.IsAllowedFileExtension(imgFile.InputStream, fileType, out fileclass, false))//判断上传文件类型，不匹配类型则删除
                    {
                        hash = showError("上传文件扩展名是不允许的扩展名。\n只允许真实的" + ((String)extTable[dirName]) + "格式。");
                        return hash;
                    }
                    else
                    {
                        imgFile.SaveAs(FileSaveDir);
                        if (isthumb)
                        {
                            CreateThumb(saveUrl, newFileName);

                        }
                        //创建水印图片并保存
                        //ToWatherMarkImage(fileExt, imgFile.InputStream, FileSaveDir);
                        string url = this.UploadImageDir + newFileName;
                        if (isthumb)
                        {
                            url = this.UploadImageDir + "Thumb_" + newFileName;
                        }
                        hash["error"] = 0;
                        hash["url"] = url;

                        return hash;
                    }
                }
                catch
                {
                    //如果出现错误,可能上传目录无写入权限,需检查
                    hash = showError("上传文件保存失败,请重试!\\n如多次重试无效,请与本网管理员联系!");
                }
            }
            else
            {
                imgFile.SaveAs(FileSaveDir);
                if (isthumb)
                {
                    CreateThumb(saveUrl, newFileName);


                }
                string url = this.UploadImageDir + newFileName;
                if (isthumb)
                {
                    url = this.UploadImageDir + "Thumb_" + newFileName;
                }
                hash["error"] = 0;
                hash["url"] = url;
                return hash;
            }

            imgFile = null;
            return hash;
        }


        /// <summary>
        /// 生成略缩图
        /// </summary>
        /// <param name="FilePath"></param>
        private void CreateThumb(string saveUrl, string newFileName)
        {
            string FilePath = saveUrl + newFileName;
            //创建图片Drawing.Image
            var DrawingImage = System.Drawing.Image.FromFile(FilePath);
            //获取图片Size
            var ImagesSize = GetNewSize(230, DrawingImage.Height, DrawingImage.Width, DrawingImage.Height);
            var ImageBit = new System.Drawing.Bitmap(DrawingImage);
            var newbimt = TheCMS.Common.ThumbImage.Thumbnail(ImageBit, ImagesSize, TheCMS.Common.ThumbImage.ThumbMode.Full, System.Drawing.ContentAlignment.MiddleCenter);

            TheCMS.Common.ThumbImage.SaveIamge(newbimt, 100, saveUrl + "Thumb_" + newFileName);
            ImageBit.Dispose();
            DrawingImage.Dispose();
        }
        /// <summary>
        /// 获取图片Size
        /// </summary>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <param name="imageOriginalWidth"></param>
        /// <param name="imageOriginalHeight"></param>
        /// <returns></returns>
        private static System.Drawing.Size GetNewSize(int maxWidth, int maxHeight, int imageOriginalWidth, int imageOriginalHeight)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(imageOriginalWidth);
            double sh = Convert.ToDouble(imageOriginalHeight);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);
            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new System.Drawing.Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }
        /// <summary>
        /// 创建缩略图并保存
        /// </summary>
        private void ToWatherMarkImage(string sExtension, Stream PicStream, string NormalPicPath)
        {
            //创建水印图片并保存
            if (!string.Equals(sExtension, ".gif", StringComparison.OrdinalIgnoreCase))//GIF图片不加水印，不压缩
            {
                string ThumbnailImg = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ThumbnailImg"]);
                using (Image image = Image.FromFile(ThumbnailImg))
                {
                    //using (MemoryStream compressImageStream = (new Tools.ImageHandleService().CompressImage(str, 90,sExtension)) as MemoryStream)//压缩
                    //{
                    //using (MemoryStream waterMarkImage = (new Rococo.Core.ImageHandleService().CreateImageWaterMark("", PicStream, image)) as MemoryStream)//加水印
                    //{
                    //    byte[] content = waterMarkImage.ToArray();
                    //    using (FileStream Fstream = new FileStream(NormalPicPath, FileMode.Create))
                    //    {
                    //        Fstream.Write(content, 0, content.Length);
                    //    }

                    //}
                    //}
                }
            }
            //释放资源
            PicStream.Close();
            PicStream.Dispose();
        }

        private Hashtable showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            return hash;
        }
        #endregion

        #region 图片浏览
        public JsonResult ViewImgs(string fileUrl)
        {
            this.UploadImageDir = "/Upload/";

            String aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);

            //根目录路径，相对路径
            String rootPath = UploadImageDir;
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = "http://" + System.Web.HttpContext.Current.Request.Url.Authority.ToString() + UploadImageDir;
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            //根据path参数，设置各路径和URL
            String path = Request.QueryString["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = Server.MapPath(rootPath);
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = Server.MapPath(rootPath) + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.QueryString["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                Response.Write("Access is not allowed.");
                Response.End();
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                Response.Write("Parameter is not valid.");
                Response.End();
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                Response.Write("Directory does not exist.");
                Response.End();
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            DirectoryInfo dir = null;
            for (int i = 0; i < dirList.Length; i++)
            {
                dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            FileInfo file = null;
            for (int i = 0; i < fileList.Length; i++)
            {
                file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }
        #endregion

        #region 图片删除
        [HttpPost, AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Delete(string imgUrl)
        {
            bool result = false;
            try
            {
                System.IO.File.Delete(Server.MapPath(imgUrl));
                result = true;
            }
            catch
            {
            }
            return Content(result ? "1" : "0");
        }
        #endregion

    }
}
