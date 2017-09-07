using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ASTRA.EMSG.Common.Utils;

namespace ASTRA.EMSG.Common.Master.Utils
{
    public static class ImageHelpers
    {
        public static bool AreEqual(List<Image> imageListOne, List<Image> imageListTwo)
        {
            if (imageListTwo.Count != imageListOne.Count)
                return false;

            return !imageListTwo.Where((t, i) => t.Size != imageListOne[i].Size || !AreEqual(t, imageListOne[i])).Any();
        }

        public static bool AreEqual(Image imageOne, Image imageTwo)
        {
            var bitmapOne = new Bitmap(imageOne);
            var bitmapTwo = new Bitmap(imageTwo);
            var differenceCount = 0.0;
            for (int i = 0; i < imageOne.Width; i++)
                for (int j = 0; j < imageOne.Height; j++)
                    if (bitmapOne.GetPixel(i, j) != bitmapTwo.GetPixel(i, j))
                    {
                        differenceCount++;
                    }

            var totalPixelCount = bitmapOne.Size.Height * bitmapOne.Size.Width;
            return (differenceCount / totalPixelCount) <= 0.01;
        }

        public static List<Image> GetAllPages(string file)
        {
            return GetAllPages((Bitmap)Image.FromFile(file));
        }

        public static List<Image> GetAllPages(Bitmap bitmap)
        {
            List<Image> images = new List<Image>();
            int count = bitmap.GetFrameCount(FrameDimension.Page);
            for (int idx = 0; idx < count; idx++)
            {
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);

                images.Add(Image.FromStream(byteStream));
            }
            return images;
        }

        public static bool AreEqual(string fileOne, string fileTwo)
        {
            using (var tiffOne = File.OpenRead(fileOne))
            {
                using (var tiffTwo = File.OpenRead(fileTwo))
                {
                    var byteArrayOne = tiffOne.ReadAllByte();
                    var byteArrayTwo = tiffTwo.ReadAllByte();

                    if (byteArrayOne.Length != byteArrayTwo.Length)
                        return false;

                    for (int i = 0; i < byteArrayOne.Length; i++)
                    {
                        if (byteArrayOne[i] != byteArrayTwo[i])
                            return false;
                    }
                }
            }

            return true;
        }

        public static bool AreEqualByPixel(string fileOne, string fileTwo)
        {
            using (var tiffOne = (Bitmap)Image.FromFile(fileOne))
            {
                using (var tiffTwo = (Bitmap)Image.FromFile(fileTwo))
                {
                    return AreEqual(GetAllPages(tiffOne), GetAllPages(tiffTwo));
                }
            }
        }
    }
}