using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Roots.SupportedFileTypes
{
    public class MimeTypes
    {
        public static readonly FileType OctetStream = new FileType("application/octet-stream");
        public static readonly FileType RootsPlaceholder = new FileType("application/roots-placeholder",".placeholder");
        public static readonly FileType Chemical = new FileType("chemical/x-mdl-sdf", ".mdl", ".sd",".sdf");
        public static readonly FileType PDF = new FileType("application/pdf",".pdf");
        public static readonly FileType PainText = new FileType("text/plain",".txt");
        public static readonly FileType ImageJPG = new FileType("image/jpeg",".jpg",".jpeg");

        public static FileType GetFileType(string extension)
        {
            return GetAllFileTypes().FirstOrDefault(x => x.MatchExtension(extension));
        }

        public static IEnumerable<FileType> GetAllFileTypes()
        {
            var types = GetAllProperties();
            return types.Select(x => (FileType)x.GetValue(null));
        }

        public static FieldInfo[] GetAllProperties()
        {
            var types = typeof(MimeTypes).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);
            return types;
        }
    }          
}
