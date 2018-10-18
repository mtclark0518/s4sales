// using System;
// using System.Diagnostics;
// using System.IO;
// using System.Linq;
// using Oracle.DataAccess.Client;

// namespace S4Sales
// {
//     public class FolderManager
//     {
//         #region Private Backing Stores

//         private string _sourcePath;
//         private string _sourcePathSeparator;
//         private string _targetPath;
//         private string _targetPathSeparator;

//         #endregion

//         #region Public Properties

//         /// <summary>
//         /// The database connection string to be used for updating GEOCODE_OUTPUT.IMG_EXT_TX
//         /// when an image is successfully copied or moved to the target location.
//         /// </summary>
//         public string ConnectionString { get; set; }

//         /// <summary>
//         /// File specification for files to match in SourceRoot, "*.pdf" for example.
//         /// </summary>
//         public string FileSpec { get; set; }

//         /// <summary>
//         /// The source of the image files, "S4" or "HSMV" for example.
//         /// </summary>
//         public string ImageSource { get; set; }

//         /// <summary>
//         /// The path to the source root directory that contains files and/or
//         /// sub-directories to be copied to a directory structure under TargetRoot.
//         /// </summary>
//         public string SourcePath
//         {
//             get { return _sourcePath; }
//             set
//             {
//                 if (value.Contains("/") && !value.Contains(@"\"))
//                 {
//                     _sourcePathSeparator = "/";
//                     _sourcePath = RemoveTrailingSlashes(value);
//                 }
//                 else if (value.Contains(@"\") && !value.Contains("/"))
//                 {
//                     _sourcePathSeparator = @"\";
//                     _sourcePath = RemoveTrailingSlashes(value);
//                 }
//                 else if (value.Contains(@"\") && value.Contains("/"))
//                 {
//                     _sourcePathSeparator = @"\";
//                     _sourcePath = RemoveTrailingSlashes(value.Replace("/", @"\"));
//                 }
//                 else
//                 {
//                     _sourcePathSeparator = @"\";
//                     _sourcePath = RemoveTrailingSlashes(value);
//                 }
//             }
//         }

//         /// <summary>
//         /// The separator between the nodes in the SourceRoot. Valid values are a forward slash "/"
//         /// for UNIX-style filesystems or a backward slash "\" for Windows-style filesystems.
//         /// The default is a backward slash "\".  Any other separator will be ignored.
//         /// </summary>
//         public string SourcePathSeparator
//         {
//             get { return _sourcePathSeparator; }
//             set
//             {
//                 if (!value.Equals("/") && !value.Equals(@"\")) return;
//                 _sourcePath = _sourcePath.Replace(_sourcePathSeparator, value);
//                 _sourcePathSeparator = value;
//             }
//         }

//         /// <summary>
//         /// The path to the target root directory that contains or will contain
//         /// the directory structure written to by this FolderManager instance.
//         /// </summary>
//         public string TargetPath
//         {
//             get { return _targetPath; }
//             set
//             {
//                 if (value.Contains("/") && !value.Contains(@"\"))
//                 {
//                     _targetPathSeparator = "/";
//                     _targetPath = RemoveTrailingSlashes(value);
//                 }
//                 else if (value.Contains(@"\") && !value.Contains("/"))
//                 {
//                     _targetPathSeparator = @"\";
//                     _targetPath = RemoveTrailingSlashes(value);
//                 }
//                 else if (value.Contains(@"\") && value.Contains("/"))
//                 {
//                     _targetPathSeparator = @"\";
//                     _targetPath = RemoveTrailingSlashes(value.Replace("/", @"\"));
//                 }
//                 else
//                 {
//                     _targetPathSeparator = @"\";
//                     _targetPath = RemoveTrailingSlashes(value);
//                 }
//             }
//         }

//         /// <summary>
//         /// The separator between the nodes in the target path. Valid values are a forward slash "/"
//         /// for UNIX-style filesystems or a backward slash "\" for Windows-style filesystems.
//         /// The default is a backward slash "\".  Any other separator will be ignored.
//         /// </summary>
//         public string TargetPathSeparator
//         {
//             get { return _targetPathSeparator; }
//             set
//             {
//                 if (!value.Equals("/") && !value.Equals(@"\")) return;
//                 _targetPath = _targetPath.Replace(_targetPathSeparator, value);
//                 _targetPathSeparator = value;
//             }
//         }

//         #endregion

//         /// <summary>
//         /// Constructor.
//         /// </summary>
//         /// <param name="imageSource">The source of the image files (S4 or HSMV).</param>
//         /// <param name="sourcePath">The path to the root directory that contains the image files to copy.</param>
//         /// <param name="fileSpec">The file spec to match, "*.tif" for example.</param>
//         /// <param name="targetPath">The path to the root directory that contains or will contain
//         /// the directory structure managed by this FolderManager instance.</param>
//         /// <param name="connectionString">The data warehouse database connection string.</param>
//         public FolderManager(string imageSource, string sourcePath, string fileSpec, string targetPath, string connectionString)
//         {
//             ImageSource = imageSource;
//             SourcePath = RemoveTrailingSlashes(sourcePath);
//             FileSpec = fileSpec;
//             TargetPath = RemoveTrailingSlashes(targetPath);
//             ConnectionString = connectionString;
//         }

//         /// <summary>
//         /// Constructor.
//         /// </summary>
//         /// <param name="targetRoot">The path to the root directory that contains or will contain
//         /// the directory structure managed by this FolderManager instance.</param>
//         /// <param name="connectionString">The data warehouse database connection string.</param>
//         public FolderManager(string targetRoot, string connectionString)
//         {
//             TargetPath = RemoveTrailingSlashes(targetRoot);
//             ConnectionString = connectionString;
//         }

//         #region Public Instance Methods

//         /// <summary>
//         /// Copies all files in SourceRoot that match SourceFileSpec to a directory structure
//         /// in TargetRoot, with full recursion into all sub-directories of SourceRoot.
//         /// </summary>
//         /// <returns>The number of files copied.</returns>
//         public int CopyDeep()
//         {
//             return CopyDeep(SourcePath);
//         }

//         /// <summary>
//         /// Copies all files in SourceRoot that match SourceFileSpec to a directory structure
//         /// in TargetRoot, without recursing into the sub-directories of SourceRoot.
//         /// </summary>
//         /// <returns>The number of files copied.</returns>
//         public int CopyShallow()
//         {
//             return Directory.GetFiles(SourcePath, FileSpec).Sum(longFileName => CopyFile(longFileName));
//         }

//         /// <summary>
//         /// Gets the full path to the image file, "\\\\xplan-imgsrv01\\s4_images\\L1-082\\L2-051\\82051975.pdf" for example.
//         /// </summary>
//         /// <param name="hsmvRptNbr">The HSMV report number.</param>
//         /// <returns>The full path and file name for the reports image file.</returns>
//         public string GetFilePath(int hsmvRptNbr)
//         {
//             return string.Format("{0}{1}{2}", TargetPath, TargetPathSeparator, GetImageUrl(hsmvRptNbr));
//         }

//         /// <summary>
//         /// Gets the relative URL for the image file, "L1-081\\L2-977\\81977205.pdf" for example.
//         /// </summary>
//         /// <param name="hsmvRptNbr">The HSMV report number.</param>
//         /// <returns>The relative URL for the reports image file.</returns>
//         public string GetImageUrl(int hsmvRptNbr)
//         {
//             return string.Format("{0}{1}{2}{3}", GetImagePath(hsmvRptNbr), TargetPathSeparator, hsmvRptNbr, GetImageExt(hsmvRptNbr));
//         }

//         /// <summary>
//         /// Given an HSMV report number, returns a calculated target path where the
//         /// source files (*.pdf, *.tif) will be copied.
//         /// </summary>
//         /// <param name="hsmvRptNbr">An HSMV report number.</param>
//         /// <returns>The target path where the source files will be copied.</returns>
//         public string GetTargetPath(int hsmvRptNbr)
//         {
//             // build the path from digits 1-3 and 4-6 and return it
//             return string.Format("{0}{1}{2}", TargetPath, TargetPathSeparator, GetImagePath(hsmvRptNbr));
//         }

//         /// <summary>
//         /// Moves all files in SourceRoot that match SourceFileSpec to a directory structure
//         /// in TargetRoot, with full recursion into all sub-directories of SourceRoot.
//         /// </summary>
//         /// <returns>The number of files moved.</returns>
//         public int MoveDeep()
//         {
//             return MoveDeep(SourcePath);
//         }

//         /// <summary>
//         /// Moves all files in SourceRoot that match SourceFileSpec to a directory structure
//         /// in TargetRoot, without recursing into the sub-directories of SourceRoot.
//         /// </summary>
//         /// <returns>The number of files moved.</returns>
//         public int MoveShallow()
//         {
//             return Directory.GetFiles(SourcePath, FileSpec).Sum(longFileName => MoveFile(longFileName));
//         }

//         #endregion

//         #region Private Instance Methods

//         /// <summary>
//         /// Copies any files from the specified directory that satisfy the SourceFileSpec,
//         /// then recurses into each sub-directory.
//         /// </summary>
//         /// <param name="directoryName">The path to a source directory containing files and/or sub-directories.</param>
//         /// <returns>The number of files copied.</returns>
//         private int CopyDeep(string directoryName)
//         {
//             // copy any files that satisfy the SourceFileSpec and recurse into any sub-directories
//             return Directory.GetFiles(directoryName, FileSpec).Sum(longFileName => CopyFile(longFileName)) + Directory.GetDirectories(directoryName).Sum(subDirectory => CopyDeep(subDirectory));
//         }

//         /// <summary>
//         /// Copies the file specified by longFileName to a calculated location within RootPath.
//         /// If the target directory does not already exist, it is created.
//         /// </summary>
//         /// <param name="longFileName">The file name to copy, including path.</param>
//         /// <returns>The number of files copied.</returns>
//         private int CopyFile(string longFileName)
//         {
//             var filesCopied = 0;
//             var hsmvReportNumber = GetHsmvReportNumber(longFileName);
//             var shortFileName = GetShortFileName(longFileName);
//             var targetPath = GetTargetPath(hsmvReportNumber);
//             var targetFile = string.Format("{0}{1}{2}", targetPath, _targetPathSeparator, shortFileName);

//             // if the target directory does not exist, create it
//             if (!Directory.Exists(targetPath))
//             {
//                 try { Directory.CreateDirectory(targetPath); }
//                 catch (Exception ex) { Debug.WriteLine("CreateDirectory failed for {0}\n    {1}", targetPath, ex.Message); }
//             }

//             // copy the file to the target location
//             try
//             {
//                 File.Copy(longFileName, targetFile, true);   // overwrite if targetFile exists
//                 filesCopied++;
//                 UpdateImageInfo(shortFileName);
//             }
//             catch (Exception ex) { Debug.WriteLine("Copy failed for {0}\n    {1}", targetFile, ex.Message); }

//             // clean up any other files in the target location
//             var files = Directory.GetFiles(targetPath, string.Format("{0}.*", hsmvReportNumber));
//             foreach (var file in from file in files let fname = Path.GetFileName(file) where fname != null && !fname.Equals(shortFileName) select file)
//             {
//                 try { File.Delete(file); }
//                 catch (Exception ex) { Debug.WriteLine("Delete failed for {0}\n    {1}", file, ex.Message); }
//             }
//             return filesCopied;
//         }

//         /// <summary>
//         /// Gets the image extension (".pdf", ".tif", etc) from the FACT_CRASH_EVT table.
//         /// </summary>
//         /// <param name="hsmvRptNbr">The HSMV report number.</param>
//         /// <returns>The image extension if the image has been copied or moved to the target
//         /// location, otherwise an empty string.</returns>
//         private string GetImageExt(int hsmvRptNbr)
//         {
//             var imgExtTx = string.Empty;
//             const string selectText = "SELECT IMG_EXT_TX FROM S4_WAREHOUSE.FACT_CRASH_EVT WHERE HSMV_RPT_NBR = :hsmvRptNbr";

//             using (var connection = new OracleConnection(ConnectionString))
//             {
//                 connection.Open();

//                 using (var selectCmd = new OracleCommand(selectText, connection) {BindByName = true})
//                 {
//                     selectCmd.Parameters.Add("hsmvRptNbr", OracleDbType.Decimal).Value = hsmvRptNbr;
//                     var col = selectCmd.ExecuteScalar();
//                     if (col != null) { imgExtTx = col.ToString(); }
//                 } // dispose OracleCommand
//             } // dispose OracleConnection

//             return imgExtTx;
//         }

//         /// <summary>
//         /// Gets the relative image path not including the actual image file name,
//         /// "L1-081\\L2-977" for example.
//         /// </summary>
//         /// <param name="hsmvRptNbr">The HSMV report number.</param>
//         /// <returns>The relative image path, not including the actual image file name.</returns>
//         private string GetImagePath(int hsmvRptNbr)
//         {
//             // pad the hsmv report number to 9 digits
//             var s = hsmvRptNbr.ToString("D9");

//             return string.Format("L1-{0}{1}L2-{2}", s.Substring(0, 3), TargetPathSeparator, s.Substring(3, 3));
//         }

//         /// <summary>
//         /// Moves any files from the specified directory that satisfy the SourceFileSpec,
//         /// then recurses into each sub-directory.
//         /// </summary>
//         /// <param name="directoryName">The path to a source directory containing files and/or sub-directories.</param>
//         /// <returns>The number of files moved.</returns>
//         private int MoveDeep(string directoryName)
//         {
//             // move any files that satisfy the SourceFileSpec and recurse into any sub-directories
//             return Directory.GetFiles(directoryName, FileSpec).Sum(longFileName => MoveFile(longFileName)) + Directory.GetDirectories(directoryName).Sum(subDirectory => MoveDeep(subDirectory));
//         }

//         /// <summary>
//         /// Moves the file specified by longFileName to a calculated location within RootPath.
//         /// If the target directory does not already exist, it is created.
//         /// </summary>
//         /// <param name="longFileName">The file name to move, including path.</param>
//         /// <returns>The number of files moved.</returns>
//         private int MoveFile(string longFileName)
//         {
//             var filesMoved = 0;
//             var hsmvReportNumber = GetHsmvReportNumber(longFileName);
//             var shortFileName = GetShortFileName(longFileName);
//             var targetPath = GetTargetPath(hsmvReportNumber);
//             var targetFile = string.Format("{0}{1}{2}", targetPath, _targetPathSeparator, shortFileName);

//             // if the target directory does not exist, create it
//             if (!Directory.Exists(targetPath))
//             {
//                 try { Directory.CreateDirectory(targetPath); }
//                 catch (Exception ex) { Debug.WriteLine("CreateDirectory failed for {0}\n    {1}", targetPath, ex.Message); }
//             }

//             // if the target file exists, delete it
//             if (File.Exists(targetFile))
//             {
//                 try { File.Delete(targetFile); }
//                 catch (Exception ex) { Debug.WriteLine("Delete failed for {0}\n    {1}", targetFile, ex.Message); }
//             }

//             // move the file to the target location
//             try
//             {
//                 File.Move(longFileName, targetFile);
//                 filesMoved++;
//                 UpdateImageInfo(shortFileName);
//             }
//             catch (Exception ex) { Debug.WriteLine("Move failed for {0}\n    {1}", targetFile, ex.Message); }

//             // clean up any other files in the target location
//             var files = Directory.GetFiles(targetPath, string.Format("{0}.*", hsmvReportNumber));
//             foreach (var file in from file in files let fname = Path.GetFileName(file) where fname != null && !fname.Equals(shortFileName) select file)
//             {
//                 try { File.Delete(file); }
//                 catch (Exception ex) { Debug.WriteLine("Delete failed for {0}\n    {1}", file, ex.Message); }
//             }
//             return filesMoved;
//         }

//         /// <summary>
//         /// Updates FACT_CRASH_EVT.IMG_EXT_TX with the image file extension indicating a) that the image
//         /// has been successfully copied or moved to the target location, and b) what the file extension
//         /// is at that target location. Now, knowing only the HSMV report number, you can construct a
//         /// relative URL to serve up the image using FileManager.GetTargetPath(hsmvRptNbr) +
//         /// TargetPathSeparator + hsmvRptNbr + FileManager.GetImageExt(hsmvRptNbr).  Also updates
//         /// FACT_CRASH_EVT.IMG_SRC_NM indicating the source of the crash report image.
//         /// </summary>
//         /// <param name="shortFileName">A short file name without the path, "99999999.pdf" for example.</param>
//         private void UpdateImageInfo(string shortFileName)
//         {
//             var hsmvRptNbr = Convert.ToInt32(Path.GetFileNameWithoutExtension(shortFileName));
//             var imgExtTx = Path.GetExtension(shortFileName);
//             const string updateText = "UPDATE S4_WAREHOUSE.FACT_CRASH_EVT SET IMG_EXT_TX = :imgExtTx, IMG_SRC_NM = :imgSrcNm WHERE HSMV_RPT_NBR = :hsmvRptNbr";

//             using (var connection = new OracleConnection(ConnectionString))
//             {
//                 connection.Open();

//                 using (var transaction = connection.BeginTransaction())
//                 {
//                     int rowsUpdated;
//                     using (var insertCmd = new OracleCommand(updateText, connection) {BindByName = true})
//                     {
//                         insertCmd.Parameters.Add("imgExtTx", OracleDbType.Varchar2).Value = imgExtTx;
//                         insertCmd.Parameters.Add("imgSrcNm", OracleDbType.Varchar2);
//                         if (ImageSource == null) insertCmd.Parameters["imgSrcNm"].Value = DBNull.Value;
//                         else insertCmd.Parameters["imgSrcNm"].Value = ImageSource;
//                         insertCmd.Parameters.Add("hsmvRptNbr", OracleDbType.Decimal).Value = hsmvRptNbr;
//                         rowsUpdated = insertCmd.ExecuteNonQuery();
//                     } // dispose OracleCommand

//                     if (rowsUpdated > 0) { transaction.Commit(); }
//                     else { transaction.Rollback(); }

//                 } // dispose OracleTransaction
//             } // dispose OracleConnection
//         }

//         #endregion

//         #region Private Class Methods

//         /// <summary>
//         /// Extracts the HSMV report number from a long file name.
//         /// </summary>
//         /// <param name="longFileName">A long file name, including path info.</param>
//         /// <returns>The HSMV report number.</returns>
//         private static int GetHsmvReportNumber(string longFileName)
//         {
//             var noPathFileName = Path.GetFileName(longFileName);
//             if (noPathFileName != null)
//             {
//                 var noPathFileNameWithoutExtension = Path.GetFileNameWithoutExtension(noPathFileName);
//                 if (noPathFileNameWithoutExtension != null && noPathFileNameWithoutExtension.Length > 11)
//                 {
//                     // new format "[YYYYMMDD] + [HSMV REPORT NUMBER (8 DIGITS)] + [COUNTY CODE (2 DIGITS)] + [L|S|U].ext"
//                     return Convert.ToInt32(noPathFileName.Substring(8, noPathFileNameWithoutExtension.Length - 11));
//                 }

//                 // old format "[HSMV REPORT NUMBER (1 to 9 DIGITS)].ext"
//                 return Convert.ToInt32(noPathFileNameWithoutExtension);
//             }

//             return 0;
//         }

//         /// <summary>
//         /// Gets the target file name, not including any path info, "82032536.tif" for example.
//         /// </summary>
//         /// <param name="longFileName">A long file name including path info.</param>
//         /// <returns>The target file name having format "hsmvRptNbr.ext".</returns>
//         private static string GetShortFileName(string longFileName)
//         {
//             var hsmvReportNumber = GetHsmvReportNumber(longFileName);
//             var fileExtension = Path.GetExtension(longFileName);

//             return string.Format("{0}{1}", hsmvReportNumber, fileExtension);
//         }

//         /// <summary>
//         /// Removes trailing forward "/" and backward "\" slashes from the end of the path.
//         /// </summary>
//         /// <param name="path">The path.</param>
//         /// <returns>The path with trailing forward and backward slashes removed.</returns>
//         private static string RemoveTrailingSlashes(string path)
//         {
//             char[] slashes = { '/', '\\' };
//             return path.TrimEnd(slashes);
//         }

//         #endregion

//     }
// }
