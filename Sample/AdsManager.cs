using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class AdsManager
    {
        public IEnumerable<Uri> GetImageFiles()
        {
            var files = Directory.CreateDirectory("Images")
                .GetFiles()
                .Where(i => i.Extension == ".jpg" || i.Extension == ".jpeg")
                .Select(i => new Uri(i.FullName)).ToArray();
            return files;
        }
    }
}
